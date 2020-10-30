using System;
using System.Collections.Generic;
using System.Linq;
using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
using DAL.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using static DAL.Tools.EnumAll;

namespace JzAPI.Controllers
{
    /// <summary>
    /// 回答表控制层
    /// </summary>
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Answer")]
    public class AnswerController : BaseController
    {
        private IAnswerDAL _ansdal;
        private IQuestionDAL _quedal;
        private IBiddingDAL _biddal;
        public AnswerController(IAnswerDAL ansdal, IQuestionDAL quedal, IBiddingDAL biddal)
        {

            _ansdal = ansdal;
            _quedal = quedal;
            _biddal = biddal;
        }
        /// <summary>
        /// 回答问题
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Add([FromBody] Answer answer)
        {

            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                if (!string.IsNullOrEmpty(answer.Content))
                {
                    string url = AppConfig.Configuration["imgurl"];
                    answer.Img = !string.IsNullOrEmpty(answer.Img) ? answer.Img.Replace(url, "") : answer.Img;
                    answer.CreateBy = ID.ToString();
                    r.Data = _ansdal.Add(answer);
                    var que = _quedal.GetQuestion(answer.QuestionId);

                    if ((int)r.Data == 1 && que.Status == (int)questionStatus.Choose)
                    {
                        //变更问题的状态为已回答
                        _quedal.ChangeStatus(answer.QuestionId);
                    }
                }
                else
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "答案内容不能为空";
                }
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        ///我的回答
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("MyAnswer")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel MyAnswer(int status, int pagenum = 1, int pagesize = 10)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                var question = _quedal.GetListByClientid(ID).OrderByDescending(x=>x.Id).ToList();
                if (status != 0)
                {
                    question = question.Where(x => x.Status == status).ToList();
                }
                var model = from x in question.Skip(pagesize * (pagenum - 1)).Take(pagesize)
                            select new
                            {
                                x.Id,
                                x.Title,
                                x.CreateTime,
                                x.Currency,
                                x.EndTime,
                                bnum = _biddal.GetList(x.Id).Count()
                            };
                page.Data = model;
                page.PageTotal = question.Count();
                r.Data = page;
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
        /// <param name="imgurl"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("RemoveImg")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel RemoveImg(string imgurl)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                //转换为绝对路径
                string path = AppConfig.Configuration["uploadurl"] + imgurl;
                //删除本地
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
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