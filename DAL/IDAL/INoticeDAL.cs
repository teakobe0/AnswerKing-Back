using DAL.DAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface INoticeDAL
    {
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        List<Notice> GetList();
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="comment_v"></param>
        /// <returns></returns>
        int Add(Comment_v comment_v );
        /// <summary>
        /// 根据客户id检索通知消息
        /// </summary>
        /// <returns></returns>
        List<Notice> GetList(int clientid);
        /// <summary>
        /// 修改通知状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int ChangeStatus(int id);

    }
}
