using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DAL.Tools.EnumAll;

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
        /// <summary>
        /// 查询未识别的图片集
        /// </summary>
        /// <returns></returns>
        public List<ClassInfoContentTest> ImgLs()
        {
            var list = _context.ClassInfoContentTest.Where(x => x.IsDel == false && !string.IsNullOrEmpty(x.Url) && string.IsNullOrEmpty(x.Contents));
            return list.ToList();

        }

        private IQueryable<ClassInfoContentTest> GetListData()
        {
            return _context.ClassInfoContentTest.Where(x => x.IsDel == false );
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        public int Add(ClassInfoContentTest classInfoContentTest)
        {
            var cit = _context.ClassInfoTest.FirstOrDefault(x => x.Id == classInfoContentTest.ClassInfoTestId);
            if (cit.Status == (int)classInfoTestStatus.Audited)
            {
                cit.Status = (int)classInfoTestStatus.NoAudit;
            }
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
        /// 根据题库集id，周名称检索类型
        /// </summary>
        /// <param name="classInfoId"></param>
        /// <param name="weekName"></param>
        /// <returns></returns>
        public List<ClassInfoContentTest> Types(int classInfoId, int weekName)
        {

            var list = GetListData().Where(x =>x.IsAudit==true&& x.ClassInfoTestId == classInfoId && x.ClassWeek == weekName).OrderBy(x => x.Id);
            return list.ToList();

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
            var cit = _context.ClassInfoTest.FirstOrDefault(x => x.Id == cict.ClassInfoTestId);
            if (cit.Status == (int)classInfoTestStatus.Audited)
            {
                cit.Status = (int)classInfoTestStatus.NoAudit;
            }
            cict.IsAudit = false;
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
        public List<ClassInfoContentTest> GetListbycinid(int classinfotestid, int status, int pagenum, int pagesize, out int PageTotal)
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
            var classInfoTestId = _context.ClassInfoContentTest.FirstOrDefault(x => x.Id == id).ClassInfoTestId;

            return _context.ClassInfoContentTest.FirstOrDefault(x => x.Id > id && x.ClassInfoTestId == classInfoTestId && x.IsAudit == false && x.IsDel == false);
        }
        /// <summary>
        /// 查询该订单未审核的答案
        /// </summary>
        /// <param name="classInfoTestId"></param>
        /// <returns></returns>
        public bool GetNoAudit(int classInfoTestId)
        {
            var cict = _context.ClassInfoContentTest.Where(x => x.ClassInfoTestId == classInfoTestId && x.IsAudit == false && x.IsDel == false);
            if (cict.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取当前导入数据的最大
        /// </summary>
        /// <returns></returns>
        public int GetImportMaxid()
        {
            var cic = _context.ClassInfoContentTest.Where(x => x.RefId != 0).OrderByDescending(x => x.RefId).FirstOrDefault();
            return cic == null ? 0 : cic.RefId;
        }
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int AddImportData(List<ClassInfoContentTest> ls)
        {
            int num = 0;
            foreach (var item in ls)
            {
                _context.ClassInfoContentTest.Add(item);
                num += _context.SaveChanges();
            }
            return num;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int Update(List<ClassInfoContentTest> ls)
        {

            _context.ClassInfoContentTest.UpdateRange(ls);
            return _context.SaveChanges();
        }
    }

}


