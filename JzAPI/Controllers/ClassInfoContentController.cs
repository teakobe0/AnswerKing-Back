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
    [Route("api/ClassInfoContent")]
    public class ClassInfoContentController : BaseController
    {
        private IClassInfoContentDAL _clicdal;
        private IUniversityDAL _undal;
        private IClassDAL _classdal;
        private IClassWeekTypeDAL _cwtdal;
        public ClassInfoContentController(IClassInfoContentDAL clicdal, IUniversityDAL undal, IClassDAL classdal, IClassWeekTypeDAL cwtdal)
        {
            _clicdal = clicdal;
            _undal = undal;
            _classdal = classdal;
            _cwtdal = cwtdal;

        }
        /// <summary>
        /// 根据内容检索答案  分页
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ContentsPage")]
        public ResultModel ContentsPage(string searchText, int pagenum = 1, int pagesize = 10)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                var queryList = _clicdal.GetList(searchText);
                page.Data = queryList.Skip(pagesize * (pagenum - 1)).Take(pagesize).ToList();
                page.PageTotal = queryList.Count();
                r.Data = page;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 根据答案id查询答案详情
        /// </summary>
        /// <param name="contentid"></param>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Roles = C_Role.admin_vip)]
        [Route("ContentDetail")]
        public ResultModel ContentDetail(int contentid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _clicdal.GetClassInfoContent(contentid);
                if (r.Data == null)
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "查询失败";
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
        [Authorize(Roles = C_Role.admin)]
        [Route("Edit")]
        public ResultModel Edit([FromBody] ClassInfoContent classInfoContent)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _clicdal.ChangeInfo(classInfoContent.Id, classInfoContent);
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 根据每周课程id检索答案
        /// </summary>
        /// <param name="classweekid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Contents")]
        public ResultModel Contents(int classweekid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _clicdal.GetList(classweekid);

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        public class uinfo
        {
            public University university { get; set; }
            public int number { get; set; }
        }
        /// <summary>
        /// 根据学校名称、课程名称、答案检索
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Search")]
        public ResultModel Search(string name)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                List<uinfo> ls = new List<uinfo>();
                uinfo uinfo = null;
                //var content = _clicdal.GetList(name);
                var classes = _classdal.GetList(name).Take(10);

                var university = _undal.GetList(name).Take(10);
                foreach (var item in university)
                {
                    uinfo = new uinfo();
                    uinfo.university = item;
                    var clas = _classdal.GetList(item.Id);
                    uinfo.number = clas.Count();
                    ls.Add(uinfo);
                }
            
                //r.Data = new { content, classes, ls };
                r.Data = new { classes,ls };

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 根据每周课程id检索类型
        /// </summary>
        /// <param name="classweekid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ClassWeekTypes")]
        public ResultModel ClassWeekTypes(int classweekid)
        {
            ResultModel r = new ResultModel();

            r.Status = RmStatus.OK;

            try
            {
                r.Data = _cwtdal.ClassWeekTypes(classweekid);

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;

            }
            return r;
        }
        [HttpGet]
        [Route("Contentls")]
        public ResultModel Contentls(int classweektypeid, int id)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _clicdal.GetByTypeid(classweektypeid, id);

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
    }
}