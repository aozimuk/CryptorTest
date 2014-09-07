using System;
using System.Collections.Generic;
using System.Text;

namespace Type
{
	public class FreeType
    {		
        private byte m_MaxCount = 0;
        private long m_Value = 0; // Поле целого типа       

		public FreeType(byte _MaxCount)
        {
			m_MaxCount = _MaxCount; 
        }

        public bool this[int i]
        { // Индексатор
            get 
            {
				return (m_Value & ((long)1 << i)) != 0 ; 
            }

            set
            {
				if (value)
                    m_Value |= ((long)1 << i);
                else
                    m_Value &= ~((long)1 << i);
            }
        }

		public void Init (long _value)
		{
			m_Value = _value;
		}

		public FreeType Clone()
		{
			FreeType clone = new FreeType(m_MaxCount);
			clone.Init(m_Value);
			return clone;
		}

		public void ConsoleOutHL()
		{
			for (int i = m_MaxCount; i >= 0; i--)
				Console.Write("{0}", this[i] == true ? 1 : 0);
			Console.WriteLine();
		}

		public void ConsoleOutLH()
		{
			for (int i = 0; i <= m_MaxCount; i++)
				Console.Write("{0}", this[i] == true ? 1 : 0);
			Console.WriteLine();
		}

		public static FreeType operator << (FreeType obj, int _count)
		{
			bool save;
			int i;

			FreeType clone = obj.Clone();

			while (_count > 0)
			{
				save = clone[clone.MaxCount];

				for (i = clone.MaxCount; i > 0; i--)
					clone[i] = clone[i - 1];

				clone[i] = save;

				_count--;
			}

			return clone;
		}

		public static FreeType operator >>( FreeType obj, int _count)
		{
			bool save;
			int i;

			FreeType clone = obj.Clone();

			while (_count > 0)
			{
				save = clone[0];
				for (i = 0; i < clone.MaxCount; i++)
					clone[i] = clone[i + 1];

				clone[i] = save;

				_count--;
			}

			return clone;
		}

		public FreeType Transfer(byte[] mass)
		{
			FreeType result = new FreeType((byte)(mass.Length-1));

			for (int i = 0; i < mass.Length; i++)
				result[i] = this[mass[i]];

			return result;
		}

		#region Accessor
		public byte MaxCount
		{
			get { return m_MaxCount; }
		}

		public long Value
		{
			get
			{ return m_Value; }
			set
			{ m_Value = value; }
		}
		#endregion
	}
}
