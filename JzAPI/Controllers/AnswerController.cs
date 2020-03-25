using System;
using System.Collections.Generic;
using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace JzAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Answer")]
    public class AnswerController : BaseController
    {
        private IAnswerDAL _ansdal;
        private IQuestionDAL _quedal;
        public AnswerController(IAnswerDAL ansdal, IQuestionDAL quedal)
        {

            _ansdal = ansdal;
            _quedal = quedal;
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
                if (answer.Id == 0)
                {
                    answer.CreateBy = ID.ToString();
                    r.Data = _ansdal.Add(answer);
                }
                else
                {
                    if (!string.IsNullOrEmpty(answer.Content))
                    {
                        if (int.Parse(answer.CreateBy) == ID)
                        {

                            r.Data = _ansdal.Edit(answer);

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
                        r.Msg = "答案不能为空";
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
        /// 根据答案id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAnswer")]
        public ResultModel GetAnswer(int id)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _ansdal.GetAnswer(id);
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
        public class ainfo
        {
            public Answer answer { get; set; }
            public string title { get; set; }
        }
        /// <summary>
        ///我的回答
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("MyAnswer")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel MyAnswer()
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                List<ainfo> ls = new List<ainfo>();
                ainfo ainfo = null;
                var ans = _ansdal.GetList(ID);
                foreach(var item in ans)
                {
                    ainfo = new ainfo();
                    ainfo.answer = item;
                    var que = _quedal.GetQuestion(item.QuestionId);
                    ainfo.title = que.Title;
                    ls.Add(ainfo);
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