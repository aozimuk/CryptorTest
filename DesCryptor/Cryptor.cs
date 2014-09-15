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
        
        /// <summary>
        /// Размер входного ключа для шифрования блока 64
        /// </summary>
        public static readonly int STD_KEY_LEN = 64;

        /// <summary>
        /// Размер ключа для шифрования после применения начальной перестановки и циклического сдвига
        /// </summary>
        public static readonly int SHIFTED_KEY_LEN = 56;

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
        private BitArray InitialBlockPermutation(BitArray block64)
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
        private BitArray FinalBlockPermutation(BitArray block64)
        {
 
            if (block64.Length != STD_BLOCK_LEN)
            {
                throw new FormatException(string.Format("Передан блок неверного размера - {0}. Размер блока должен быть равен 64 бит", block64.Length));
            }

            // перестановка битов переданного блока по массиву индексов обратной перестановки IP_rev
            return BitPermutation(block64, StandartDataTables.IP_rev);
        }

        
        
        /// <summary>
        /// Выполняет завершающую обработку ключа
        /// </summary>
        /// <param name="key56">Ключ составленный из частей C(i) И D(i) после циклического сдвига</param>
        /// <returns>48 битовый ключ</returns>
        private BitArray FinalKeyPermutation(BitArray key56)
        {
            // проверка длинны ключа шифрования, алгоритм работает с ключами длинной 64 бита
            if (key56.Length != SHIFTED_KEY_LEN)
            {
                throw new FormatException(string.Format("Передан блок неверного размера - {0}. Размер блока должен быть равен 64 бит.", key56.Length));
            }

            return BitPermutation(key56, StandartDataTables.H);
        }

       

        private BitArray Encrypting(BitArray block64, BitArray key64)
        {

            // проверка длинны шифруемого блока, алгоритм работает с блоками длинной 64 бита
            if (block64.Length != STD_BLOCK_LEN)
            {
                throw new FormatException(string.Format("Передан блок неверного размера - {0}. Размер блока должен быть равен 64 бит.", block64.Length));
            }

            // проверка длинны ключа шифрования, алгоритм работает с ключами длинной 64 бита
            if (key64.Length != STD_KEY_LEN)
            {
                throw new FormatException(string.Format("Передан блок неверного размера - {0}. Размер блока должен быть равен 64 бит.", block64.Length));
            }

            // шаг 1 - начальная перестановка
            
            BitArray res = this.InitialBlockPermutation(block64);

            // шаг 2 - шифрование (16 раз ключом 56-бит)
            
            BitArray L_prev = res.GetLeftHalf();
            BitArray R_prev = res.GetRightHalf();

            BitArray L_cur, R_cur;
            
            //два 28-битовых блока ключа после начальной перестановки
            BitArray C = BitPermutation(key64,StandartDataTables.C0);
            BitArray D = BitPermutation(key64,StandartDataTables.D0);

            BitArray K_cur;

            for (int i = 0; i < 16; i++)
            {
                // поясняю ахтунг ниже. на самом деле все красиво
                // для массивов C и D выполняется циклический сдвиг влево (RotateShiftLeft) на заданное количество бит (R[i]),
                // далее массив C объединется (Concatenate) с массивом D 
                // полученное объединение перестанавливается в соответствии с переданным массивом индексов H
                K_cur = BitPermutation((C.RotateLeftShift(StandartDataTables.R[i])).Concatenate(D.RotateLeftShift(StandartDataTables.R[i])), StandartDataTables.H);

                L_cur = R_prev;
                R_cur = L_prev.Xor(f(R_prev,K_cur));

                R_prev = R_cur;
                L_prev = L_cur;
            }

            // шаг 3 - конечная перестановка
            res = BitPermutation(R_prev.Concatenate(L_prev),StandartDataTables.IP_rev);

            // зашифрованный текст
            return res;
        }
        /// <summary>
        /// Выполняет перестановку переданного массива бит в соответсвтии с указанным массивом индексов.
        /// <para>
        /// Внимание. Если в массиве индексов содержатся индексы указывающие на несуществующие элементы массива, то будет весьма неприятно )
        /// </para>
        /// </summary>
        /// <param name="bitArr">Массив бит для перестановки</param>
        /// <param name="indArr">Массив индексов</param>
        /// <returns>Массив бит полученный из исходного bitArr путем перестановки в соотвтествии с индексами indArr</returns>
        
        private BitArray BitPermutation(BitArray bitArr, byte[] indArr)
        {
            //if (bitArr.Length <= indArr.Length)
            //{
            //    throw new FormatException(string.Format("Размер переданного блока ({0}) не может быть меньше блока индексов ({1}).", bitArr.Length, indArr.Length));
            //}

            int len = indArr.Length;
            BitArray res = new BitArray(indArr.Length);

            // перестановка битов переданного блока по массиву индексов  
            for (int i = 0; i < len; i++)
            {
                res[i] = bitArr[indArr[i]];
            }

            return res;
        }

        /// <summary>
        /// Функция шифрования
        /// </summary>
        /// <param name="R"></param>
        /// <param name="K"></param>
        /// <returns></returns>
        private BitArray f(BitArray R, BitArray K)
        {
            BitArray res = BitPermutation(R, StandartDataTables.E).Xor(K);
            BitArray[] B = new BitArray[8];

            // инициализируем 8 блоков по 6 бит
            for (int i = 0; i < B.Length; i++)
            {
                B[i] = new BitArray(6);
                for (int ii = 0; ii < 6; ii++)
                {
                    B[i][ii] = res[i * 6 + ii];
                }
            }

            res = B[0];
            for (int i = 1; i < B.Length; i++)
            {
                res = res.Concatenate(B[i]);
            }

            return BitPermutation(res, StandartDataTables.P);
        }
        /// <summary>
        /// Преобразует из 6 бит в 4, с помощью таблицы S
        /// </summary>
        private BitArray BitFunction_6to4(BitArray b6, int index)
        {

            int row = (new BitArray(new bool[] { b6[0], b6[5] })).ToInt32();
            int col = (new BitArray(new bool[] { b6[1], b6[2], b6[3], b6[4] })).ToInt32();

            return new BitArray(4,StandartDataTables.S[index,row,col]);
        }
    }
}
