using DAL.Tools;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DAL.PgSql
{
    public class PgHelper
    {
        private NpgsqlConnection conn;
        string path = AppConfig.Configuration["pg"];

        public DataTable GetList(string table, int maxid)
        {
            conn = new NpgsqlConnection(path);
            conn.Open();
            string sql = "";
            sql = string.Format(@" select  * from " + table + " where id>" + maxid + " order by id");

            NpgsqlCommand command = new NpgsqlCommand(sql, conn);
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter();
            DataTable dt = new DataTable();
            dataAdapter.SelectCommand = command;
            int count = dataAdapter.Fill(dt);
            return dt;
        }
    }
}
