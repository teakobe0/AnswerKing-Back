using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    public class Answer : BaseModel
    {
        public int QuestionId { get; set; }//问题id
        public string Content { get; set; }//内容
        public int Sign { get; set; }//标记 5:好评，1：差评,3：超时
        public bool IsAudit { get; set; }//是否审核 0：未审核，1：已审核
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }
}
