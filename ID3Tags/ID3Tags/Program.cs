using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3Tags
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ID3v1 mp3 = new ID3v1();

                mp3.GetTag(@"D:\Sia - I go to sleep.mp3");
                Console.WriteLine(mp3.ID3v1Tag.Artist + "!");
                Console.WriteLine(mp3.ID3v1Tag.Title + "!");
                Console.WriteLine(mp3.ID3v1Tag.Album + "!");
                Console.WriteLine(mp3.ID3v1Tag.Comment + "!");
                
                Console.Read();
                
                mp3.ID3v1Tag.Album = "AlbumTest";
                mp3.ID3v1Tag.Title = "TitleTest";
                mp3.ID3v1Tag.Artist = "ArtistTest";
                mp3.ID3v1Tag.Year = "0000";
                mp3.ID3v1Tag.Comment = "CommentTest";
                mp3.ID3v1Tag.Genre = "0";
                mp3.SetTag(@"D:\Sia - I go to sleep.mp3");
                
                Console.Read();
                
                mp3.GetTag(@"D:\Sia - I go to sleep.mp3");
                Console.WriteLine(mp3.ID3v1Tag.Artist + "!");
                Console.WriteLine(mp3.ID3v1Tag.Title + "!");
                Console.WriteLine(mp3.ID3v1Tag.Album + "!");
                Console.WriteLine(mp3.ID3v1Tag.Comment + "!");  
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
            Console.ReadLine();
        }
    }
}
