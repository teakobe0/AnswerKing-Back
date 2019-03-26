using DAL.IDAL;
using System;
using System.Collections.Generic;
using System.Text;
using DAL.Model;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DAL.Model.Const;
using DAL.Tools;

namespace DAL.DAL
{
    public class UserDAL : BaseDAL, IUserDAL
    {
        public UserDAL(DataContext context)
        {
            _context = context;
        }
        public User GetUser(string Email, string pwd, out string errmsg)
        {
            errmsg = "";
            var user = _context.User.FirstOrDefault(x => x.Email == Email && x.Password == pwd);
            if (user == null)
            {
                user = _context.User.FirstOrDefault(x => x.Email == Email);
                if (user == null)
                    errmsg = "账号不存在";
                else
                    errmsg = "密码错误，请重新输入密码";
                return null;
            }
            return user;
        }
        //注册
        public int Register(User user)
        {
            user.CreateTime = DateTime.Now;
            user.isTerminated = false;
            user.Role = C_Role.admin;

            _context.User.Add(user);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            if (id != 0)
            {
                var user = _context.User.FirstOrDefault(x => x.Id == id);
                Utils.WriteInfoLog("User:Delete" + user.ToJson());
                _context.User.Remove(user);
                return _context.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// 校验注册邮箱是否存在
        /// </summary>
        /// <param name="email"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public bool GetEmail(string email)
        {
            return _context.User.Any(x => x.Email == email);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="NewPassword"></param>
        /// <returns></returns>
        public int ChangePassword(int ID, string NewPassword, out string errmsg)
        {
            errmsg = "";
            var user = _context.User.FirstOrDefault(x => x.Id == ID);
            if (user.Password == Utils.GetMD5(NewPassword))
            {
                errmsg = "新密码与旧密码重复，请重新设置";
                return 0;
            }

            user.Password = Utils.GetMD5(NewPassword) ;
            return _context.SaveChanges();
        }
        /// <summary>
        /// 修改用户资料
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int ChangeUserInfo(User user)
        {
            _context.User.Update(user);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 修改用户资料
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int ChangeUserInfo(int ID, User user)
        {
            var data = _context.User.FirstOrDefault(x => x.Id == ID);
            data.Tel = user.Tel;
            data.QQ = user.QQ;
            data.Group = user.Group;
            data.Email = user.Email;
            return _context.SaveChanges();
        }
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <returns></returns>
        public List<User> GetList(int id)
        {
            if (id != 0)
            {
                var list = GetListData();
                list = list.Where(x => x.Id == id);
                return list.ToList();
            }
            return null;
        }
        /// <summary>
        ///根据管理员id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User Get(int id)
        {
            return  _context.User.FirstOrDefault(x => x.Id == id);
            
        }
        /// <summary>
        ///  查询列表 根据条件
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<User> GetListBySearch(string search)
        {
            var list = GetListData();
            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Name.Contains(search) || x.Email.Contains(search) || x.Tel.Contains(search));

            return list.ToList();
        }
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        public List<User> GetList()
        {
            return GetListData().ToList();
        }

        private IQueryable<User> GetListData()
        {
            return _context.User.Where(x => 1 == 1);
        }
    }
}
