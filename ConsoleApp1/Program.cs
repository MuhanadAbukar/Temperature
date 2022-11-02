using DBLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading;
namespace HarvesterTemp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var DBL = new DBL();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://api.met.no/weatherapi/nowcast/2.0/complete?lat=59.218633&lon=10.942062");
            httpWebRequest.UserAgent = "Muhanad";
            var httpResponse = httpWebRequest.GetResponse();
            var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();
            var real = JsonSerializer.Deserialize<WeatherData.WeatherData>(result);
            var properties = real.properties.timeseries[0].data.instant.details;
            DBL.writeTempWithDate(properties.air_temperature, properties.wind_speed, properties.precipitation_rate, properties.relative_humidity, properties.wind_speed_of_gust);
            Console.WriteLine(properties.air_temperature);
            Console.WriteLine(properties.wind_speed);
            Thread.Sleep(0);

        }
    }
}
