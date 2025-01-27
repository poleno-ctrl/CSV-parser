using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AustralianPostOffice
{
	/// <summary>
	/// Статический класс с расширениями для массивов типа PostOfficep[]
	/// </summary>
	internal static class ArrayStatisticExtension
	{
		/// <summary>
		/// Метод расширение, фильтрующий офисы по штату
		/// </summary>
		/// <param name="offices">Массив офисов</param>
		/// <param name="stateName">Название штата</param>
		/// <returns>массив с офисам из штата stateName</returns>
		public static PostOffice[] OfficeFromState(this PostOffice[] offices, string stateName)
		{
			return Array.FindAll(offices, x => x.stateName == stateName);
		}
		/// <summary>
		/// Метод расширение, считающий количество офисов с разными почтовыми индексами для всех штатов
		/// </summary>
		/// <param name="offices">Массив офисов</param>
		/// <returns>Строка с названиями штатов и количеством офисов с разными почтовыми индексами для каждого штата</returns>
		public static string OfficeCountDifIndex(this PostOffice[] offices)
		{
			Array.Sort(offices, (a, b) => (a.stateName, a.postCode).CompareTo((b.stateName, b.postCode))); // сортируем по штатам, а внутри по почтовому индексу
			StringBuilder ans = new StringBuilder();
			int cur = 1;
			for (int i = 1; i < offices.Length; i++)
			{
				if (offices[i - 1].stateName != offices[i].stateName) // если предыдущий офис из другого штата, значит больше офисов из того штата не будет
				{
					ans.Append($"{offices[i - 1].stateName} : {cur}\n");
					cur = 1;
				}
				else if (offices[i - 1].postCode != offices[i].postCode) // если предыдущий офис из того же штата, но с другим индексом, значит мы нашли новый уникальный индекс
				{
					cur++;
				}
			}
			ans.Append($"{offices[^1].stateName} : {cur}\n"); // добавляем информацию по последнемиу штату
			return ans.ToString();
		}
		/// <summary>
		/// Метод расширение, считающий процент, который составляют почтовые офисы в конкретном месте от общего количества офисов в штате
		/// </summary>
		/// <param name="offices">Массив офисов</param>
		/// <param name="placeName">Название места</param>
		/// <returns>Строка с процентом в формате x%</returns>
		public static string OfficeProcent(this PostOffice[] offices, string placeName)
		{
			if (Array.Find(offices, x => x.placeName == placeName) == null) // проверяем, что в этом месте вообще существуют офисы
			{
				return "0%";
			}
			string stateName = Array.Find(offices, x => x.placeName == placeName).stateName; // находим название штата, в котором находится место
			double inState = 0, inPlace = 0;
			foreach (var item in offices)
			{
				if (item.stateName == stateName) // считаем количество офисов в штате
				{
					inState++;
				}
				if (item.placeName == placeName) // считаем количество офисов в месте
				{
					inPlace++;
				}
			}
			double part = inPlace / inState; // считаем долю офисов в месте от количества офисов в штате
			return $"{part * 100:f1}%";
		}
		/// <summary>
		/// Метод расширение, считающий среднее количество почтовых отделений в городах штата
		/// </summary>
		/// <param name="offices">Массив офисов</param>
		/// <param name="stateName">Название штата</param>
		/// <returns>Строку со средним количеством почтовых отделений в городах штата</returns>
		public static string OfficeAverage(this PostOffice[] offices, string stateName)
		{
			if (Array.Find(offices, x => x.stateName == stateName) == null) // проверяем, что в этом штате есть хоть одно отделение
			{
				return "0";
			}
			HashSet<string> places = new HashSet<string>(); // храним здесь названия городов без повторений
			double inState = 0; 
			foreach (var item in offices)
			{
				if (item.stateName == stateName)
				{
					inState++; // считаем количество отделений в штате
					places.Add(item.placeName); // добавляем название города
				}
			}
			return $"{inState / places.Count:f1}"; // возвращаем среднее
		}
		/// <summary>
		/// Метод расширение, считающий количество почтовых офисов в штате
		/// </summary>
		/// <param name="offices">Массив офисов</param>
		/// <returns>Строку с названием штата и количеством офисов в нем</returns>
		public static string OfficeCount(this PostOffice[] offices)
		{
			Dictionary<string, int> statesData = new Dictionary<string, int>(); // храним словарь с парами (название штата : количество офисов в штате)
			foreach (var item in offices)
			{
				if (!statesData.ContainsKey(item.stateName)) // проверяем наличие ключа
				{
					statesData.Add(item.stateName, 0);
				}
				statesData[item.stateName]++;
			}
			StringBuilder ans = new StringBuilder();
			foreach (var office in statesData)
			{
				ans.Append($"{office.Key} : {office.Value}\n"); // добавляем в строку названия штата и количество офисов в этом штате
			}
			return ans.ToString();
		}
		/// <summary>
		/// Метод расширение, возвращающий массив офисов, сгруппированных по коду штата и внутри группы отсортированных по названию места их нахождения
		/// </summary>
		/// <param name="offices">Массив офисов</param>
		/// <returns>Массив офисов, отсортированных по правилу</returns>
		public static PostOffice[] OfficeSorted(this PostOffice[] offices)
		{
			Array.Sort(offices, (a, b) => (a.stateCode, a.placeName).CompareTo((b.stateCode, b.placeName))); // отсортируем сначала по state_code, потом по place_name. Из того что мы отсортируем по state_code следует, что мы сгруппировали по state_code
			return offices;
		}
		/// <summary>
		/// Метод расширение, возвращающий все офисы с одинаковыми координатыми, если их больше трех
		/// </summary>
		/// <param name="offices">Массив офисов</param>
		/// <returns>Массив офисов с одинаковыми координатыми, если их больше трех</returns>
		public static PostOffice[] OfficeSameCord(this PostOffice[] offices)
		{
			Array.Sort(offices, (a, b) => (a.latitude, a.longitude).CompareTo((b.latitude, b.longitude))); // группируем(сортируем) по координатам
			int cur = 1;
			List<PostOffice> good = new List<PostOffice>();
			for (int i = 1; i < offices.Length; i++)
			{
				double dif = double.Abs(offices[i].latitude - offices[i - 1].latitude) + double.Abs(offices[i].longitude - offices[i - 1].longitude); // считаем суммарную разницу между координатами у соседних элементов
				if (dif < 0.000001) // если разница не превышает погрешность, значит координаты одинаковые (разницу списываем на неточность double)
				{
					cur++;
				}
				else // если координаты разные, значит координаты как в предыдущем элементе больше не встретятся
				{
					if (cur >= 3)
					{
						for (int j = i - cur; j < i; j++) // проходимся по всей группе с координатами как у предыдущего и добавляем ее к нашим выходным данным
						{
							good.Add(offices[j]);
						}
					}
					cur = 1; // обнуляем счетчик
				}
			}
			if (cur >= 3)
			{
				for (int j = offices.Length - cur; j < offices.Length; j++) // проходимся по оставшейся группе с координатами и добавляем ее к нашим выходным данным
				{
					good.Add(offices[j]);
				}
			}
			cur = 1;
			PostOffice[] ans = good.ToArray();
			return ans;
		}
	}
}
