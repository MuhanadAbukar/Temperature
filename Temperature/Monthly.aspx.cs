using BusinessLayer;
using SQLWeather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Temperature
{
    public partial class Monthly : System.Web.UI.Page
    {

        public static List<WeatherFromSQL> weatherFromSQLs = new List<WeatherFromSQL>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var bs = new BL();
                bs.IntializeDropDownsForMonth(MonthDrop, YearDrop);
                weatherFromSQLs = bs.GetAnyYear(int.Parse(YearDrop.SelectedValue));
                UpdateGridAndChart();
            }
        }
        protected void ChangedDropDown(object sender, EventArgs e)
        {
            UpdateGridAndChart();

        }
        protected void UpdateGridAndChart()
        {
            var bs = new BL();
            var newmonth = bs.MonthNameToMonthNum(MonthDrop.SelectedValue);
            var weatherlist = weatherFromSQLs.Where(x => x.Month == newmonth).ToList();
            var report = bs.ParseWeatherDataToMonthReport(weatherlist);
            GridView2.DataSource = report;
            GridView2.DataBind();
            bs.CreateChartMonth(ChartTemp, report);

        }

        protected void Month_Click(object sender, EventArgs e)
        {
            Response.Redirect((((Button)sender).ID + ".aspx").Replace("1", ""));
        }

        protected void Increment_Click(object sender, EventArgs e)
        {
            if (MonthDrop.SelectedIndex < 12)
            {
                
                    MonthDrop.SelectedIndex++;
                    UpdateGridAndChart();

            }
        }

        protected void Descend_Click(object sender, EventArgs e)
        {
            if (MonthDrop.SelectedIndex >= 1)
            {

                MonthDrop.SelectedIndex--;
                UpdateGridAndChart();

            }
        }
        protected void Updating()
        {
            var bs = new BL();
            var DATA = weatherFromSQLs;
            var year = int.Parse(YearDrop.SelectedValue);
            var month = bs.MonthNameToMonthNum(MonthDrop.SelectedValue);
            var SelectedData = DATA.Where(x => x.Month == month).ToList();
            var weatherlist = bs.ParseWeatherDataToMonthReport(SelectedData);
            weatherlist.Reverse();
            GridView2.DataSource = weatherlist;
            GridView2.DataBind();
            if (weatherlist.Count > 0)
            {
                bs.CreateChartMonth(ChartTemp, weatherlist);
            }
            
        }
        protected void ChangedDropDownYear(object sender, EventArgs e)
        {
            var year = YearDrop.SelectedValue;
            weatherFromSQLs.Clear();
            var bs = new BL();
            weatherFromSQLs = bs.GetAnyYear(int.Parse(year));
            bs.UpdateMonthsOfYear(year,MonthDrop);
            UpdateGridAndChart();
        }
    }
}
