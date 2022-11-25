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
                bs.IntializeDropDowns(DropDownList1, DropDownList2, DropDownList3);
                DropDownList2.SelectedIndex = 30;
                var weatherlist = bs.UpdateGridViewOnDropDown(DropDownList1, DropDownList2, DropDownList3);
                GridView2.DataSource = weatherlist;
                GridView2.DataBind();
                if (weatherlist.Count > 0)
                {
                    bs.EnableTexts(weatherlist, Min, Max, Average, MaxHour, MinHour);
                }
                else
                {
                    bs.DisableTexts(Min, Max, Average, MaxHour, MinHour);
                }

            }
        }
        protected void ChangedDropDown(object sender, EventArgs e)
        {
            var bs = new BL();
            var weatherlist = bs.UpdateGridViewOnDropDown(DropDownList1, DropDownList2, DropDownList3);
            GridView2.DataSource = weatherlist;
            GridView2.DataBind();
            if (weatherlist.Count > 0)
            {
                bs.EnableTexts(weatherlist, Min, Max, Average, MaxHour, MinHour);
            }
            else
            {
                bs.DisableTexts(Min, Max, Average, MaxHour, MinHour);
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

            if (DropDownList2.SelectedIndex < 30 )
            {
                var bs = new BL();
                DropDownList2.SelectedIndex++;
                var weatherlist = bs.UpdateGridViewOnDropDown(DropDownList1, DropDownList2, DropDownList3);
                GridView2.DataSource = weatherlist;
                GridView2.DataBind();
                Min.Text = DropDownList2.SelectedIndex.ToString();
                if (weatherlist.Count > 0)
                {
                    bs.EnableTexts(weatherlist, Min, Max, Average, MaxHour, MinHour);
                }
                else
                {
                    bs.DisableTexts(Min, Max, Average, MaxHour, MinHour);
                }
            }

        }

        protected void Descend_Click(object sender, EventArgs e)
        {
            if (DropDownList2.SelectedIndex > 1)
            {
                var bs = new BL();
                DropDownList2.SelectedIndex--;
                var weatherlist = bs.UpdateGridViewOnDropDown(DropDownList1, DropDownList2, DropDownList3);
                GridView2.DataSource = weatherlist;
                GridView2.DataBind();
                if (weatherlist.Count > 0)
                {
                    bs.EnableTexts(weatherlist, Min, Max, Average, MaxHour, MinHour);
                }
                else
                {
                    bs.DisableTexts(Min, Max, Average, MaxHour, MinHour);
                }
            }
        }
    }
}