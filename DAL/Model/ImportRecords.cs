using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
  public class ImportRecords:BaseModel
    {
        public int StartId { get; set; }//起始id
        public int EndId { get; set; }//截止id
        public string  Table { get; set; }//表名
    }
}
