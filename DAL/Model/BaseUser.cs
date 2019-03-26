using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
   public class BaseUser:BaseModel
    {
        public string Email { get; set; }//邮箱
        public string Password { get; set; }//密码
        public string Name { get; set; }//姓名
        public string QQ { get; set; }//qq
        public string Tel { get; set; }//手机号
        public string Role { get; set; }//角色
    }
}
