using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IUseRecordsDAL
    {
        List<UseRecords> GetList();
        /// <summary>
        /// 根据客户id，课程资料id检索课程资料是有用/无用
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="classInfoid"></param>
        /// <returns></returns>
        UseRecords GetUseRecords(int ID, int classInfoid);
        /// <summary>
        /// 根据客户的id检索
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        List<UseRecords> GetUseRecords(int clientid);
    }
}
