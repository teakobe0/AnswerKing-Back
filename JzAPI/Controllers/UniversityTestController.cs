using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
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
    [Route("api/UniversityTest")]
    public class UniversityTestController : BaseController
    {
        private IUniversityTestDAL _untdal;
        private IUniversityDAL _undal;

        public UniversityTestController(IUniversityTestDAL untdal, IUniversityDAL undal)
        {
            _untdal = untdal;
            _undal = undal;
        }
        /// <summary>
        /// 新增学校
        /// </summary>
        /// <param name="universityTest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Add([FromBody] UniversityTest universityTest)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                if (!string.IsNullOrEmpty(universityTest.Name))
                {
                    if (universityTest.Id == 0)
                    {
                        universityTest.ClientId = ID;
                        //新旧表合并之后，需要改回来
                        //查询添加的学校是否存在
                        //bool name = _untdal.GetName(universityTest.Name,0);
                        //if (name == true)
                        //{
                        //    r.Status = RmStatus.Error;
                        //    r.Msg = "该学校名称已经存在";
                        //}
                        var university = _untdal.GetUniversityTest(universityTest.Name);
                        if (university != null)
                        {
                            r.Data = university;
                            r.Status = RmStatus.Error;
                            r.Msg = "该学校名称已经存在";
                        }
                        else
                        {
                            r.Data = _untdal.Add(universityTest);
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
        /// <param name="universityTest"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Edit")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Edit([FromBody] UniversityTest universityTest)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                if (!string.IsNullOrEmpty(universityTest.Name))
                {
                    if (universityTest.ClientId == ID)
                    {
                        bool name = _untdal.GetName(universityTest.Name, universityTest.Id);
                        if (name == true)
                        {
                            r.Status = RmStatus.Error;
                            r.Msg = "该学校名称已经存在";
                        }
                        else
                        {
                            r.Data = _untdal.Edit(universityTest);
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
        [Route("UniversityTests")]
        public ResultModel UniversityTests(string name)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            //新旧表合并之后，需要改回来
            //try
            //{
            //    r.Data = _untdal.GetList(name).Take(10);

            //}
            try
            {
                var test = _untdal.GetList(name).Take(10);
                //r.Data = _untdal.GetList(name);
                var ls = _undal.GetList(name).Take(10);
                List<University> list= new List<University>();
                if (ls.Count() > 0)
                {
                    list = ls.Where(x => x.Name.Trim() == name.Trim()).ToList();
                    if (list.Count() > 0)
                    {
                        r.Data = list;
                    }
                }
                if (test.Count() == 0&& list.Count()==0)
                {
                    r.Data = ls;
                }
                if(list.Count() == 0 && test.Count() > 0)
                {
                    r.Data=test;
                }
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
        [Route("GetUniversityTest")]
        public ResultModel GetUniversityTest(int id)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
               
                r.Data = _untdal.GetUniversityTest(id);

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
                var clientid = _untdal.GetUniversityTest(id).ClientId;
                if (clientid == ID)
                {
                    r.Data = _untdal.Del(id);
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

    }
}