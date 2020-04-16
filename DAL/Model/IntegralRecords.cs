using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
   public class IntegralRecords : BaseModel
    {
        public int ClientId { get; set; }//客户id
        public string Source { get; set; }//积分来源
        public int Integral { get; set; } //积分
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }
}
