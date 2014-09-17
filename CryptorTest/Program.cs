using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DesCryptor;

namespace CryprtorTest
{
    class Program
    {
        static void Main(string[] args)
        {

            /*
            BitArray b1 = new BitArray(8);

            b1[0] = true; b1[1] = true; b1[2] = true;

            Console.WriteLine(b1.ToInt32().ToString() + " = " + b1.ToBitString());
            
            BitArray b3 = ((BitArray)b1.Clone()).GetLeftHalf().RotateLeftShift(2);

            Console.WriteLine(b3.ToInt32().ToString() + " = " + b3.ToBitString());



            BitArray b2 = new BitArray(new bool[] { false, false, true, true });

            Console.WriteLine(b2.ToInt32().ToString() + " = " + b2.ToBitString() + " left: " + b2.GetLeftHalf() + " right: " + b2.GetRightHalf());

            BitArray b4 = b2.Concatenate(b3);

            Console.WriteLine(b2 + " + " + b3 + " = " + b4 + " = " + b4.ToInt32());


            Console.WriteLine(b2 + " xor " + b4 + " = " + b2.Xor(b4) + " = " + b2.Xor(b4).ToInt32());

            */


            /*
            BitArray b1 = new BitArray(64, 12345);
            BitArray k1 = new BitArray(64, 16);

            Cryptor cr = new Cryptor();

            BitArray tb = new BitArray(new bool[] { false, true, true, false, true, true });

            BitArray bit6to4 = cr.BitFunction_6to4(tb, 1);

            Console.WriteLine(bit6to4 + " = " + bit6to4.ToInt32());

            BitArray bperm = cr.BitPermutation(b1, StandartDataTables.IP);


            Console.WriteLine();
            Console.WriteLine(b1);
            Console.WriteLine(bperm);

            System.IO.StreamWriter sw = new System.IO.StreamWriter("test.txt");

            sw.WriteLine(b1);
            sw.WriteLine(bperm);
            sw.Close();
            
            */

            /*
            
            //BitArray src = new BitArray(64, 123456789012);
            //BitArray key = new BitArray(64, 169992929929);


            byte[] key = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };


            byte[] p = new BitArray(key).ToByteArray();
            

            byte[] arrSrc = new byte[] { 11, 12, 13, 14, 15, 16, 17, 18 };


            byte[] arrCr = Cryptor.Encrypting(arrSrc, key);

            byte[] arrDst = Cryptor.Decrypting(arrCr, key);


             
            //Console.WriteLine("do:          " + src);
            //Console.WriteLine("vo vremya:   " + srcCrypted);
            //Console.WriteLine("posle:       " + srcDeCrypted);
            */
              
             
            /*
            byte[] key = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            byte[] arrSrc = new byte[] { 11, 12, 13, 14, 15, 16, 17, 18,
                                         21, 22, 23, 24, 25, 26, 27, 28};

            byte[] arrCr = Cryptor.Encrypt(arrSrc, key);

            byte[] arrDst = Cryptor.Decrypt(arrCr, key);

            for (int i = 0; i < arrSrc.Length; i++)
            {
                Console.Write(string.Format("{0:3}",arrSrc[i].ToString("d2")));
            }
            Console.WriteLine();

            for (int i = 0; i < arrCr.Length; i++)
            {
                Console.Write(string.Format("{0:3}", arrCr[i].ToString("d2")));
            }
            Console.WriteLine();

            for (int i = 0; i < arrDst.Length; i++)
            {
                Console.Write(string.Format("{0:3}", arrDst[i].ToString("d2")));
            }
            Console.WriteLine();
            */



            string srcStr = "лена набила рожу мужа а муж орал и банан ел";
            string keyStr = "ahtung!!";

            string encrStr = Encoding.Default.GetString(Cryptor.Encrypt(Encoding.Default.GetBytes(srcStr.ToCharArray(), 0, srcStr.Length),
                    Encoding.Default.GetBytes(keyStr.ToCharArray(), 0, keyStr.Length)));

            string decrStr = Encoding.Default.GetString(Cryptor.Decrypt(Encoding.Default.GetBytes(encrStr.ToCharArray(), 0, encrStr.Length),
                    Encoding.Default.GetBytes(keyStr.ToCharArray(), 0, keyStr.Length)));

            
            Console.WriteLine(encrStr);
            Console.WriteLine(decrStr);


            Console.ReadLine();
            

        }
    }
}
