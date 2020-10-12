
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
        Question Add(Question question);
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
        /// <summary>
        /// 根据客户id检索(回答者)
        /// </summary>
        /// <returns></returns>
         List<Question> GetListByClientid(int clientId);
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        object GetLs(int number, int status, int pagenum, int pagesize, out int PageTotal);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Update(int id, Bidding bidding);
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int ChangeStatus(int id);
        /// <summary>
        ///  审核
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <param name="qintegral"></param>
        /// <param name="aintegral"></param>
        /// <returns></returns>
        int Audit(int id, int userid,int qintegral,int aintegral);
        /// <summary>
        /// 修改未处理订单状态
        /// </summary>
        /// <returns></returns>
        int ChangeStatus();
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        int DelImg(int id, string img);
        /// <summary>
        /// 查询未识别的图片集
        /// </summary>
        /// <returns></returns>
        List<Question> ImgLs();
        /// <summary>
        /// 更新ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
         int Update(List<Question> ls);
    }
}
