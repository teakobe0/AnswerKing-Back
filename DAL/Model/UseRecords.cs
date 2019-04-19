using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
   public class UseRecords:BaseModel
    {
        public int ClientId { get; set; }//客户id
        public int ClassInfoId { get; set; }//课程资料id
        public int Check{ get; set; }// 选中、取消  1：选中，-1：取消
        public string Type { get; set; }//类型  Y:有用，N：无用
       
          
    }
}
