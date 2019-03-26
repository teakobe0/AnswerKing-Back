using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.DAL;
using DAL.IDAL;
using DAL.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JzAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Comment")]
    public class CommentController : BaseController
    {
        private ICommentDAL _comdal;
        private INoticeDAL _notdal;

        public CommentController(ICommentDAL comdal, INoticeDAL notdal)
        {
            _comdal = comdal;
            _notdal = notdal;
        }

        /// <summary>
        /// 根据课程资料id检索评论
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Comments")]
        public ResultModel Comments(int classinfoid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _comdal.GetList(classinfoid);

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 新增评论
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public ResultModel Add([FromBody] Comment_v comment)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _comdal.Add(comment);
                if (comment.ParentId != "0")
                {

                    r.Data = _notdal.Add(comment);
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
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Del")]
        public ResultModel Del(int id)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _comdal.Del(id);
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