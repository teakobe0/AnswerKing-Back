
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace DAL.Tools
{
    public class CSVTestHelper
    {

        public enum CsvType
        {
            universitytest = 1,
            classtest = 2,
            classinfotest = 3,
           
            classinfocontenttest = 4
        }
        public static void SaveCSV(DataTable dt, CsvType CsvType, string filePath)
        {

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullfilepath = filePath + CsvType.ToString() + ".csv";
            string newnamepath = filePath + CsvType.ToString() + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".csv";
            if (File.Exists(fullfilepath))
            {
                FileInfo myfile = new FileInfo(fullfilepath);
                myfile.CopyTo(newnamepath);
            }
            FileStream fs = new FileStream(fullfilepath, System.IO.FileMode.Create, System.IO.FileAccess.Write);

            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            string data = "";

            //写出列名称
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);

            //写出各行数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    var str = dt.Rows[i][j].ToString().Trim();
                    if (str.Contains(','.ToString()) || str.Contains('"'.ToString()) || str.Contains('\r'.ToString()) || str.Contains('\n'.ToString()))
                    {
                        str = string.Format("\"{0}\"", str.Replace(',', '，').Replace("\n", "\\N"));

                    }
                    data += str;
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();

        }
        public static DataTable OpenCSV(CsvType CsvType)
        {
            string fileName = System.IO.Path.GetFullPath("wwwroot/data/" + DateTime.Now.ToString("yyyy-MM-dd") + "\\" + CsvType.ToString() + ".csv");
            //string file= "C:/Users/admin/Desktop/akht/AK/Admin/wwwroot/data\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\" + CsvType.ToString() + ".csv";
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;

            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {

                aryLine = strLine.Split(',');
                if (IsFirst == true)
                {
                    IsFirst = false;
                    columnCount = aryLine.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(aryLine[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j].Replace('，', ',').Replace("\\N", "\n");
                    }
                    dt.Rows.Add(dr);
                }
            }

            sr.Close();
            fs.Close();
            return dt;
        }
        public static void SaveTxt(string file,string type)
        {
            if (!File.Exists(file))
            {
                FileStream fs1 = new FileStream(file, FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1, System.Text.Encoding.UTF8);
                sw.WriteLine(type+" "+DateTime.Now);//开始写入值
                sw.Close();
                fs1.Close();
            }
            else
            {
                string content = "";
                StreamReader sr = new StreamReader(file);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Split(' ')[0] == type)
                        continue;
                    else
                        content += line + ",";
                }
                content += type + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",";
                sr.Close();

                StreamWriter sw = new StreamWriter(file);
                foreach (var item in content.Split(','))
                {
                    if (item.Trim() != "")
                        sw.WriteLine(item);//开始写入值
                }

                sw.Close();
            }
        }

        public static string[]  ReadTxt(string file)
        {
            string[] arr = File.ReadAllLines(file, Encoding.UTF8);
            return arr;
        }

        public static bool IsExists(string file)
        {
            if (!File.Exists(file))
            {
                return false;
            }
            else
            {
                return true;
            }

        }
       

    }
}