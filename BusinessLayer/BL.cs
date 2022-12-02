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

        public void IntializeDropDowns(DropDownList DropDownList1, DropDownList DropDownList2, DropDownList DropDownList3)
        {
            int z = 0;
            var months1 = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            if (DropDownList2.Items.Count == 0)
            {
                var months = dbl.GetValidMonths();
                foreach (DataRow dr in months.Rows)
                {
                    DropDownList2.Items.Insert(z, new ListItem(months1[(int)dr[0] - 1], months1[(int)dr[0] - 1]));
                    z++;

                }
            }
            DropDownList3.Items.Clear();
            var oldind = DropDownList1.SelectedIndex;
            DropDownList1.Items.Clear();
            var monthnum = MonthNameToMonthNum(DropDownList2.SelectedValue);
            var monthname = MonthNumToMonthName(monthnum);
            var month = Array.IndexOf(months1, monthname);
            var days = dbl.GetValidDaysOfMonth(month + 1);
            z = 0;
            foreach (DataRow dr in days.Rows)
            {
                DropDownList1.Items.Insert(z, new ListItem(dr[0].ToString(), dr[0].ToString()));
                z++;
            }
            try
            {
                DropDownList1.SelectedIndex = oldind;
            }
            catch (ArgumentOutOfRangeException) { }
            var years = dbl.GetValidYears();
            z = 0;
            foreach (DataRow dr in years.Rows)
            {
                DropDownList3.Items.Insert(z, new ListItem(dr[0].ToString(), dr[0].ToString()));
                z++;
            }
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
            var months = dbl.GetValidMonths();
            var months1 = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            int z = 0;
            foreach (DataRow dr in months.Rows)
            {
                Month.Items.Insert(z, new ListItem(months1[(int)dr[0] - 1], months1[(int)dr[0] - 1]));
                z++;

            }
            var years = dbl.GetValidYears();
            z = 0;
            foreach (DataRow dr in years.Rows)
            {
                Year.Items.Insert(z, new ListItem(dr[0].ToString(), dr[0].ToString()));
                z++;
            }
            Month.SelectedIndex = 0;
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
                    var mdata = new YData
                    {
                        Month = MonthNumToMonthName(Data.Month),
                        Max = max + "°C",
                        Min = min + "°C",
                        AverageOfMonth = Math.Round(avg, 2) + "°C",
                        MaxDay = GetOrdinalNumber(maxday, MonthNumToMonthName(Data.Month)),
                        MinDay = GetOrdinalNumber(minday, MonthNumToMonthName(Data.Month))
                    };
                    list.Add(mdata);
                }
            }
            return list;
        }


        public List<MonthDataFormat> UpdateGridViewOnDropDown(DropDownList DropDownList1, DropDownList DropDownList2, DropDownList DropDownList3)
        {
            var db = new DBL();
            return db.UpdateGridViewOnDropDown(DropDownList1, DropDownList2, DropDownList3);
        }
        public List<MonthDataFormat> GetLast24Hours()
        {

            return dbl.GetLast24Hours();
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
            
            ChartTemp.Series.Clear();
            ChartTemp.DataBindTable(list, "Hour");
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
                    Temperature = double.Parse(weather.Average.Replace("°C", ""))
                };
                list.Add(cr);
            }
            ChartTemp.Series.Clear();
            ChartTemp.DataBindTable(list, "Day");
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
        public void CreateChartYear(Chart ChartTemp, List<YData> weatherlist)
        {

            var list = new List<ChartYear>();
            foreach (var weather in weatherlist)
            {
                var cr = new ChartYear
                {
                    Month = MonthNameToMonthNum(weather.Month),
                    Temperature = double.Parse(weather.AverageOfMonth.Replace("°C", ""))
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
                    var mdata = new MData
                    {
                        Day = Data.Day,
                        Max = max + "°C",
                        Min = min + "°C",
                        Average = Math.Round(today.Average(x => x.Temperature), 2) + "°C",
                        MaxHour = maxhour + ":00",
                        MinHour = minhour + ":00"
                    };
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
                    var mdata = new YData
                    {
                        Month = MonthNumToMonthName(Data.Month),
                        Max = max + "°C",
                        Min = min + "°C",
                        AverageOfMonth = Math.Round(avg, 2) + "°C",
                        MaxDay = maxday + "",
                        MinDay = minday + ""
                    };
                    list.Add(mdata);
                }
            }

            return list;
        }
        public List<MData> GetAnyMonth(int month)
        {

            var themonth = dbl.GetAnyMonth(month);
            var list = new List<MData>();
            foreach (WeatherFromSQL Data in themonth)
            {

                var today = themonth.Where(x => x.Day == Data.Day);
                if (list.Where(x => x.Day == Data.Day).Count() == 0)
                {
                    var max = today.Max(x => x.Temperature);
                    var min = today.Where(x => x.Day == Data.Day).Min(x => x.Temperature);
                    var minhour = int.Parse(today.Where(x => x.Day == Data.Day && x.Temperature == min).Select(x => x.Hour).ToList()[0].ToString());
                    var maxhour = int.Parse(today.Where(x => x.Day == Data.Day && x.Temperature == max).Select(x => x.Hour).ToList()[0].ToString());
                    var mdata = new MData
                    {
                        Day = Data.Day,
                        Max = max + "°C",
                        Min = min + "°C",
                        Average = Math.Round(today.Average(x => x.Temperature), 2) + "°C",
                        MaxHour = maxhour + ":00",
                        MinHour = minhour + ":00"
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
        public List<WeatherFromSQL> UpdateGridViewOnDropDownMonth(DropDownList Month, DropDownList Year)
        {

            return dbl.UpdateGridViewOnDropDownMonth(Month, Year);
        }
    }
}
