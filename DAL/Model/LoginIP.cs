using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    /// <summary>
    /// 登录ip表
    /// </summary>
    public class LoginIP : BaseModel
    {
        public string IP { get; set; }//登录ip
        public int ClientId { get; set; } //客户id
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }
}
