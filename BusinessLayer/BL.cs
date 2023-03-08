using DBLayer;
using SQLWeather;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using WeatherData;

namespace BusinessLayer
{

    public class BL
    {
        DBL dbl = new DBL();
        public string GetTheMostCommonSkyCode(List<MonthDataFormat> weatherlist)
        {
            return weatherlist.GroupBy(a => a.image).OrderByDescending(g => g.Count()).First().Key;
        }
        public void UpdateMonthsOfYear(string year, DropDownList month)
        {
            month.Items.Clear();
            var val = dbl.GetValidMonthsOfYear(year);
            int i = 0;
            foreach (DataRow dr in val.Rows)
            {
                
                month.Items.Insert(i, new ListItem( MonthNumToMonthName( (int)dr[0]), MonthNumToMonthName((int)dr[0])));
                i++;
            }
        }
        public void UpdateDaysOfMonth(int month, DropDownList day, int year)
        {
            day.Items.Clear();
            var days = dbl.GetValidDaysOfMonth(month,year);
            
        }
        public void InsertDaysToDayDropDown(DataTable data, DropDownList daydrop)
        {
            int i = 0;
            foreach (DataRow dr in data.Rows)
            {
                daydrop.Items.Insert(i, new ListItem(dr[0].ToString(), dr[0].ToString()));
                i++;
            }
        }
        public void InsertYearsToYearDropDown(DataTable data, DropDownList yeardrop)
        {
            int i = 0;
            foreach (DataRow dr in data.Rows)
            {
                yeardrop.Items.Insert(i, new ListItem(dr[0].ToString(), dr[0].ToString()));
                i++;
            }
        }
        public void InsertMonthsToMonthDropDown(DataTable data, DropDownList monthdrop)
        {
            int i = 0;
            foreach (DataRow dr in data.Rows)
            {
                monthdrop.Items.Insert(i, new ListItem(MonthNumToMonthName((int)dr[0]), MonthNumToMonthName((int)dr[0])));
                i++;
            }
        }
        public void IntializeDropDownsForMonthChanged(DropDownList Day, DropDownList Month, DropDownList Year)
        {
            Day.Items.Clear();
            var days = dbl.GetValidDaysOfMonth(MonthNameToMonthNum(Month.SelectedValue),int.Parse(Year.SelectedValue));
            int i = 0;
            foreach (DataRow dr in days.Rows)
            {
                Day.Items.Insert(i, new ListItem(dr[0].ToString(), dr[0].ToString()));
                i++;
            }
        }
        public void IntializeDropDownsForYearChanged(DropDownList Day, DropDownList Month, DropDownList Year)
        {
            var months = dbl.GetValidMonthsOfYear(Year.SelectedValue);
            Month.Items.Clear();
            InsertMonthsToMonthDropDown(months, Month);
            Day.Items.Clear();
            var days = dbl.GetValidDaysOfMonth(MonthNameToMonthNum(Month.SelectedValue), int.Parse(Year.SelectedValue));
            InsertDaysToDayDropDown(days, Day);
        }
        public void IntializeDropDowns(DropDownList Day, DropDownList Month, DropDownList Year)
        {
            int i = 0;
            if (Month.Items.Count == 0)
            {
                var months = dbl.GetValidMonthsOfYear(DateTime.Now.Year.ToString());
                InsertMonthsToMonthDropDown(months, Month);
            }
            var year = DateTime.Now.Year;
            Year.Items.Clear();
            Day.Items.Clear();
            var month = MonthNameToMonthNum(Month.SelectedValue);
            var days = dbl.GetValidDaysOfMonth(month,year);
            var years = dbl.GetValidYears();
            InsertDaysToDayDropDown(days, Day);
            InsertYearsToYearDropDown(years, Year);
        }
        public void IntializeDropDownsForYear(DropDownList Year)
        {
            var years = dbl.GetValidYears();
            int i = 0;
            foreach (DataRow dr in years.Rows)
            {
                var content = dr[0].ToString();
                Year.Items.Insert(i, new ListItem(content, content));
                i++;
            }
        }
        public void IntializeDropDownsForMonth(DropDownList Month, DropDownList Year)
        {
            Month.SelectedIndex = 0;
            var months = dbl.GetValidMonthsOfYear(DateTime.Now.Year.ToString());
            var years = dbl.GetValidYears();
            InsertMonthsToMonthDropDown(months, Month);
            InsertYearsToYearDropDown(years, Year);
            Year.SelectedIndex = 0;
            Month.SelectedIndex = 0;
        }
        public void IntializeDropDownsForDayOnYearChanged(DropDownList Day, DropDownList Month)
        {

        }
        private string GetOrdinalNumber(int num, string month)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return $"{num}th of {month}";
            }

            switch (num % 10)
            {
                case 1:
                    return $"{num}st of {month}";
                case 2:
                    return $"{num}nd of {month}";
                case 3:
                    return $"{num}rd of {month}";
                default:
                    return $"{num}th of {month}";
            }
        }
        public List<YData> GetYear(int year)
        {
            var lastyear = dbl.GetAnyYear(year);
            var list = new List<YData>();

            foreach (WeatherFromSQL Data in lastyear)
            {
                var thismonth = lastyear.Where(x => x.Month == Data.Month);
                if (list.Where(x => MonthNameToMonthNum(x.Month) == Data.Month).Count() == 0)
                {

                    var max = thismonth.Max(x => x.Temperature);
                    var min = thismonth.Min(x => x.Temperature);
                    var minday = int.Parse(thismonth.Where(x => x.Temperature == min).Select(x => x.Day).ToList()[0].ToString());
                    var maxday = int.Parse(thismonth.Where(x => x.Temperature == max).Select(x => x.Day).ToList()[0].ToString());
                    var avg = thismonth.Average(x => x.Temperature);
                    var avgSky = thismonth.GroupBy(a => a.Sky).OrderByDescending(g => g.Count()).First().Key;
                    if (avgSky == "")
                        avgSky = "errors";
                    var mdata = new YData
                    {
                        Month = MonthNumToMonthName(Data.Month),
                        Max = max + "°C",
                        Min = min + "°C",
                        AverageTempOfMonth = Math.Round(avg, 2) + "°C",
                        MaxDay = GetOrdinalNumber(maxday, MonthNumToMonthName(Data.Month)),
                        MinDay = GetOrdinalNumber(minday, MonthNumToMonthName(Data.Month)),
                        AverageSkyOfMonth = $@"~\images\{avgSky}.png"
                    };
                    list.Add(mdata);
                }
            }
            return list;
        }

        public List<WeatherFromSQL> GetAnyYear(int year)
        {
            return dbl.GetAnyYear(year);
        }
        private string AngleToDirection(double angle)
        {
            var index = int.Parse((Math.Round(((angle %= 360) < 0 ? angle + 360 : angle) / 45) % 8).ToString());
            var directions = new[] { "North", "North-West", "West", "South-West", "South", "South-East", "East", "North-East" };
            return directions[index];
        }
        public List<MonthDataFormat> ParseWeatherDataToDayReport(List<WeatherFromSQL> weatherFromSQLs)
        {
            var list = new List<MonthDataFormat>();
            foreach (WeatherFromSQL weatherFromSQL in weatherFromSQLs)
            {
                var angle = double.Parse(weatherFromSQL.WindDirection.ToString());
                var nullweather = weatherFromSQL.Sky;
                var ws = new MonthDataFormat();
                if (nullweather == "")
                {
                    ws = new MonthDataFormat
                    {
                        Hour = weatherFromSQL.Hour + ":00",
                        Temperature = weatherFromSQL.Temperature + "°C",
                        WindSpeed = weatherFromSQL.WindSpeed + " m/s",
                        Precipitation_Amount = weatherFromSQL.Precipitation_Amount + " mm",
                        Precipitation_Rate = weatherFromSQL.Precipitation_Rate + " mm/h",
                        Humidity = weatherFromSQL.Humidity + "%",
                        WindSpeedGust = weatherFromSQL.WindSpeedGust + " m/s",
                        WindDirection = AngleToDirection(angle),
                        image = $@"~\images\errors1.png"
                    };
                }
                else
                {
                    ws = new MonthDataFormat
                    {
                        Hour = weatherFromSQL.Hour + ":00",
                        Temperature = weatherFromSQL.Temperature + "°C",
                        WindSpeed = weatherFromSQL.WindSpeed + " m/s",
                        Precipitation_Rate = weatherFromSQL.Precipitation_Rate + " mm/h",
                        Precipitation_Amount = weatherFromSQL.Precipitation_Amount+" mm",
                        Humidity = weatherFromSQL.Humidity + "%",
                        WindSpeedGust = weatherFromSQL.WindSpeedGust + " m/s",
                        WindDirection = AngleToDirection(angle),
                        image = $@"~\images\{weatherFromSQL.Sky}.png"
                    };
                }
                list.Add(ws);
            }

            return list;
        }
        public List<MData> ParseWeatherDataToMonthReport(List<WeatherFromSQL> weatherFromSQLs)
        {
            var list = new List<MData>();
            var themonth = weatherFromSQLs;
            foreach (WeatherFromSQL Data in themonth)
            {

                var today = themonth.Where(x => x.Day == Data.Day);
                if (list.Where(x => x.Day == Data.Day).Count() == 0)
                {
                    var max = today.Max(x => x.Temperature);
                    var min = today.Where(x => x.Day == Data.Day).Min(x => x.Temperature);
                    var minhour = int.Parse(today.Where(x => x.Day == Data.Day && x.Temperature == min).Select(x => x.Hour).ToList()[0].ToString());
                    var maxhour = int.Parse(today.Where(x => x.Day == Data.Day && x.Temperature == max).Select(x => x.Hour).ToList()[0].ToString());
                    var avgSky = today.GroupBy(a => a.Sky).OrderByDescending(g => g.Count()).First().Key;
                    var mdata = new MData();
                    if (avgSky == "")
                    {
                        mdata = new MData
                        {
                            Day = Data.Day,
                            Max = max + "°C",
                            Min = min + "°C",
                            AverageTemp = Math.Round(today.Average(x => x.Temperature), 2) + "°C",
                            MaxHour = maxhour + ":00",
                            MinHour = minhour + ":00",
                            AverageSky = $@"~\images\errors1.png"
                        };
                    }
                    else
                    {
                        mdata = new MData
                        {
                            Day = Data.Day,
                            Max = max + "°C",
                            Min = min + "°C",
                            AverageTemp = Math.Round(today.Average(x => x.Temperature), 2) + "°C",
                            MaxHour = maxhour + ":00",
                            MinHour = minhour + ":00",
                            AverageSky = $@"~\images\{avgSky}.png"
                        };
                    }

                    list.Add(mdata);
                }
            }
            list.Reverse();
            return list;
        }

        public List<MonthDataFormat> GetLast24Hours()
        {
            var db = new DBL();
            return db.GetLast24Hours();
        }
        public void CreateChartDay(Chart ChartTemp, List<MonthDataFormat> weatherlist)
        {
            var list = new List<ChartDay>();
            foreach (var weather in weatherlist)
            {
                var cr = new ChartDay
                {
                    Hour = int.Parse(weather.Hour.Replace(":00", "")),
                    Temperature = double.Parse(weather.Temperature.Replace("°C", ""))
                };
                list.Add(cr);
            }
            list.Reverse();
            ChartTemp.Series.Clear();
            ChartTemp.DataBindTable(list, "Hour");
            ChartTemp.Series[0].IsXValueIndexed = true;
            ChartTemp.Series[0].ChartType = SeriesChartType.Line;
            var pointCounter = 0;
            ChartTemp.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
            ChartTemp.Series[0].LegendText = "Temperature";
            foreach (DataPoint p in ChartTemp.Series[0].Points)
            {
                ChartTemp.Series[0].Points[pointCounter].ToolTip = p.YValues[0].ToString() + " - " + p.XValue.ToString();
                ChartTemp.Series[0].Points[pointCounter].Label = p.YValues[0] + "°C";
                ChartTemp.Series[0].Points[pointCounter].Color = Color.FromArgb(56, 80, 93);//page blue
                pointCounter++;
            }
        }
        public void CreateChartMonth(Chart ChartTemp, List<MData> weatherlist)
        {
            var list = new List<ChartMonth>();
            foreach (var weather in weatherlist)
            {
                var cr = new ChartMonth
                {
                    Day = weather.Day,
                    Temperature = double.Parse(weather.AverageTemp.Replace("°C", ""))
                };
                list.Add(cr);
            }
            ChartTemp.Series.Clear();
            ChartTemp.DataBindTable(list, "Day");
            ChartTemp.Series[0].ChartType = SeriesChartType.Line;
            var pointCounter = 0;
            ChartTemp.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
            ChartTemp.Series[0].LegendText = "Temperature";
            ChartTemp.Series[0].IsXValueIndexed = true;
            foreach (DataPoint p in ChartTemp.Series[0].Points)
            {
                var label = ChartTemp.Series[0].Label;

                ChartTemp.Series[0].Points[pointCounter].ToolTip = p.YValues[0].ToString() + " - " + p.XValue.ToString();
                ChartTemp.Series[0].Points[pointCounter].Label = p.YValues[0] + "°C";
                ChartTemp.Series[0].Points[pointCounter].Color = Color.FromArgb(56, 80, 93);//page blue
                pointCounter++;
            }
        }
        public void CreateChartYear(Chart ChartTemp, List<YData> weatherlist)
        {

            var list = new List<ChartYear>();
            foreach (var weather in weatherlist)
            {
                var cr = new ChartYear
                {
                    Month = MonthNameToMonthNum(weather.Month),
                    Temperature = double.Parse(weather.AverageTempOfMonth.Replace("°C", ""))
                };
                list.Add(cr);
            }
            ChartTemp.Series.Clear();
            ChartTemp.DataBindTable(list, "Month");
            ChartTemp.Series[0].ChartType = SeriesChartType.Line;
            var pointCounter = 0;
            ChartTemp.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
            ChartTemp.Series[0].LegendText = "Temperature";
            foreach (DataPoint p in ChartTemp.Series[0].Points)
            {
                var label = ChartTemp.Series[0].Label;

                ChartTemp.Series[0].Points[pointCounter].ToolTip = p.YValues[0].ToString() + " - " + p.XValue.ToString();
                ChartTemp.Series[0].Points[pointCounter].Label = p.YValues[0] + "°C";
                ChartTemp.Series[0].Points[pointCounter].Color = Color.FromArgb(56, 80, 93);//page blue
                pointCounter++;
            }
        }
        public List<MData> GetLastMonth()
        {

            var lastmonth = dbl.GetLastMonth();
            var list = new List<MData>();
            foreach (WeatherFromSQL Data in lastmonth)
            {

                var today = lastmonth.Where(x => x.Day == Data.Day);
                if (list.Where(x => x.Day == Data.Day).Count() == 0)
                {
                    var max = today.Max(x => x.Temperature);
                    var min = today.Where(x => x.Day == Data.Day).Min(x => x.Temperature);
                    var minhour = int.Parse(today.Where(x => x.Day == Data.Day && x.Temperature == min).Select(x => x.Hour).ToList()[0].ToString());
                    var maxhour = int.Parse(today.Where(x => x.Day == Data.Day && x.Temperature == max).Select(x => x.Hour).ToList()[0].ToString());
                    var avgsky = today.GroupBy(a => a.Sky).OrderByDescending(g => g.Count()).First().Key;
                    var avgtemp = Math.Round(today.Average(x => x.Temperature), 2);
                    var mdata = new MData();
                    if (avgsky == "")
                    {
                        mdata = new MData
                        {
                            Day = Data.Day,
                            Max = max + "°C",
                            Min = min + "°C",
                            AverageTemp = avgtemp + "°C",
                            MaxHour = maxhour + ":00",
                            MinHour = minhour + ":00",
                            AverageSky = $@"~\images\errors1.png"
                        };
                    }
                    else
                    {
                        mdata = new MData
                        {
                            Day = Data.Day,
                            Max = max + "°C",
                            Min = min + "°C",
                            AverageTemp = avgtemp + "°C",
                            MaxHour = maxhour + ":00",
                            MinHour = minhour + ":00",
                            AverageSky = $@"~\images\{avgsky}.png"
                        };
                    }

                    list.Add(mdata);
                }
            }

            return list;
        }
        public string MonthNumToMonthName(int num)
        {
            var months = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            return months[num - 1];
        }
        public int MonthNameToMonthNum(string num)
        {
            var months = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            return Array.IndexOf(months, num) + 1;
        }
        public List<YData> GetLastYear()
        {

            var lastyear = dbl.GetLastYear(DateTime.Now.Year);
            var list = new List<YData>();

            foreach (WeatherFromSQL Data in lastyear)
            {
                var thismonth = lastyear.Where(x => x.Month == Data.Month);
                if (list.Where(x => MonthNameToMonthNum(x.Month) == Data.Month).Count() == 0)
                {

                    var max = thismonth.Max(x => x.Temperature);
                    var min = thismonth.Min(x => x.Temperature);
                    var minday = int.Parse(thismonth.Where(x => x.Temperature == min).Select(x => x.Day).ToList()[0].ToString());
                    var maxday = int.Parse(thismonth.Where(x => x.Temperature == max).Select(x => x.Day).ToList()[0].ToString());
                    var avg = thismonth.Average(x => x.Temperature);
                    var skymonth = thismonth.Where(x => x.Sky != "");
                    var avgSky = skymonth.GroupBy(a => a.Sky).OrderByDescending(g => g.Count()).First().Key;
                    if (avgSky == "")
                        avgSky = "errors";
                    var mdata = new YData
                    {
                        Month = MonthNumToMonthName(Data.Month),
                        Max = max + "°C",
                        Min = min + "°C",
                        AverageTempOfMonth = Math.Round(avg, 2) + "°C",
                        MaxDay = GetOrdinalNumber(maxday, MonthNumToMonthName(Data.Month)),
                        MinDay = GetOrdinalNumber(minday, MonthNumToMonthName(Data.Month)),
                        AverageSkyOfMonth = $@"~\images\{avgSky}.png"
                    };
                    list.Add(mdata);
                }
            }

            return list;
        }

        public void EnableTexts(List<MonthDataFormat> weatherlist, Label Min, Label Max, Label Average, Label MaxHour, Label MinHour, Chart ChartTemp)
        {
            ChartTemp.Visible = true;
            Min.Visible = true;
            Max.Visible = true;
            Average.Visible = true;
            MaxHour.Visible = true;
            MinHour.Visible = true;
            var intList = weatherlist.Select(x => double.Parse(x.Temperature.Replace("°C", ""))).ToList();
            var max = intList.Max();
            var min = intList.Min();
            var maxhour = weatherlist.Where(x => double.Parse(x.Temperature.Replace("°C", "")) == max).Select(x => x.Hour).ToList()[0];
            var minhour = weatherlist.Where(x => double.Parse(x.Temperature.Replace("°C", "")) == min).Select(x => x.Hour).ToList()[0];
            Max.Text = $"Highest Temperature: {max}°C";
            MaxHour.Text = $"Time of the highest temperature: {maxhour}";
            Min.Text = $"Lowest Temperature: {min}°C";
            MinHour.Text = $"Time of the lowest temperature: {minhour}";
            var avg = weatherlist.Select(x => x.Temperature);
            var realavg = avg.Select(x => double.Parse(x.Replace("°C", ""))).ToList();
            Average.Text = $"Average Temperature: {Math.Round(realavg.Average(), 2)}°C";
        }
        public void EnableTextsForMonth(List<WeatherFromSQL> weatherlist, Label Min, Label Max, Label Average, Label MaxHour, Label MinHour)
        {
            Min.Visible = true;
            Max.Visible = true;
            Average.Visible = true;
            var max = weatherlist.Max(x => x.Temperature);
            var min = weatherlist.Min(x => x.Temperature);
            var maxday = string.Join(" ", weatherlist.Where(x => x.Temperature == max).Select(x => x.Day).ToList());
            var minday = string.Join(" ", weatherlist.Where(x => x.Temperature == min).Select(x => x.Day).ToList());
            var maxhour = string.Join(" ", weatherlist.Where(x => x.Temperature == max).Select(x => x.Hour).ToList());
            var minhour = string.Join(" ", weatherlist.Where(x => x.Temperature == min).Select(x => x.Hour).ToList());
            var avg = Math.Round(weatherlist.Average(x => x.Temperature), 2);
            Max.Text = $"Highest Temperature: {max}°C";
            MaxHour.Text = $"Day of the highest temperature:  {maxday}, Hour: {maxhour}:00";
            Min.Text = $"Lowest Temperature: {min}°C";
            MinHour.Text = $"Day of the lowest temperature: {minday}, Hour: {minhour}:00";
            Average.Text = $"Average Temperature: {avg}°C";
        }


        public void DisableTexts(Label Min, Label Max, Label Average, Label MaxHour, Label MinHour, Chart chart)
        {

            Min.Visible = false;
            Max.Visible = false;
            Average.Visible = false;
            MinHour.Visible = false;
            MaxHour.Visible = false;
            chart.Visible = false;
        }

    }
}
