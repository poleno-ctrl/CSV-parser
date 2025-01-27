// Тищенко Никита Алексеевич БПИ-247 Вариант 14

using System.Drawing;
using System.Globalization;
using System.IO.Enumeration;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AustralianPostOffice
{
	/// <summary>
	/// Класс с точкой входа в программу, отвечающий за настройки программы
	/// </summary>
	internal class Programm
	{
		/// <summary>
		/// Метод с точкой входа в программу. Перезапускает цикл, пока пользователь не решит остановиться.
		/// </summary>
		static void Main()
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US"); // меняем локализацию на английскую
			Menu menu = new Menu(); // создем меню

			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Все csv файлы для работы программы должны лежать в папке CSVFiles в корневой папке программы"); // выводим справку о работе с файловой системой
			Console.WriteLine("Все файлы сохраняются в эту же папку. Пользователь должен вводить только имя файла, но не путь к нему.");
			Console.WriteLine("Если в файле нет ничего кроме заголовка, то он будет считаться некорректным.");
			Console.WriteLine("Если файл открыт другой программой работа с файлом может быть невозможна.");
			Console.WriteLine("Если структура файла нарушена, то он будет считаться некорректным\n");
			Console.ForegroundColor = ConsoleColor.Gray;

			while (menu.StartMenu()) 
			{
				Console.WriteLine(); // разделяем каждую итерацию меню пустой строкой для читаемости
			}
		}
	}
}
