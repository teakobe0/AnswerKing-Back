using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    /// <summary>
    /// 课程表数据访问层
    /// </summary>
    public class ClassDAL : BaseDAL, IClassDAL
    {
        public ClassDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        public List<Class> GetList()
        {
            return GetListData().ToList();

        }
        public Class GetRandomClass()
        {
            return _context.Class.Where(x => 1 == 1).OrderBy(x => Guid.NewGuid()).First();
        }
        private IQueryable<Class> GetListData()
        {
            return _context.Class.Where(x => x.IsDel == false);
        }
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Class> GetList(string name)
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
        public Class Add(Class cla)
        {
            cla.CreateTime = DateTime.Now;
            cla.University = _context.University.FirstOrDefault(x => x.Id == cla.UniversityId).Name;
            _context.Class.Add(cla);
            _context.SaveChanges();
            return cla;
        }
        /// <summary>
        /// 编辑课程
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        public int Edit(Class cla)
        {
            cla.University = _context.University.FirstOrDefault(x => x.Id == cla.UniversityId).Name;
            cla.IsAudit = false;
            _context.Class.Update(cla);
            //查询该课答案
            var cls = _context.ClassInfoContent.Where(x => x.ClassId == cla.Id && x.UniversityId != cla.UniversityId);
            foreach (var i in cls)
            {
                i.UniversityId = cla.UniversityId;
            }
            _context.ClassInfoContent.UpdateRange(cls);
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
                Class ct = _context.Class.FirstOrDefault(x => x.Id == id);
                ct.IsDel = true;
                _context.Class.Update(ct);
                //删除这个课下面的题库集
                var cit = _context.ClassInfo.Where(x => x.ClassId == id);
                foreach (var i in cit)
                {
                    i.IsDel = true;
                }
                _context.ClassInfo.UpdateRange(cit);
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 根据条件检索
        /// </summary>
        /// <param name="universityId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Class> GetList(int universityId, string name)
        {
            var list = GetListData();
            if (universityId != 0)
            {
                list = list.Where(x => x.UniversityId == universityId);
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
        public List<Class> GetList(int universityid, string alif, string name)
        {
            var list = GetListData();
            if (universityid != 0)
            {
                list = list.Where(x => x.UniversityId == universityid);

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
        /// <param name="universityId"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool GetName(int universityId, string name, int id)
        {
            if (id == 0)
            {
                return _context.Class.Any(x => x.UniversityId == universityId && x.Name.Trim() == name.Trim());
            }
            else
            {
                return _context.Class.Any(x => x.Id != id && x.UniversityId == universityId && x.Name.Trim() == name.Trim());
            }


        }
        /// <summary>
        /// 根据课程id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Class GetClass(int id)
        {
            return _context.Class.FirstOrDefault(x => x.Id == id);

        }
        /// <summary>
        /// 根据客户id检索课程
        /// </summary>
        /// <returns></returns>
        public List<Class> GetList(int clientId)
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
        public List<Class> GetListbyname(string name, int status)
        {

            var ls = GetListData();
            if (!string.IsNullOrEmpty(name))
            {
                ls = ls.Where(x => x.Name.Trim().Contains(name.Trim()));
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
            return ls.ToList();
        }
        /// <summary>
        /// 获取当前导入数据的最大id
        /// </summary>
        /// <returns></returns>
        public int GetImportMaxid()
        {
            var c = _context.Class.OrderByDescending(x => x.RefId).FirstOrDefault();
            return c == null ? 0 : c.RefId;
        }
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int AddImportData(List<Class> ls)
        {
            int num = 0;
            foreach (var item in ls)
            {
                _context.Class.Add(item);
                num += _context.SaveChanges();
            }
            return num;
        }
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Class> GetLs(int universityid)
        {
            var list = GetListData();
            if (universityid != 0)
            {
                list = list.Where(x => x.UniversityId == universityid);
                return list.ToList();
            }
            return null;
        }
        /// <summary>
        /// 根据学校id查询
        /// </summary>
        /// <returns></returns> 
        public List<Class> GetClasses(int universityid)
        {
            var list = GetListData().Where(x => x.UniversityId == universityid);
            return list.ToList();
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="clas"></param>
        /// <returns></returns>
        public int Audit(Class clas)
        {
            clas.University = _context.University.FirstOrDefault(x => x.Id == clas.UniversityId).Name;
            clas.IsAudit = true;
            _context.Class.Update(clas);
            //查询该课答案
            var cls = _context.ClassInfoContent.Where(x => x.ClassId == clas.Id && x.UniversityId != clas.UniversityId);
            foreach (var i in cls)
            {
                i.UniversityId = clas.UniversityId;
            }
            _context.ClassInfoContent.UpdateRange(cls);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 检索下一个未审核学校
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Class GetNext(int id)
        {

            return _context.Class.FirstOrDefault(x => x.Id > id && x.IsAudit == false && x.IsDel == false);
        }
        /// <summary>
        /// 根据条件检索
        /// </summary>
        /// <param name="universityId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Class GetClasses(int universityId, string name)
        {
            return _context.Class.FirstOrDefault(x => x.UniversityId == universityId && x.Name.Trim() == name.Trim() && x.IsDel == false);

        }
        /// <summary>
        /// 合并课程
        /// </summary>
        /// <param name="cbmodel"></param>
        /// <param name="targetid"></param>
        /// <returns></returns>
        public int Combine(Class cbmodel, int targetid)
        {
            cbmodel.IsDel = true;
            _context.Class.Update(cbmodel);
            Utils.WriteInfoLog("Class:CombineDel" + cbmodel.ToJson() + ",targetid:" + targetid);
            var classinfo = _context.ClassInfo.Where(x => x.ClassId == cbmodel.Id);
            foreach (var ci in classinfo)
            {
                ci.ClassId = targetid;
            }
            var classinfocontent = _context.ClassInfoContent.Where(x => x.ClassId == cbmodel.Id);
            foreach (var co in classinfocontent)
            {
                co.ClassId = targetid;
            }
            return _context.SaveChanges();
        }

    }
}
