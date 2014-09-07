using System;
using System.Collections.Generic;
using System.Text;

using Type;
using Core;
using des.FileService;
namespace des
{
	class Program
	{
		static void Main(string[] args)
		{
			System.Console.WriteLine("---------------------------------------------------------------------");
			System.Console.WriteLine("Алгоритм шифрования - DES, реализован на Visual Studio 2008(C#)");
			System.Console.WriteLine("К автору обращатся на black_@ukr.net или ICQ: 387872511");
			System.Console.WriteLine("---------------------------------------------------------------------");

			if (args.Length != 2)
			{
				System.Console.WriteLine("Количество аргуметнов должно быть = 2(имя файла, ключ)");
				System.Console.ReadKey();
				return;
			}

			FileService.FileService FS = new des.FileService.FileService(args[0], args[1]);
			FS.Start();
		}
	}
}