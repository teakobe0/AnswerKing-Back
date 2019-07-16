using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class ClassWeekTypeDAL : BaseDAL, IClassWeekTypeDAL
    {
        public ClassWeekTypeDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 根据每周课程类型id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClassWeekType GetClassWeekType(int id)
        {
            return _context.ClassWeekType.FirstOrDefault(x => x.Id == id);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="classWeekType"></param>
        /// <returns></returns>
        public int Add(ClassWeekType classWeekType)
        {
            classWeekType.CreateTime = DateTime.Now;
            classWeekType.ClassWeekTypeId = -1; 
            _context.ClassWeekType.Add(classWeekType);
            return _context.SaveChanges();
        }
            /// <summary>
            /// 根据每周课程id检索课程类型(前台)
            /// </summary>
            /// <param name="classWeekId"></param>
            /// <returns></returns>
            public List<ClassWeekType> ClassWeekTypes(int classWeekId)
        {

            if (classWeekId != 0)
            {
                var list = GetListData();
                list = list.Where(x => x.ClassWeekId == classWeekId && (x.ClassWeekTypeId == 0 || x.ClassWeekTypeId == -1));
                return list.ToList();
            }
            return null;
        }
        /// <summary>
        /// 根据课程资料单号检索
        /// </summary>
        /// <param name="classinfoid"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <param name="PageTotal"></param>
        /// <returns></returns>
        public object GetList(int classinfoid, int pagenum, int pagesize, out int PageTotal)
        {
            PageTotal = 0;
            var ls = GetListData().Where(x => x.ClassWeekTypeId != 0);
            var list = from x in ls
                       join cw in _context.ClassWeek on x.ClassWeekId equals cw.Id
                       join ci in _context.ClassInfo on cw.ClassInfoId equals ci.Id

                       select new
                       {
                           x.ClassWeekTypeId,
                           ClassWeek = _context.ClassWeek.FirstOrDefault(z => z.Id == x.ClassWeekId) != null ? _context.ClassWeek.FirstOrDefault(z => z.Id == x.ClassWeekId).No.ToString() : "",
                           x.ContentType,
                           x.ClassWeekId,
                           x.Grade,
                           x.Id,
                           ClassInfo = cw.ClassInfoId

                       };
            if (classinfoid != 0)
            {
                list = list.Where(x => x.ClassInfo == classinfoid);
            }
            PageTotal = list.Count();
            list = list.Skip(pagesize * (pagenum - 1)).Take(pagesize).OrderBy(x=>x.Id);
            return list.ToList();
        }
        /// <summary>
        /// 根据每周课程id检索课程类型
        /// </summary>
        /// <param name="classWeekId"></param>
        /// <returns></returns>

        public List<ClassWeekType> GetClassWeek(int classWeekId)
        {

            if (classWeekId != 0)
            {
                var list = GetListData();
                list = list.Where(x => x.ClassWeekId == classWeekId && x.ClassWeekTypeId != 0);
                return list.ToList();
            }
            return null;
        }
        /// <summary>
        /// 修改每周课程类型
        /// </summary>
        /// <param name="classWeekType"></param>
        /// <returns></returns>
        public int ChangeInfo(ClassWeekType classWeekType)
        {
            _context.ClassWeekType.Update(classWeekType);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            if (id != 0)
            {
                var classWeekType = _context.ClassWeekType.FirstOrDefault(x => x.Id == id);
                Utils.WriteInfoLog("ClassWeekType:Delete" + classWeekType.ToJson());
                _context.ClassWeekType.Remove(classWeekType);
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 获取当前导入数据的最大
        /// </summary>
        /// <returns></returns>
        public int GetImportMaxid()
        {
          var cwt=_context.ClassWeekType.Where(x => x.RefId!=0).OrderByDescending(x => x.RefId).FirstOrDefault();
            return cwt == null ? 0 : cwt.RefId;
           
        }
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int AddImportData(List<ClassWeekType> ls)
        {
            int num = 0;
            foreach (var item in ls)
            {
                _context.ClassWeekType.Add(item);
                num += _context.SaveChanges();
            }
            return num;
        }
        /// <summary>
        /// 获取全部导入数据
        /// </summary>
        /// <returns></returns>
        public List<ClassWeekType> GetImportList()
        {
            var list = GetListData().Where(x => x.RefId != 0);
            return list.ToList();
        }
        /// <summary>
        /// 查询列表 全部数据
        /// </summary>
        /// <returns></returns>
        public List<ClassWeekType> GetList()
        {
            return GetListData().ToList();
        }

        private IQueryable<ClassWeekType> GetListData()
        {
            return _context.ClassWeekType.Where(x => 1 == 1);
        }
    }
}
