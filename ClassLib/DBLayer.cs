using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer
{
    public class DBL
    {
        static string connstr = "Data Source=DESKTOP-QJBSGQ4\\MSSQLSERVER01;Initial Catalog=Harvester_Muhanad;Persist Security Info=True;User ID=sa;Password=muhanad123";
        static SqlConnection conn = new SqlConnection(connstr);
        
        public void writeTempWithDate(float temp, float windspeed, float precipitation, float humidity)
        {
            conn.Open();
            var dt = DateTime.Now;
            var yr = dt.Year;
            var mnth = dt.Month;
            var day = dt.Day;
            var hour = dt.Hour;
            var cmd = new SqlCommand($"insert into TempData2 values({yr},{mnth},{day},{hour},{temp},{windspeed},{precipitation},{humidity})",conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public DataTable getReader(string query)
        {
            conn.Open();
            var cmd = new SqlCommand(query, conn);
            var reader = cmd.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);
            conn.Close();
            return dt;

        }
       
       
        public void editColumn(string key, string keyname, string values, string tablename)
        {
            conn.Open();
            var cmd = new SqlCommand($"update {tablename} set {values} where {keyname} = {key}", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
