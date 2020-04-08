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
        /// 查询每个类型下的最后一次记录
        /// </summary>
        /// <returns></returns>
        public List<ImportRecords> GetEnd()
        {
            string[] type = new string[5] { "Class", "ClassInfo", "ClassWeek", "ClassWeekType" , "ClassInfoContent" };
            List<ImportRecords> ls = new List<ImportRecords>();
            foreach (var item in type)
            {
                var model = _context.ImportRecords.Where(x => x.Table == item).OrderByDescending(x=>x.Id).FirstOrDefault();
                ls.Add(model);
            }

            return ls;
                

        }
        /// <summary>
        /// 查询每个类型下的最后一次记录
        /// </summary>
        /// <returns></returns>
        public List<ImportRecords> GetEndTest()
        {
            string[] type = new string[4] { "UniversityTest", "ClassTest", "ClassInfoTest", "ClassInfoContentTest" };
            List<ImportRecords> ls = new List<ImportRecords>();
            foreach (var item in type)
            {
                var model = _context.ImportRecords.Where(x => x.Table == item).OrderByDescending(x => x.Id).FirstOrDefault();
                ls.Add(model);
            }

            return ls;


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
