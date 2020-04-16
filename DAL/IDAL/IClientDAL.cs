using DAL.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IClientDAL
    {
        /// <summary>
        /// 客户登录
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="pwd"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        Client GetClient(string Email, string pwd, out string errmsg);
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        int Register(Client client);
        /// <summary>
        /// 注册机器人
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        int RegisterBot(Client client);
        /// <summary>
        /// 检验邮箱是否存在
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool GetEmail(string email);
        /// <summary>
        /// 查询机器人列表带条件
        /// </summary>
        /// <returns></returns>
        List<Client> GetBotList(string search);
        /// <summary>
        /// 查询列表带条件
        /// </summary>
        /// <returns></returns>
        List<Client> GetList(string search);
        /// <summary>
        /// 根据客户id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Client GetClientById(int id);
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        List<Client> GetList();
        /// <summary>
        /// 修改客户有效期
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        int ChangeEffectiveDate(int clientid, DateTime date);
        /// <summary>
        /// 修改客户身份及有效期
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        Client ChangeEffectiveDate(int clientid);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="NewPassword"></param>
        /// <returns></returns>
        int ChangePassword(int ID, string NewPassword, out string errmsg);
        /// <summary>
        /// 修改客户资料
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        int ChangeClientInfo(int ID, Client client, out string errmsg);
        /// <summary>
        /// 修改客户资料（后台）
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        int ChangeClientInfo(Client client, out string errmsg);
        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        /// 根据邮箱查询客户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Client GetClientByEmail(string email);
        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        int SaveImg(int ID, string url);
        /// <summary>
        /// 修改用户为7天vip
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        Client ChangeVip(int clientid, out string errmsg);
        /// <summary>
        /// 积分兑换
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Client Exchange(int clientid, out string errmsg);
    }
}
