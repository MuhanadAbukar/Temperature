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
                bs.IntializeDropDownsForMonth(DropDownList1, DropDownList3);
                DropDownList1.SelectedIndex = 0;
                var newmonth = bs.ConvertMonthNameToMonthNumber(DropDownList1.SelectedValue);
                var weatherlist = bs.GetAnyMonth(newmonth);
                GridView2.DataSource = weatherlist;
                GridView2.DataBind();
                

            }
        }
        protected void ChangedDropDown(object sender, EventArgs e)
        {
            var bs = new BL();
            var newmonth = bs.ConvertMonthNameToMonthNumber(DropDownList1.SelectedValue);
            var weatherlist = bs.GetAnyMonth(newmonth);
            GridView2.DataSource = weatherlist;
            GridView2.DataBind();
            
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
            if (DropDownList1.SelectedIndex < 12)
            {
                try
                {
                    var bs = new BL();
                    DropDownList1.SelectedIndex++;
                    var newmonth = bs.ConvertMonthNameToMonthNumber(DropDownList1.SelectedValue);
                    var weatherlist = bs.GetAnyMonth(newmonth);
                    GridView2.DataSource = weatherlist;
                    GridView2.DataBind();
                    
                }
                catch(ArgumentOutOfRangeException) { }
                
            }
        }

        protected void Descend_Click(object sender, EventArgs e)
        {
            if (DropDownList1.SelectedIndex >= 1)
            {
                var bs = new BL();
                DropDownList1.SelectedIndex--;
                var newmonth = bs.ConvertMonthNameToMonthNumber(DropDownList1.SelectedValue);
                var weatherlist = bs.GetAnyMonth(newmonth);
                GridView2.DataSource = weatherlist;
                GridView2.DataBind();
                
            }
        }
    }
}
