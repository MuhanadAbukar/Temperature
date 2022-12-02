using BusinessLayer;
using System;
using System.Web.UI.WebControls;

namespace Temperature
{
    public partial class Monthly : System.Web.UI.Page
    {



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var bs = new BL();
                bs.IntializeDropDownsForMonth(MonthDrop, YearDrop);
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
            var weatherlist = bs.GetAnyMonth(newmonth);
            GridView2.DataSource = weatherlist;
            GridView2.DataBind();
            bs.CreateChartMonth(ChartTemp, weatherlist);

        }

        protected void Month_Click(object sender, EventArgs e)
        {
            Response.Redirect((((Button)sender).ID + ".aspx").Replace("1", ""));
        }

        protected void Increment_Click(object sender, EventArgs e)
        {
            if (MonthDrop.SelectedIndex < 12)
            {
                try
                {
                    MonthDrop.SelectedIndex++;
                    UpdateGridAndChart();


                }
                catch (ArgumentOutOfRangeException) { }

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
    }
}
