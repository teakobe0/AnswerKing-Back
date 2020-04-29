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
    [Route("api/Class")]
    public class ClassController : BaseController
    {

        private IClassInfoDAL _cidal;
        private IUniversityDAL _udal;
        private IClientDAL _clientdal;
        private IClassDAL _cladal;
        public ClassController(IClassInfoDAL cidal, IClientDAL clientdal, IClassDAL cladal, IUniversityDAL udal)
        {

            _cidal = cidal;
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
                var query = _cladal.GetList(name);
                var model = query.Skip(pagesize * (pagenum - 1)).Take(pagesize).ToList();
                foreach (var item in model)
                {
                    c = new cinfo();
                    c.cla = item;
                    var classinfo = _cidal.GetLs(item.Id);
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
            public Class cla { get; set; }
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
                var cla = _cladal.GetList(universityid, alif, name);
                foreach (var item in cla)
                {
                    info = new cinfo();
                    info.cla = item;
                    var clas = _cidal.GetLs(item.Id);
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
        [Route("Classes")]
        public ResultModel Classes(int universityId, string name)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _cladal.GetList(universityId, name).Take(10);

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
        /// <param name="clas"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Add([FromBody] Class clas)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                ClassInfo cit = null;
                var Clas = clas;
                clas.ClientId = ID;
                if (!string.IsNullOrEmpty(clas.Name))
                {

                    if (clas.Id == 0)
                    {
                        //查询添加的课程是否存在
                        bool name = _cladal.GetName(clas.UniversityId, clas.Name, 0);
                        if (name == true)
                        {
                            r.Status = RmStatus.Error;
                            r.Msg = "该课程名称已经存在";
                        }
                        else
                        {
                            Clas = _cladal.Add(clas);
                            clas.Id = Clas.Id;
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
                    cit = new ClassInfo();
                    cit.ClassId = clas.Id;
                    cit.ClientId = clas.ClientId;
                    Random random = new Random();
                    int num = random.Next(10000000, 99999999);
                    cit.Name = "题库集" + num;
                    var classInfo = _cidal.Add(cit);

                    r.Data = new { clas, classInfo };
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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClass")]
        public ResultModel GetClass(int id)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _cladal.GetClass(id);
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
        /// <param name="clas"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Edit")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Edit([FromBody] Class clas)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                if (!string.IsNullOrEmpty(clas.Name))
                {
                    if (clas.ClientId == ID)
                    {
                        //查询添加的课程是否存在
                        bool name = _cladal.GetName(clas.UniversityId, clas.Name, clas.Id);
                        if (name == true)
                        {
                            r.Data = 0;
                            r.Status = RmStatus.Error;
                            r.Msg = "该课程名称已经存在";
                        }
                        else
                        {
                            r.Data = _cladal.Edit(clas);
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
                var clientid = _cladal.GetClass(id).ClientId;
                if (clientid == ID)
                {
                    r.Data = _cladal.Del(id);
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