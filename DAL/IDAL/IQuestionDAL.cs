
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IQuestionDAL
    {
        /// <summary>
        /// 发布问题
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        int Add(Question question);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        List<Question> GetList();
        /// <summary>
        /// 评价
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        int Evaluate(int id, string content, int grade);
        /// <summary>
        /// 申请客服
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        int ForService(int id, string reason);
        /// <summary>
        /// 提交修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        int Edit(int id);
        /// <summary>
        /// 根据问题id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Question GetQuestion(int id);
        /// <summary>
        /// 根据客户id检索问题
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        List<Question> GetList(int clientid);
    }
}
