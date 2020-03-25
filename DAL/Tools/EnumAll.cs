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
            /// 已回答
            /// </summary>
            Answer = 3,
            /// <summary>
            /// 提交修改
            /// </summary>
            Edit = 4,
            /// <summary>
            /// 申请客服
            /// </summary>
            ForService = 5,
            /// <summary>
            /// 已完成
            /// </summary>
            Complete = 6

        }
    }
}
