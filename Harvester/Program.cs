using DBLayer;
using System;
using System.Threading;
namespace HarvesterTemp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var DBL = new DBL();
            var discord = new WebhookPost();
            var tr = false;
            while (true)
            {
                if (!tr && DateTime.Now.Minute == 0)
                {
                    tr = true;
                    var properties = DBL.GetPropertiesOfWeather();
                    if (properties.air_temperature != 99999)
                    {
                        DBL.InsertTemperatureWithDate(properties.air_temperature, properties.wind_speed, properties.precipitation_rate, properties.relative_humidity, properties.wind_speed_of_gust, properties.wind_from_direction);
                        Console.WriteLine("Written current temps to DB");
                        discord.PostWebhook("Written current temps to DB <@568374878500159490>");
                        discord.PostWebhook($"Temp: {properties.air_temperature} \nWind_Speed: {properties.wind_speed} \nPrecipitation: {properties.precipitation_rate} \nHumidity: {properties.relative_humidity} \nWind_Speed_Gust: {properties.wind_speed_of_gust} \nWind_Direction: {properties.wind_from_direction}");
                        Thread.Sleep(60000);
                        tr = false;
                    }
                    else
                    {
                        Thread.Sleep(5000);
                        discord.PostWebhook("Trying to reconnect.. <@568374878500159490>");
                    }
                }
                else if (DateTime.Now.Minute > 55 && DateTime.Now.Minute < 60)
                {
                    discord.PostWebhook("Within 5 minutes now.");
                    Thread.Sleep(20000);
                }
                else
                {
                    discord.PostWebhook("Still checking...");
                    Thread.Sleep(120000);
                }
            }
        }
    }
}
