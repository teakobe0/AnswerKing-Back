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
    [Route("api/ClassInfoTest")]
    public class ClassInfoTestController : BaseController
    {

        private IClassInfoTestDAL _citdal;
        private IUniversityTestDAL _utdal;
        private IClassTestDAL _ctdal;
        public ClassInfoTestController(IClassInfoTestDAL citdal, IUniversityTestDAL utdal, IClassTestDAL ctdal)
        {

            _citdal = citdal;
            _utdal = utdal;
            _ctdal = ctdal;
        }

        /// <summary>
        /// 编辑订单(题库集）名称
        /// </summary>
        /// <param name="classInfoContentTest"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Edit")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Edit([FromBody] ClassInfoTest classInfoTest)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                if (!string.IsNullOrEmpty(classInfoTest.Name))
                {
                    if (classInfoTest.ClientId == ID)
                    {
                        r.Data = _citdal.Edit(classInfoTest);
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
                    r.Msg = "题库集名称不能为空";
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
            public ClassInfoTest cict { get; set; }
            public string university { get; set; }
            public string clas { get; set; }

        }
        /// <summary>
        /// 根据题库集id检索
        /// </summary>
        /// <param name="classTest"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClassInfoTest")]
        public ResultModel GetClassInfoTest(int id)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                info info = new info();
                info.cict = _citdal.GetClassInfoTest(id);
                var cla = _ctdal.GeClassTest(info.cict.ClassTestId);
                info.clas = cla.Name;
                info.university = _utdal.GetUniversityTest(cla.UniversityTestId).Name;
                r.Data = info;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 删除题库集
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
                var clientid = _citdal.GetClassInfoTest(id).ClientId;
                if (clientid == ID)
                {
                    r.Data = _citdal.Del(id);
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
        /// 更改题库集状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Change")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Change(int id)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                var clientid = _citdal.GetClassInfoTest(id).ClientId;
                if (clientid == ID)
                {
                    r.Data = _citdal.Change(id);
                }
                else
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "你没有权限操作";
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