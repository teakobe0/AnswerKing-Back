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
    /// <summary>
    /// 通知表控制层
    /// </summary>
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
        public ResultModel Notices(int pagenum = 1, int pagesize = 40)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                var model = _notdal.GetList(ID).OrderByDescending(x => x.Id);
                page.PageTotal = model.Count();
                page.Data = model.Skip(pagesize * (pagenum - 1)).Take(pagesize).ToList();
                r.Data = page;

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
                notice.Type = (int)noticeType.Chat;
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
        public ResultModel ChatRecords(int receiveid)
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
                var notices = from x in _notdal.GetList()
                              .Where(x => x.Type == (int)noticeType.Chat && (x.SendId == ID && x.ReceiveId == receiveid) || (x.SendId == receiveid && x.ReceiveId == ID))
                              select new
                              {
                                  x.ContentsUrl,
                                  x.CreateTime,
                                  x.SendId
                              };
                foreach (var item in notices)
                {
                    ninfo = new ninfo();
                    ninfo.contentsUrl = item.ContentsUrl;
                    ninfo.createTime = item.CreateTime;
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
            public string contentsUrl { get; set; }
            public DateTime createTime { get; set; }
            public string img { get; set; } //图像
        }
    }
}