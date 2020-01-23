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
    [Route("api/UniversityTest")]
    public class UniversityTestController : BaseController
    {
        private IUniversityTestDAL _untdal;

        public UniversityTestController(IUniversityTestDAL untdal)
        {
            _untdal = untdal;
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
                        //查询添加的学校是否存在
                        bool name = _untdal.GetName(universityTest.Name,0);
                        if (name == true)
                        {
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
            try
            {
                r.Data = _untdal.GetList(name).Take(10);
                
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
        /// <param name="classTest"></param>
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