using System;
using System.Collections.Generic;
using System.Text;
using NAudio;
using NAudio.Wave;


namespace NAudioTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string mp3FileName = @"[NFS Most Wanted].mp3";
            string wavFileName = @"[NFS Most Wanted].wav";

            int len = (int)new System.IO.FileInfo(mp3FileName).Length;

            using (Mp3FileReader mp3Reader = new Mp3FileReader(mp3FileName))
            {
                using (WaveStream pcmStream = new BlockAlignReductionStream(WaveFormatConversionStream.CreatePcmStream(mp3Reader)))
                {
                    WaveChannel32 s = new WaveChannel32(pcmStream);

                    
                }
                
                
            }

            Console.ReadLine();
            
            
            
            
            
            //string mp3file;

            //Try to read a mp3 file path until it gets valid one.            
            //do
            //{
            //    do
            //    {
            //        Console.Out.Write("Please enter the mp3 path:");
            //        mp3file = Console.In.ReadLine();
            //    } while (!System.IO.File.Exists(mp3file));

            //} while (!mp3file.EndsWith(".mp3"));


            //


            //Generate the wav file path for output.
            //string wavfile = mp3file.Replace(".mp3", ".wav");
            //string wavpath = wavfile;



            ////Get audio file name for display in console.
            //int index = wavfile.LastIndexOf("\\");
            //string wavname = wavfile.Substring(index + 1, wavfile.Length - index - 1);
            //index = mp3file.LastIndexOf("\\");
            //string mp3name = mp3file.Substring(index + 1, mp3file.Length - index - 1);
                        
            ////Display message.
            //Console.Out.WriteLine("Converting"+"\n"+"{0}"+"\n"+"to"+"\n"+"{1}", mp3name, wavname);



            ////step 1: read in the MP3 file with Mp3FileReader.
            //using (Mp3FileReader reader = new Mp3FileReader(mp3file))
            //{
            //    //step 2: get wave stream with CreatePcmStream method.
            //    using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
            //    {
            //        //step 3: write wave data into file with WaveFileWriter.
            //        WaveFileWriter.CreateWaveFile(wavfile, pcmStream);
            //    }
            //}
            

            //string fileName = mp3file;

            //using (WaveFileReader outStream = new WaveFileReader(fileName))
            //{
            //    byte[] header = new byte[2048];

            //    float[] samples = new float[outStream.SampleCount];

            //    using (RawSourceWaveStream rawStream = new RawSourceWaveStream(outStream, outStream.WaveFormat))
            //    {
            //        rawStream.ToSampleProvider().Read(samples, 0, outStream.SampleCount);
            //    }
                

            //    for (int i = 0; i < header.Length; i++)
            //    {
            //        Console.Write(header[i] + " ");
            //    }
            //}
          

            //Console.WriteLine();
            ////Console.Out.WriteLine("Conversion finish and wav is saved at {0}.\nPress any key to finish.", wavpath);
            //Console.In.ReadLine();
        }
    }
}
