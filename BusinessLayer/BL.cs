using DBLayer;
using SQLWeather;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using WeatherData;

namespace BusinessLayer
{

    public class BL
    {
        DBL dbl = new DBL();
        public int ConvertMonthNameToMonthNumber(string name)
        {
            var months1 = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            return Array.IndexOf(months1, name) + 1;
        }
        public void IntializeDropDowns(DropDownList DropDownList1, DropDownList DropDownList2, DropDownList DropDownList3)
        {
            
            var months = dbl.GetValidMonths();
            var months1 = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            int z = 0;
            foreach (DataRow dr in months.Rows)
            {
                DropDownList1.Items.Insert(z, new ListItem(months1[(int)dr[0] - 1], months1[(int)dr[0] - 1]));
                z++;

            }
            for (int i = 1; i <= 31; i++)
            {
                DropDownList2.Items.Insert(0, new ListItem(i.ToString(), i.ToString()));
            }
            var years = dbl.GetValidYears();
            z = 0;
            foreach (DataRow dr in years.Rows)
            {
                DropDownList3.Items.Insert(z, new ListItem(dr[0].ToString(), dr[0].ToString()));
                z++;
            }
        }
        public void IntializeDropDownsForMonth(DropDownList Month, DropDownList Year)
        {
            
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
                        Max = max+ "°C",
                        Min = min + "°C",
                        Average = Math.Round(today.Average(x => x.Temperature), 2)+ "°C",
                        MaxHour = maxhour+":00",
                        MinHour = minhour+":00"
                    };
                    list.Add(mdata);
                }
            }

            return list;
        }
        private string MonthNumToMonthName(int num)
        {
            var months = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            return months[num - 1];
        }
        private int MonthNameToMonthNum(string num)
        {
            var months = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            return Array.IndexOf(months,num)+1;
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
                    var minday = int.Parse(thismonth.Where(x=>x.Temperature == min).Select(x => x.Day).ToList()[0].ToString());
                    var maxday = int.Parse(thismonth.Where(x=>x.Temperature == max).Select(x => x.Day).ToList()[0].ToString());
                    var avg = thismonth.Average(x => x.Temperature);
                    var mdata = new YData
                    {
                        Month = MonthNumToMonthName(Data.Month),
                        Max = max + "°C",
                        Min = min + "°C",
                        AverageOfMonth = Math.Round(avg, 2)+ "°C",
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
                        Average = Math.Round(today.Average(x => x.Temperature), 2)+ "°C",
                        MaxHour = maxhour + ":00",
                        MinHour = minhour + ":00"
                    };
                    list.Add(mdata);
                }
            }

            return list;
        }
        public void EnableTexts(List<MonthDataFormat> weatherlist, Label Min, Label Max, Label Average, Label MaxHour, Label MinHour)
        {
            Min.Visible = true;
            Max.Visible = true;
            Average.Visible = true;
            MaxHour.Visible = true;
            MinHour.Visible = true;
            var intList = weatherlist.Select(x => double.Parse(x.Temperature.Replace("°C",""))).ToList();
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
            var maxday = string.Join(" ",weatherlist.Where(x => x.Temperature == max).Select(x => x.Day).ToList());
            var minday = string.Join(" ",weatherlist.Where(x => x.Temperature == min).Select(x => x.Day).ToList());
            var maxhour = string.Join(" ", weatherlist.Where(x => x.Temperature == max).Select(x => x.Hour).ToList());
            var minhour = string.Join(" ", weatherlist.Where(x => x.Temperature == min).Select(x => x.Hour).ToList());
            var avg = Math.Round(weatherlist.Average(x => x.Temperature), 2);
            Max.Text = $"Highest Temperature: {max}°C";
            MaxHour.Text = $"Day of the highest temperature:  {maxday}, Hour: {maxhour}:00";
            Min.Text = $"Lowest Temperature: {min}°C";
            MinHour.Text = $"Day of the lowest temperature: {minday}, Hour: {minhour}:00";
            Average.Text = $"Average Temperature: {avg}°C";
        }
        

        public void DisableTexts(Label Min, Label Max, Label Average, Label MaxHour, Label MinHour)
        {

            Min.Visible = false;
            Max.Visible = false;
            Average.Visible = false;
            MinHour.Visible = false;
            MaxHour.Visible = false;
        }
        public List<WeatherFromSQL> UpdateGridViewOnDropDownMonth(DropDownList Month, DropDownList Year)
        {
            
            return dbl.UpdateGridViewOnDropDownMonth(Month, Year);
        }
    }
}
