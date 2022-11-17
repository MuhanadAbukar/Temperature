using DBLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace BusinessLayer
{
    public class BL
    {
        public void IntializeDropDowns(DropDownList DropDownList1, DropDownList DropDownList2, DropDownList DropDownList3)
        {
            var dbl = new DBL();
            var months = dbl.GetValidMonths();
            var months1 = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            int z = 0;
            foreach (DataRow dr in months.Rows)
            {
                DropDownList1.Items.Insert(z, new ListItem(months1[(int)dr[0] - 1], months1[(int)dr[0] - 1]));
                z++;

            }
            for (int i = 1; i <= 31; i++)
            {
                DropDownList2.Items.Insert(0, new ListItem(i.ToString(), i.ToString()));
            }
            var years = dbl.GetValidYears();
            z = 0;
            foreach (DataRow dr in years.Rows)
            {
                DropDownList3.Items.Insert(z, new ListItem(dr[0].ToString(), dr[0].ToString()));
                z++;
            }
        }
    }
}
