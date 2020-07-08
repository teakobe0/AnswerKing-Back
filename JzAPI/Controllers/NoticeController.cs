using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
using DAL.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DAL.Tools.EnumAll;

namespace JzAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Notice")]
    public class NoticeController : BaseController
    {
        private INoticeDAL _notdal;
        private IClientDAL _clientdal;
        public NoticeController(INoticeDAL notdal, IClientDAL clientdal)
        {
            _notdal = notdal;
            _clientdal = clientdal;
        }
        
        /// <summary>
        /// 根据客户id检索通知消息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Notices")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Notices()
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _notdal.GetList(ID);

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 修改通知状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("ChangeStatus")]
        public ResultModel ChangeStatus(int id)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _notdal.ChangeStatus(id);
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 新增通知
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Add([FromBody]Notice notice)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                notice.SendId = ID;
                r.Data = _notdal.Add(notice);

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 聊天记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ChatRecords")]
        [Authorize(Roles = C_Role.all)]
        public  ResultModel ChatRecords(int receiveid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            ninfo ninfo = null;
            List<ninfo> ls = new List<ninfo>();
            try
            {
                var client = _clientdal.GetClientById(ID);
                var answerclient = _clientdal.GetClientById(receiveid);
                string url = AppConfig.Configuration["imgurl"];
                var notices = _notdal.GetList().Where(x => x.Type == (int)noticeType.Chat);
                notices = notices.Where(x => (x.SendId == ID && x.ReceiveId == receiveid) || (x.SendId == receiveid && x.ReceiveId == ID)).OrderBy(x => x.CreateTime);
                foreach(var item in notices)
                {
                    ninfo = new ninfo();
                    ninfo.notice = item;
                    if (item.SendId == ID)
                    {
                        ninfo.img = !string.IsNullOrEmpty(client.Image) ? url + client.Image : client.Image;
                    }
                    else
                    {
                        ninfo.img = !string.IsNullOrEmpty(answerclient.Image) ? url + answerclient.Image : answerclient.Image;
                    }
                    ls.Add(ninfo);
                }
                r.Data = ls;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }

        public class ninfo
        {
            public Notice notice { get; set; } 
            public string img { get; set; } //图像
        }
    }
}