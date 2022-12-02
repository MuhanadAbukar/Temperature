using Newtonsoft.Json;
using SQLWeather;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.UI.WebControls;
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
        static string connstr = "Data Source=DESKTOP-QJBSGQ4\\MSSQLSERVER01;Initial Catalog=Harvester_Muhanad;Persist Security Info=True;User ID=sa;Password=muhanad123";
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
                Precipitation = (double)reader["Precipitation"],
                Humidity = (double)reader["Humidity"],
                WindSpeedGust = (double)reader["WindSpeedGust"],
                WindDirection = (double)reader["WindDirection"]
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
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://api.met.no/weatherapi/nowcast/2.0/complete?lat=59.218633&lon=10.942062");
                httpWebRequest.UserAgent = "Muhanad";
                var httpResponse = httpWebRequest.GetResponse();
                var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var result = streamReader.ReadToEnd();
                var real = JsonConvert.DeserializeObject<WeatherData.WeatherData>(result);
                var properties = real.properties.timeseries[0].data;
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
        public DataTable GetValidDaysOfMonth(int month)
        {
            var cmd = new SqlCommand($"select distinct day from tempdatar where month = {month}", conn);
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
        public DataTable GetValidYears()
        {
            var cmd = new SqlCommand("select distinct year from tempdatar order by year desc", conn);
            conn.Open();
            var dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            conn.Close();
            return dt;
        }
        public List<MonthDataFormat> GetLast24Hours()
        {
            var list = new List<MonthDataFormat>();
            var z = new SqlCommand("select * from tempdatar where day=day(Cast(Getdate() as date))", conn);
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
                    Precipitation = reader["Precipitation"] + " mm",
                    Humidity = reader["Humidity"] + "%",
                    WindSpeedGust = reader["WindSpeedGust"] + " m/s",
                    WindDirection = AngleToDirection(angle),
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
            var z = new SqlCommand("select * from tempdatar order by id desc", conn);
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
        private string AngleToDirection(double angle)
        {
            var index = int.Parse((Math.Round(((angle %= 360) < 0 ? angle + 360 : angle) / 45) % 8).ToString());
            var directions = new[] { "North", "North-West", "West", "South-West", "South", "South-East", "East", "North-East" };
            return directions[index];
        }
        public List<MonthDataFormat> UpdateGridViewOnDropDown(DropDownList Day, DropDownList Month, DropDownList Year)
        {
            var months1 = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            var selectedmonth = Array.IndexOf(months1, Month.SelectedItem.Text) + 1;
            var selectedday = Day.SelectedItem.Text;
            var selectedyear = Year.SelectedItem.Text;
            var cmd = new SqlCommand($"select Year,Month,Day,Hour,Temperature,Windspeed,Precipitation,Humidity,Windspeedgust,Winddirection from tempdatar where year = @year and month = @month and day = @day", conn);
            cmd.Parameters.AddWithValue("month", selectedmonth);
            cmd.Parameters.AddWithValue("year", selectedyear);
            cmd.Parameters.AddWithValue("day", selectedday);
            conn.Open();
            var list = new List<MonthDataFormat>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var angle = double.Parse(reader["WindDirection"].ToString());
                var ws = new MonthDataFormat
                {
                    Hour = reader["Hour"] + ":00",
                    Temperature = reader["Temperature"] + "°C",
                    WindSpeed = reader["WindSpeed"] + " m/s",
                    Precipitation = reader["Precipitation"] + " mm",
                    Humidity = reader["Humidity"] + "%",
                    WindSpeedGust = reader["WindSpeedGust"] + " m/s",
                    WindDirection = AngleToDirection(angle),
                };
                list.Add(ws);
            }
            conn.Close();
            return list;
        }
        public List<WeatherFromSQL> UpdateGridViewOnDropDownMonth(DropDownList Month, DropDownList Year)
        {
            var months1 = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            var selectedmonth = Array.IndexOf(months1, Month.SelectedItem.Text) + 1;
            var selectedyear = Year.SelectedItem.Text;
            var cmd = new SqlCommand($"select Year,Month,Day,Hour,Temperature,Windspeed,Precipitation,Humidity,Windspeedgust,Winddirection from tempdatar where year = @year and month = @month", conn);
            cmd.Parameters.AddWithValue("month", selectedmonth);
            cmd.Parameters.AddWithValue("year", selectedyear);
            conn.Open();
            var list = new List<WeatherFromSQL>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(CreateObjectFromSql(reader));
            }
            conn.Close();
            return list;
        }
        public void InsertTemperatureWithDate(float temp, float windspeed, float precipitation, float humidity, float gust, float direction, string skycode)
        {
            conn.Open();
            var dt = DateTime.Now;
            var yr = dt.Year;
            var mnth = dt.Month;
            var day = dt.Day;
            var hour = dt.Hour;
            var cmd = new SqlCommand($"insert into TempDataR values({yr},{mnth},{day},{hour},{temp},{windspeed},{precipitation},{humidity},{gust}, {direction},'{skycode}')", conn);
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
