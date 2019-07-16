using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JzAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/University")]
    public class UniversityController : BaseController
    {
        private IUniversityDAL _undal;
        private IAreaDAL _areadal;
        private IClassDAL _classdal;
        public UniversityController(IUniversityDAL undal, IAreaDAL areadal, IClassDAL classdal)
        {
            _undal = undal;
            _areadal = areadal;
            _classdal = classdal;
           
        }
        /// <summary>
        /// 根据学校名称检索 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("UniversitysPage")]
        public ResultModel UniversitysPage(string name, int pagenum = 1, int pagesize = 10)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;

            try
            {
                var queryList = _undal.GetList(name);
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
        /// 根据学校id检索学校
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUniversity")]
        public ResultModel GetUniversity(int id)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _undal.GetUniversity(id);
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
        public class uinfo
        {
            public University university { get; set; }
            public int number { get; set; }
        }
        /// <summary>
        /// 根据国家 州/省份检索学校
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUniversitys")]
        public ResultModel GetUniversitys(string name, string state)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {

                List<uinfo> ls = new List<uinfo>();
                uinfo uinfo = null;
                var university = _undal.GetByCountry(name, state);

                foreach (var item in university)
                {
                    uinfo = new uinfo();
                    uinfo.university = item;
                    var clas = _classdal.GetList(item.Id);
                    uinfo.number = clas.Count();
                    ls.Add(uinfo);
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
        /// 查询国家
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Countrys")]
        public ResultModel Countrys()
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _areadal.GetCountryList();
              
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;

            }
            return r;
        }
        /// <summary>
        /// 查询所有的州/省份
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("States")]
        public ResultModel States(string name)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _areadal.GetList(name);
              
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;

            }
            return r;
        }
       
    }
}