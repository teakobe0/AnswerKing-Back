using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
   public class Notice:BaseModel
    {
        public int InitiatorId { get; set; } //发送人id
        public int RecipientId { get; set; } //接收人id
        public string ContentsUrl { get; set; } //通知内容 地址
        public int IsRead { get; set; } // 是否已读 0：未读，1：已读
    }
}
