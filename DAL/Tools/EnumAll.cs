using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Tools
{
    public class EnumAll

    {
        public enum questionStatus
        {

            /// <summary>
            /// 保存
            /// </summary>
            Save = 1,
            /// <summary>
            /// 正在竞拍
            /// </summary>
            Bidding = 2,
            /// <summary>
            /// 已选竞拍者
            /// </summary>
            Choose = 3,
            /// <summary>
            /// 已回答
            /// </summary>
            Answer = 4,
            /// <summary>
            /// 申请客服
            /// </summary>
            ForService = 5,
            /// <summary>
            /// 已完成
            /// </summary>
            Complete = 6,
            /// <summary>
            /// 已关闭
            /// </summary>
            Close = 7

        }
        public enum classInfoStatus
        {
            /// <summary>
            /// 未创建
            /// </summary>
            NoCreate = 0,
            /// <summary>
            /// 全新未审核
            /// </summary>
            NoAudit = 1,
            /// <summary>
            /// 已审核
            /// </summary>
            Audited = 2,
            /// <summary>
            /// 修改未审核
            /// </summary>
            Edit = 3
        }
        public  enum noticeType
        {
            /// <summary>
            /// 系统通知
            /// </summary>
            System=0,
            /// <summary>
            /// 评论通知
            /// </summary>
            Comment=1,
            /// <summary>
            /// 聊天内容通知
            /// </summary>
            Chat=2
        }
    }
}
