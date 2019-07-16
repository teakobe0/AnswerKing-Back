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
    [Route("api/Class")]
    public class ClassController : BaseController
    {
        private IClassDAL _cladal;
        private IClassInfoDAL _clindal;

        public ClassController(IClassDAL cladal, IClassInfoDAL clindal)
        {
            _cladal = cladal;
            _clindal = clindal;
        }

      
        /// <summary>
        /// 根据课程名称检索 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ClassPage")]
        public ResultModel ClassPage(string name, int pagenum = 1, int pagesize = 40)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                List<cinfo> ls = new List<cinfo>();
                cinfo c = null;
                var query = _cladal.GetList(name);
                var model = query.Skip(pagesize * (pagenum - 1)).Take(pagesize).ToList();
                foreach (var item in model)
                {
                    c = new cinfo();
                    c.cla = item;
                    var classinfo = _clindal.GetList(item.Id);
                    c.order = classinfo.Count();
                    ls.Add(c);
                }
                page.Data = ls;
                page.PageTotal = query.Count();
                r.Data = page;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        public class cinfo
        {
            public Class cla { get; set; }
            public int order { get; set; }
        }
        /// <summary>
        /// 根据学校id/课程首字母/课程名称检索课程
        /// </summary>
        /// <param name="universityid"></param>
        /// <param name="alif"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Class")]
        public ResultModel Class(int universityid, string alif, string name)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                List<cinfo> ls = new List<cinfo>();
                cinfo info = null;
                var cla = _cladal.GetList(universityid, alif, name);
                foreach (var item in cla)
                {
                    info = new cinfo();
                    info.cla = item;
                    var clas = _clindal.GetList(item.Id);
                    info.order = clas.Count();
                    ls.Add(info);
                }
                r.Data = ls;

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 根据课程id检索课程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClass")]
        public ResultModel GetClass(int id)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _cladal.GetClass(id);
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