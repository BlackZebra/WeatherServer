using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Xml;

namespace WeatherDemon
{
    /// <summary>
    /// Класс погодных данных
    /// </summary>
    class GetWeatherData
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="City">Город, написанный латиницей</param>
        public GetWeatherData(string City, string APIKEY)
        {
            city = City;
            type = "weather";
            dt = DateTime.Now;
            API_KEY = APIKEY;
        }
        public GetWeatherData(string City, DateTime DT, string APIKEY)
        {
            city = City;
            dt = DT;
            type = "forecast";
            API_KEY = APIKEY;
        }
        //Название города
        private string city;
        //Температура
        private float temp;
        //Единица измерения температуры
        private string unit;
        //Время дата для прогноза
        private DateTime dt;
        //Тип запроса
        private string type;
        //Ключ API
        private string API_KEY;
        /// <summary>
        /// Проверка погоды для данного города
        /// </summary>
        public void CheckWeather()
        {
            WeatherAPI DataAPI = new WeatherAPI(city,type,dt, API_KEY);
            temp = DataAPI.GetTemp(type);
            unit = DataAPI.GetUnit();
        }
        /// <summary>
        /// Универсальный вывод
        /// </summary>
        /// <returns>Возвращает строку в удобном формате</returns>
        public string GetRequestedData ()
        {
            return ($"{{\r\t\"city\":\"{city}\"\r\t\"unit\":\"{unit}\"\r\t\"temperature\":\"{temp}\"\r}}");
        }
        //Методы инициализаторы 
        public string City { get { return city; } set { city = value; } }
        public float Temp { get { return temp; } set { temp = value; } }
        public string Unit { get { return unit; } set { unit = value; } }
        public DateTime DT { get { return dt; } set { dt = value; } }
        public string Type { get { return type; } set { type = value; } }

    }
    /// <summary>
    /// Класс получения данных с WeatherAPI
    /// </summary>
    class WeatherAPI
    {
        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        /// <param name="city">Город написанный латиницей</param>
        public WeatherAPI(string city, string type, DateTime DT, string APIKEY)
        {
            dt = DT;
            SetCurrentURL(city, type, APIKEY);
            xmlDocument = GetXML(currentURL);
        }
        /// <summary>
        /// Получение темпертутры
        /// </summary>
        /// <returns>Температура в формате float</returns>
        public float GetTemp(string type)
        {
            if (type == "weather")
            {
                var temp_node = xmlDocument.SelectSingleNode("//temperature");
                return Single.Parse(temp_node.Attributes["value"].Value, CultureInfo.InvariantCulture);
            }
            else
            {
                var temp_node = xmlDocument.SelectSingleNode("//forecast");
                var temp_node2 = temp_node.SelectNodes("//time");
                XmlNode temp_node3 = null;
                foreach (XmlNode c in temp_node2)
                {
                    //Console.WriteLine(c.Attributes["from"].Value);
                    //Console.WriteLine(c.Attributes["to"].Value);
                    //Console.WriteLine(c.SelectSingleNode("temperature").Attributes["value"].Value);
                    //Console.WriteLine(c.SelectSingleNode("temperature").Attributes["max"].Value);
                    //Console.WriteLine(c.SelectSingleNode("temperature").Attributes["min"].Value);
                    if ((Convert.ToDateTime(c.Attributes["from"].Value)<=dt) &&(Convert.ToDateTime(c.Attributes["to"].Value)>=dt))
                    {
                        temp_node3 = c;
                    }
                    
                }
                Console.WriteLine(dt);
                //Console.WriteLine(temp_node3.Attributes["from"].Value);
                //Console.WriteLine(temp_node3.Attributes["to"].Value);
                //Console.WriteLine(temp_node3.SelectSingleNode("temperature").Attributes["value"].Value);
                //Console.WriteLine(temp_node3.SelectSingleNode("temperature").Attributes["max"].Value);
                //Console.WriteLine(temp_node3.SelectSingleNode("temperature").Attributes["min"].Value);
                return Single.Parse(temp_node3.SelectSingleNode("temperature").Attributes["value"].Value, CultureInfo.InvariantCulture);
            }
        }
        /// <summary>
        /// Получение метрики
        /// </summary>
        /// <returns>Название метрики</returns>
        public string GetUnit()
        {
            return currentURL.Contains("metric")? "Celsius": "Fahrenheit";
        }
        ////Ключ WeatherAPI 
        //private string APIkey;
        //URL запроса к WeatherAPI
        private string currentURL;
        //XML ответа WeatherAPI
        private XmlDocument xmlDocument;
        //Дата и время для прогноза
        private DateTime dt;
        /// <summary>
        /// Формирование URL запроса к WeatherAPI
        /// </summary>
        /// <param name="location">Город латиницей</param>
        private void SetCurrentURL(string location, string param, string APIkey)
            {
                currentURL = $"http://api.openweathermap.org/data/2.5/{param}?q={location}" + $"&mode=xml&units=metric&APPID={APIkey}";
            }
        /// <summary>
        /// Получение ответа по сформированному URL 
        /// </summary>
        /// <param name="CurrentURL">URL запрос к WeatherAPI</param>
        /// <returns>XML документ с результатами</returns>
        private XmlDocument GetXML(string CurrentURL)
            {
                using (WebClient client = new WebClient())
                {
                    var xmlContent = client.DownloadString(CurrentURL);
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(xmlContent);
                    return xmlDocument;
                }
            }
    }
}
