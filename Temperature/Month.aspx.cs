using BusinessLayer;
using System;
using System.Web.UI.WebControls;

namespace Temperature
{
    public partial class Month : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var bs = new BL();
                var weatherlist = bs.GetLastMonth();
                GridView1.DataSource = weatherlist;
                GridView1.DataBind();
                bs.CreateChartMonth(ChartTemp, weatherlist);
            }
        }



        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void Month_Click(object sender, EventArgs e)
        {
            Response.Redirect(((Button)sender).ID + ".aspx");
        }
    }
}