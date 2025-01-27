using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustralianPostOffice
{
	/// <summary>
	/// Класс для взаимодействия с пользователем
	/// </summary>
	internal class Menu
	{
		/// <summary>
		/// Массив с данными об отделениях почты
		/// </summary>
		private PostOffice[] offices = null;
		/// <summary>
		/// Метод, предлагающий пользователю меню с действиями
		/// </summary>
		/// <returns>False если пользователь хочет прекратить работу и true в обратном случае</returns>
		public bool StartMenu()
		{
			Console.WriteLine("Введите номер пункта меню для запуска действия:");
			Console.WriteLine((offices == null ? "1. Загрузить данные из файла" : "1. Забыть старые данные и загрузить новые данные из файла"));
			Console.WriteLine("2. Вывести почтовые отделения в Новом Южном Уэллсе");
			Console.WriteLine("3. Сохранить в фиксированный файл выборку об отделениях из введенного штата");
			Console.WriteLine("4. Вывести статистику");
			Console.WriteLine("5. Завершить работу");
			Console.WriteLine("6. Отсортировать, вывести и записать в файл данные о почтовых отделениях");
			Console.WriteLine("7. Найти все офисы, находящиеся на одной широте и долготе, количество которых больше 3, и записать в файл");

			string num = Console.ReadLine();

			if (num != "1" && num != "5" && offices == null)  // для всех действий кроме 1 и 5 нужны входные данные, если таковых нет, то сообщаем об этом пользователю
			{
				Console.WriteLine("Некорректный ввод или данных для этого действия не существует.");
				return true;
			}

			switch (num)
			{
				case "1":
					ReadFile();
					break;
				case "2":
					GetDataWales();
					break;
				case "3":
					GetDataState();
					break;
				case "4":
					Console.WriteLine("Введите номер пункта меню для запуска действия:");
					Console.WriteLine("1. Количество почтовых отделений в каждом штате с разным почтовым индексом");
					Console.WriteLine("2. Процентное отношение почтовых отделений в конкретном месте от общего числа почтовых отделений в штате");
					Console.WriteLine("3. Среднее количество почтовых отделений в городах штата");
					Console.WriteLine("4. Статистику по количеству почтовых отделений в каждом штате");
					string num1 = Console.ReadLine();
					switch (num1)
					{
						case "1":
							GetDif();
							break;
						case "2":
							GetProc();
							break;
						case "3":
							GetAverage();
							break;
						case "4":
							GetCount();
							break;
						default:
							Console.WriteLine("Некорректный ввод");
							return true;
					}
					break;
				case "5": // пользователь хочет завершить работу
					return false; 
				case "6":
					GetSorted();
					break;
				case "7":
					GetSameCord();
					break;
				default:
					Console.WriteLine("Некорректный ввод");
					break;
			}
			return true;
		}
		/// <summary>
		/// Метод обработки запроса пользователя на считывание файла
		/// </summary>
		private void ReadFile()
		{
			Console.Write("Введите название файла с расширением: ");
			string name = Console.ReadLine();
			CsvFile f = new CsvFile(name);
			if (f.ReadFile(ref offices)) // пытаемся прочитать файл
			{
				Console.WriteLine("Успешное чтение файла");
			}
			else
			{
				Console.WriteLine("Проблемы с открытием файла");
			}
		}
		/// <summary>
		/// Метод обработки запроса пользователя на получение информации об отделениях в Новом Южном Уэльсе
		/// </summary>
		private void GetDataWales()
		{
			PostOffice[] good = offices.OfficeFromState("New South Wales");
			if (good.Length == 0)
			{
				Console.WriteLine("Таких почтовых отделений нет");
			}
			else
			{
				Console.WriteLine(good.ToConsoleString());
			}
		}
		/// <summary>
		/// Метод обработки запроса пользователя на получение информации об отделениях в штате
		/// </summary>
		private void GetDataState()
		{
			Console.Write("Введите название штата, данные из которого нужно вывести: ");
			string name = Console.ReadLine();
			CsvFile f = new CsvFile("NS-Wales-postcodes.csv");
			PostOffice[] good = offices.OfficeFromState(name);
			if (good.Length == 0)
			{
				Console.WriteLine("Таких почтовых отделений нет");
			}
			else
			{
				f.WriteFile(good);
				Console.WriteLine("Данные успешно записаны");
			}
		}
		/// <summary>
		/// Метод обработки запроса пользователя на получение информации о количестве отделений с разными почтовыми индексами для каждого штата
		/// </summary>
		private void GetDif()
		{
			Console.WriteLine(offices.OfficeCountDifIndex());
		}
		/// <summary>
		/// Метод обработки запроса пользователя на получение информации о проценте, который составляют отделения в конкретном месте от отделений во всем штате
		/// </summary>
		private void GetProc()
		{
			Console.Write("Введите название места: ");
			string name = Console.ReadLine();
			Console.WriteLine($"Офисы в этом месте от общего количества офисов в штате составляют: {offices.OfficeProcent(name)}");
		}
		/// <summary>
		/// Метод обработки запроса пользователя на получение информации о среднем количестве отделений в городе штата
		/// </summary>
		private void GetAverage()
		{
			Console.Write("Введите название штата: ");
			string name = Console.ReadLine();
			Console.WriteLine($"Среднее количество почтовых отделений в городе штата: {offices.OfficeAverage(name)}");
		}
		/// <summary>
		/// Метод обработки запроса пользователя на получение информации о количестве отделений в штате
		/// </summary>
		private void GetCount()
		{
			Console.WriteLine(offices.OfficeCount());
		}
		/// <summary>
		/// Метод обработки запроса пользователя на получение списка всех отделений, сгрупированных по коду штата и отсортированных по названию их местанахождения
		/// </summary>
		private void GetSorted()
		{
			PostOffice[] good = offices.OfficeSorted(); // получаем отсортированную выборку
			CsvFile f = new CsvFile("grouped-postcodes.csv");
			f.WriteFile(good);
			Console.WriteLine(good.ToConsoleString());
		}
		/// <summary>
		/// Метод обработки запроса пользователя на получение списка всех отделений, сгрупированного по координатам. Если на координатах меньше трех отделений, то 
		/// эти отделения не войдут в список.
		/// </summary>
		private void GetSameCord()
		{
			Console.WriteLine("Введите название файла:");
			string name = Console.ReadLine();
			PostOffice[] good = offices.OfficeSameCord();
			CsvFile f = new CsvFile(name);
			if (good.Length == 0) // если таких офисов не существует, сообщаем об этом пользователю
			{
				Console.WriteLine("Таких почтовых отделений нет");
			}
			else if (f.WriteFile(good)) // пытаемся записать в файл
			{
				Console.WriteLine(good.ToConsoleString());
			}
			else
			{
				Console.WriteLine("Проблемы с открытием файла");
			}
		}
	}
}
