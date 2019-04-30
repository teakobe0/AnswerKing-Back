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
    public class ClassInfoController : BaseController
    {
        private IClassInfoDAL _clindal;
        private IUseRecordsDAL _urdal;
        public ClassInfoController(IClassInfoDAL clindal, IUseRecordsDAL urdal)
        {
            _clindal = clindal;
            _urdal= urdal;
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
       /// <summary>
       /// 更改课程资料有用/没用
       /// </summary>
       /// <param name="classInfoId"></param>
       /// <param name="type">有用:Y,没用:N</param>
       /// <param name="check">选中:1，取消:-1</param>
       /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = C_Role.admin_vip)]
        [Route("ChangeClassInfo")]
        public ResultModel ChangeClassInfo(int classInfoId,string type,int check)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                 r.Data = _clindal.Change(ID, classInfoId, type, check);
               

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;

            }
            return r;
        }
        /// <summary>
        /// 根据课程资料id检索该课程资料有用、无用
        /// </summary>
        /// <param name="classInfoid"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = C_Role.all)]
        [Route("UseRecords")]
        public ResultModel UseRecords(int classInfoid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                
                r.Data = _urdal.GetUseRecords(ID, classInfoid);
               
          
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;

            }
            return r;
        }
    }
}