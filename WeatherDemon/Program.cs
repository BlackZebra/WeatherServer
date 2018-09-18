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
            string env = "LISTEN_PORT";

            Console.WriteLine(Environment.GetEnvironmentVariable(env,EnvironmentVariableTarget.Process));
            Environment.SetEnvironmentVariable(env,)
            // Start the server  
            HttpHelper.StartServer();

            //Environment.SetEnvironmentVariable[]
            HttpHelper.Listen(); // Start listening.  
        }
    }
}
