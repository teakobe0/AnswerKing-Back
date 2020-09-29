using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    /// <summary>
    /// 导入记录表
    /// </summary>
  public class ImportRecords:BaseModel
    {
        public int StartId { get; set; }//起始id
        public int EndId { get; set; }//截止id
        public string  Table { get; set; }//表名
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }
}
