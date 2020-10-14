using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
using DAL.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DAL.Tools.EnumAll;

namespace JzAPI.Controllers
{
    /// <summary>
    /// 题库集表控制层
    /// </summary>
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/ClassInfo")]
    public class ClassInfoController : BaseController
    {

        private IClassInfoDAL _cidal;
        private IUniversityDAL _udal;
        private IClassDAL _cdal;
        private IClientDAL _clientdal;
        private IUseRecordsDAL _urdal;

        public ClassInfoController(IClassInfoDAL cidal, IUniversityDAL udal, IClassDAL cdal, IClientDAL clientdal, IUseRecordsDAL urdal)
        {

            _cidal = cidal;
            _udal = udal;
            _cdal = cdal;
            _clientdal = clientdal;
            _urdal = urdal;
        }

        /// <summary>
        /// 编辑订单(题库集）名称
        /// </summary>
        /// <param name="classInfo"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Edit")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Edit([FromBody] ClassInfo classInfo)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                if (!string.IsNullOrEmpty(classInfo.Name))
                {
                    if (classInfo.ClientId == ID)
                    {
                        r.Data = _cidal.Edit(classInfo);
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
            public ClassInfo cict { get; set; }
            public string university { get; set; }
            public string clas { get; set; }
        }
        /// <summary>
        /// 根据题库集id检索
        /// </summary>
        /// <param name="classTest"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClassInfo")]
        public ResultModel GetClassInfo(int id)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                info info = new info();
                info.cict = _cidal.GetClassInfo(id);
                var cla = _cdal.GetClass(info.cict.ClassId);
                info.clas = cla.Name;
                info.university = _udal.GetUniversity(cla.UniversityId).Name;
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
                var clientid = _cidal.GetClassInfo(id).ClientId;
                if (clientid == ID)
                {
                    r.Data = _cidal.Del(id);
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
                var university = _udal.GetList(ID);
                foreach (var i in university)
                {
                    actioninfo = new actioninfo();
                    actioninfo.id = i.Id;
                    actioninfo.name = i.Name;
                    actioninfo.type = "学校";
                    actioninfo.status = i.IsAudit == false ? 0 : 1;
                    actioninfo.CreateTime = i.CreateTime;
                    list.Add(actioninfo);
                }

                //课程
                var clas = _cdal.GetList(ID);
                foreach (var t in clas)
                {
                    actioninfo = new actioninfo();
                    actioninfo.id = t.Id;
                    actioninfo.name = t.Name;
                    actioninfo.type = "课程";
                    actioninfo.status = t.IsAudit == false ? 0 : 1;
                    actioninfo.CreateTime = t.CreateTime;
                    list.Add(actioninfo);
                }
                //课程资料(题库集）
                var classInfo = _cidal.GetList(ID);
                foreach (var e in classInfo)
                {
                    actioninfo = new actioninfo();
                    actioninfo.id = e.Id;
                    actioninfo.name = e.Name;
                    actioninfo.type = "题库集";
                    actioninfo.status = e.Status;
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
            public int status { get; set; }//状态

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
                var clientid = _cidal.GetClassInfo(id).ClientId;
                if (clientid == ID)
                {
                    r.Data = _cidal.Change(id);
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
        /// <summary>
        /// 根据课程id检索课程资料
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ClassInfos")]
        public ResultModel ClassInfos(int classid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                string url = AppConfig.Configuration["imgurl"];
                List<cinfo> ls = new List<cinfo>();
                cinfo cinfo = null;
                var cils = _cidal.GetLs(classid).Where(x => x.Status == (int)classInfoStatus.Audited);
                foreach (var item in cils)
                {
                    cinfo = new cinfo();
                    cinfo.classinfo = item;
                    var client = _clientdal.GetClientById(item.ClientId);
                    if (client != null)
                    {
                        cinfo.clientname = client.Name;
                        if (client.Image.Contains("/clientImg"))
                        {
                            cinfo.clientimg = client.Image.Replace("/clientImg", url + "/clientImg");
                        }
                        else
                        {
                            cinfo.clientimg = client.Image;
                        }
                    }
                    ls.Add(cinfo);
                }
                r.Data = ls;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        public class cinfo
        {
            public ClassInfo classinfo { get; set; }
            public string clientname { get; set; }
            public string clientimg { get; set; }
        }
        /// <summary>
        /// 更改课程资料有用/没用
        /// </summary>
        /// <param name="classInfoId"></param>
        /// <param name="type">有用:Y,没用:N</param>
        /// <param name="check">选中:1，取消:-1</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = C_Role.all)]
        [Route("ChangeClassInfo")]
        public ResultModel ChangeClassInfo(int classInfoId, string type, int check)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;

            try
            {
                r.Data = _cidal.Change(ID, classInfoId, type, check, null);


            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;

            }
            return r;
        }
        /// <summary>
        /// 根据课程资料id检索该课程资料有用、无用
        /// </summary>
        /// <param name="classInfoid"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = C_Role.all)]
        [Route("UseRecords")]
        public ResultModel UseRecords(int classInfoid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {

               var useRecords= _urdal.GetUseRecords(ID, classInfoid);
                r.Data = new { useRecords.Type, useRecords.Check };


            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;

            }
            return r;
        }

        public class model
        {
            public string university_name { get; set; }
            public int university_id { get; set; }
            public int class_num { get; set; }

            public int classinfo_num { get; set; }
            public string class_name { get; set; }
            public int class_id { get; set; }

            public string client_name { get; set; }
            public int client_id { get; set; }
            public int client_num { get; set; }
        }
        /// <summary>
        /// 首页查询题库，课程，贡献者
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ClassInfo")]
        public ResultModel ClassInfo()
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            List<model> ls = new List<model>();
            try
            {
                var clas = _cdal.GetList().Where(x => x.IsAudit == true);
                var num = new Random().Next(1, clas.Count() - 15);
                var eachclas = clas.Where(x => x.Id > num).Take(15);
                var classinfos = _cidal.GetList().Where(x => x.Status == (int)classInfoStatus.Audited);
                model m = null;
                foreach (var item in eachclas)
                {
                    m = new model();
                    m.university_id = item.UniversityId;
                    m.university_name = item.University;
                    m.class_num = clas.Count();
                    m.class_name = item.Name;
                    m.class_id = item.Id;
                    m.classinfo_num = classinfos.Count();
                    var classinfo = classinfos.Where(x => x.ClassId == item.Id);
                    foreach (var i in classinfo)
                    {
                        var client = _clientdal.GetClientById(i.ClientId);
                        if (client != null)
                        {
                            m.client_name = client.Name;
                            m.client_id = client.Id;
                            break;
                        }
                    }
                    m.client_num = _cidal.GetClients();
                    ls.Add(m);
                }

                r.Data = ls;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;

            }
            return r;
        }

    }
}