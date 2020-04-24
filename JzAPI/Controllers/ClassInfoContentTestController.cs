using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.DAL;
using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
using DAL.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JzAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/ClassInfoContentTest")]
    public class ClassInfoContentTestController : BaseController
    {
        private IClassInfoContentTestDAL _cictdal;
        private IUniversityTestDAL _utdal;
        private IClassTestDAL _ctdal;
        private IClassInfoTestDAL _citdal;
        public ClassInfoContentTestController(IClassInfoContentTestDAL cictdal, IUniversityTestDAL utdal, IClassTestDAL ctdal, IClassInfoTestDAL citdal)
        {
            _cictdal = cictdal;
            _utdal = utdal;
            _ctdal = ctdal;
            _citdal = citdal;
        }
        /// <summary>
        /// 新增答案
        /// </summary>
        /// <param name="classInfoContentTest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Add([FromBody] ClassInfoContentTest classInfoContentTest)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                if (!string.IsNullOrEmpty(classInfoContentTest.Url) || !string.IsNullOrEmpty(classInfoContentTest.Contents))
                {
                    classInfoContentTest.ClientId = ID;
                    r.Data = _cictdal.Add(classInfoContentTest);
                }
                else
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "答案图片/内容不能为空";
                }

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 根据订单id检索所有周
        /// </summary>
        /// <param name="classInfoTestId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Week")]
        public ResultModel Week(int classInfoTestId)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _cictdal.GetWeek(classInfoTestId);
                if (r.Data == null)
                {
                    r.Status = RmStatus.Error;
                }

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 根据客户id,订单id检索答案
        /// </summary>
        /// <param name="classInfoTestId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ClassInfoContentTests")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel ClassInfoContentTests(int classInfoTestId)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            List<info> ls = new List<info>();
            info info = null;
            try
            {
                var cict = _cictdal.GetLs(ID, classInfoTestId);
                foreach (var i in cict)
                {
                    info = new info();
                    info.ClassInfoContentTest = i;

                    i.NameUrl = AppConfig.Configuration["imgurl"] + i.NameUrl;
                    i.Url = AppConfig.Configuration["imgurl"] + i.Url;
                    info.universityname = _utdal.GetUniversityTest(i.UniversityTestId) == null ? null : _utdal.GetUniversityTest(i.UniversityTestId).Name;
                    info.classname = _ctdal.GeClassTest(i.ClassTestId) == null ? null : _ctdal.GeClassTest(i.ClassTestId).Name;
                    ls.Add(info);
                }
                r.Data = ls;
                if (r.Data == null)
                {
                    r.Status = RmStatus.Error;
                }
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        public class info
        {
            public ClassInfoContentTest ClassInfoContentTest { get; set; }
            public string universityname { get; set; }
            public string classname { get; set; }
        }


        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("RemoveImg")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel RemoveImg(int id, string imgurl)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                if (id != 0)
                {
                    //删除数据库图片
                    var clientid = _cictdal.GeClassInfoContentTest(id).ClientId;
                    if (clientid == ID)
                    {
                        r.Data = _cictdal.DelImg(id, imgurl);
                    }
                    else
                    {
                        r.Status = RmStatus.Error;
                        r.Msg = "你没有权限操作";
                    }
                }
                if (r.Msg == null)
                {
                    //转换为绝对路径
                    string path = AppConfig.Configuration["uploadurl"] + imgurl;
                    //删除本地
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 根据答案id检索
        /// </summary>
        /// <param name="classTest"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClassInfoContentTest")]
        public ResultModel GetClassInfoContentTest(int id)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                var ClassInfoContentTest = _cictdal.GeClassInfoContentTest(id);
                var universtiyname = _utdal.GetUniversityTest(ClassInfoContentTest.UniversityTestId) == null ? null : _utdal.GetUniversityTest(ClassInfoContentTest.UniversityTestId).Name;
                var classname = _ctdal.GeClassTest(ClassInfoContentTest.ClassTestId) == null ? null : _ctdal.GeClassTest(ClassInfoContentTest.ClassTestId).Name;
                r.Data = new { ClassInfoContentTest, universtiyname, classname };
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 删除答案
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
                var clientid = _cictdal.GeClassInfoContentTest(id).ClientId;
                if (clientid == ID)
                {
                    r.Data = _cictdal.Del(id);
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
        /// <summary>
        /// 编辑答案
        /// </summary>
        /// <param name="classInfoContentTest"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Edit")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Edit([FromBody] ClassInfoContentTest classInfoContentTest)
        {
            ResultModel r = new ResultModel();

            r.Status = RmStatus.OK;
            try
            {
                if (!string.IsNullOrEmpty(classInfoContentTest.Url) || !string.IsNullOrEmpty(classInfoContentTest.Contents))
                {
                    if (classInfoContentTest.ClientId == ID)
                    {
                        r.Data = _cictdal.Edit(classInfoContentTest);
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
                    r.Msg = "答案图片/内容不能为空";
                }
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }

        /// <summary>
        /// 根据每周课程id检索类型答案
        /// </summary>
        /// <param name="classweekid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Types")]
        public ResultModel Types(int weekname, int classinfoid)
        {
            ResultModel r = new ResultModel();

            r.Status = RmStatus.OK;

            try
            {
                var ls = _cictdal.Types(classinfoid, weekname);
                var img = AppConfig.Configuration["imgurl"];
             
                foreach (var item in ls)
                {
                   
                    if (!string.IsNullOrEmpty(item.Url))
                    {
                        item.Url = img + item.Url;
                    }
                    if (!string.IsNullOrEmpty(item.NameUrl))
                    {
                        item.NameUrl = img + item.NameUrl;
                    }
                   
                }
                r.Data =ls;

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;

            }
            return r;
        }
    }
}