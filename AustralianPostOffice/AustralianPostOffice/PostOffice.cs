using System.Text;

namespace AustralianPostOffice
{
	/// <summary>
	/// Класс почтовых офисов
	/// </summary>
	internal class PostOffice
	{
		/// <summary>
		/// строковое представление записи о почтовом офисе
		/// </summary>
		public readonly string stringFormat = "";
		/// <summary>
		/// коректность записи о почтовом офисе
		/// </summary>
		public readonly bool isCorrect = true;
		/// <summary>
		/// код почтового офиса
		/// </summary>
		public readonly int postCode = -1000000000;
		/// <summary>
		/// название места
		/// </summary>
		public readonly string placeName = "";
		/// <summary>
		/// название штата
		/// </summary>
		public readonly string stateName = "";
		/// <summary>
		/// код штата
		/// </summary>
		public readonly string stateCode = "";
		/// <summary>
		/// широта
		/// </summary>
		public readonly double latitude = -1000000000;
		/// <summary>
		/// долгота
		/// </summary>
		public readonly double longitude = -1000000000;
		/// <summary>
		/// точность
		/// </summary>
		public readonly int accuracy = -1000000000;
		/// <summary>
		/// Конструктор без парраметров
		/// </summary>
		public PostOffice() { }
		/// <summary>
		/// Конструктор, принимающий массив строк с информацией о полях. Если какие-то данные некорректны, то записываем в поле значение по умолчанию.
		/// </summary>
		/// <param name="data">Массив строк с полями</param>
		/// <param name="line">Строковое представление почтового офиса</param>
		private PostOffice(string[] data, string line)
		{
			stringFormat = line;
			if (!int.TryParse(data[0], out postCode)) // проверяем, корректны ли данные, иначе записываем значение по умолчанию
			{
				isCorrect = false;
			}
			placeName = data[1];
			stateName = data[2];
			stateCode = data[3];
			if (data[1] == "")
			{
				isCorrect = false;
			}
			if (data[2] == "")
			{
				isCorrect = false;
			}
			if (data[3] == "")
			{
				isCorrect = false;
			}
			if (!double.TryParse(data[4], out latitude))
			{
				isCorrect = false;
			}
			if (!double.TryParse(data[5], out longitude))
			{
				isCorrect = false;
			}
			if (!int.TryParse(data[6], out accuracy))
			{
				isCorrect = false;
			}
		}
		/// <summary>
		/// Метод для преобразования строки с информацией о полях через запятую в объект класса PostOffice
		/// </summary>
		/// <param name="line">Строка с полями</param>
		/// <param name="office">Ссылка на объект, в который хотим записать данные</param>
		/// <returns>True, если удалось успешно записать данные, false в обратном случае</returns>
		public static bool TryParse(string line, out PostOffice office)
		{
			bool isOpen = false; // открыты ли кавычки
			StringBuilder cur = new StringBuilder(); // текущее поле
			List<string> futureData = new List<string>(); // список полей
			for (int i = 0; i < line.Length; i++)
			{
				if (line[i] == '"') // если появилась кавычка, переключаем isOpen
				{
					isOpen ^= true;
				}
				else if (line[i] == ',' && isOpen) // если запятая внутри кавычек, то игнорируем
				{
					cur.Append(line[i]);
				}
				else if (line[i] == ',' && !isOpen) // если запятая не внутри кавычек, значит поле закончилось
				{
					futureData.Add(cur.ToString().Trim(' ')); // добавляем поле в массив полей, удалив лишние пробелы по краям
					cur.Clear();
				}
				else // если это обычный элемент, то просто добавляем его в поле
				{
					cur.Append(line[i]);
				}
			}
			futureData.Add(cur.ToString().Trim(' ')); // добавляем последнее поле, после которого нет запятой
			if (futureData.Count != 7) // проверяем, что в строке ровно столько полей, сколько полей у объекта класса PostOffice
			{
				office = null;
				return false;
			}
			string[] data = futureData.ToArray();
			office = new PostOffice(data, line);
			return true;
		}
		/// <summary>
		/// Возвращает строковый формат записи о почтовом отделении
		/// </summary>
		/// <returns></returns>
		public override string ToString() 
		{
			return stringFormat;
		}
	}
}
