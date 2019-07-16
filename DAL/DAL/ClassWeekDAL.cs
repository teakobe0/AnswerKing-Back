using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class ClassWeekDAL : BaseDAL, IClassWeekDAL
    {
        public ClassWeekDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="classinfoid"></param>
        /// <returns></returns>
        public List<ClassWeek> GetList(int classinfoid)
        {
            if (classinfoid != 0)
            {
                var list = GetListData();
                list = list.Where(x => x.ClassInfoId == classinfoid);
                return list.ToList();
            }
            return null;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="classWeek"></param>
        /// <returns></returns>
        public int Add(ClassWeek classWeek)
        {
            var model = _context.ClassWeek.Where(x => x.ClassInfoId == classWeek.ClassInfoId).ToList();
            if (model.Count() > 0)
            {
                var maxno = model.OrderByDescending(x => x.No).FirstOrDefault().No;
                classWeek.No = maxno + 1;
            }
            else
            {
                classWeek.No = 1;
            }
            classWeek.CreateTime = DateTime.Now;
            _context.ClassWeek.Add(classWeek);
            return _context.SaveChanges();

        }
        /// <summary>
        /// 修改每周课程
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="classWeek"></param>
        /// <returns></returns>
        public int ChangeInfo(int ID, ClassWeek classWeek)
        {
            var data = _context.ClassWeek.FirstOrDefault(x => x.Id == ID);
            data.No = classWeek.No;
            data.Grade = classWeek.Grade;
            return _context.SaveChanges();
        }
        public int ChangeInfo(ClassWeek classWeek)
        {
            _context.ClassWeek.Update(classWeek);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 根据每周课程id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClassWeek GetClassWeek(int id)
        {
            return _context.ClassWeek.FirstOrDefault(x => x.Id == id);
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
                var classweek = _context.ClassWeek.FirstOrDefault(x => x.Id == id);
                Utils.WriteInfoLog("ClassWeek:Delete" + classweek.ToJson());
                _context.ClassWeek.Remove(classweek);
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 根据课程资料单号检索
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public List<ClassWeek> GetListByNo(int no)
        {
            var list = GetListData().Where(x => x.ClassInfoId !=-99);
            if (no != 0)
            {
                list = list.Where(x => x.ClassInfoId == no);

            }
            return list.ToList();
        }
        /// <summary>
        /// 获取当前导入数据的最大
        /// </summary>
        /// <returns></returns>
        public int GetImportMaxid()
        {
            var cw = _context.ClassWeek.Where(x => x.RefId != 0).OrderByDescending(x => x.RefId).FirstOrDefault();

            return cw == null ? 0 : cw.RefId;

        }
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int AddImportData(List<ClassWeek> ls)
        {
            int num = 0;
            foreach(var item in ls)
            {
                _context.ClassWeek.Add(item);
               num+= _context.SaveChanges();
            }
            return num;
        }
        /// <summary>
        /// 获取全部导入数据
        /// </summary>
        /// <returns></returns>
        public List<ClassWeek> GetImportList()
        {
            var list = GetListData().Where(x => x.RefId != 0);
            return list.ToList();
        }
        /// <summary>
        /// 根据课程资料id
        /// </summary>
        /// <param name="classinfoid"></param>
        /// <returns></returns>
        public List<ClassWeek> GetListByClassinfoid(int classinfoid)
        {
            var list = GetListData().Where(x => x.ClassInfoId == classinfoid&&x.Grade>0);
            return list.ToList();
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        public List<ClassWeek> GetList()
        {
            return GetListData().ToList();

        }
        private IQueryable<ClassWeek> GetListData()
        {
            return _context.ClassWeek.Where(x => 1 == 1);
        }

    }
}
