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
        public ClassTest GetRandomClass()
        {
            return _context.ClassTest.Where(x => 1 == 1).OrderBy(x => Guid.NewGuid()).First();
        }
        private IQueryable<ClassTest> GetListData()
        {
            return _context.ClassTest.Where(x => x.IsDel == false);
        }
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<ClassTest> GetList(string name)
        {
            var list = GetListData().OrderBy(x => x.Name.Replace("\"", "").Trim()).ToList();

            if (!string.IsNullOrEmpty(name))
            {
                list = list.Where(x => x.Name.Contains(name.Trim())).ToList();
            }
            return list;

        }
        /// <summary>
        /// 新增课程
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        public ClassTest Add(ClassTest cla)
        {
            cla.CreateTime = DateTime.Now;
            cla.UniversityTest = _context.UniversityTest.FirstOrDefault(x => x.Id == cla.UniversityTestId).Name;
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
                //删除这个课下面的题库集
                var cit = _context.ClassInfoTest.Where(x => x.ClassTestId == id);
                if (cit.Count() > 0)
                {
                    foreach (var i in cit)
                    {
                        i.IsDel = true;
                    }
                }
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
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="universityid"></param>
        /// <param name="alif"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<ClassTest> GetList(int universityid, string alif, string name)
        {
            var list = GetListData();
            if (universityid != 0)
            {
                list = list.Where(x => x.UniversityTestId == universityid);

            }
            if (!string.IsNullOrEmpty(alif))
            {
                list = list.Where(x => x.Name.Substring(0, 1).ToUpper() == alif);

            }
            if (!string.IsNullOrEmpty(name))
            {
                list = list.Where(x => x.Name.Trim().Contains(name.Trim()));
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
        /// <returns></returns>
        public List<ClassTest> GetListbyname(string name)
        {
           
            var ls = GetListData();
            if (!string.IsNullOrEmpty(name))
            {
                ls = ls.Where(x => x.Name.Trim().Contains(name.Trim()));
            }
            return ls.ToList();
        }
        /// <summary>
        /// 获取当前导入数据的最大id
        /// </summary>
        /// <returns></returns>
        public int GetImportMaxid()
        {
            var c = _context.ClassTest.OrderByDescending(x => x.RefId).FirstOrDefault();
            return c == null ? 0 : c.RefId;
        }
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int AddImportData(List<ClassTest> ls)
        {
            int num = 0;
            foreach (var item in ls)
            {
                _context.ClassTest.Add(item);
                num += _context.SaveChanges();
            }
            return num;
        }
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<ClassTest> GetLs(int universityid)
        {
            var list = GetListData();
            if (universityid != 0)
            {
                list = list.Where(x => x.UniversityTestId == universityid);
                return list.ToList();
            }
            return null;
        }

    }
}
