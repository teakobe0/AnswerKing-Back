using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Model
{
    /// <summary>
    /// 客户表
    /// </summary>
    public class Client : BaseUser
    {

        public string Sex { get; set; }//性别
        public DateTime Birthday { get; set; }//生日
        public DateTime EffectiveDate { get; set; }//有效期
        public string Image { get; set; }//客户图像
        public string School { get; set; }//学校
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
        public string Inviterid { get; set; }//邀请人id
        public bool IsValidate { get; set; }//是否验证 0：未验证 1：已验证
        public int Integral { get; set; } //积分
        public string IP { get; set; }//注册ip
        public string LoginIP { get; set; }//最近登录ip
        public DateTime UpdateTime { get; set; }//最近登录时间



    }
}
