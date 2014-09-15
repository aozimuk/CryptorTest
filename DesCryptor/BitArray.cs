using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesCryptor
{
    public class BitArray: ICloneable
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
        /// Формирует массив бит
        /// <para>length не больше 64</para>
        /// </summary>
        /// <param name="number">число</param>
        /// <param name="length">количество бит</param>
        public BitArray(int length, long number)
        {
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

        public BitArray(BitArray bitArr)
        {
            this.bits = new bool[bitArr.bits.Length];
            Array.Copy(bitArr.bits, 0, this.bits, 0, bitArr.bits.Length);
        }

        public BitArray(bool[] bitArr)
        {
            Array.Copy(bitArr, 0, this.bits, 0, bitArr.Length);
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


        /// <summary>
        /// Сложение по модулю 2 (исключающее "или")
        /// </summary>
        /// <param name="bArr">Массив с которым выполняется операция сложения по модулю 2</param>
        /// <returns>Результат операции сложения по модулю 2 массивов бит исходного и переданного</returns>
        public BitArray Xor(BitArray bArr)
        {
            int len = Math.Max(this.Length, bArr.Length);

            BitArray res = new BitArray(len);

            for (int i = 0; i < len; i++)
            {
                res[i] = (this[i] == bArr[i]) ? false : true;
            }

            return res;
        }

        /// <summary>
        /// Объединяет исходный массив бит с переданным, 
        /// </summary>
        /// <param name="right">Массив бит присоединяемы справа (старшие биты)</param>
        public BitArray Concatenate(BitArray right)
        {
            BitArray res = new BitArray(this.Length + right.Length);

            Array.Copy(this.bits, 0, res.bits, 0, this.Length);
            Array.Copy(right.bits, 0, res.bits, this.Length, res.Length);

            return res;
        }

        /// <summary>
        /// Циклический cдвиг влево
        /// </summary>
        /// <param name="count">Количество бит на которое производится сдвиг</param>
        public BitArray RotateLeftShift(int count)
        {
            bool tmp;
            BitArray res = (BitArray)this.Clone();

            // цикл по количеству сдвигов
            for (int i = 0; i < count; i++)
            {
                // запоминаем старший бит
                tmp = this.bits[bits.Length - 1];

               // выполняем сдвиг
                for (int ii = bits.Length - 1; ii > 0; ii--)
                {
                    res[ii] = res[ii - 1];
                }
                
                // на место младшего приходит старший бит
                res[0] = tmp;
            }

            return res;
        }

        /// <summary>
        /// Возравщает левую (младшую) половину бит
        /// </summary>
        /// <returns></returns>
        public BitArray GetLeftHalf()
        {
            if (this.Length % 2 != 0)
            {
                throw new FormatException(string.Format("Исходный массив имеет нечетное количество элементов ({0}).", this.Length));
            }
            
            BitArray res = new BitArray(this.Length/2);

            Array.Copy(this.bits, 0, res.bits, 0, res.Length);

            return res;
        }

        /// <summary>
        /// Возравщает правую (старшую) половину бит
        /// </summary>
        /// <returns></returns>
        public BitArray GetRightHalf()
        {
            if (this.Length % 2 != 0)
            {
                throw new FormatException(string.Format("Исходный массив имеет нечетное количество элементов ({0}).", this.Length));
            }

            BitArray res = new BitArray(this.Length / 2);

            Array.Copy(this.bits, res.Length, res.bits, 0, res.Length);

            return res;
        }


        /// <summary>
        /// Возравщает младшую половину бит
        /// </summary>
        public BitArray GetLowHalf()
        {
            return this.GetLeftHalf();
        }

        /// <summary>
        /// Возравщает старшую половину бит
        /// </summary>
        public BitArray GetHighHalf()
        {
            return this.GetRightHalf();
        }


        /// <summary>
        /// Преобразует массив бит к Int32
        /// </summary>
        public int ToInt32()
        {
            int res = 0;
            int len = Math.Min(32,bits.Length);

            for (int i = 0; i < len; i++)
            {
                res = (res << 1) | (bits[i] ? 1 : 0);
            }

            return res;
        }

        public object Clone()
        {
            return new BitArray(this);
        }
    }
}
