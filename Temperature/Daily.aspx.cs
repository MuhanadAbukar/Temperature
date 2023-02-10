using BusinessLayer;
using SQLWeather;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;

namespace Temperature
{
    public partial class Daily : System.Web.UI.Page
    {
        public static List<WeatherFromSQL> weatherFromSQLs = new List<WeatherFromSQL>();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var bs = new BL();
                bs.IntializeDropDowns(DayDrop, MonthDrop, YearDrop);
                var year = YearDrop.SelectedValue;
                weatherFromSQLs = bs.GetAnyYear(int.Parse(year));
                Updating();
            }
        }
        protected void ChangedDropDown(object sender, EventArgs e)
        {
            var bs = new BL();
            bs.IntializeDropDownsForMonthChanged(DayDrop, MonthDrop, YearDrop);
            Updating();

        }
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void Month_Click(object sender, EventArgs e)
        {
            Response.Redirect((((Button)sender).ID + ".aspx").Replace("1", ""));
        }

        protected void Increment_Click(object sender, EventArgs e)
        {
            if (DayDrop.SelectedIndex - 1 > -1)
            {
                DayDrop.SelectedIndex--;
                Updating();
            }
            else
            {
                var month = MonthDrop.SelectedValue;
                if(month == "January")
                {

                }
                else
                {
                    MonthDrop.SelectedIndex++;
                }
            }

        }
        protected void ChangedDropDownDay(object sender, EventArgs e)
        {
            Updating();
        }
        protected void Updating()
        {
            if (MonthDrop.SelectedValue == DateTime.Now.ToString("MMMM") && !IsPostBack)
            {
                DayDrop.SelectedIndex = DateTime.Now.Day - 1;
            }
            var bs = new BL();
            var DATA = weatherFromSQLs;
            var year = int.Parse(YearDrop.SelectedValue);
            var month = bs.MonthNameToMonthNum(MonthDrop.SelectedValue);
            var day = int.Parse(DayDrop.SelectedValue);
            var SelectedData = DATA.Where(x => x.Day == day && x.Month == month).ToList();
            var weatherlist = bs.ParseWeatherDataToDayReport(SelectedData);
            weatherlist.Reverse();
            GridView2.DataSource = weatherlist;
            GridView2.DataBind();
            if (weatherlist.Count > 0)
            {
                bs.EnableTexts(weatherlist, Min, Max, Average, MaxHour, MinHour, ChartTemp);
                bs.CreateChartDay(ChartTemp, weatherlist);

            }
            else
            {
                bs.DisableTexts(Min, Max, Average, MaxHour, MinHour, ChartTemp);
            }
        }
        protected void Descend_Click(object sender, EventArgs e)
        {
            if (DayDrop.SelectedIndex + 1 <= DayDrop.Items.Count - 1)
            {
                DayDrop.SelectedIndex++;
                Updating();
            }
            else
            {
                var month = MonthDrop.SelectedValue;
                if (month == "January")
                {

                }
                else
                {
                    MonthDrop.SelectedIndex++;
                }
            }
        }

        protected void ChangedDropDownYear(object sender, EventArgs e)
        {
            var year = YearDrop.SelectedValue;
            weatherFromSQLs.Clear();
            var bs = new BL();
            weatherFromSQLs = bs.GetAnyYear(int.Parse(year));
            bs.UpdateMonthsOfYear(year, MonthDrop);
            var month = bs.MonthNameToMonthNum( MonthDrop.SelectedValue);
            bs.UpdateDaysOfMonth(month,DayDrop,int.Parse(year));
            Updating();
        }
    }
}