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

                //var max = weatherlist.Max(x => x.Max);
                //var maxhour = weatherlist.Where(x => x.Temperature == max).Select(x => x.Hour).ToList();
                //Max.Text = $"Max: {string.Join(" ", maxhour)}, {max}";
                //Min.Text = $"Min: {weatherlist.Min(x => x.Temperature)}";
                //Average.Text = $"Average: {weatherlist.Average(x => x.Temperature)}";
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