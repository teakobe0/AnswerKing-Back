using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DAL.Tools
{
   public class Utils
    {
        public static string GetMD5(string password)
        {
            return BitConverter.ToString(MD5.Create().ComputeHash(Encoding.Default.GetBytes(password))).Replace("-", "");
        }

        /// <summary>  
        /// 写入错误日志到文本文件  
        /// </summary>  
        public static void WriteErrorLog(string strMessage, string action = "")
        {
            DateTime time = DateTime.Now;
            string path = Directory.GetCurrentDirectory() + @"\Log\Error\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + time.ToString("yyyy-MM-dd") + ".txt";
            StringBuilder str = new StringBuilder();
            str.Append("Time:    " + time.ToString() + "\r\n");
            str.Append("Action:  " + action + "\r\n");
            str.Append("Message: " + strMessage + "\r\n");
            str.Append("-----------------------------------------------------------\r\n\r\n");
            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }

        /// <summary>
        /// 写重要点日志文件
        /// </summary>
        /// <param name="strMessage"></param>
        public static void WriteInfoLog(string strMessage)
        {
            DateTime time = DateTime.Now;
            string path = Directory.GetCurrentDirectory() + @"\Log\Info\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + time.ToString("yyyy-MM-dd") + ".txt";
            StringBuilder str = new StringBuilder();
            str.Append("Time:    " + time.ToString() + "\r\n");
            str.Append("Message: " + strMessage + "\r\n");
            str.Append("-----------------------------------------------------------\r\n\r\n");
            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }

        /// <summary>
        /// 写操作日志文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        public static void WriteOperationLog(string name,string model,string type, int id)
        {
            DateTime time = DateTime.Now;
            string path = Directory.GetCurrentDirectory() + @"\Log\Operation\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + time.ToString("yyyy-MM-dd") + ".txt";
            StringBuilder str = new StringBuilder();
            //str.Append(time.ToString()+"/t"+name+"----->"+model+"----->"+type+"----->"+"Id:"+id + "<br/>");
            str.Append(time.ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;" + "用户：" + "【"+name+"】 "+ "模块：" +"【"+ model+"】" + "操作：" + "【" + type + "】" + "数据：" + "【"+"Id=" + id + "】" + "<br/>");

            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }
        public static TOut TransReflection<TIn, TOut>(TIn tIn)
        {
            TOut tOut = Activator.CreateInstance<TOut>();
            var tInType = tIn.GetType();
            foreach (var itemOut in tOut.GetType().GetProperties())
            {
                var itemIn = tInType.GetProperty(itemOut.Name); ;
                if (itemIn != null)
                {
                    itemOut.SetValue(tOut, itemIn.GetValue(tIn));
                }
            }
            return tOut;
        }
    }
}
