﻿using System;
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
            string mp3file;

            //Try to read a mp3 file path until it gets valid one.            
            //do
            //{
            //    do
            //    {
            //        Console.Out.Write("Please enter the mp3 path:");
            //        mp3file = Console.In.ReadLine();
            //    } while (!System.IO.File.Exists(mp3file));

            //} while (!mp3file.EndsWith(".mp3"));


            mp3file = @"[NFS Most Wanted] - Nine Thou (OST Need For Speed Most Wanted).mp3";


            //Generate the wav file path for output.
            string wavfile = mp3file.Replace(".mp3", ".wav");
            string wavpath = wavfile;



            //Get audio file name for display in console.
            int index = wavfile.LastIndexOf("\\");
            string wavname = wavfile.Substring(index + 1, wavfile.Length - index - 1);
            index = mp3file.LastIndexOf("\\");
            string mp3name = mp3file.Substring(index + 1, mp3file.Length - index - 1);


            
            //Display message.
            Console.Out.WriteLine("Converting {0} to {1}", mp3name, wavname);



            //step 1: read in the MP3 file with Mp3FileReader.
            using (Mp3FileReader reader = new Mp3FileReader(mp3file))
            {

                //step 2: get wave stream with CreatePcmStream method.
                using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
                {
                    

                    //step 3: write wave data into file with WaveFileWriter.
                    WaveFileWriter.CreateWaveFile(wavfile, pcmStream);
                }
            }
            Console.Out.WriteLine("Conversion finish and wav is saved at {0}.\nPress any key to finish.", wavpath);
            Console.In.ReadLine();
        }
    }
}
