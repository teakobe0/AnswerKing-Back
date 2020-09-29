using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using Microsoft.AspNetCore.Authorization;
using DAL.Model.Const;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;

namespace JzAPI.Controllers
{
    /// <summary>
    /// 管路员表控制层
    /// </summary>
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : BaseController
    {
        private IUserDAL _udal;
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration,IUserDAL udal)
        {
            _configuration = configuration;
            _udal = udal;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        public ResultModel Register([FromBody] User user)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                string msg = "";

                if (string.IsNullOrEmpty(user.Email))
                {
                    msg += "邮箱地址不能为空;";
                }
                else if (!ValidateUtil.IsEmail(user.Email))
                {
                    msg += "邮箱格式不正确";
                }
                else if (_udal.GetEmail(user.Email))
                {
                    msg += "此邮箱已被注册，请使用其他邮箱注册";
                }

                if (string.IsNullOrEmpty(user.Password))
                {
                    msg += "密码不能为空;";
                }
                r.Msg = msg;
                if (msg == "")
                {
                    int data = _udal.Register(user);
                    if (data > 0)
                    {
                        TokenParam param = new TokenParam();
                        param.Id = user.Id;
                        param.Username = user.Email;
                        param.Role = user.Role;
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
            User user = null;

            user = _udal.GetUser(request.Username, Utils.GetMD5(request.Password), out login_errmsg);

            if (user != null)
            {
                TokenParam param = new TokenParam();
                param.Id = user.Id;
                param.Username = request.Username;
                param.Role = user.Role;
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
        [Authorize(Roles = C_Role.admin)]
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
                        r.Data = _udal.ChangePassword(ID, NewPassword, out errmsg);
                        r.Msg = errmsg;
                        if (r.Msg != ""||(int)r.Data==0)
                            r.Status = RmStatus.Error;
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
        /// 修改个人资料
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = C_Role.admin)]
        [Route("Users")]
        public ResultModel Users([FromBody] User user)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            r.Data = 1;
            try
            {
                  _udal.ChangeUserInfo(ID, user);
            }
            catch (Exception ex)
            {
                r.Data = 0;
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 根据管理员id检索
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = C_Role.admin)]
        [Route("GetUser")]
        public ResultModel GetUser()
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data =_udal.Get(ID);
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