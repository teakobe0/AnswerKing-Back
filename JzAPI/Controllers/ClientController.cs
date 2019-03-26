using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.IDAL;
using DAL.Model;
using System.Collections;
using DAL.Tools;
using Microsoft.AspNetCore.Authorization;
using DAL.Model.Const;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;

namespace JzAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Client")]
    public class ClientController : BaseController
    {
        private IClientDAL _clidal;
        private readonly IConfiguration _configuration;
        public ClientController(IClientDAL clidal, IConfiguration configuration)
        {
            _configuration = configuration;
            _clidal = clidal;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        public ResultModel Register([FromBody] Client client)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                string msg = "";
                if (string.IsNullOrEmpty(client.Email))
                {
                    msg = "邮箱地址不能为空";
                }
                else if (!ValidateUtil.IsEmail(client.Email))
                {
                    msg = "邮箱格式不正确";
                }
                else if (_clidal.GetEmail(client.Email))
                {
                    msg = "此邮箱已被注册，请使用其他邮箱注册";
                }
                if (string.IsNullOrEmpty(client.Password))
                {
                    msg = "密码不能为空";
                }
                r.Msg = msg;
                if (msg == "")
                {
                    int data = _clidal.Register(client);
                    if (data > 0)
                    {
                        TokenParam param = new TokenParam();
                        param.Id = client.Id;
                        param.Username = client.Email;
                        param.Role = client.Role;
                        param.configuration = _configuration;

                        r.Data = getToken(param);
                    }
                    else
                    {
                        r.Data = 0;
                        r.Status = RmStatus.Error;
                    }
                }
                else
                {
                    r.Data = 0;
                    r.Status = RmStatus.Error;
                }
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public ResultModel Login([FromBody] TokenRequest request)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            //登录错误返回msg
            string login_errmsg = "";
            Client client = null;

            client = _clidal.GetClient(request.Username, request.Password, out login_errmsg);

            if (client != null)
            {
                TokenParam param = new TokenParam();
                param.Id = client.Id;
                param.Username = request.Username;
                param.Role = client.Role;
                param.configuration = _configuration;

                r.Data = getToken(param);
            }
            else
            {
                r.Msg = login_errmsg;
                r.Status = RmStatus.Error;
            }

            return r;

        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="NewPassword"></param>
        /// <param name="RepeatPwd"></param>
        /// <returns></returns>
        [Authorize(Roles = C_Role.vip_guest)]
        [HttpPut]
        [Route("ChangePwd")]
        public ResultModel ChangePwd(string NewPassword, string RepeatPwd)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            string errmsg = "";
            try
            {
                if ((!string.IsNullOrEmpty(NewPassword) && (!string.IsNullOrEmpty(RepeatPwd))))
                {
                    if (NewPassword == RepeatPwd)
                    {
                        r.Data = _clidal.ChangePassword(ID, NewPassword, out errmsg);
                        r.Msg = errmsg;
                        if (r.Msg != "" || (int)r.Data == 0)
                        {
                            r.Status = RmStatus.Error;
                        }
                    }
                    else
                    {
                        r.Msg = "两次输入的密码不一致";
                        r.Status = RmStatus.Error;
                        r.Data = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }

            return r;
        }

        /// <summary>
        /// 根据客户名称检索，分页
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ClientsPage")]
        public ResultModel ClientsPage(string search, int pagenum = 1, int pagesize = 10)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();

            r.Status = RmStatus.OK;
            try
            {
                var queryList = _clidal.GetList(search);
                page.Data = queryList.Skip(pagesize * (pagenum - 1)).Take(pagesize).ToList();
                page.PageTotal = queryList.Count();
                r.Data = page;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 修改个人资料
        /// put /api/clients/:id
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = C_Role.vip_guest)]
        [Route("Clients")]
        public ResultModel Clients([FromBody]Client client)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            string errmsg = "";
            r.Data = 1;
            try
            {
                _clidal.ChangeClientInfo(ID, client, out errmsg);
                r.Msg = errmsg;
                if (r.Msg != "")
                {
                    r.Data = 0;
                    r.Status = RmStatus.Error;
                }

            }
            catch (Exception ex)
            {
                r.Data = 0;
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 根据客户id检索
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = C_Role.vip_guest)]
        [Route("GetClient")]
        public ResultModel GetClient()
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _clidal.GetClientById(ID);
                if (r.Data == null)
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "查询失败";
                }
            }
            catch (Exception ex)
            {

                r.Status = RmStatus.Error;
            }
            return r;
        }

    }
}