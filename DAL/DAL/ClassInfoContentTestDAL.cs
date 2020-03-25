using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class ClassInfoContentTestDAL : BaseDAL, IClassInfoContentTestDAL
    {
        public ClassInfoContentTestDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        public List<ClassInfoContentTest> GetList()
        {
            return GetListData().ToList();

        }

        private IQueryable<ClassInfoContentTest> GetListData()
        {
            return _context.ClassInfoContentTest.Where(x => x.IsDel == false);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        public int Add(ClassInfoContentTest classInfoContentTest)
        {
            classInfoContentTest.CreateTime = DateTime.Now;
            classInfoContentTest.ClassTestId = _context.ClassInfoTest.FirstOrDefault(x => x.Id == classInfoContentTest.ClassInfoTestId).ClassTestId;
            classInfoContentTest.UniversityTestId = _context.ClassTest.FirstOrDefault(x => x.Id == classInfoContentTest.ClassTestId).UniversityTestId;
            _context.ClassInfoContentTest.Add(classInfoContentTest);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 根据订单id检索周
        /// </summary>
        /// <param name="classInfoTestId"></param>
        /// <returns></returns>
        public object GetWeek(int classInfoTestId)
        {
            if (classInfoTestId != 0)
            {
                var list = GetListData().Where(x => x.ClassInfoTestId == classInfoTestId).GroupBy(x => x.ClassWeek).Select(x => x.Key).ToList();
                return list;
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 根据客户id,订单id检索答案
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="classInfoTestId"></param>
        /// <returns></returns>
        public List<ClassInfoContentTest> GetLs(int clientId, int classInfoTestId)
        {
            var list = GetListData();
            if (clientId != 0)
            {
                list = list.Where(x => x.ClientId == clientId);
                if (classInfoTestId != 0)
                {
                    list = list.Where(x => x.ClassInfoTestId == classInfoTestId).OrderByDescending(x => x.Id);
                }
            }
            else
            {
                return null;
            }
            return list.ToList();
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public int DelImg(int id, string img)
        {
            if (id != 0)
            {
                var cict = _context.ClassInfoContentTest.FirstOrDefault(x => x.Id == id);
                if ((!string.IsNullOrEmpty(cict.NameUrl)) || (!string.IsNullOrEmpty(cict.Url)))
                {
                    if (cict.NameUrl == img)
                    {
                        cict.NameUrl = cict.NameUrl.Replace(img, "");
                    }
                    if (cict.Url == img)
                    {
                        cict.Url = cict.Url.Replace(img, "");
                    }
                    return _context.SaveChanges();
                }
            }
            return 0;

        }
        /// <summary>
        /// 根据答案id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClassInfoContentTest GeClassInfoContentTest(int id)
        {
            return _context.ClassInfoContentTest.FirstOrDefault(x => x.Id == id);

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
                ClassInfoContentTest cict = _context.ClassInfoContentTest.FirstOrDefault(x => x.Id == id);
                cict.IsDel = true;
                _context.ClassInfoContentTest.Update(cict);
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="cict"></param>
        /// <returns></returns>
        public int Edit(ClassInfoContentTest cict)
        {
            _context.ClassInfoContentTest.Update(cict);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 根据课程资料id检索
        /// </summary>
        /// <param name="classinfotestid"></param>
        /// <param name="status"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <param name="PageTotal"></param>
        /// <returns></returns>
        public object GetListbycinid(int classinfotestid, int status, int pagenum, int pagesize, out int PageTotal)
        {
            PageTotal = 0;
            var ls = GetListData();
            if (classinfotestid != 0)
            {
                ls = ls.Where(x => x.ClassInfoTestId == classinfotestid);
            }
            if (status != -1)
            {
                if (status == 1)
                {
                    ls = ls.Where(x => x.IsAudit == true);
                }
                else
                {
                    ls = ls.Where(x => x.IsAudit == false);
                }
            }
            PageTotal = ls.Count();
            ls = ls.Skip(pagesize * (pagenum - 1)).Take(pagesize).OrderBy(x => x.Id);
            return ls.ToList();
        }
        /// <summary>
        /// 审核、取消审核
        /// </summary>
        /// <param name="classInfoContentTest"></param>
        /// <returns></returns>
        public int Audit(ClassInfoContentTest classInfoContentTest)
        {
            var cict = _context.ClassInfoContentTest.FirstOrDefault(x => x.Id == classInfoContentTest.Id);
            if (cict.IsAudit == true)
            {
                cict.IsAudit = false;
            }
            else
            {
                cict.IsAudit = true;
                cict.Url = classInfoContentTest.Url;
                cict.Contents = classInfoContentTest.Contents;
                Utils.WriteInfoLog("ClassInfoContentTest:Audit" + cict.ToJson());
            }
            return _context.SaveChanges();
        }
        /// <summary>
        /// 检索下一个未审核答案
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClassInfoContentTest GetNext(int id)
        {
            return _context.ClassInfoContentTest.FirstOrDefault(x => x.Id > id && x.IsAudit == false && x.Url != null && x.Url != "");
        }

    }

}


