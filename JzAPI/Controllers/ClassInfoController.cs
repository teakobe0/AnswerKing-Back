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
    [Route("api/ClassInfo")]
    public class ClassInfoController : Controller
    {
        private IClassInfoDAL _clindal;
        public ClassInfoController(IClassInfoDAL clindal)
        {
            _clindal = clindal;
        }
        /// <summary>
        /// 根据课程id检索课程资料
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ClassInfos")]
        public ResultModel ClassInfos(int classid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;

            try
            {
                r.Data = _clindal.GetList(classid);
               
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;

            }
            return r;
        }

    }
}