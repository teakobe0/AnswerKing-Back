using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JzAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Notice")]
    public class NoticeController : BaseController
    {
        private INoticeDAL _notdal;

        public NoticeController( INoticeDAL notdal)
        {
            _notdal = notdal;
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
    }
}