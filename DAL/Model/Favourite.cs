using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    public class Favourite:BaseModel
    {
        public int ClientId { get; set; } //客户id
        public int QuestionId{ get; set; }//问题表id
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }
}
