using BusinessLayer;
using System;
using System.Web.Helpers;
using System.Web.UI.WebControls;
using WeatherData;
using System.Linq;
using System.Collections.Generic;
namespace Temperature
{
    public partial class Year : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var bs = new BL();
                bs.IntializeDropDownsForYear(Year123);
                UpdateGridAndChart();
            }
        }



        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateGridAndChart();
        }
        protected void Month_Click(object sender, EventArgs e)
        {
            Response.Redirect( (((Button)sender).ID + ".aspx" ).Replace("1",""));
        }
        protected void UpdateGridAndChart()
        {
            var bs = new BL();
            var weatherlist = bs.GetYear(int.Parse(Year123.SelectedValue));
            GridView1.DataSource = weatherlist;
            GridView1.DataBind();
            bs.CreateChartYear(ChartTemp, weatherlist);

        }

        protected void Year_SelectedIndexChanged(object sender, EventArgs e)
        {
            var bs = new BL();
            var weatherlist = bs.GetLastYear();
            GridView1.DataSource = weatherlist;
            GridView1.DataBind();
            bs.CreateChartYear(ChartTemp, weatherlist);
        }
    }
}