using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustralianPostOffice
{
	/// <summary>
	/// Класс с расширениями для парсинга массивов типа PostOffice[]
	/// </summary>
	internal static class ArrayParseExtension
	{
		/// <summary>
		/// Метод расширение для парсинга массива в строку для вывода в консоль, если массив пустой, возвращаем
		/// </summary>
		/// <param name="offices">Массив</param>
		/// <returns>строка для вывода в консоль</returns>
		public static string ToConsoleString(this PostOffice[] offices)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var office in offices)
			{
				if (office.isCorrect) // в консоль не выводятся строки с некорректными или пустыми полями
				{
					sb.Append(office.ToString());
				}
				sb.Append('\n');
			}
			return sb.ToString();
		}
		/// <summary>
		/// Метод расширение для парсинга массива в строку для вывода в файл
		/// </summary>
		/// <param name="offices">Массив</param>
		/// <returns>строка для вывода в файл</returns>
		public static string ToFileString(this PostOffice[] offices)
		{
			StringBuilder sb = new StringBuilder("postcode,place_name,state_name,state_code,latitude,longitude,accuracy\n"); // в начале файла идет строка с названиями столбцов
			foreach (var office in offices)
			{
				sb.Append(office.ToString());
				sb.Append('\n');
			}
			return sb.ToString();
		}
	}
}
