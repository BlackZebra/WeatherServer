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
            // Start the server  
            TcpHelper.StartServer(5678);
            TcpHelper.Listen(); // Start listening.  
        }
    }
}
