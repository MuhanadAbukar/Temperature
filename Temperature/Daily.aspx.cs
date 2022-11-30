using BusinessLayer;
using SQLWeather;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Temperature
{
    public partial class Daily : System.Web.UI.Page
    {
        public List<WeatherFromSQL> weatherFromSQLs = new List<WeatherFromSQL>();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var bs = new BL();
                bs.IntializeDropDowns(DayDrop, MonthDrop, YearDrop);
                var diff = (DateTime.Now.Day - 31)*-1;
                DayDrop.SelectedIndex = diff;
                var weatherlist = bs.UpdateGridViewOnDropDown(DayDrop, MonthDrop, YearDrop);
                GridView2.DataSource = weatherlist;
                GridView2.DataBind();
                if (weatherlist.Count > 0)
                {
                    bs.EnableTexts(weatherlist, Min, Max, Average, MaxHour, MinHour,ChartTemp);
                    bs.CreateChartDay(ChartTemp, weatherlist);
                }
                else
                {
                    bs.DisableTexts(Min, Max, Average, MaxHour, MinHour, ChartTemp);
                }
            }
        }
        protected void ChangedDropDown(object sender, EventArgs e)
        {
            var bs = new BL();
            var weatherlist = bs.UpdateGridViewOnDropDown(DayDrop, MonthDrop, YearDrop);
            GridView2.DataSource = weatherlist;
            GridView2.DataBind();
            if (weatherlist.Count > 0)
            {
                bs.EnableTexts(weatherlist, Min, Max, Average, MaxHour, MinHour, ChartTemp);
                bs.CreateChartDay(ChartTemp, weatherlist);

            }
            else
            {
                bs.DisableTexts(Min, Max, Average, MaxHour, MinHour, ChartTemp); ;
            }
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

            if (DayDrop.SelectedIndex < 30)
            {
                var bs = new BL();
                DayDrop.SelectedIndex++;
                var weatherlist = bs.UpdateGridViewOnDropDown(DayDrop, MonthDrop, YearDrop);
                GridView2.DataSource = weatherlist;
                GridView2.DataBind();
                Min.Text = DayDrop.SelectedIndex.ToString();
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

        }

        protected void Descend_Click(object sender, EventArgs e)
        {
            if (DayDrop.SelectedIndex > 1)
            {
                var bs = new BL();
                DayDrop.SelectedIndex--;
                var weatherlist = bs.UpdateGridViewOnDropDown(DayDrop, MonthDrop, YearDrop);
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
        }
    }
}