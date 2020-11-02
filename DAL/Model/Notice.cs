using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    /// <summary>
    /// 通知表
    /// </summary>
    public class Notice : BaseModel
    {
        public int SendId { get; set; } //发送人id
        public int ReceiveId { get; set; } //接收人id
        public string ContentsUrl { get; set; } //通知内容
        public string ContentsId { get; set; } //内容id,type=1,id:classid,classinfocontentid;type=3,id:questionid
        public int IsRead { get; set; } // 是否已读 0：未读，1：已读
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
        public int Type { get; set; } //0:系统通知 1：评论通知 2：聊天内容通知 3:问题评论通知
    }
}
