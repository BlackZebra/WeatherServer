using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDemon
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip;
            string port;
            string APIKEY;
            if (args[1] != null)
                ip = args[1];
            else
                ip = "0.0.0.0";
            if (args[0] != null)
                port = args[0];
            else
                port = "8080";
            APIKEY = args[2];
            // Start the server  
            HttpHelper.StartServer(ip, port);

            //Environment.SetEnvironmentVariable[]
            HttpHelper.Listen(); // Start listening.  
        }
    }
}
