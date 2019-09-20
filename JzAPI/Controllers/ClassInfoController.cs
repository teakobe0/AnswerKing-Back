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
    [Route("api/ClassInfo")]
    public class ClassInfoController : BaseController
    {
        private IClassInfoDAL _clindal;
        private IUseRecordsDAL _urdal;
        private IClassWeekDAL _clwdal;
        private IUniversityDAL _undal;
        private IClassDAL _clasdal;
        private IClientDAL _clientdal;
        public ClassInfoController(IClassInfoDAL clindal, IClassWeekDAL clwdal, IUseRecordsDAL urdal, IUniversityDAL undal, IClassDAL clasdal, IClientDAL clientdal, IFocusDAL focusdal, ICommentDAL comdal)
        {
            _clindal = clindal;
            _urdal = urdal;
            _clwdal = clwdal;
            _undal = undal;
            _clasdal = clasdal;
            _clientdal = clientdal;
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
                List<cinfo> ls = new List<cinfo>();
                cinfo cinfo = null;
                var cils = _clindal.GetList(classid);
                foreach (var item in cils)
                {
                    cinfo = new cinfo();
                    cinfo.classinfo = item;
                    var client = _clientdal.GetClientById(item.ClientId);
                    if (client != null)
                    {
                        cinfo.clientname = client.Name;
                        cinfo.clientimg = client.Image;
                    }
                    var cwls = _clwdal.GetListByClassinfoid(item.Id);
                    if (cwls.Count() > 0)
                    {
                        item.TotalGrade = cwls.Sum(x => x.Grade) / cwls.Count();
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
                r.Data = _clindal.Change(ID, classInfoId, type, check);


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

                r.Data = _urdal.GetUseRecords(ID, classInfoid);


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
                var clas = _clasdal.GetList();
                var num = new Random().Next(1, clas.Count() - 15);
                var eachclas = clas.Where(x => x.Id > num).Take(15);
                var classinfos = _clindal.GetList();
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
                    var classinfo = _clindal.GetList(item.Id);
                    foreach (var i in classinfo)
                    {
                        var client = _clientdal.GetClientById(i.ClientId);
                        if (client != null)
                        {
                            m.client_name = client.Name;
                            break;
                        }
                    }
                    m.client_num = _clindal.GetClients();
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