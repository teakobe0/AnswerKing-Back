using DAL.IDAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class ClassTestDAL : BaseDAL, IClassTestDAL
    {
        public ClassTestDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        public List<ClassTest> GetList()
        {
            return GetListData().ToList();

        }

        private IQueryable<ClassTest> GetListData()
        {
            return _context.ClassTest.Where(x => x.IsDel == false);
        }
        /// <summary>
        /// 新增课程
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        public ClassTest Add(ClassTest cla)
        {
            cla.CreateTime = DateTime.Now;
            _context.ClassTest.Add(cla);
            _context.SaveChanges();
            return cla;
        }
        /// <summary>
        /// 编辑课程
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        public int Edit(ClassTest cla)
        {
            _context.ClassTest.Update(cla);
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
                ClassTest ct = _context.ClassTest.FirstOrDefault(x => x.Id == id);
                ct.IsDel = true;
                _context.ClassTest.Update(ct);
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 根据条件检索
        /// </summary>
        /// <param name="universityTestId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<ClassTest> GetList(int universityTestId, string name)
        {
            var list = GetListData();
            if (universityTestId != 0)
            {
                list = list.Where(x => x.UniversityTestId == universityTestId);
                if (!string.IsNullOrEmpty(name))
                {
                    list = list.Where(x => x.Name.Trim().Contains(name.Trim()));
                }
            }
            else
            {
                return null;
            }
            return list.ToList();

        }
        /// <summary>
        /// 查询同一学校的该课程名称是否存在
        /// </summary>
        /// <param name="universityTestId"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool GetName(int universityTestId,string name,int id)
        {
            if (id == 0)
            {
             return _context.ClassTest.Any(x=>x.UniversityTestId == universityTestId && x.Name.Trim() == name.Trim());
            }
            else
            {
                return _context.ClassTest.Any(x =>x.Id!=id&& x.UniversityTestId == universityTestId && x.Name.Trim() == name.Trim());
            }

            
        }
        /// <summary>
        /// 根据课程id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClassTest GeClassTest(int id)
        {
            return _context.ClassTest.FirstOrDefault(x => x.Id == id);

        }
        /// <summary>
        /// 根据客户id检索课程
        /// </summary>
        /// <returns></returns>
        public List<ClassTest> GetList(int clientId)
        {
            var list = GetListData().ToList();
            if (clientId != 0)
            {
                list = list.Where(x => x.ClientId == clientId).ToList();
            }
            return list;
        }
       /// <summary>
       /// 根据课程名称检索
       /// </summary>
       /// <param name="name"></param>
       /// <param name="status"></param>
       /// <param name="pagenum"></param>
       /// <param name="pagesize"></param>
       /// <param name="PageTotal"></param>
       /// <returns></returns>
        public object GetListbyname(string name,  int pagenum, int pagesize, out int PageTotal)
        {
            PageTotal = 0;
            var ls = GetListData();
            if (!string.IsNullOrEmpty(name))
            {
                ls = ls.Where(x => x.Name.Trim().Contains(name.Trim()));
            }
            var list = from x in ls
                       select new
                       {
                           x.Id,
                           x.Name,
                           x.ClientId,
                           universityTest = _context.UniversityTest.FirstOrDefault(z => z.Id == x.UniversityTestId) != null ? _context.UniversityTest.FirstOrDefault(z => z.Id == x.UniversityTestId).Name : "",
                           x.Professor,
                           x.IsAudit,
                           x.UniversityTestId
                       };

            PageTotal = list.Count();
            list = list.Skip(pagesize * (pagenum - 1)).Take(pagesize).OrderBy(x => x.Id);
            return list.ToList();
        }
    }
}
