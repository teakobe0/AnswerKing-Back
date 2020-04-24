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
        private IClassDAL _cladal;
        private IUniversityDAL _udal;
        public ClassTestController(IClassTestDAL clatdal, IClassInfoTestDAL citdal, IUniversityTestDAL utdal, IClientDAL clientdal, IClassDAL cladal, IUniversityDAL udal)
        {
            _clatdal = clatdal;
            _citdal = citdal;
            _utdal = utdal;
            _clientdal = clientdal;
            _cladal = cladal;
            _udal = udal;
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
                var query = _clatdal.GetList(name).Where(x=>x.IsAudit==true);
                var model = query.Skip(pagesize * (pagenum - 1)).Take(pagesize).ToList();
                foreach (var item in model)
                {
                    c = new cinfo();
                    c.cla = item;
                    var classinfo = _citdal.GetLs(item.Id);
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
            public ClassTest cla { get; set; }
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
                var cla = _clatdal.GetList(universityid, alif, name).Where(x=>x.IsAudit==true);
                foreach (var item in cla)
                {
                    info = new cinfo();
                    info.cla = item;
                    var clas = _citdal.GetLs(item.Id);
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
        /// 根据学校id/课程名称检索课程
        /// </summary>
        /// <param name="universityid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ClassTests")]
        public ResultModel ClassTests(int universityTestId, int universityId, string name)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            //新旧表合并之后，需要改回来,需要的是已审核的数据
            //try
            //{
            //    r.Data = _clatdal.GetList(universityTestId, name).Take(10);

            //}
            //根据旧库学校id,查询当前课程
            try
            {
                List<ClassTest> ctls = new List<ClassTest>();
                List<Class> cls = new List<Class>();
                if (universityTestId != 0)
                {
                    ctls = _clatdal.GetList(universityTestId, name).Take(10).ToList();
                }
                if (universityId == 0)
                {
                    string universityname = _utdal.GetUniversityTest(universityTestId).Name;
                    universityId = _udal.Getbyname(universityname);
                }
                if (universityId != 0)
                {
                    var ls = _cladal.GetList(universityId, name).Take(10);
                    if (ls.Count() > 0)
                    {
                        cls = ls.Where(x => x.Name.Trim() == name.Trim()).ToList();
                        if(cls.Count() > 0)
                        {
                            r.Data = cls;
                        }
                    }
                }
                if (ctls.Count() == 0 && cls.Count() == 0)
                {
                    r.Data = _cladal.GetList(universityId, name).Take(10);
                }
                if (cls.Count() == 0 && ctls.Count() > 0)
                {
                    r.Data = ctls;
                }
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
                        bool name = _clatdal.GetName(classTest.UniversityTestId, classTest.Name, 0);
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
                    cit = new ClassInfoTest();
                    cit.ClassTestId = classTest.Id;
                    cit.ClientId = classTest.ClientId;
                    Random random = new Random();
                    int num = random.Next(10000000, 99999999);
                    cit.Name = "题库集" + num;
                    var classInfoTest = _citdal.Add(cit);

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