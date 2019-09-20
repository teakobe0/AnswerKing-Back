using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
   public class Notice:BaseModel
    {
        public int SendId { get; set; } //发送人id
        public int ReceiveId { get; set; } //接收人id
        public string ContentsUrl { get; set; } //通知内容 地址
        public int IsRead { get; set; } // 是否已读 0：未读，1：已读
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }
}
