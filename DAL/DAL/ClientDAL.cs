using DAL.IDAL;
using System;
using System.Collections.Generic;
using System.Text;
using DAL.Model;
using System.Linq;
using DAL.Model.Const;
using System.Collections;
using DAL.Tools;

namespace DAL.DAL
{
    public class ClientDAL : BaseDAL, IClientDAL
    {
        public ClientDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="pwd"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public Client GetClient(string Email, string pwd, out string errmsg)
        {
            errmsg = "";
            var client = _context.Client.FirstOrDefault(x => x.Email == Email && x.Password == pwd);
            if (client == null)
            {
                client = _context.Client.FirstOrDefault(x => x.Email == Email);
                if (client == null)
                    errmsg = "账号不存在";
                else
                    errmsg = "密码错误，请重新输入密码";
                return null;
            }

            return client;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int Register(Client client)
        {
            client.CreateTime = DateTime.Now;
            if (client.Inviterid != 0)
            {
                client.Role = C_Role.vip;
                client.EffectiveDate = DateTime.Now.AddDays(7);
                var inviter = _context.Client.FirstOrDefault(x => x.Id == client.Inviterid);
                if (inviter != null)
                {
                    inviter.Role = C_Role.vip;
                    inviter.EffectiveDate = inviter.EffectiveDate==DateTime.MinValue? DateTime.Now.AddDays(7): inviter.EffectiveDate.AddDays(7);
                }
            }
            else
            {
                client.Role = C_Role.guest;
            }
            client.Name = "ak_" +  DateTimeToUnixTimestamp(client.CreateTime);
            _context.Client.Add(client);
            return _context.SaveChanges();

        }
        public int RegisterBot(Client client)
        {
            client.CreateTime = DateTime.Now;
            _context.Client.Add(client);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int SaveImg(int ID,string url)
        {
            var client = _context.Client.FirstOrDefault(x => x.Id == ID);
            client.Image = url;
            return _context.SaveChanges();

        }
        /// <summary>
        /// 检验邮箱是否存在
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool GetEmail(string email)
        {
            return _context.Client.Any(x => x.Email == email);
        }
        /// <summary>
        /// 根据邮箱查询客户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Client GetClientByEmail(string email)
        {
            return _context.Client.FirstOrDefault(x => x.Email==email);
        }
        /// <summary>
        /// 查询真实客户列表 根据条件
        /// </summary>
        /// <returns></returns>
        public List<Client> GetList(string search)
        {
            var list = GetListData().Where(x => x.Role != "bot");
            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Name.Contains(search) || x.Email.Contains(search));

            return list.ToList();

        }
        /// <summary>
        /// 查询机器人列表 根据条件
        /// </summary>
        /// <returns></returns>
        public List<Client> GetBotList(string search)
        {
            var list = GetListData().Where(x => x.Role == "bot");
            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Name.Contains(search) || x.Email.Contains(search));

            return list.ToList();
        }
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <returns></returns>
        public Client GetClientById(int id)
        {
            return _context.Client.FirstOrDefault(x => x.Id == id);
        }
        /// <summary>
        /// 修改客户有效期
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        public int ChangeEffectiveDate(int clientid,DateTime date)
        {
            var client = _context.Client.FirstOrDefault(x => x.Id == clientid);
            client.Role =C_Role.vip;
            client.EffectiveDate = date;
            return _context.SaveChanges();
        }
        /// <summary>
        /// 修改客户身份及有效期
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        public Client ChangeEffectiveDate(int clientid)
        {
            var client = _context.Client.FirstOrDefault(x => x.Id == clientid);
            client.Role = C_Role.guest;
            client.EffectiveDate = DateTime.MinValue;
            _context.SaveChanges();
            return client;
        }

        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        public List<Client> GetList()
        {
            return GetListData().ToList();
        }

        private IQueryable<Client> GetListData()
        {
            return _context.Client.Where(x => 1 == 1);
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
            var client = _context.Client.FirstOrDefault(x => x.Id == ID);
            if (client.Password == NewPassword)
            {
                errmsg = "新密码与旧密码重复，请重新设置";
                return 0;
            }
            client.Password = NewPassword;
            return _context.SaveChanges();
        }
        /// <summary>
        /// 修改客户资料
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public int ChangeClientInfo(int ID, Client client, out string errmsg)
        {
            errmsg = "";
            bool nick = _context.Client.Any(x => x.Name == client.Name && x.Id != ID);
            if (nick)
            {
                errmsg = "该昵称已经存在，请重新修改.";
                return 0;
            }
            var data = _context.Client.FirstOrDefault(x => x.Id == ID);
            data.Name = client.Name;
            data.QQ = client.QQ;
            data.Sex = client.Sex;
            data.Tel = client.Tel;
            data.Birthday = client.Birthday;
            data.School = client.School;
            return _context.SaveChanges();
        }
        /// <summary>
        /// 修改客户(后台)
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int ChangeClientInfo(Client client, out string errmsg)
        {
            errmsg = "";
            bool nick = _context.Client.Any(x => x.Name == client.Name && x.Id != client.Id);
            if (nick)
            {
                errmsg = "该昵称已经存在，请重新修改.";
                return 0;
            }
            var data = _context.Client.FirstOrDefault(x => x.Id == client.Id);
            data.Name = client.Name;
            data.QQ = client.QQ;
            data.Sex = client.Sex;
            data.Tel = client.Tel;
            data.Birthday = client.Birthday;
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
                var client = _context.Client.FirstOrDefault(x => x.Id == id);
                Utils.WriteInfoLog("Client:Delete" + client.ToJson());
                client.IsDel = true;
                _context.Client.Update(client);
                return _context.SaveChanges();
            }
            return 0;
        }

    }
}

