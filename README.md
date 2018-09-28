# WeatherServer
Simple service that provides information about weather.

## Trouble 
Right now this poject not compile on Unix system. This will be fixed in future.
And .NET CORE not working with enviromental environmental variable. 
IP, PORT, APIKEY is passed through parameters when the application is started.
like this 
mono bin/Debug/WeatherDemon.exe 8080 127.0.0.1 APIKEY

## Run
mono bin/Debug/WeatherDemon.exe <port> <ip> <APIKEY>

## Request 
GET /v1/forecast/?city=<city>&dt=<timestamp> - returns weather for UNIX timestamp.
GET /v1/forecast/?city=<city> - returns current weather.

## Response example
{
    "city": "moscow",
    "unit": "celsius",
    "temperature": 13.83
}
