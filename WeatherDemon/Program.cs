namespace WeatherDemon
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip;
            string port;
            string APIKEY;
            if (args.Length >= 1)
                port = args[0];
            else
                port = "8080";
            if (args.Length >= 2)
                ip = args[1];
            else
                ip = "127.0.0.1";

            if (args.Length >= 3)
                APIKEY = args[2];
            else
                APIKEY = "62296672f275766c561de09e0538ab29";
            
            // Start the server  
            HttpHelper.StartServer(ip, port, APIKEY);

            //Environment.SetEnvironmentVariable[]
            HttpHelper.Listen(); // Start listening.  
        }
    }
}
