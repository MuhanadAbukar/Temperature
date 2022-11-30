using BusinessLayer;
using System;
using System.Data;
using System.Linq;
using System.Web.Helpers;
using System.Web.UI.WebControls;

namespace Temperature
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                var bs = new BL();
                var weatherlist = bs.GetLast24Hours();
                GridView1.DataSource = weatherlist;
                GridView1.DataBind();
                bs.EnableTexts(weatherlist, Min, Max, Average, MaxHour, MinHour, ChartTemp);
                bs.CreateChartDay(ChartTemp, weatherlist);
                var currentTemp = weatherlist[weatherlist.Count-1].Temperature;
                h1.InnerHtml = $"Current Temperature: {currentTemp}";
            }
        }
        protected void ChangedDropDown(object sender, EventArgs e)
        {
            
        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void Month_Click(object sender, EventArgs e)
        {
            Response.Redirect((((Button)sender).ID + ".aspx").Replace("1", ""));
        }
    }
}