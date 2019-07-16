using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IImportRecordsDAL
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="import"></param>
        /// <returns></returns>
        int Add(ImportRecords import);
        /// <summary>
        /// 根据表名查询导入的截止id
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        int GetEndid(string tablename);
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        List<ImportRecords> GetList();
        /// <summary>
        /// 查询每个类型下的最后一次记录
        /// </summary>
        /// <returns></returns>
        List<ImportRecords> GetEnd();
    }
}
