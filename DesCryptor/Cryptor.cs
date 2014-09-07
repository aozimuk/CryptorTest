using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesCryptor
{
    public class Cryptor
    {
        /// <summary>
        /// Размер шифруемого блока - 64 бит
        /// </summary>
        public static readonly int STD_BLOCK_LEN = 64;

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
        private BitArray InitialPermutation(BitArray block64)
        {
            
            if (block64.Length != STD_BLOCK_LEN)
            {
                throw new FormatException(string.Format("Передан блок неверного размера - {0}. Размер блока должен быть равен 64 бит", block64.Length));
            }

            // перестановка битов переданного блока по массиву индексов начальной перестановки IP 
            return BitPermutation(block64, StandartDataTables.IP);
        }

        /// <summary>
        /// Выполняет обратную перестановку в соответсвии с алгоритмом DES
        /// </summary>
        /// <param name="block64">Блок размером 8 байт - 64 бита</param>
        private BitArray FinalPermutation(BitArray block64)
        {
 
            if (block64.Length != STD_BLOCK_LEN)
            {
                throw new FormatException(string.Format("Передан блок неверного размера - {0}. Размер блока должен быть равен 64 бит", block64.Length));
            }

            // перестановка битов переданного блока по массиву индексов обратной перестановки IP_rev
            return BitPermutation(block64, StandartDataTables.IP_rev);
        }

        private BitArray Encrypting(BitArray block64)
        {

            // проверка длинны шифруемого блока, алгоритм работает с блоками длинной 64 бита
            if (block64.Length != STD_BLOCK_LEN)
            {
                throw new FormatException(string.Format("Передан блок неверного размера - {0}. Размер блока должен быть равен 64 бит.", block64.Length));
            }

            // шаг 1 - начальная перестановка
            
            BitArray res = this.InitialPermutation(block64);

            // шаг 2 - шифрование (16 раз ключом 56-бит)
            
            BitArray L_prev = res.GetLeftHalf();
            BitArray R_prev = res.GetRightHalf();

            BitArray L_cur, R_cur;

            BitArray K_cur = 

            for (int i = 0; i < 16; i++)
            {
                L_cur = R_prev;
                R_cur = L_prev.Xor(f(R_prev,K_cur));

                R_prev = R_cur;
                L_prev = L_cur;
            }

            // шаг 3 - конечная перестановка

            // зашифрованный текст
            return res;
        }
        /// <summary>
        /// Выполняет перестановку переданного массива бит в соответсвтии с указанным массивом индексов
        /// </summary>
        /// <param name="bitArr">Массив бит для перестановки</param>
        /// <param name="indArr">Массив индексов</param>
        /// <returns>Массив бит полученный из исходного bitArr путем перестановки в соотвтествии с индексами indArr</returns>
        private BitArray BitPermutation(BitArray bitArr, byte[] indArr)
        {
            if (bitArr.Length != indArr.Length)
            {
                throw new FormatException(string.Format("Размер переданного блока ({0}) не соответствует размеру блока индексов ({1}).", bitArr.Length, indArr.Length));
            }

            int len = indArr.Length;
            BitArray res = new BitArray(indArr.Length);
                       
            // перестановка битов переданного блока по массиву индексов  
            for (int i = 0; i < len; i++)
            {
                res[i] = bitArr[indArr[i]];
            }

            return res;
        }


        private BitArray f(BitArray R, BitArray K)
        {
            return null;
        }
    }
}
