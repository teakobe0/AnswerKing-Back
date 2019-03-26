using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PgDataToSql
{
    public partial class Form1 : Form
    {
        public Form1()
        {
           
            InitializeComponent();
        }
        JzDBEntities entity = new JzDBEntities();


        private void btnexport_Click(object sender, EventArgs e)
        {
            string imgHost = ConfigurationManager.AppSettings["ImgHost"];
            int sussNum = 0;
            int errNum = 0;
            lblmsg.Text = "";
            if (lvFile.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择文件");
                return;
            }
            if (lvDb.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择表名");
                return;
            }
            try
            {

                DataTable dt = OpenCSV(lvFile.SelectedItems[0].Name);

                string tbname = lvDb.SelectedItems[0].Text;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        string c_pgvalue = "";
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (dr[i].ToString() != "")
                                c_pgvalue += "," + dr[i].ToString().Replace("\"", "");
                        }
                        c_pgvalue = c_pgvalue != "" ? c_pgvalue.Substring(1) : c_pgvalue;
                        //classweektype表
                        if (tbname == "ClassWeekType")
                        {
                            ClassWeekType rowvalue = Json.ToObject<ClassWeekType>(c_pgvalue);
                            rowvalue.ClassWeekId = rowvalue.moduleid;
                            rowvalue.ClassWeekTypeId = rowvalue.modulesContentid;
                            entity.ClassWeekType.Add(rowvalue);
                        }

                        //University表
                        else if (tbname == "University")
                        {
                            University rowvalue = Json.ToObject<University>(c_pgvalue);
                            entity.University.Add(rowvalue);
                        }

                        //class表
                        else if (tbname == "Class")
                        {
                            Class rowvalue = Json.ToObject<Class>(c_pgvalue);
                            entity.Class.Add(rowvalue);
                        }

                        //ClassInfo表
                        else if (tbname == "ClassInfo")
                        {
                            ClassInfo rowvalue = Json.ToObject<ClassInfo>(c_pgvalue);
                            entity.ClassInfo.Add(rowvalue);
                        }

                        //ClassWeek表
                        else if (tbname == "ClassWeek")
                        {
                            ClassWeek rowvalue = Json.ToObject<ClassWeek>(c_pgvalue);
                            rowvalue.ClassInfoId = rowvalue.classorderid;
                            entity.ClassWeek.Add(rowvalue);
                        }
                        //ClassInfoContent表
                        else if (tbname == "ClassInfoContent")
                        {
                            ClassInfoContent rowvalue = Json.ToObject<ClassInfoContent>(c_pgvalue);
                            rowvalue.ClassWeekId = rowvalue.moduleid;
                            rowvalue.ClassWeekTypeId = rowvalue.modulesContentid;
                            rowvalue.Url = rowvalue.Contents;
                            rowvalue.Contents = "";

                            var API_KEY = "FYPPSLIAYq014ObvG32MrCOk";
                            var SECRET_KEY = "LBmHcH7jerHQL8OCfwsMVnbDaIHtPGEv ";
                            if (rowvalue.Url != "" && rowvalue.Url != null)
                            {
                                string imgurl = rowvalue.Url;
                                var client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
                                client.Timeout = 60000;  // 修改超时时间

                                foreach (var item in imgurl.Split('|'))
                                {
                                    var result = AccurateBasicDemo(client, imgHost + item.Replace('/', '\\'));
                                    JObject json1 = (JObject)JsonConvert.DeserializeObject(result.ToString());
                                    JArray array = (JArray)json1["words_result"];

                                    string text = "";

                                    foreach (var jObject in array)
                                    {
                                        //赋值属性
                                        text = jObject["words"].ToString();//获取字符串中内容
                                        rowvalue.Contents += text;
                                    }
                                }

                            }

                            entity.ClassInfoContent.Add(rowvalue);
                        }

                        entity.SaveChanges();
                        sussNum += 1;
                      //  lblmsg.Text = "导入成功！";
                    }
                    catch(Exception ex) {
                        errNum += 1;
                        entity = new JzDBEntities();
                        continue;
                    }
                }
               
                lblmsg.Text = "导入成功："+sussNum+"条，错误数据："+errNum+"条";

            }
            catch (Exception ex)
            {
                lblmsg.Text = "导入失败！";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            lvDb.Clear();
            lvFile.Clear();
            //加载文件list
            DirectoryInfo root = new DirectoryInfo(Environment.CurrentDirectory);
            FileInfo[] files = root.GetFiles();
            foreach (var item in files)
            {
                if (item.Name.Contains(".csv"))
                {
                    ListViewItem li = new ListViewItem();
                    li.Name = item.FullName;
                    li.Text = item.Name;
                    lvFile.Items.Add(li);
                }
            }
            //加载表名list

            var tbname = entity.Database.SqlQuery<string>("select name from sysobjects where xtype='u'").ToList();
            foreach (var tb in tbname)
            {
                ListViewItem li = new ListViewItem();
                li.Name = tb;
                li.Text = tb;
                lvDb.Items.Add(li);
            }
        }


        private void lvFile_Click(object sender, EventArgs e)
        {
            if (lvFile.SelectedItems.Count > 0)
            {
                lblfile.Text = lvFile.SelectedItems[0].Text;
            }
        }

        private void lvDb_Click(object sender, EventArgs e)
        {
            if (lvDb.SelectedItems.Count > 0)
            {
                lbldb.Text = lvDb.SelectedItems[0].Text;
            }
        }
        #region CSV
        /// <summary>
        /// 读取csv
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable OpenCSV(string filePath)
        {
            System.Text.Encoding encoding = GetType(filePath); //Encoding.ASCII;//
            DataTable dt = new DataTable();
            System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open,
                System.IO.FileAccess.Read);

            System.IO.StreamReader sr = new System.IO.StreamReader(fs, encoding);

            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }

                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }

            sr.Close();
            fs.Close();
            return dt;
        }
        /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
        /// <param name="FILE_NAME">文件路径</param>
        /// <returns>文件的编码类型</returns>

        public static System.Text.Encoding GetType(string FILE_NAME)
        {
            System.IO.FileStream fs = new System.IO.FileStream(FILE_NAME, System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            System.Text.Encoding r = GetType(fs);
            fs.Close();
            return r;
        }
        /// 通过给定的文件流，判断文件的编码类型
        /// <param name="fs">文件流</param>
        /// <returns>文件的编码类型</returns>
        public static System.Text.Encoding GetType(System.IO.FileStream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
            System.Text.Encoding reVal = System.Text.Encoding.Default;

            System.IO.BinaryReader r = new System.IO.BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = System.Text.Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = System.Text.Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = System.Text.Encoding.Unicode;
            }
            r.Close();
            return reVal;
        }
        /// 判断是否是不带 BOM 的 UTF8 格式
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;  //计算当前正分析的字符应还有的字节数
            byte curByte; //当前分析的字节.
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X　
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }
        #endregion

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }


        public Newtonsoft.Json.Linq.JObject AccurateBasicDemo(Baidu.Aip.Ocr.Ocr client, string imgurl)
        {
            var image = System.IO.File.ReadAllBytes(imgurl);
            // 调用通用文字识别（高精度版），可能会抛出网络等异常，请使用try/catch捕获
            var result = client.AccurateBasic(image);
            Console.WriteLine(result);
            // 如果有可选参数
            var options = new Dictionary<string, object>{
        {"detect_direction", "true"},
        {"probability", "true"}
    };
            // 带参数调用通用文字识别（高精度版）
            result = client.AccurateBasic(image, options);
            return result;
        }
    }
}
