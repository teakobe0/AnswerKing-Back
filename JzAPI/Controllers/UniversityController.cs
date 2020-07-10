using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
using DAL.Tools;
using JzAPI.tool;
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
        private IClassDAL _cladal;
        private IAreaDAL _areadal;

        public UniversityController(IUniversityDAL undal, IClassDAL cladal, IAreaDAL areadal)
        {
            _undal = undal;
            _cladal = cladal;
            _areadal = areadal;
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
                var queryList = _undal.GetList(name,-1).Where(x=>x.IsAudit==true);
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
        /// 新增学校
        /// </summary>
        /// <param name="university"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Add([FromBody] University university)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                if (!string.IsNullOrEmpty(university.Name))
                {
                    if (university.Id == 0)
                    {
                        university.ClientId = ID;
                        //查询添加的学校是否存在
                        bool name = _undal.GetName(university.Name, 0);
                        if (name == true)
                        {
                            r.Status = RmStatus.Error;
                            r.Msg = "该学校名称已经存在";
                        }
                        else
                        {  
                            //针对李龙添加的学校状态为已审核
                            if (university.ClientId == 553)
                            {
                                university.IsAudit = true;
                            }
                            r.Data = _undal.Add(university);
                        }

                    }
                }
                else
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "学校名称不能为空";
                }

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 编辑学校
        /// </summary>
        /// <param name="university"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Edit")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Edit([FromBody] University university)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                if (!string.IsNullOrEmpty(university.Name))
                {
                    if (university.ClientId == ID)
                    {
                        bool name = _undal.GetName(university.Name, university.Id);
                        if (name == true)
                        {
                            r.Status = RmStatus.Error;
                            r.Msg = "该学校名称已经存在";
                        }
                        else
                        {
                            r.Data = _undal.Edit(university);
                        }
                    }
                }
                else
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "学校名称不能为空";
                }
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 查询学校
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Universitys")]
        public ResultModel Universitys(string name)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _undal.GetList(name,-1).Where(x=>x.IsAudit==true).Take(10);

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 根据学校id检索
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
                var university = _undal.GetUniversity(id);
                if (!string.IsNullOrEmpty(university.Image))
                {
                    university.Image = AppConfig.Configuration["imgurl"] + university.Image;
                }
                r.Data = university;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Del")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Del(int id)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                var clientid = _undal.GetUniversity(id).ClientId;
                if (clientid == ID)
                {
                    r.Data = _undal.Del(id);
                }
                else
                {
                    r.Data = 0;
                }
                if ((int)r.Data == 0)
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "删除失败。";
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
                var university = _undal.GetByCountry(name, state).Where(x=>x.IsAudit==true);

                foreach (var item in university)
                {
                    uinfo = new uinfo();
                   
                    uinfo.university = item;
                    var clas = _cladal.GetLs(item.Id);
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
        /// <summary>
        /// 查询学校数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("UniversityNum")]
        public ResultModel UniversityNum()
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data =_undal.GetList().Count();

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;

            }
            return r;
        }
    }
}