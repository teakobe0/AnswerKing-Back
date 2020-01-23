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
    [Route("api/ClassTest")]
    public class ClassTestController : BaseController
    {
        private IClassTestDAL _clatdal;
        private IClassInfoTestDAL _citdal;
        private IUniversityTestDAL _utdal;
        private IClientDAL _clientdal;
        public ClassTestController(IClassTestDAL clatdal, IClassInfoTestDAL citdal, IUniversityTestDAL utdal, IClientDAL clientdal)
        {
            _clatdal = clatdal;
            _citdal = citdal;
            _utdal = utdal;
            _clientdal = clientdal;
        }

        /// <summary>
        /// 根据学校id/课程名称检索课程
        /// </summary>
        /// <param name="universityid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ClassTests")]
        public ResultModel ClassTests(int universityTestId, string name)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _clatdal.GetList(universityTestId, name).Take(10);
               
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 新增课程
        /// </summary>
        /// <param name="classTest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Add([FromBody] ClassTest classTest)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                ClassInfoTest cit = null;
                var classtest = classTest;
                classTest.ClientId = ID;
                if (!string.IsNullOrEmpty(classTest.Name))
                {
                    if (classTest.Id == 0)
                    {
                        //查询添加的课程是否存在
                        bool name = _clatdal.GetName(classTest.UniversityTestId, classTest.Name,0);
                        if (name == true)
                        {
                            r.Status = RmStatus.Error;
                            r.Msg = "该课程名称已经存在";
                        }
                        else
                        {
                            classtest = _clatdal.Add(classTest);
                            classTest.Id = classtest.Id;
                        }
                    }
                }
                else
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "课程名称不能为空";
                }
                //生成订单
                if (r.Msg == null)
                {
                    //该客户是否创建过该课程的题库集
                    var classInfoTest = _citdal.GetClassInfoTest(ID, classTest.Id);
                    if (classInfoTest==null)
                    {
                        cit = new ClassInfoTest();
                        cit.ClassTestId = classTest.Id;
                        cit.ClientId = classTest.ClientId;
                        var clientName = _clientdal.GetClientById(ID).Name;
                        cit.Name = clientName + "的题库集";
                        classInfoTest = _citdal.Add(cit);
                    }
                    r.Data = new { classtest, classInfoTest };
                }
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 根据课程id检索
        /// </summary>
        /// <param name="classTest"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClassTest")]
        public ResultModel GetClassTest(int id)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {

                var classTest = _clatdal.GeClassTest(id);
                var universityName = _utdal.GetUniversityTest(classTest.UniversityTestId) == null ? null : _utdal.GetUniversityTest(classTest.UniversityTestId).Name;
                r.Data = new { classTest, universityName };
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 编辑课程
        /// </summary>
        /// <param name="classTest"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Edit")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Edit([FromBody] ClassTest classTest)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                if (!string.IsNullOrEmpty(classTest.Name))
                {
                    if (classTest.ClientId == ID)
                    {
                        //查询添加的课程是否存在
                        bool name = _clatdal.GetName(classTest.UniversityTestId, classTest.Name, classTest.Id);
                        if (name == true)
                        {
                            r.Data = 0;
                            r.Status = RmStatus.Error;
                            r.Msg = "该课程名称已经存在";
                        }
                        else
                        {
                            r.Data = _clatdal.Edit(classTest);
                        }
                    }
                    else
                    {
                        r.Status = RmStatus.Error;
                        r.Msg = "你没有权限操作";
                    }
                }
                else
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "课程名称不能为空";
                }
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
                var clientid = _clatdal.GeClassTest(id).ClientId;
                if (clientid == ID)
                {
                    r.Data = _clatdal.Del(id);
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