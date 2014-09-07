using System;
using System.Collections.Generic;
using System.Text;

using Resource;
using Type;

namespace Core
{
    public class CoreService
    {
		// счетчики
		private int i = 0;
		private int ii = 0;
		private int count = 0;

		#region Переменные функции шифрования
		private FreeType m_CF_T_48 = new FreeType(47);
		private FreeType m_CF_T_32 = new FreeType(31);
		
		private FreeType m_CF_H_48 = new FreeType(47);

		private FreeType[] m_CF_list_h = new FreeType[8];
		private FreeType[] m_CF_list_t = new FreeType[8];

		private FreeType m_2 = new FreeType(1);
		private FreeType m_4 = new FreeType(3);

		#endregion

		#region Key
		private FreeType[] m_key_48 = new FreeType[16];
		#endregion

		#region Переменные Crypt функции
		FreeType m_T1 = new FreeType(63);
		FreeType m_T2 = new FreeType(63);

		FreeType m_L32 = new FreeType(31);
		FreeType m_H32 = new FreeType(31);
		FreeType m_H32prev = new FreeType(31);
		#endregion

		public CoreService()
		{
			for (i = 0; i < 8; i++)
			{
				 m_CF_list_h[i] = new FreeType(5);
				 m_CF_list_t[i] = new FreeType(3);
			}

			for (i = 0; i < 16; i++)
				m_key_48[i] = new FreeType(47);
		}

		public FreeType XOR(FreeType X, FreeType Y, byte _count)
		{
			FreeType result = new FreeType((byte)(_count-1));

			for ( i = 0; i < _count; i++)
			{
				if (X[i] == Y[i])
					result[i] = false;
				else result[i] = true;
			}

			return result; 
		}

		/// <summary>
		/// Преоброзование с 6 битового в 4 битовое значение с помощью узла замен
		/// </summary>
		/// <param name="X_6">6 битовое</param>
		/// <param name="X_4">новое 4 битовое значение</param>
		/// <returns></returns>
		public void UzelChanging(FreeType[] X_6, FreeType[] X_4)
		{
			for (i = 0; i < 8; i++)
			{
				m_2[0] = X_6[i][0];
				m_2[1] = X_6[i][5];

				m_4[0] = X_6[i][1];
				m_4[1] = X_6[i][2];
				m_4[2] = X_6[i][3];
				m_4[3] = X_6[i][4];

				X_4[i].Value = ResourceData.Snew[(m_2.Value), (m_4.Value) + i*16];
			}
		}

		public FreeType CryptFunction(FreeType X_32, FreeType k_48)
		{
			m_CF_T_48 = X_32.Transfer(ResourceData.E); // STEP 1

			m_CF_H_48 = XOR(m_CF_T_48, k_48, 48); // STEP 2

			count = 0; ii = 0;
			while (count < 48)
			{
				for (i = 0; i < 6; i++,count++)
					m_CF_list_h[ii][i] = m_CF_H_48[count];

				ii++;
			}

			UzelChanging( m_CF_list_h, m_CF_list_t); // STEP 3

			count = 0;ii = 0;
			while (count < 32)// STEP 4
			{
				for (i = 3; i >= 0; i--, count++)
					m_CF_T_32[count] = m_CF_list_t[ii][i];

				ii++;
			}

			return m_CF_T_32.Transfer(ResourceData.P); // STEP 5
		}

		public void CreateKeys(FreeType key_64)
		{
			FreeType key_56 = new FreeType(55);
			FreeType key_48 = new FreeType(47);

			FreeType key_28_low = new FreeType(27);
			FreeType key_28_high = new FreeType(27);

			// Step 1,2
			key_28_low = key_64.Transfer(ResourceData.C0);
			key_28_high = key_64.Transfer(ResourceData.D0);

			i = 0;
			while (i < 16)
			{
				// Step 3
				key_28_high = key_28_high << ResourceData.R[i];
				key_28_low = key_28_low << ResourceData.R[i];

				// Step 4
				for (ii = 0; ii < 28; ii++)
				{
					key_56[ii] = key_28_low[ii];
					key_56[ii + 28] = key_28_high[ii];
				}

				m_key_48[i] = key_56.Transfer(ResourceData.PC2);

				i++;
			}
		}


		public FreeType Crypt(FreeType _block_64)
		{
			// step 1
			m_T1 = _block_64.Transfer(ResourceData.IP);

			// step 2
			for (i = 0; i < 32; i++)
			{
				m_L32[i] = m_T1[i];
				m_H32[i] = m_T1[i + 32];
			}

			int _i = 0;
			while (_i < 16)
			{
				// step 3
				m_H32prev = m_H32.Clone();

				m_H32 = XOR(m_L32, CryptFunction(m_H32prev, m_key_48[_i]), 32);
				m_L32 = m_H32prev;

				_i++;
			}

			// step 4
			for (i = 0; i < 32; i++)
			{
				m_T2[i] = m_H32[i];
				m_T2[i+32] = m_L32[i];
			}
			
			// step 5
			return m_T2.Transfer(ResourceData._IP);
		}

		public void DeCryptKeys()
		{
			FreeType[] DeKey_48 = new FreeType[16];

			for (i = 0; i < 16; i++)
				DeKey_48[i] = new FreeType(47);

			for (i = 0, ii = 15; (i < 16 && ii >= 0); i++,ii--)
				DeKey_48[i].Value = m_key_48[ii].Value;
			
			m_key_48 = DeKey_48;
		}
	}
}