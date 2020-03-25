using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    public class Bidding : BaseModel
    {

        public DateTime EndTime { get; set; }//截止时间
        public int Currency { get; set; }//货币
        public int QuestionId { get; set; }//问题id
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }
}
