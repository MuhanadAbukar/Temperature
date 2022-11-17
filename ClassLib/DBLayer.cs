using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI.WebControls;

namespace DBLayer
{
    public class WebhookPost
    {
        public string content { set; get; }
        public void PostWebhook(string content)
        {
            var info = new WebhookPost()
            {
                content = content,
            };
            var js = JsonConvert.SerializeObject(info);
            var _cl = new HttpClient();
            var pl = new StringContent(js, Encoding.UTF8, "application/json");
            _cl.PostAsync("https://discord.com/api/webhooks/1037668127242203136/gg8ne3IzPN49PD9q6xNLkvv2ciKclffnMh2QJ37mkx02bjHkzP2TP8L38udQvXav5AhB", pl);
        }
    }
    public class DBL
    {
        static string connstr = "Data Source=DESKTOP-QJBSGQ4\\MSSQLSERVER01;Initial Catalog=Harvester_Muhanad;Persist Security Info=True;User ID=sa;Password=muhanad123";
        static SqlConnection conn = new SqlConnection(connstr);
        public WeatherData.Details GetPropertiesOfWeather()
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://api.met.no/weatherapi/nowcast/2.0/complete?lat=59.218633&lon=10.942062");
                httpWebRequest.UserAgent = "Muhanad";
                var httpResponse = httpWebRequest.GetResponse();
                var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var result = streamReader.ReadToEnd();
                var real = JsonConvert.DeserializeObject<WeatherData.WeatherData>(result);
                var properties = real.properties.timeseries[0].data.instant.details;
                return properties;
            }
            catch(Exception m)
            {
                var d = new WebhookPost();
                d.PostWebhook(m.Message+ " <@568374878500159490>");
                var d2 = new WeatherData.Details();
                d2.air_temperature = 99999;
                return d2;
            }
        }
        
        public DataTable GetValidMonths()
        {
            var x = new SqlCommand("select distinct Month from tempdatar order by month desc", conn);
            conn.Open();
            var dt = new DataTable();
            dt.Load(x.ExecuteReader());
            conn.Close();
            return dt;
        }
        public DataTable GetValidYears()
        {
            var x = new SqlCommand("select distinct year from tempdatar order by year desc", conn);
            conn.Open();
            var dt = new DataTable();
            dt.Load(x.ExecuteReader());
            conn.Close();
            return dt;
        }
        
        
        public DataTable UpdateGridViewOnDropDown(DropDownList DropDownList1, DropDownList DropDownList2, DropDownList DropDownList3) 
        {
            var months1 = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            var dt = new DataTable();
            var selectedmonth = Array.IndexOf(months1,DropDownList1.SelectedItem.Text)+1;
            var selectedday = DropDownList2.SelectedItem.Text;
            var selectedyear = DropDownList3.SelectedItem.Text;
            var cmd = new SqlCommand($"select Year,Month,Day,Hour,Temperature,Windspeed,Precipitation,Humidity,Windspeedgust,Winddirection from tempdatar where year = @year and month = @month and day = @day",conn);
            cmd.Parameters.AddWithValue("month", selectedmonth);
            cmd.Parameters.AddWithValue("year", selectedyear);
            cmd.Parameters.AddWithValue("day", selectedday);
            conn.Open();
            dt.Load(cmd.ExecuteReader());
            conn.Close();
            return dt;
        }
        public void InsertTemperatureWithDate(float temp, float windspeed, float precipitation, float humidity, float gust, float direction)
        {
            conn.Open();
            var dt = DateTime.Now;
            var yr = dt.Year;
            var mnth = dt.Month;
            var day = dt.Day;
            var hour = dt.Hour;
            var cmd = new SqlCommand($"insert into TempDataR values({yr},{mnth},{day},{hour},{temp},{windspeed},{precipitation},{humidity},{gust}, {direction})", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public DataTable getReader(string query)
        {
            conn.Open();
            var cmd = new SqlCommand(query, conn);
            var reader = cmd.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);
            conn.Close();
            return dt;

        }
        public void editColumn(string key, string keyname, string values, string tablename)
        {
            conn.Open();
            var cmd = new SqlCommand($"update {tablename} set {values} where {keyname} = {key}", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
