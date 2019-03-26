using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IUserDAL
    {
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="pwd"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        User GetUser(string Email, string pwd, out string errmsg);
        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        int Register(User user);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        /// 校验邮箱是否存在
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool GetEmail(string email);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="NewPassword"></param>
        /// <returns></returns>
        int ChangePassword(int ID, string NewPassword, out string errmsg);
        /// <summary>
        /// 修改用户资料
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        int ChangeUserInfo(int ID, User user);
        int ChangeUserInfo( User user);
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<User> GetList(int id);
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        List<User> GetListBySearch(string search);
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        List<User> GetList();
        /// <summary>
        /// 根据id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User Get(int id);
    }
}
