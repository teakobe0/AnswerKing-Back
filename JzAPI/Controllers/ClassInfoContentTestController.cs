using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.DAL;
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
                if (!string.IsNullOrEmpty(classInfoContentTest.Name))
                {
                    r.Data = _cictdal.Add(classInfoContentTest);
                }
                else
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "答案名称不能为空";
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
                    info.universityname = _utdal.GetUniversityTest(i.UniversityTestId).Name;
                    info.classname = _ctdal.GeClassTest(i.ClassTestId).Name;
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
        /// 根据客户id检索学校、课程、订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Action")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Action()
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            List<actioninfo> list = new List<actioninfo>();
            actioninfo actioninfo = null;
            try
            {
                //学校
                var universityTest = _utdal.GetList(ID);
                foreach (var i in universityTest)
                {
                    actioninfo = new actioninfo();
                    actioninfo.id = i.Id;
                    actioninfo.name = i.Name;
                    actioninfo.type = "学校";
                    actioninfo.CreateTime = i.CreateTime;
                    list.Add(actioninfo);
                }

                //课程
                var classTest = _ctdal.GetList(ID);
                foreach (var t in classTest)
                {
                    actioninfo = new actioninfo();
                    actioninfo.id = t.Id;
                    actioninfo.name = t.Name;
                    actioninfo.type = "课程";
                    actioninfo.CreateTime = t.CreateTime;
                    list.Add(actioninfo);
                }
                //课程资料(题库集）
                var classInfoTest = _citdal.GetList(ID);
                foreach (var e in classInfoTest)
                {
                    actioninfo = new actioninfo();
                    actioninfo.id = e.Id;
                    actioninfo.name = e.Name;
                    actioninfo.type = "题库集";
                    actioninfo.CreateTime = e.CreateTime;
                    list.Add(actioninfo);
                }
                r.Data = list.OrderByDescending(x => x.CreateTime);
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        public class actioninfo
        {
            public int id { get; set; }//id
            public string name { get; set; }//项目名称
            public string type { get; set; }//类别 
            public DateTime CreateTime { get; set; }//创建时间
        }

        /// <summary>
        /// 保存订单(题库集）名称
        /// </summary>
        /// <param name="classInfoContentTest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Save")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Save([FromBody] ClassInfoTest classInfoTest)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                if (!string.IsNullOrEmpty(classInfoTest.Name))
                {
                    r.Data = _citdal.Edit(classInfoTest);
                }
                else
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "题库集名称不能为空";
                }

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
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
                        r.Data = 0;
                        r.Status = RmStatus.Error;
                        r.Msg = "删除失败";
                    }
                }
                if (r.Msg == null)
                {
                    //转换为绝对路径
                    string path = System.IO.Path.GetFullPath("wwwroot/" + imgurl);
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
        /// <param name="classTest"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Edit")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Edit([FromBody] ClassInfoContentTest classInfoContentTest)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                if (classInfoContentTest.ClientId == ID)
                {
                    r.Data = _cictdal.Edit(classInfoContentTest);
                }
                else
                {
                    r.Data = 0;
                }
                if ((int)r.Data == 0)
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
    }
}