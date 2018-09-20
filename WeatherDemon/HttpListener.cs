using System;
using System.Net;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace WeatherDemon
{
    /// <summary>
    /// Класс сервер HTTP 
    /// </summary>
    class HttpHelper
    {
        private static HttpListener listener { get; set; }
        private static bool accept { get; set; } = false;
        private static string APIkey;

        /// <summary>
        /// Запуск сервера
        /// </summary>
        public static void StartServer(string ip, string port, string APIKEY)
        {
            APIkey = APIKEY;
            //IPAddress address = IPAddress.Parse("127.0.0.1");
            listener = new HttpListener();
            listener.Prefixes.Add($"http://{ip}:{port}/v1/forecast/");
            listener.Prefixes.Add($"http://{ip}:{port}/v1/current/");

            listener.Start();
            accept = true;
            Console.WriteLine($"Server started. Listening to HTTP ..");
        }
        /// <summary>
        /// Прослцшивание HTTP
        /// </summary>
        public static void Listen()
        {
            if (listener != null && accept)
            {
                while (true)
                {
                    Console.WriteLine("Waiting GET request from users..");
                    HttpListenerContext Context = listener.GetContext();
                    HttpListenerRequest Request = Context.Request;
                    HttpListenerResponse Response = Context.Response;
                    //TODO Добавить проверки на запрос
                    //Проверка на содержимое запроса - параметры
                    int iqs = Request.RawUrl.IndexOf('?');
                    if (iqs < 0)
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes( "Error: 204 " + HttpStatusCode.NoContent.ToString());
                        Response.OutputStream.Write(buffer, 0, buffer.Length);
                        Response.Close();
                        Console.WriteLine("No client data was sent with the request.");
                    }
                    else
                    {
                        Console.WriteLine($"New GET request from {Request.RemoteEndPoint.Address}:{Request.RemoteEndPoint.Port}");
                        //Переменная флаг
                        //Если значение 0 - то запрос на прогноз
                        //Если значение 1 - то запрос на текущую погоду
                        int type = -1;
                        type = Request.RawUrl.Contains("forecast") ? 0 : 1 ;
                        string querystring = (iqs < Request.RawUrl.Length) ?Request.RawUrl.Substring(iqs+1): String.Empty;
                        NameValueCollection collectionUrl = HttpUtility.ParseQueryString(querystring);

                        string returnvalue ="";
                        switch (type)
                        {
                            case 0:
                                {
                                    returnvalue = DataExchangerForecast(collectionUrl);
                                    break;
                                }
                            case 1:
                                {
                                    returnvalue = DataExchangerCurrent(collectionUrl);
                                    break;
                                }
                            default:
                                break;
                        }
                        
                        byte[] buffer = Encoding.UTF8.GetBytes(returnvalue);
                        Response.OutputStream.Write(buffer, 0, returnvalue.Length);
                        Response.Close();
                    }
                }
            }
        }
        /// <summary>
        /// Функция, передающая данные в класс GetWeatherAPI
        /// </summary>
        /// <param name="data">Коллекция данных из запроса</param>
        /// <returns>Строка, готовая для отправки пользователю</returns>
        public static string DataExchangerCurrent (NameValueCollection data)
        {
            GetWeatherData weather = new GetWeatherData(data.GetValues("city")[0], APIkey);
            weather.CheckWeather();
            return weather.GetRequestedData();
        }
        /// <summary>
        /// Функция, передающая данные в класс GetWeatherAPI
        /// </summary>
        /// <param name="data">Коллекция данных из запроса</param>
        /// <returns>Строка, готовая для отправки пользователю</returns>
        public static string DataExchangerForecast(NameValueCollection data)
        {
            GetWeatherData weather = new GetWeatherData(data.GetValues("city")[0], UnixTimeStampToDateTime(Convert.ToDouble(data.GetValues("dt")[0])), APIkey);
            weather.CheckWeather();
            return weather.GetRequestedData();
        }
        /// <summary>
        /// Перевод timeStamp в привычное DateTime
        /// </summary>
        /// <param name="unixTimeStamp">Unix timestamp</param>
        /// <returns>DateTime из TimeStamp</returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }

    
}
