using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model.Const
{
   public class C_Role
    {
        /// <summary>
        /// 公司所有人员角色
        /// </summary>
        public const string admin = "admin";

        /// <summary>
        /// 会员
        /// </summary>
        public const string vip = "vip";

        /// <summary>
        /// 访客
        /// </summary>
        public const string guest = "guest";

        /// <summary>
        /// 所有角色
        /// </summary>
        public const string all = "admin,vip,guest";

        /// <summary>
        /// 会员和访客
        /// </summary>
        public const string vip_guest = "vip,guest";
        /// <summary>
        /// 管理员和会员
        /// </summary>
        public const string admin_vip = "admin,vip";
    }
}
