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
    /// <summary>
    /// 反馈表控制层
    /// </summary>
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Feedback")]
    public class FeedbackController : BaseController
    {
        private IFeedbackDAL _fbackdal;
      
        public FeedbackController(IFeedbackDAL fbackdal)
        {
            _fbackdal = fbackdal;
        }
        /// <summary>
        /// 新增反馈
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public ResultModel Add([FromBody] Feedback feedback)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _fbackdal.Add(feedback);
                if ((int)r.Data == 0)
                {
                    r.Status = RmStatus.Error;
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