using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.IDAL;
using DAL.Model;
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
        /// <param name="clientid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Notices")]
        public ResultModel Notices(int clientid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _notdal.GetList(clientid);

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

    }
}