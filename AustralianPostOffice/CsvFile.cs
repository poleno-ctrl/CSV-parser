using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AustralianPostOffice
{
	/// <summary>
	/// Класс csv файлов
	/// </summary>
	internal class CsvFile
	{
		/// <summary>
		/// путь до файла
		/// </summary>
		private string _path = "";
		/// <summary>
		/// Метод для того, чтобы записать записи об офисах из файла в массив
		/// </summary>
		/// <param name="offices">Ссылка на массив для записи данных</param>
		/// <returns>True, если данные успешно записаны, false в обратном случае</returns>
		public bool ReadFile(ref PostOffice[] offices)
		{
			string[] lines;
			try
			{
				lines = File.ReadAllLines(_path); // пытаемся прочесть файл
			}
			catch
			{
				return false;
			}
			if (lines.Length <= 1) // пустой файл или файл только с заголовками некорректен
			{
				return false;
			}
			PostOffice[] new_offices = new PostOffice[lines.Length - 1];
			for (int i = 1; i < lines.Length; i++)
			{
				if (!PostOffice.TryParse(lines[i], out new_offices[i - 1])) // пытаемся записать запись об офисе в массив
				{
					return false;
				}
			}
			offices = new_offices;
			return true;
		}
		/// <summary>
		/// Метод для записи в файл записей об офисах
		/// </summary>
		/// <param name="offices">Массив офисов</param>
		/// <returns>True, если данные успешно записаны, false в обратном случае</returns>
		public bool WriteFile(PostOffice[] offices)
		{
			string toWrite = offices.ToFileString(); // создаем массив строк для записи в файл
			try
			{
				File.WriteAllText(_path, toWrite); // пытаемся записать в файл
			}
			catch
			{
				return false;
			}
			return true;
		}
		/// <summary>
		/// Конструктор, принимающий название файла
		/// </summary>
		/// <param name="path">название файла</param>
		public CsvFile(string path)
		{
			if (path.Contains('/') || path.Contains(@"\"[0])) // если в названии встречаются слеши, то название некорректно - оставляем значение по умолчанию
			{
				return;
			}
			_path = "../../../../CSVFiles/" + path; // к названию добавляем путь к папке с файлами
		}
	}
}
