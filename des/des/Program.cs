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
			System.Console.WriteLine("�������� ���������� - DES, ���������� �� Visual Studio 2008(C#)");
			System.Console.WriteLine("� ������ ��������� �� black_@ukr.net ��� ICQ: 387872511");
			System.Console.WriteLine("---------------------------------------------------------------------");

			if (args.Length != 2)
			{
				System.Console.WriteLine("���������� ���������� ������ ���� = 2(��� �����, ����)");
				System.Console.ReadKey();
				return;
			}

			FileService.FileService FS = new des.FileService.FileService(args[0], args[1]);
			FS.Start();
		}
	}
}