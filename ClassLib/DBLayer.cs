﻿using Newtonsoft.Json;
using SQLWeather;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using WeatherData;
namespace DBLayer
{
    public class WebhookPost
    {
        public Embed[] embeds { get; set; }
        public class Embed
        {
            public string title { get; set; }
            public string description { get; set; }
        }

        public void PostWebhook(string content)
        {
            var embed = new Embed()
            {
                title = "Weather Project Status",
                description = content
            };
            var info = new Embed[1];
            info[0] = embed;
            var js = "{\"embeds\":" + JsonConvert.SerializeObject(info) + "}";
            var _cl = new HttpClient();
            var pl = new StringContent(js, Encoding.UTF8, "application/json");
            Console.WriteLine(js);
            _cl.PostAsync("https://discord.com/api/webhooks/1037668127242203136/gg8ne3IzPN49PD9q6xNLkvv2ciKclffnMh2QJ37mkx02bjHkzP2TP8L38udQvXav5AhB", pl);
        }
    }
    public class DBL
    {
        static string connstr = "Data Source=itaserver;Initial Catalog=Harvester_Muhanad;Persist Security Info=True;User ID=muhanadharvester;Password=muhanad123";
        static SqlConnection conn = new SqlConnection(connstr);
        private WeatherFromSQL CreateObjectFromSql(SqlDataReader reader)
        {
            var wt = new WeatherFromSQL
            {
                Year = (int)reader["Year"],
                Month = (int)reader["Month"],
                Day = (int)reader["Day"],
                Hour = (int)reader["Hour"],
                Temperature = (double)reader["Temperature"],
                WindSpeed = (double)reader["WindSpeed"],
                Precipitation_Rate = (double)reader["Precipitation_Rate"],
                Precipitation_Amount = (double)reader["Precipitation_Amount"],
                Humidity = (double)reader["Humidity"],
                WindSpeedGust = (double)reader["WindSpeedGust"],
                WindDirection = (double)reader["WindDirection"],
                Sky = reader["Sky"].ToString()
            };
            return wt;
        }
        public List<WeatherFromSQL> GetAnyYear(int year)
        {

            var list = new List<WeatherFromSQL>();
            var z = new SqlCommand("select * from tempdatar where year = @year order by id desc", conn);
            z.Parameters.AddWithValue("year", year);
            conn.Open();
            var reader = z.ExecuteReader();
            while (reader.Read())
            {
                list.Add(CreateObjectFromSql(reader));
            }
            reader.Close();
            conn.Close();
            return list;

        }
        public Data GetPropertiesOfWeather()
        {

            try
            {//69.649570, 18.956684


                var http = new HttpClient();
                http.DefaultRequestHeaders.Add("User-Agent", "Muhanad");
                var str = http.GetAsync("https://api.met.no/weatherapi/nowcast/2.0/complete?lat=59.218633&lon=10.942062");
                var result = str.Result.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<WeatherData.WeatherData>(result);
                var properties = json.properties.timeseries[0].data;
                return properties;
            }
            catch (Exception m)
            {
                var d = new WebhookPost();
                d.PostWebhook(m.Message + " <@568374878500159490>");
                var dat = new Data();
                return dat;
            }
        }
        public DataTable GetValidDaysOfMonth(int month, int year)
        {
            var cmd = new SqlCommand($"select distinct day from tempdatar where month = {month} and year = {year} order by day", conn);
            conn.Open();
            var dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            conn.Close();
            return dt;
        }
        public DataTable GetValidMonths()
        {
            var cmd = new SqlCommand("select distinct Month from tempdatar order by month desc", conn);
            conn.Open();
            var dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            conn.Close();
            return dt;
        }
        public DataTable GetValidMonthsOfYear(string year)
        {
            var cmd = new SqlCommand("select distinct Month from tempdatar where year = @year order by month desc", conn);
            conn.Open();
            cmd.Parameters.AddWithValue("year",year);
            var dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            conn.Close();
            return dt;
        }
        public DataTable GetValidYears()
        {
            var cmd = new SqlCommand("select distinct year from tempdatar order by year desc", conn);
            conn.Open();
            var dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            conn.Close();
            return dt;
        }
        private string AngleToDirection(double angle)
        {
            var index = int.Parse((Math.Round(((angle %= 360) < 0 ? angle + 360 : angle) / 45) % 8).ToString());
            var directions = new[] { "North", "North-West", "West", "South-West", "South", "South-East", "East", "North-East" };
            return directions[index];
        }
        public List<MonthDataFormat> GetLast24Hours()
        {
            var list = new List<MonthDataFormat>();
            var z = new SqlCommand("select top(24)*from tempdatar\r\norder by id desc", conn);
            conn.Open();
            var reader = z.ExecuteReader();
            while (reader.Read())
            {
                var angle = double.Parse(reader["WindDirection"].ToString());
                var ws = new MonthDataFormat
                {
                    Hour = reader["Hour"] + ":00",
                    Temperature = reader["Temperature"] + "°C",
                    WindSpeed = reader["WindSpeed"] + " m/s",
                    Precipitation_Rate = reader["Precipitation_Rate"] + " mm/h",
                    Precipitation_Amount = reader["Precipitation_Amount"] + " mm/h",
                    Humidity = reader["Humidity"] + "%",
                    WindSpeedGust = reader["WindSpeedGust"] + " m/s",
                    WindDirection = AngleToDirection(angle),
                    image = $@"~\images\{reader["Sky"]}.png"
                };
                list.Add(ws);
            }
            reader.Close();
            conn.Close();
            return list;
        }
        public List<WeatherFromSQL> GetLastMonth()
        {
            var list = new List<WeatherFromSQL>();
            var z = new SqlCommand("SELECT  *\r\nFROM tempdatar\r\nWHERE (YEAR(GETDATE()) = [Year] AND MONTH(GETDATE()) = [Month] AND DAY(GETDATE()) - [Day] <= 30)\r\nOR (YEAR(GETDATE()) = [Year] AND MONTH(GETDATE()) - [Month] = 1 AND DAY(GETDATE()) + (30 - [Day]) <= 30)\r\nOR (YEAR(GETDATE()) = [Year] - 1 AND MONTH(GETDATE()) = 12 AND MONTH([Month]) = 1 AND DAY(GETDATE()) + (30 - [Day]) <= 30)\r\nORDER BY [Id] DESC", conn);
            conn.Open();
            var reader = z.ExecuteReader();
            while (reader.Read())
            {
                list.Add(CreateObjectFromSql(reader));
            }
            reader.Close();
            conn.Close();
            return list;
        }
        public List<WeatherFromSQL> GetLastYear(int yr)
        {
            var list = new List<WeatherFromSQL>();
            var z = new SqlCommand("select * from tempdatar where year=@year order by id desc", conn);
            z.Parameters.AddWithValue("year", yr);
            conn.Open();
            var reader = z.ExecuteReader();
            while (reader.Read())
            {
                list.Add(CreateObjectFromSql(reader));
            }
            reader.Close();
            conn.Close();
            return list;
        }

        public List<WeatherFromSQL> GetAnyMonth(int month)
        {
            var list = new List<WeatherFromSQL>();
            var z = new SqlCommand("select * from tempdatar where month = @month order by id desc", conn);
            z.Parameters.AddWithValue("month", month);
            conn.Open();
            var reader = z.ExecuteReader();
            while (reader.Read())
            {
                list.Add(CreateObjectFromSql(reader));
            }
            reader.Close();
            conn.Close();
            return list;
        }



        public void InsertTemperatureWithDate(float temp, float windspeed, float precipitation_rate,float precipitation_amount, float humidity, float gust, float direction, string skycode)
        {
            conn.Open();
            var dt = DateTime.Now;
            var yr = dt.Year;
            var mnth = dt.Month;
            var day = dt.Day;
            var hour = dt.Hour;
            var cmd = new SqlCommand($"insert into TempDataR values({yr},{mnth},{day},{hour},{temp},{windspeed},{precipitation_rate},{precipitation_amount},{humidity},{gust}, {direction},'{skycode}')", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public DataTable GetReader(string query)
        {
            conn.Open();
            var cmd = new SqlCommand(query, conn);
            var reader = cmd.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);
            conn.Close();
            return dt;
        }
        public void EditColumn(string key, string keyname, string values, string tablename)
        {
            conn.Open();
            var cmd = new SqlCommand($"update {tablename} set {values} where {keyname} = {key}", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
