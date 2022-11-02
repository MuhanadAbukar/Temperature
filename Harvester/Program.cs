using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HarvesterTemp
{
    internal class Program
    {
        public class Rootobject
        {
            public string type { get; set; }
            public Geometry geometry { get; set; }
            public Properties properties { get; set; }
        }

        public class Geometry
        {
            public string type { get; set; }
            public float[] coordinates { get; set; }
        }

        public class Properties
        {
            public Meta meta { get; set; }
            public Timesery[] timeseries { get; set; }
        }

        public class Meta
        {
            public DateTime updated_at { get; set; }
            public Units units { get; set; }
            public string radar_coverage { get; set; }
        }

        public class Units
        {
            public string air_temperature { get; set; }
            public string precipitation_amount { get; set; }
            public string precipitation_rate { get; set; }
            public string relative_humidity { get; set; }
            public string wind_from_direction { get; set; }
            public string wind_speed { get; set; }
            public string wind_speed_of_gust { get; set; }
        }

        public class Timesery
        {
            public DateTime time { get; set; }
            public Data data { get; set; }
        }

        public class Data
        {
            public Instant instant { get; set; }
            public Next_1_Hours next_1_hours { get; set; }
        }

        public class Instant
        {
            public Details details { get; set; }
        }

        public class Details
        {
            public float air_temperature { get; set; }
            public float precipitation_rate { get; set; }
            public float relative_humidity { get; set; }
            public float wind_from_direction { get; set; }
            public float wind_speed { get; set; }
            public float wind_speed_of_gust { get; set; }
        }

        public class Next_1_Hours
        {
            public Summary summary { get; set; }
            public Details1 details { get; set; }
        }

        public class Summary
        {
            public string symbol_code { get; set; }
        }

        public class Details1
        {
            public float precipitation_amount { get; set; }
        }

        static void Main(string[] args)
        {
            var x = new Program();
            x.getYR();
        }
        public void getYR()
        {
            var cl = new HttpClient();
            var res = cl.GetStringAsync("https://api.met.no/weatherapi/nowcast/2.0/complete?lat=59.9333&lon=10.7166");
            var f = JsonSerializer.Deserialize<Rootobject>(res);
        }
        private int[] GetPMValues()
        {
            //http://jsonviewer.stack.hu/
            //59.202752, 10.953535
            int[] values = new int[2];
            values[0] = -999;

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.smartcitizen.me/v0/devices/14057");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.UserAgent = "bolle";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return values;
        }

        public int GetStuff()
        {
            //http://jsonviewer.stack.hu/
            //https://peterdaugaardrasmussen.com/2022/01/18/how-to-get-a-property-from-json-using-selecttoken/
            //create the httpwebrequest
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.met.no/weatherapi/nowcast/2.0/complete?lat=59.9333&lon=10.7166");

            //the usual stuff. run the request, parse your json
            try
            {
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.UserAgent = "bolle";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    //JToken data = jObj.SelectToken("path");
                    //int valuepm1 = data.Value<int>("keyname");//key name - getting key.value
                    //int valuepm25 = data.Value<int>("pm25");
                    //int radonValue = data.Value<int>("radonShortTermAvg");
                    // inn i db

                }
                return 0;
            }
            catch { Exception ex; }
            return 0;
        }
    }
}
