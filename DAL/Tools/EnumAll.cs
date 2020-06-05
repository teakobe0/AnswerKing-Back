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
            /// 提交修改
            /// </summary>
            Edit = 5,
            /// <summary>
            /// 申请客服
            /// </summary>
            ForService = 6,
            /// <summary>
            /// 已完成
            /// </summary>
            Complete = 7,
            /// <summary>
            /// 已关闭
            /// </summary>
            Close = 8

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
    }
}
