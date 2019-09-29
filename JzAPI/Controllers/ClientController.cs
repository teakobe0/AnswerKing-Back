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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using JzAPI.tool;

namespace JzAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Client")]
    public class ClientController : BaseController
    {
        private IClientDAL _clidal;
        private IFocusDAL _focusdal;
        private ICommentDAL _comdal;
        private readonly IConfiguration _configuration;
        private IHostingEnvironment _environment;
        private IClassInfoDAL _clindal;
        private IUseRecordsDAL _urdal;
        private IClassDAL _clasdal;
        public ClientController(IClientDAL clidal, IConfiguration configuration, IHostingEnvironment environment, IFocusDAL focusdal, ICommentDAL comdal, IClassInfoDAL clindal, IUseRecordsDAL urdal, IClassDAL clasdal)
        {
            _configuration = configuration;
            _clidal = clidal;
            _environment = environment;
            _focusdal = focusdal;
            _comdal = comdal;
            _clindal = clindal;
            _urdal = urdal;
            _clasdal = clasdal;
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
        public ResultModel ChangePwd(string OldPassword, string NewPassword, string RepeatPwd)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            string errmsg = "";
            try
            {
                if ((!string.IsNullOrEmpty(OldPassword) && !string.IsNullOrEmpty(NewPassword) && (!string.IsNullOrEmpty(RepeatPwd))))
                {
                    var client = _clidal.GetClientById(ID);
                    if (client.Password != OldPassword)
                    {
                        r.Msg = "原密码输入错误";
                        r.Status = RmStatus.Error;
                        r.Data = 0;
                    }
                    else
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
                            r.Msg = "新密码与重复密码不一致";
                            r.Status = RmStatus.Error;
                            r.Data = 0;
                        }
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
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ForgetPwd")]
        public ResultModel ForgetPwd(string email)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                var client = _clidal.GetEmail(email);

                if (client)
                {
                    var id = _clidal.GetClientByEmail(email).Id;
                    Mail.SendEmail(email, id);
                }
                else
                {
                    r.Msg = "该邮箱不存在";
                    r.Status = RmStatus.Error;
                }
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
                r.Msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="NewPassword"></param>
        /// <param name="RepeatPwd"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("ResetPwd")]
        public ResultModel ResetPwd(string param, string NewPassword, string RepeatPwd)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            string errmsg = "";
            var decrip = DES.Decode(param);
            string[] array = decrip.Split("&");
            int id = int.Parse(array[0]);
            try
            {
                if ((!string.IsNullOrEmpty(NewPassword) && (!string.IsNullOrEmpty(RepeatPwd))))
                {
                    if (NewPassword == RepeatPwd)
                    {
                        r.Data = _clidal.ChangePassword(id, NewPassword, out errmsg);
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
        /// 上传图片
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [Authorize(Roles = C_Role.all)]
        [HttpPost]
        [Route("UploadImg")]
        public ResultModel UploadImg(IFormCollection collection)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            var files = collection.Files;
            long size = files.Sum(f => f.Length);
            var filePath = "";
            filePath = CheckDirectory();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string suffix = formFile.FileName.Substring(formFile.FileName.LastIndexOf("."));
                    var number = Guid.NewGuid().ToString();
                    string filename = number + suffix;
                    string pathImg = Path.Combine(filePath, filename);
                    try
                    {
                        using (var stream = new FileStream(pathImg, FileMode.CreateNew))
                        {
                            formFile.CopyTo(stream);
                        }
                        string cropimg = "/clientImg/" + filename.Replace(".", "_small.");
                        bool isCompress = IMGHelper.CompressImage(_environment.WebRootPath + "/clientImg/" + filename, _environment.WebRootPath + cropimg);
                        if (isCompress)
                        {
                            _clidal.SaveImg(ID, cropimg);
                            r.Data = cropimg;
                        }
                    }
                    catch (IOException e)
                    {
                        r.Msg = "该文件已存在！请重命名后重新上传";
                        r.Status = RmStatus.Error;
                    }
                }
            }
            return r;
        }

        private string CheckDirectory()
        {
            var filePath = Path.Combine(_environment.WebRootPath, "clientImg");
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
            return filePath;
        }
        /// <summary>
        /// 根据客户id检索客户的行为记录
        /// </summary>
        /// <param name="Clientid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Action")]
        public ResultModel Action(int Clientid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            List<actioninfo> ls = new List<actioninfo>();
            actioninfo ac = null;
            try
            {
                //有用、没用
                var usels = _urdal.GetUseRecords(Clientid);
                var classid = 0;
                foreach (var item in usels)
                {
                    ac = new actioninfo();
                    if (item.Check == 1)
                    {
                        ac.content = item.Type == "N" ? "点击无用" : "点击有用";
                    }
                    else
                    {
                        ac.content = item.Type == "N" ? "取消无用" : "取消有用";
                    }
                    classid = _clindal.GetClassInfo(item.ClassInfoId).ClassId;
                    ac.classname = _clasdal.GetClass(classid) == null ? null : _clasdal.GetClass(classid).Name;
                    ac.CreateTime = item.CreateTime;
                    ls.Add(ac);
                }
                //关注
                var focusls = _focusdal.GetListByClientid(Clientid,true);
                foreach (var i in focusls)
                {
                    ac = new actioninfo();
                    ac.content = i.Type == 1 ? "关注课程" : "关注题库";
                    ac.classname = i.Name;
                    ac.CreateTime = i.CreateTime;
                    ls.Add(ac);

                    if (i.CancelTime != DateTime.MinValue)
                    {
                        actioninfo accancel = new actioninfo();
                        accancel.content = i.Type == 1 ? "取消关注课程" : "取消关注题库";
                        accancel.classname = i.Name;
                        accancel.CreateTime = i.CancelTime;
                        ls.Add(accancel);
                    }
                }
                //评论
                var commentls = _comdal.GetListByClientid(Clientid);
                foreach (var t in commentls)
                {
                    ac = new actioninfo();
                    ac.content = t.IsDel == true ? "删除评论" : "发表评论";
                    classid = _clindal.GetClassInfo(t.ClassInfoId).ClassId;
                    ac.classname = _clasdal.GetClass(classid).Name;
                    ac.CreateTime = t.CreateTime;
                    ls.Add(ac);
                }
                ls = ls.OrderByDescending(x => x.CreateTime).ToList();
                r.Data = ls;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;

            }
            return r;
        }
        public class actioninfo
        {
            public string classname { get; set; } //课程名称
            public string content { get; set; }//行为内容
            public DateTime CreateTime { get; set; }//创建时间
        }
        /// <summary>
        /// 根据客户id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClientById")]
        public ResultModel GetClientById(int id)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _clidal.GetClientById(id);
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