using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using NAudio.Wave;
using System.Collections;

namespace NAudioWinFormTest
{

    public class Steganography
    {


        public static void HideMessage(byte[] message, string srcFileName, string dstFileName)
        {
            using (var srcWave = new FileStream(srcFileName,FileMode.Open))
            using (var dstWave = new FileStream(dstFileName,FileMode.Create))
            {
                HideMessage(message, srcWave, dstWave);
            }
        }

        public static void HideMessage(byte[] message, Stream srcWaveStream, Stream dstWaveStream)
        {
            byte[] msgAndLength = new byte[message.Length + 4];
            Array.Copy(BitConverter.GetBytes(message.Length), 0, msgAndLength, 0, 4);
            Array.Copy(message, 0, msgAndLength, 4, message.Length);

            using (var waveStreamReader = new WaveFileReader(srcWaveStream))
            using (var waveStreamWriter = new WaveFileWriter(dstWaveStream, waveStreamReader.WaveFormat))
            {
                if (CanHide(msgAndLength.Length, waveStreamReader))
                {
                    BitArray msgBits = new BitArray(msgAndLength);
                    int bytesPerSample = waveStreamReader.WaveFormat.BitsPerSample / 8;
                    byte[] waveData = new byte[waveStreamReader.Length]; //new byte[msgBits.Length * bytesPerSample];

                    waveStreamReader.Read(waveData, 0, waveData.Length);

                    for (int i = 0; i < msgBits.Length; i++)
                    {
                        if (msgBits[i] == true && waveData[i * bytesPerSample] % 2 == 0)
                        {
                            waveData[i * bytesPerSample]++;
                        }
                        else if (msgBits[i] == false && waveData[i * bytesPerSample] % 2 == 1)
                        {
                            waveData[i * bytesPerSample]--;
                        }
                    }
                    
                    waveStreamWriter.Write(waveData, 0, waveData.Length);
                }
            }
        }

        public static byte[] UnhideMessage(string srcFileName)
        {
            byte[] result = null;
            using (var srcWave = new FileStream(srcFileName, FileMode.Open))
            {
                result = UnhideMessage(srcWave);
            }
            return result;
        }

        public static byte[] UnhideMessage(Stream srcWaveStream)
        {
            byte[] result = null;
            

            using (var waveStreamReader = new WaveFileReader(srcWaveStream))
            {
                int bytesPerSample = waveStreamReader.WaveFormat.BitsPerSample / 8;
                byte[] waveData = new byte[4 * 8 * bytesPerSample]; // 4 - int size, 8 - bits in byte
                byte[] lengthBytes = new byte[4];

                waveStreamReader.Read(waveData, 0, waveData.Length);
                
                for (int i = 0; i < waveData.Length; i += bytesPerSample)
                {
                    lengthBytes[(i / bytesPerSample) / 8] >>= 1;
                    if (waveData[i] % 2 == 1)
                    {
                        lengthBytes[(i / bytesPerSample) / 8] += 128;
                    }
                }

                int messageLength = BitConverter.ToInt32(lengthBytes, 0);
                byte[] message = new byte[messageLength];

                waveData = new byte[messageLength * 8 * bytesPerSample];


                waveStreamReader.Read(waveData, 0, waveData.Length);

                for (int i = 0; i < waveData.Length; i += bytesPerSample)
                {
                    message[(i / bytesPerSample) / 8] >>= 1;
                    if (waveData[i] % 2 == 1)
                    {
                        message[(i / bytesPerSample) / 8] += 128;
                    }
                }

                result = message;
            }

            return result;
        }

        /// <summary>
        /// Определяет можно ли спрятать сообщение указанной длинны в аудиопотоке 
        /// </summary>
        /// <param name="messageLength">Длинна сообщения в байт</param>
        /// <param name="waveStream">Аудио поток</param>
        /// <returns>true - можно спрятать, false - нельзя спрятать</returns>
        public static bool CanHide(int messageLength, WaveStream waveStream)
        {
            bool result = false;

            if (waveStream != null)
            {
                result = waveStream.Length / (waveStream.WaveFormat.BitsPerSample / 8) >= (long)messageLength * 8;
            }

            return result;
        }


    }
}
