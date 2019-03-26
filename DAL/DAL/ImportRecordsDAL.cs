using DAL.IDAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class ImportRecordsDAL : BaseDAL, IImportRecordsDAL
    {
        public ImportRecordsDAL(DataContext context)
        {
            _context = context;
        }
      /// <summary>
      /// 新增
      /// </summary>
      /// <param name="import"></param>
      /// <returns></returns>
        public int Add(ImportRecords import)
        {
            import.CreateTime = DateTime.Now;
            _context.ImportRecords.Add(import);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 根据表名查询导入的截止id
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public int GetEndid(string tablename)
        {
            var m= _context.ImportRecords.Where(x => x.Table == tablename).OrderByDescending(x => x.EndId).FirstOrDefault();
            return m == null ? 0 : m.EndId;
        }
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        public List<ImportRecords> GetList()
        {
            return GetListData().ToList();
        }

        private IQueryable<ImportRecords> GetListData()
        {
            return _context.ImportRecords.Where(x => 1 == 1);
        }

    }
}
