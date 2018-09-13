using System.Net;
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
        public GetWeatherData(string City)
        {
            city = City;
        }
        //Название города
        private string city;
        //Температура
        private float temp;
        //Максимальная температура
        private float tempMax;
        //Минимальная температура
        private float tempMin;
        /// <summary>
        /// Проверка погоды для данного города
        /// </summary>
        public void CheckWeather()
        {
            WeatherAPI DataAPI = new WeatherAPI(city);
            temp = DataAPI.GetTemp();
        }
        //Методы инициализаторы 
        public string City { get { return city; } set { city = value; } }
        public float Temp { get { return temp; } set { temp = value; } }
        public float TempMax { get { return tempMax; } set { tempMax = value; } }
        public float TempMin { get { return tempMin; } set { tempMin = value; } }

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
            public WeatherAPI(string city)
            {
                SetCurrentURL(city);
                xmlDocument = GetXML(currentURL);
            }
        /// <summary>
        /// Получение темпертутры
        /// </summary>
        /// <returns>Температура в формате float</returns>
            public float GetTemp()
            {
                XmlNode temp_node = xmlDocument.SelectSingleNode("//temperature");
                XmlAttribute temp_value = temp_node.Attributes["value"];
                string temp_string = temp_value.Value;
                return float.Parse(temp_string);
            }
        //Ключ WeatherAPI 
            private const string APIkey = "62296672f275766c561de09e0538ab29";
        //URL запроса к WeatherAPI
            private string currentURL;
        //XML ответа WeatherAPI
            private XmlDocument xmlDocument;
        /// <summary>
        /// Формирование URL запроса к WeatherAPI
        /// </summary>
        /// <param name="location">Город латиницей</param>
        private void SetCurrentURL(string location)
            {
                currentURL = $"http://api.openweathermap.org/data/2.5/weather?q={location}" + $"&mode=xml&units=metric&APPID={APIkey}";
            }
        /// <summary>
        /// Получение ответа по сформированному URL 
        /// </summary>
        /// <param name="CurrentURL">URL запрос к WeatherAPI</param>
        /// <returns></returns>
        private XmlDocument GetXML(string CurrentURL)
            {
                using (WebClient client = new WebClient())
                {
                    var xmlContent = client.DownloadString(CurrentURL);
                    var xmlDocumet = new XmlDocument();
                    xmlDocument.LoadXml(xmlContent);
                    return xmlDocument;
                }
            }
    }
}
