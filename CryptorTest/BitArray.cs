using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptorTest
{
    public class BitArray
    {
        /// <summary>
        /// Массив бит
        /// </summary>
        private bool[] bits;

        /// <summary>
        /// Формирует массив бит указанной длины
        /// </summary>
        /// <param name="length">Длина массива бит</param>
        public BitArray(int length)
        {
            bits = new bool[length];
        }

        /// <summary>
        /// Формирует массив 64 бит из указанного числа long
        /// </summary>
        /// <param name="number">64-битовое число</param>
        public BitArray(long number)
        {
            int length = sizeof(long);
            bits = new bool[length];
            for (int i = 0; i < length; i++)
            {
                bits[i] = (number & ((long)1 << i)) != 0;
            }
        }

        public BitArray(byte[] byteArr)
        {
            // количество байт
            int cnt = byteArr.Length;
            bits = new bool[cnt * 8];
            // цикл по байтам массива
            for (int i = 0; i < cnt; i++)
            {
                // цикл по битам каждого байта в массиве
                for (int ii = 0; ii < 8; ii++)
                {
                    bits[8*i + ii] = (byteArr[i] & ((long)1 << ii)) != 0;
                }
            }
        }

        /// <summary>
        /// Получение / установка бита на указанной позиции
        /// </summary>
        /// <param name="index">Номер бита</param>
        /// <returns>Значение бита в указанной позиции</returns>
        public bool this[int index]
        {
            get { return this.bits[index]; }
            set { this.bits[index] = value; }
        }

        /// <summary>
        /// Количество бит в массиве
        /// </summary>
        public int Length
        {
            get { return this.bits.Length; }
        }
    }
}
