﻿
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IAnswerDAL
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        List<Answer> GetList();
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        int Add(Answer answer);
        /// <summary>
        /// 根据答案id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Answer GetAnswer(int id);
        /// <summary>
        /// 根据问题id查询
        /// </summary>
        /// <param name="questionid"></param>
        /// <returns></returns>
        List<Answer> GetLs(int questionid);
        /// <summary>
        /// 根据客户id查询
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        List<Answer> GetList(int clientid);
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        int DelImg(int id, string img);
    }
}


