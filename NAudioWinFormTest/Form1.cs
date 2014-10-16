using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NAudioWinFormTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            using (System.IO.FileStream fs = new System.IO.FileStream(wavFileName, System.IO.FileMode.Open))
            {
                byte[] buffer = new byte[fs.Length];

                fs.Read(buffer, 0, buffer.Length);

                int AudioFormat = ((int)buffer[20 + 1] << 8) + (int)buffer[20];
                int NumChanels = ((int)buffer[22 + 1] << 8) + (int)buffer[22];
                int SampleRate = ((int)buffer[24+3] << 24) + ((int)buffer[24+2] << 16) + ((int)buffer[24+1] << 8) + (int)buffer[24];
                int ByteRate = ((int)buffer[28+3] << 24) + ((int)buffer[28+2] << 16) + ((int)buffer[28+1] << 8) + (int)buffer[28];
                int BlockAlign = ((int)buffer[32+1] << 8) + (int)buffer[32];
                int BitsPerSample = ((int)buffer[34+1] << 8) + (int)buffer[34];
                // 2 byte for extra param. why? nobody knows.
                int extraParamSize = 2;
                string Subchank2ID = Encoding.Default.GetString(buffer, 36 + extraParamSize, 4);
                int AudioDataSize = ((int)buffer[40 + 3 + extraParamSize] << 24) + ((int)buffer[40 + 2 + extraParamSize] << 16) + ((int)buffer[40 + 1 + extraParamSize] << 8) + (int)buffer[40 + extraParamSize];

                MessageBox.Show(string.Format(
                       "AudioFormat: {0}"+Environment.NewLine + 
                       "NumChanels: {1}"+Environment.NewLine + 
                       "SampleRate: {2}"+Environment.NewLine +
                       "ByteRate: {3}" + Environment.NewLine +
                       "BlockAlign: {4}" + Environment.NewLine +
                       "BitsPerSample: {5}" + Environment.NewLine +
                       "Subchank2ID: {6}" + Environment.NewLine +
                       "AudioDataSize: {7}" + Environment.NewLine,
                       AudioFormat, NumChanels, SampleRate, 
                       ByteRate, BlockAlign, BitsPerSample,
                       Subchank2ID, AudioDataSize
                        ));
            }

            using (WaveFileReader wavStream = new WaveFileReader(wavFileName))
            {
                byte[] buffer = new byte[wavStream.Length];

                wavStream.Read(buffer, 0, buffer.Length);

                MessageBox.Show(string.Format(
                       "AudioFormat: {0}" + Environment.NewLine +
                       "NumChanels: {1}" + Environment.NewLine +
                       "SampleRate: {2}" + Environment.NewLine +
                       "ByteRate: {3}" + Environment.NewLine +
                       "BlockAlign: {4}" + Environment.NewLine +
                       "BitsPerSample: {5}" + Environment.NewLine +
                       "Subchank2ID: {6}" + Environment.NewLine +
                       "AudioDataSize: {7}" + Environment.NewLine,
                       (int)wavStream.WaveFormat.Encoding, wavStream.WaveFormat.Channels, wavStream.WaveFormat.SampleRate,
                       wavStream.WaveFormat.AverageBytesPerSecond, wavStream.BlockAlign, wavStream.WaveFormat.BitsPerSample,
                       "unknw", wavStream.Length
                        ));
            }
        }

        string wavFileName = @"..\..\..\audio\[NFS Most Wanted].wav";
        WaveFileReader wavStream;

        private void btn_testWaveViewer_Click(object sender, EventArgs e)
        {             
            try
            {
                if (customWaveViewer1.WaveStream != null)
                {
                    customWaveViewer1.WaveStream.Close();
                }
                customWaveViewer1.WaveStream = new WaveFileReader(new System.IO.FileStream(wavFileName, System.IO.FileMode.Open));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("время: {0} :: ошибка: {1}", DateTime.Now.TimeOfDay, ex.Message));
            }
            
//            customWaveViewer1.FitToScreen();

        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            customWaveViewer1.Invalidate();
        }


    }
}
