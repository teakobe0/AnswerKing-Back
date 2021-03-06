﻿using DAL.DAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface ICommentDAL
    {
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        List<Comment> GetListPage(int type, int typeid);
        /// <summary>
        /// 查询列表全部数据 根据条件
        /// </summary>
        /// <returns></returns>
        List<Comment_v> GetList(int type,int typeid);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        int Add(Comment comment);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        /// 根据客户id检索评论
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        List<Comment> GetListByClientid(int clientid);
    }
}
