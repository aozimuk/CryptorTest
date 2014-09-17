using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesCryptor
{
    /// <summary>
    /// Класс шифроватор, использует алгоритм DES   
    /// <para>http://protect.htmlweb.ru/des.htm</para>
    /// </summary>
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

        /// <summary>
        /// Шифрует переданное сообщение
        /// </summary>
        /// <param name="msg">Сообщение - массив байт </param>
        /// <param name="key">Ключ - массив байт</param>
        /// <returns>Зашифрованное сообщение</returns>
        public static byte[] Encrypt(byte[] msg, byte[] key)
        {

            // целое число блоков
            int blockCnt = msg.Length / 8; // 64-бит / 8-бит = 8-байт
            // остаток байт не попавших в блок
            int tailCnt = msg.Length % 8;
            // текущий шифруемый блок
            byte[] block = new byte[8];
            // результирующий массив зашифрованного сообщения +1 блок(8-байт) для записи хвоста или пустоты
            byte[] res = new byte[blockCnt * 8 + 8];
            
            
            for (int i = 0; i < blockCnt; i++)
            {
                // подготовил блок 64 бит для шифрования
                Array.Copy(msg, i * 8, block, 0, block.Length);
                // шифрование блока и копирование в результирующий массив
                Array.Copy(Encrypting(block, key), 0, res, i * 8, block.Length);
            }

            // копирование оставшихся байт (которые занимают не целый блок)
            // каждый незначащий байт заполняется чем-то (тем что осталось после предыдущего шага)
            Array.Copy(msg, blockCnt * 8, block, 0, tailCnt);
            // в последний байт записываем количество значащих байт и шифруем
            block[block.Length - 1] = (byte)tailCnt;
            block = Encrypting(block, key);
            Array.Copy(block, 0, res, blockCnt * 8, block.Length);
            
            return res;
        }

        /// <summary>
        /// Дешифрует переданное сообщение
        /// </summary>
        /// <param name="msg">Шифрованное сообщение - массив байт </param>
        /// <param name="key">Ключ - массив байт</param>
        /// <returns>Расшифрованное сообщение</returns>
        public static byte[] Decrypt(byte[] msg, byte[] key)
        {
            // количество пришедших блоков, будет минимум 1, иначе полетит все к чертям )
            int blockCnt = msg.Length / 8; // 64-бит / 8-бит = 8-байт
            // массив для хранения всего что расшифруется
            byte[] tmpRes = new byte[msg.Length];
            // текущий расшифруемый блок
            byte[] block = new byte[8];
            // результирующий массив расшифрованного сообщения 
            byte[] res;

            for (int i = 0; i < blockCnt; i++)
            {
                // подготовил блок 64 бит для расшифрования
                Array.Copy(msg, i * 8, block, 0, block.Length);
                // расшифрование блока и копирование во временный массив
                Array.Copy(Decrypting(block, key), 0, tmpRes, i * 8, block.Length);
            }
            // полчил количество значащих байт в последнем блоке
            int tailCnt = tmpRes[tmpRes.Length-1];

            // вычисляем размер результирующего массива
            //   если кол-во блоков > 1, то из длинны пришедшего сообщения вычитаем количество незначащих байт
            //   если кол-во блоко = 1, то рамер равен количеству значащих байт в последнем(единственном) блоке
            res = new byte[(blockCnt > 1) ? (msg.Length - (8 - tailCnt)) : tailCnt];

            // копируем в результирущий массив
            Array.Copy(tmpRes, 0, res, 0, res.Length);

            return res;
        }

        /// <summary>
        /// Выполняет шифрование переданного блока 64 бит ключом 64 бит по алгоритму DES
        /// </summary>
        /// <param name="block64">Шифруемый блок</param>
        /// <param name="key64">Ключ шифрования</param>
        /// <returns>Зашифрованный блок</returns>
        private static byte[] Encrypting(byte[] block64, byte[] key64)
        {
            return Encrypting(new BitArray(block64), new BitArray(key64)).ToByteArray();
        }

        private static byte[] Decrypting(byte[] block64, byte[] key64)
        {
            return Decrypting(new BitArray(block64), new BitArray(key64)).ToByteArray();
        }
        /// <summary>
        /// Выполняет шифрование переданного блока 64 бит ключом 64 бит по алгоритму DES
        /// </summary>
        /// <param name="block64">Шифруемый блок</param>
        /// <param name="key64">Ключ шифрования</param>
        /// <returns>Зашифрованный блок</returns>
        private static BitArray Encrypting(BitArray block64, BitArray key64)
        {

            // проверка длинны шифруемого блока, алгоритм работает с блоками длинной 64 бита
            if (block64.Length != STD_BLOCK_LEN)
            {
                throw new FormatException(string.Format("Передан блок неверного размера - {0}. Размер блока должен быть равен 64 бит.", block64.Length));
            }

            // проверка длинны ключа шифрования, алгоритм работает с ключами длинной 64 бита
            if (key64.Length != STD_KEY_LEN)
            {
                throw new FormatException(string.Format("Передан ключ неверного размера - {0}. Размер ключа должен быть равен 64 бит.", key64.Length));
            }

            // шаг 1 - начальная перестановка

            BitArray res = BitPermutation(block64, StandartDataTables.IP);

            // шаг 2 - шифрование (16 раз ключом 56-бит)

            BitArray L_prev = res.GetLeftHalf();
            BitArray R_prev = res.GetRightHalf();

            BitArray L_cur, R_cur;

            //два 28-битовых блока ключа после начальной перестановки
            BitArray C = BitPermutation(key64, StandartDataTables.C0);
            BitArray D = BitPermutation(key64, StandartDataTables.D0);

            BitArray[] K = new BitArray[16];

            // формируем ключи для каждого шага шифрования
            for (int i = 0; i < 16; i++)
            {
                C = C.RotateLeftShift(StandartDataTables.R[i]);
                D = D.RotateLeftShift(StandartDataTables.R[i]);
                K[i] = BitPermutation(C.Concatenate(D), StandartDataTables.H);
            }

            for (int i = 0; i < 16; i++)
            {

                L_cur = R_prev;
                R_cur = L_prev.Xor(f(R_prev, K[i]));

                R_prev = R_cur;
                L_prev = L_cur;
            }

            // шаг 3 - конечная перестановка
            res = BitPermutation(R_prev.Concatenate(L_prev), StandartDataTables.IP_rev);

            // зашифрованный текст
            return res;
        }

        /// <summary>
        /// Выполняет расшифрование переданного блока 64 бит ключом 64 бит по алгоритму DES
        /// </summary>
        /// <param name="block64">Расшифруемый блок</param>
        /// <param name="key64">Ключ шифрования</param>
        /// <returns>Рашифрованный блок</returns>
        private static BitArray Decrypting(BitArray block64, BitArray key64)
        {

            // проверка длинны расшифруемого блока, алгоритм работает с блоками длинной 64 бита
            if (block64.Length != STD_BLOCK_LEN)
            {
                throw new FormatException(string.Format("Передан блок неверного размера - {0}. Размер блока должен быть равен 64 бит.", block64.Length));
            }

            // проверка длинны ключа шифрования, алгоритм работает с ключами длинной 64 бита
            if (key64.Length != STD_KEY_LEN)
            {
                throw new FormatException(string.Format("Передан ключ неверного размера - {0}. Размер ключа должен быть равен 64 бит.", key64.Length));
            }

            // шаг 1 - начальная перестановка

            BitArray res = BitPermutation(block64, StandartDataTables.IP);

            // шаг 2 - шифрование (16 раз ключом 56-бит)

            BitArray L_prev = res.GetLeftHalf();
            BitArray R_prev = res.GetRightHalf();

            BitArray L_cur, R_cur;

            //два 28-битовых блока ключа после начальной перестановки
            BitArray C = BitPermutation(key64, StandartDataTables.C0);
            BitArray D = BitPermutation(key64, StandartDataTables.D0);

            BitArray[] K = new BitArray[16];

            // формируем ключи для каждого шага шифрования
            for (int i = 0, ii = 15; i < 16 && ii >= 0; i++, ii--)
            {
                C = C.RotateLeftShift(StandartDataTables.R[i]);
                D = D.RotateLeftShift(StandartDataTables.R[i]);
                K[ii] = BitPermutation(C.Concatenate(D), StandartDataTables.H);
            }

            for (int i = 0; i < 16; i++)
            {

                L_cur = R_prev;
                R_cur = L_prev.Xor(f(R_prev, K[i]));

                R_prev = R_cur;
                L_prev = L_cur;
            }

            // шаг 3 - конечная перестановка
            res = BitPermutation(R_prev.Concatenate(L_prev), StandartDataTables.IP_rev);

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
        private static BitArray BitPermutation(BitArray bitArr, byte[] indArr)
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
        private static BitArray f(BitArray R, BitArray K)
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

            res = BitFunction_6to4(B[0], 0);
            for (int i = 1; i < B.Length; i++)
            {
                res = res.Concatenate(BitFunction_6to4(B[i], i));
            }

            return BitPermutation(res, StandartDataTables.P);
        }

        /// <summary>
        /// Преобразует из 6 бит в 4, с помощью таблицы S
        /// </summary>
        private static BitArray BitFunction_6to4(BitArray b6, int index)
        {

            int row = (new BitArray(new bool[] { b6[5], b6[0] })).ToInt32();
            int col = (new BitArray(new bool[] { b6[4], b6[3], b6[2], b6[1] })).ToInt32();

            return new BitArray(4, StandartDataTables.S[index, row, col]);
        }
    }
}
