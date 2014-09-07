using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Core;
using Type;
using System.Xml;
namespace des.FileService
{
	class FileService
	{
		private int i = 0, ii = 0, k = 0;

		private bool m_CanStart = true;
		private bool m_IsCrypt = true;
		private CoreService m_CryptCore = null;

		private FileStream m_FileR;//шифруемый файл
		private FileStream m_FileW;//зашифрованый файл

		private string m_FileName;
		private string m_Key;

		private long m_CountBlocks64 = 0;
		private int m_Tail = 0;//хвост

		private byte[] array;

		private FreeType m_block64;
		private FreeType m_byte8;

		public FileService(string _FileName, string _Key)
		{
			m_CryptCore = new CoreService();
			m_FileName = _FileName;
			m_Key = _Key;
			m_block64 = new FreeType(63);
			m_byte8 = new FreeType(7);
		}

		public void Start()
		{
			m_CanStart = true;
			long start;

			if (!File.Exists(m_FileName)) { m_CanStart = false; System.Console.WriteLine("Файл не существует"); return; }
			
			m_FileR = File.OpenRead(m_FileName);

			m_CountBlocks64 = (long)(m_FileR.Length / 8);
			m_Tail = (int)(m_FileR.Length % 8);

			if (m_FileR.Name.Contains(".des"))
			{
				if (!File.Exists(m_FileName + ".xml"))
				{
					System.Console.WriteLine("Не найден файл: " + m_FileName + ".xml");
					System.Console.ReadKey();
					return;
				}

				m_IsCrypt = false;

				start = DateTime.Now.Ticks;

				StartDeCrypt();
				if (m_CanStart)
				{
					System.Console.WriteLine("Файл разшифрован успешно");
					System.Console.WriteLine("Время : {0}сек", (DateTime.Now.Ticks - start) / 10000000);
					System.Console.ReadKey();
				}

				return;
			}

			start = DateTime.Now.Ticks;

			CreateKey();

			if (!m_CanStart)
				return;

			try
			{
				m_FileW = File.Open(m_FileR.Name + ".des", FileMode.CreateNew);
			}
			catch (Exception ex)
			{
				m_CanStart = false;
				System.Console.WriteLine("Файл " + m_FileR.Name + ".des уже существует");
				System.Console.ReadKey();
				return;
			}

			StartCrypt();

			CreateXMLFile();

			System.Console.WriteLine("Файл зашифрован успешно");
			System.Console.WriteLine("Время : {0}сек", (DateTime.Now.Ticks - start) / 10000000);
			System.Console.ReadKey();
			
	}

		private void CreateXMLFile()
		{
			XmlDocument xml = new XmlDocument();
			XmlNode node = xml.CreateElement(m_FileName+".des");

			XmlAttribute attr = xml.CreateAttribute("tail");
			attr.Value = m_Tail.ToString();
			
			node.Attributes.Append(attr);
			xml.AppendChild(node);
			
			xml.Save(m_FileR.Name + ".des.xml");
		}

		private void CreateKey()
		{
			FreeType key = new FreeType(63);

			try
			{
				key.Value = long.Parse(m_Key);
			}
			catch (Exception ex)
			{
				System.Console.WriteLine("Ошибка : Наверное слишком длинный ключ, или в ключе присутствуют символы");

				m_CanStart = false;

				System.Console.ReadKey();
				return;
			}

			if (key.Value == 0)
			{
				System.Console.WriteLine("Ошибка ключа. Ключ должен содержать только цыфры и не быть пустым(а также =0)");
				System.Console.ReadKey();
				return;
			}

			m_CryptCore.CreateKeys(key);
			if (!m_IsCrypt)
				m_CryptCore.DeCryptKeys();
		}

		public void StartCrypt()
		{
			int offset = 0, count = 8;
			array = new byte[count];

			for (int i = 0; i < m_CountBlocks64; i++)
			{
				m_FileR.Read(array, offset, count);
				FullBlockCrypt(array);
			}
			if (m_Tail != 0)
			{
				m_FileR.Read(array, offset, m_Tail);
				FullBlockCrypt(array);
			}
			m_FileW.Close();
		}

		public void StartDeCrypt()
		{
			CreateKey();

			if (!m_CanStart)
				return;

			try
			{
				m_FileW = File.Open(m_FileName.Remove((m_FileName.Length - 4), 4), FileMode.CreateNew);
			}
			catch (Exception ex)
			{
				m_CanStart = false;
				System.Console.WriteLine("Ошибка: Файл " + m_FileName.Remove((m_FileName.Length - 4), 4) + " уже существует");
				System.Console.ReadKey();
				return;
			}

			StartCrypt();

			//нужно убрать хвост
			XmlDocument xml = new XmlDocument();
			xml.Load(m_FileName + ".xml");

			XmlNode node = xml.SelectSingleNode(m_FileName);

			m_FileW = File.Open(m_FileW.Name, FileMode.Open);
			m_FileW.SetLength(m_FileW.Length - (8 - long.Parse(node.Attributes["tail"].Value)));

			m_FileW.Close();

		}

		private void FullBlockCrypt(byte[] arrray)
		{
			m_block64.Value = 0;
			i = k = 0;

			while ( k < array.Length )
			{
				m_byte8.Value = array[k];

				for (ii = 0; ii < 8 && i < 64; i++, ii++)
				{
					m_block64[i] = m_byte8[ii];
				}
				k++;
			}

			m_block64 = m_CryptCore.Crypt(m_block64);

			i = k = 0;
			while (k < 8)
			{
				for (ii = 0; ii < 8 && i < 64; i++, ii++)
				{
					m_byte8[ii] = m_block64[i];
				}

				array[k] = (byte)m_byte8.Value;

				k++;
			}

			m_FileW.Write(array, 0, 8);
		}
	}
}
