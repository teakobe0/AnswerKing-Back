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
    [Route("api/ClassInfoContent")]
    public class ClassInfoContentController : BaseController
    {
        private IClassInfoContentDAL _cicdal;
        private IUniversityDAL _udal;
        private IClassDAL _cdal;
        private IClassInfoDAL _cidal;
        public ClassInfoContentController(IClassInfoContentDAL cicdal, IUniversityDAL udal, IClassDAL cdal, IClassInfoDAL cidal)
        {
            _cicdal = cicdal;
            _udal = udal;
            _cdal = cdal;
            _cidal = cidal;
        }
        /// <summary>
        /// 新增答案
        /// </summary>
        /// <param name="classInfoContent"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Add([FromBody] ClassInfoContent classInfoContent)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                if (!string.IsNullOrEmpty(classInfoContent.Url) || !string.IsNullOrEmpty(classInfoContent.Contents))
                {
                    classInfoContent.ClientId = ID;
                    //针对李龙添加的答案状态为已审核
                    if (classInfoContent.ClientId == 553)
                    {
                        classInfoContent.IsAudit = true;
                    }
                    string id = "";
                    r.Data = _cicdal.Add(classInfoContent,out id);
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
        /// <param name="classInfoId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Week")]
        public ResultModel Week(int classInfoId)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _cicdal.GetWeek(classInfoId);
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
        /// <param name="classInfoId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ClassInfoContents")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel ClassInfoContents(int classInfoId)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            List<info> ls = new List<info>();
            info info = null;
            ClassInfoContent classInfoContent = null;
            try
            {
                var cict = _cicdal.GetLs(ID, classInfoId) ;
                var maxweek = cict.OrderByDescending(x => x.ClassWeek).FirstOrDefault().ClassWeek;

                for(int i = 1; i <= maxweek; i++)
                {
                    var weekls = cict.Where(x => x.ClassWeek == i);
                    info = new info();
                    info.week = i;
                    List<ClassInfoContent> list = new List<ClassInfoContent>();
                    foreach (var t in weekls)
                    {
                        classInfoContent = new ClassInfoContent();
                        t.Url = !string.IsNullOrEmpty(t.Url) ? AppConfig.Configuration["imgurl"] + t.Url : t.Url;
                        t.NameUrl = !string.IsNullOrEmpty(t.NameUrl) ? AppConfig.Configuration["imgurl"] + t.NameUrl : t.NameUrl;
                        classInfoContent = t;
                        list.Add(classInfoContent);
                        info.list = list;
                    }
                    ls.Add(info);
                }
                r.Data = ls.OrderByDescending(x=>x.week);
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
           
            public int week { get; set; }
            public List<ClassInfoContent> list { get; set; }
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
                    var clientid = _cicdal.GeClassInfoContent(id).ClientId;
                    if (clientid == ID)
                    {
                        r.Data = _cicdal.DelImg(id, imgurl);
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
        [Route("GetClassInfoContent")]
        public ResultModel GetClassInfoContent(int id)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                var ClassInfoContent = _cicdal.GeClassInfoContent(id);
                var universtiyname = _udal.GetUniversity(ClassInfoContent.UniversityId) == null ? null : _udal.GetUniversity(ClassInfoContent.UniversityId).Name;
                var classname = _cdal.GetClass(ClassInfoContent.ClassId) == null ? null : _cdal.GetClass(ClassInfoContent.ClassId).Name;
                r.Data = new { ClassInfoContent, universtiyname, classname };
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
                var clientid = _cicdal.GeClassInfoContent(id).ClientId;
                if (clientid == ID)
                {
                    r.Data = _cicdal.Del(id);
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
        /// <param name="classInfoContent"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Edit")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Edit([FromBody] ClassInfoContent classInfoContent)
        {
            ResultModel r = new ResultModel();

            r.Status = RmStatus.OK;
            try
            {
                if (!string.IsNullOrEmpty(classInfoContent.Url) || !string.IsNullOrEmpty(classInfoContent.Contents))
                {
                    if (classInfoContent.ClientId == ID)
                    {
                        r.Data = _cicdal.Edit(classInfoContent);
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
                var ls = _cicdal.Types(classinfoid, weekname).Where(x=>x.IsAudit==true);
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