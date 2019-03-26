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
    [Route("api/ClassWeek")]
    public class ClassWeekController : BaseController
    {
        private IClassWeekDAL _clwdal;
        public ClassWeekController(IClassWeekDAL clwdal)
        {
            _clwdal = clwdal;
        }
        /// <summary>
        /// 根据课程资料id检索每周课程 
        /// </summary>
        /// <param name="classinfoid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ClassWeeks")]
        public ResultModel ClassWeeks(int classinfoid)
        {
            ResultModel r = new ResultModel();
          
            r.Status = RmStatus.OK;
          
            try
            {
                r.Data = _clwdal.GetList(classinfoid);
              
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;

            }
            return r;
        }
    }
}