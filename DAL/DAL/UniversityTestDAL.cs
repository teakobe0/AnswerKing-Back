using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class UniversityTestDAL : BaseDAL, IUniversityTestDAL
    {
        public UniversityTestDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        public List<UniversityTest> GetList()
        {
            return GetListData().ToList();
        }

        private IQueryable<UniversityTest> GetListData()
        {
            return _context.UniversityTest.Where(x => x.IsDel == false);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        public List<UniversityTest> GetList(string name)
        {
            var list = GetListData();
            if (!string.IsNullOrEmpty(name))
            {
                list = list.Where(x => x.Name.Trim().Contains(name.Trim()));
            }
            return list.ToList();

        }
        /// <summary>
        /// 根据学校id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UniversityTest GetUniversityTest(int id)
        {
            return _context.UniversityTest.FirstOrDefault(x => x.Id == id);
        }
        /// <summary>
        /// 查询学校名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GetName(string name, int id)
        {
            if (id == 0)
            {
                return _context.UniversityTest.Any(x => x.Name.Trim() == name.Trim());
            }
            else
            {
                return _context.UniversityTest.Any(x => x.Id != id && x.Name.Trim() == name.Trim());
            }

        }
        /// <summary>
        /// 根据学校名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UniversityTest GetUniversityTest(string name)
        {
            return _context.UniversityTest.FirstOrDefault(x => x.Name.Trim() == name.Trim());

        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="university"></param>
        /// <returns></returns>
        public UniversityTest Add(UniversityTest universityTest)
        {

            universityTest.CreateTime = DateTime.Now;
            _context.UniversityTest.Add(universityTest);
            _context.SaveChanges();
            return universityTest;
            
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public int DelImg(int id, string img)
        {
            var u = _context.UniversityTest.FirstOrDefault(x => x.Id == id);
            if (u.Image != null && u.Image != "")
            {

                u.Image = u.Image.Replace(img, "");
                return _context.SaveChanges();
            }
            else
            {
                return 0;
            }

        }
        /// <summary>
        /// 编辑学校
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        public int Edit(UniversityTest universityTest)
        {
            _context.UniversityTest.Update(universityTest);
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
                UniversityTest ut = _context.UniversityTest.FirstOrDefault(x => x.Id == id);
                ut.IsDel = true;
                _context.UniversityTest.Update(ut);
                //删除这个学校下面的课
                var clas = _context.ClassTest.Where(x => x.UniversityTestId == id);
                if (clas.Count() > 0)
                {
                    foreach (var i in clas)
                    {
                        i.IsDel = true;
                    }
                }
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 根据客户id检索学校
        /// </summary>
        /// <returns></returns>
        public List<UniversityTest> GetList(int clientId)
        {
            var list = GetListData().ToList();
            if (clientId != 0)
            {
                list = list.Where(x => x.ClientId == clientId).ToList();
            }
            return list;
        }
        /// <summary>
        /// 获取当前导入数据的最大id
        /// </summary>
        /// <returns></returns>
        public int GetImportMaxid()
        {
            var c = _context.UniversityTest.OrderByDescending(x => x.RefId).FirstOrDefault();
            return c == null ? 0 : c.RefId;
        }
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int AddImportData(List<UniversityTest> ls)
        {
            int num = 0;
            foreach (var item in ls)
            {
                _context.UniversityTest.Add(item);
                num += _context.SaveChanges();
            }
            return num;
        }
    }
}
