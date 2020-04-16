using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DAL.Sql
{
   public class SqlHelper
    {
        private SqlConnection conn;
        string path = AppConfig.Configuration["sql"];
        public DataTable GetList(string table,int maxid,int type)
        {
            conn = new SqlConnection(path);
            conn.Open();
            string sql = "";
           
            if (type != 0)
            {
                sql = " select * from " + table + " where id> " + maxid + "and ClassId!=-99 and CwtParentId!=0 and Url is not null and Url !=''and Status=0 order by id";
            }
            else
            {
                sql = "select  * from " + table + " where id> " + maxid + " order by id";

            }
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            dataAdapter.SelectCommand = command;
            int count = dataAdapter.Fill(dt);
           
            return dt;
        }
    }
}
