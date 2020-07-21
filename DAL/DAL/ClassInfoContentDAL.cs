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
    public class ClassInfoContentDAL : BaseDAL, IClassInfoContentDAL
    {
        public ClassInfoContentDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        public List<ClassInfoContent> GetList()
        {
            return GetListData().ToList();

        }
        /// <summary>
        /// 查询未识别的图片集
        /// </summary>
        /// <returns></returns>
        public List<ClassInfoContent> ImgLs()
        {
            var list = _context.ClassInfoContent.Where(x => x.IsDel == false && x.IsAudit == true && !string.IsNullOrEmpty(x.Url) && string.IsNullOrEmpty(x.Contents));
            return list.ToList();

        }

        private IQueryable<ClassInfoContent> GetListData()
        {
            return _context.ClassInfoContent.Where(x => x.IsDel == false);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        public int Add(ClassInfoContent classInfoContent, out string id)
        {
            id = "";
            if (classInfoContent.ClientId != 553)
            {
                var cit = _context.ClassInfo.FirstOrDefault(x => x.Id == classInfoContent.ClassInfoId);

                if (cit.Status == (int)classInfoStatus.Audited)
                {
                    cit.Status = (int)classInfoStatus.Edit;
                }
            }
            int num = 0;
            string[] urls = classInfoContent.Url.Split("|");
            foreach (var item in urls)
            {
                ClassInfoContent cic = new ClassInfoContent();
                cic.ClassWeek = classInfoContent.ClassWeek;
                cic.CreateBy = classInfoContent.CreateBy;
                cic.ClassWeekType = classInfoContent.ClassWeekType;
                cic.ClassInfoId = classInfoContent.ClassInfoId;
                cic.Name = classInfoContent.Name;
                cic.NameUrl = classInfoContent.NameUrl;
                cic.IsAudit = classInfoContent.IsAudit;
                cic.ClientId = classInfoContent.ClientId;
                cic.Url = item;
                cic.Contents = classInfoContent.Contents;
                cic.CreateTime = DateTime.Now;
                cic.ClassId = _context.ClassInfo.FirstOrDefault(x => x.Id == cic.ClassInfoId).ClassId;
                cic.UniversityId = _context.Class.FirstOrDefault(x => x.Id == cic.ClassId).UniversityId;
                _context.ClassInfoContent.Add(cic);
                num += _context.SaveChanges();
                id += ","+ cic.Id;
                
            }
            id = id.Substring(1);
            return num;
        }
        /// <summary>
        /// 根据订单id检索周
        /// </summary>
        /// <param name="classInfoId"></param>
        /// <returns></returns>
        public object GetWeek(int classInfoId)
        {
            if (classInfoId != 0)
            {
                var list = _context.ClassInfoContent.Where(x => x.ClassInfoId == classInfoId).GroupBy(x => x.ClassWeek).Select(x => x.Key).ToList();
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
        public List<ClassInfoContent> Types(int classInfoId, int weekName)
        {

            var list = GetListData().Where(x => x.IsAudit == true && x.ClassInfoId == classInfoId && x.ClassWeek == weekName).OrderBy(x => x.Id);
            return list.ToList();

        }
        /// <summary>
        /// 根据客户id,订单id检索答案
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="classInfoId"></param>
        /// <returns></returns>
        public List<ClassInfoContent> GetLs(int clientId, int classInfoId)
        {
            var list = GetListData();
            if (clientId != 0)
            {
                list = list.Where(x => x.ClientId == clientId);
                if (classInfoId != 0)
                {
                    list = list.Where(x => x.ClassInfoId == classInfoId).OrderByDescending(x => x.Id);
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
                var cict = _context.ClassInfoContent.FirstOrDefault(x => x.Id == id);
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
        public ClassInfoContent GetClassInfoContent(int id)
        {
            return _context.ClassInfoContent.FirstOrDefault(x => x.Id == id);

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
                ClassInfoContent cict = _context.ClassInfoContent.FirstOrDefault(x => x.Id == id);
                cict.IsDel = true;
                _context.ClassInfoContent.Update(cict);
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="cict"></param>
        /// <returns></returns>
        public int Edit(ClassInfoContent cict)
        {
            var cit = _context.ClassInfo.FirstOrDefault(x => x.Id == cict.ClassInfoId);
            if (cit.Status == (int)classInfoStatus.Audited)
            {
                cit.Status = (int)classInfoStatus.Edit;
            }
            cict.IsAudit = false;
            _context.ClassInfoContent.Update(cict);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 根据课程资料id检索
        /// </summary>
        /// <param name="classinfoid"></param>
        /// <param name="status"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <param name="PageTotal"></param>
        /// <returns></returns>
        public List<ClassInfoContent> GetListbycinid(int classinfoid, int status, int pagenum, int pagesize, out int PageTotal)
        {
            PageTotal = 0;
            var ls = GetListData();
            if (classinfoid != 0)
            {
                ls = ls.Where(x => x.ClassInfoId == classinfoid);
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
        /// <param name="ClassInfoContent"></param>
        /// <returns></returns>
        public int Audit(ClassInfoContent classInfoContent)
        {
            var cict = _context.ClassInfoContent.FirstOrDefault(x => x.Id == classInfoContent.Id);
            if (cict.IsAudit == true)
            {
                cict.IsAudit = false;
            }
            else
            {
                cict.IsAudit = true;
                cict.Url = classInfoContent.Url;
                cict.Contents = classInfoContent.Contents;
                cict.Name = classInfoContent.Name;
                cict.NameUrl = classInfoContent.NameUrl;
                Utils.WriteInfoLog("ClassInfoContent:Audit" + cict.ToJson());
            }
            return _context.SaveChanges();
        }
        /// <summary>
        /// 检索下一个未审核答案
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClassInfoContent GetNext(int id)
        {
            var classInfoTestId = _context.ClassInfoContent.FirstOrDefault(x => x.Id == id).ClassInfoId;

            return _context.ClassInfoContent.FirstOrDefault(x => x.Id > id && x.ClassInfoId == classInfoTestId && x.IsAudit == false && x.IsDel == false);
        }
        /// <summary>
        /// 查询该订单未审核的答案
        /// </summary>
        /// <param name="classInfoId"></param>
        /// <returns></returns>
        public bool GetNoAudit(int classInfoId)
        {
            var cict = _context.ClassInfoContent.Where(x => x.ClassInfoId == classInfoId && x.IsAudit == false && x.IsDel == false);
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
            var cic = _context.ClassInfoContent.Where(x => x.RefId != 0).OrderByDescending(x => x.RefId).FirstOrDefault();
            return cic == null ? 0 : cic.RefId;
        }
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int AddImportData(List<ClassInfoContent> ls)
        {
            int num = 0;
            foreach (var item in ls)
            {
                _context.ClassInfoContent.Add(item);
                num += _context.SaveChanges();
            }
            return num;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int Update(List<ClassInfoContent> ls)
        {

            _context.ClassInfoContent.UpdateRange(ls);
            return _context.SaveChanges();
        }
    }

}


