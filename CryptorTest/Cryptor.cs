using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptorTest
{
    public class Cryptor
    {


        public byte[] encode(byte[] msg)
        {
            return null;
        }

        public byte[] decode(byte[] msg)
        {
            return null;
        }

        /// <summary>
        /// Выполняет начальную перестановку в соответсвии с алгоритмом DES
        /// </summary>
        /// <param name="block64">Блок размером 8 байт - 64 бита</param>
        public BitArray InitialPermutation(ref BitArray block64)
        {
            int blockLen = 64;

            if (block64.Length != blockLen)
            {
                throw new FormatException(string.Format("Передан блок неверного размера - {0}. Размер блока должен быть равен 64 бит", block64.Length));
            }

            BitArray res = new BitArray(blockLen);

            // перестановка битов переданного блока по массиву индексов начальной перестановки IP 
            for (int i = 0; i < blockLen; i++)
            {
                res[i] = block64[StandartDataTables.IP[i]];
            }

            return res;
        }


    }
}
