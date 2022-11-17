using DBLayer;
using System;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLayer;
namespace Temperature
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                var bs = new BL();
                bs.IntializeDropDowns(DropDownList1, DropDownList2, DropDownList3);
            }
        }

        protected void ChangedDropDown(object sender, EventArgs e)
        {
            var chang = (DropDownList)sender;
            Label1.Text += chang.SelectedIndex;
            var dbl = new DBL();
            GridView1.DataSource = dbl.UpdateGridViewOnDropDown(DropDownList1, DropDownList2, DropDownList3);
            GridView1.DataBind();
        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label1.Text += "aa";
        }
    }
}