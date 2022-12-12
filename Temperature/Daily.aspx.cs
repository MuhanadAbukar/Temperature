using BusinessLayer;
using SQLWeather;
using System;
using System.Collections.Generic;
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
                var year = YearDrop.SelectedItem.Text;
                weatherFromSQLs = bs.GetAnyYear(int.Parse(year));
                Updating();
            }
        }
        protected void ChangedDropDown(object sender, EventArgs e)
        {
            var bs = new BL();
            bs.IntializeDropDowns(DayDrop, MonthDrop, YearDrop);
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
            var year = int.Parse(YearDrop.SelectedItem.Text);
            var month = bs.MonthNameToMonthNum(MonthDrop.SelectedItem.Text);
            var day = int.Parse(DayDrop.SelectedItem.Text);
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
        }

        protected void ChangedDropDownYear(object sender, EventArgs e)
        {
            var year = YearDrop.SelectedItem.Text;
            weatherFromSQLs.Clear();
            var bs = new BL();
            weatherFromSQLs = bs.GetAnyYear(int.Parse(year));
        }
    }
}