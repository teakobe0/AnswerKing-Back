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
    /// 学校表数据访问层
    /// </summary>
    public class UniversityDAL : BaseDAL, IUniversityDAL
    {
        public UniversityDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        public List<University> GetList()
        {
            return GetListData().ToList();
        }

        private IQueryable<University> GetListData()
        {
            return _context.University.Where(x => x.IsDel == false);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        public List<University> GetList(string name, int status)
        {
            var list = GetListData();
            if (!string.IsNullOrEmpty(name))
            {
                list = list.Where(x => x.Name.Trim().Contains(name.Trim()));
            }
            if (status != -1)
            {
                if (status == 1)
                {
                    list = list.Where(x => x.IsAudit == true);
                }
                else
                {
                    list = list.Where(x => x.IsAudit == false);
                }
            }
            return list.ToList();

        }
        /// <summary>
        /// 根据学校id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public University GetUniversity(int id)
        {
            return _context.University.FirstOrDefault(x => x.Id == id);
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
                return _context.University.Any(x => x.Name.Trim() == name.Trim() && x.IsDel == false);
            }
            else
            {
                return _context.University.Any(x => x.Id != id && x.Name.Trim() == name.Trim() && x.IsDel == false);
            }

        }
        /// <summary>
        /// 根据学校名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public University GetUniversity(string name)
        {
            return _context.University.FirstOrDefault(x => x.Name.Trim() == name.Trim() && x.IsDel == false);

        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="university"></param>
        /// <returns></returns>
        public University Add(University university)
        {

            university.CreateTime = DateTime.Now;
            _context.University.Add(university);
            _context.SaveChanges();
            return university;

        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public int DelImg(int id, string img)
        {
            var u = _context.University.FirstOrDefault(x => x.Id == id);
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
        public int Edit(University university)
        {
            try
            {
                var model = _context.University.FirstOrDefault(x => x.Id == university.Id);
                if (model.Name != university.Name)
                {
                    model.Name = university.Name;
                    var clist = _context.Class.Where(x => x.UniversityId == university.Id);
                    foreach (var item in clist)
                    {
                        item.University = university.Name;
                    }
                    _context.Class.UpdateRange(clist);
                }
                model.Country = university.Country;
                model.City = university.City;
                model.State = university.State;
                model.Intro = university.Intro;
                model.Image = university.Image;
                model.IsAudit = false;
                _context.University.Update(model);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

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
                University ut = _context.University.FirstOrDefault(x => x.Id == id);
                ut.IsDel = true;
                _context.University.Update(ut);
                //删除这个学校下面的课
                var clas = _context.Class.Where(x => x.UniversityId == id);
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
        public List<University> GetList(int clientId)
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
            var c = _context.University.OrderByDescending(x => x.RefId).FirstOrDefault();
            return c == null ? 0 : c.RefId;
        }
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int AddImportData(List<University> ls)
        {
            int num = 0;
            foreach (var item in ls)
            {
                _context.University.Add(item);
                num += _context.SaveChanges();
            }
            return num;
        }
        /// <summary>
        /// 根据国家和州/省份检索学校
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<University> GetByCountry(string name, string state)
        {
            var list = GetListData();
            if (!string.IsNullOrEmpty(name))
                list = list.Where(x => x.Country == name);
            if (!string.IsNullOrEmpty(state))
            {
                list = list.Where(x => x.State == state);
            }
            return list.ToList();
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="university"></param>
        /// <returns></returns>
        public int Audit(University university)
        {
            university.IsAudit = true;
            _context.University.Update(university);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 检索下一个未审核学校
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public University GetNext(int id)
        {

            return _context.University.FirstOrDefault(x => x.Id > id && x.IsAudit == false && x.IsDel == false);
        }
        /// <summary>
        /// 合并
        /// </summary>
        /// <param name="cbmodel"></param>
        /// <param name="targetid"></param>
        /// <returns></returns>
        public int Combine(University cbmodel, int targetid)
        {
            var target = _context.University.FirstOrDefault(x => x.Id == targetid);
            cbmodel.IsDel = true;
            _context.University.Update(cbmodel);
            Utils.WriteInfoLog("University:CombineDel" + cbmodel.ToJson() + ",targetid:" + targetid);
            var classes = _context.Class.Where(x => x.UniversityId == cbmodel.Id);
            foreach (var cs in classes)
            {
                cs.UniversityId = target.Id;
                cs.University = target.Name;
            }
            var cinfos = _context.ClassInfoContent.Where(x => x.UniversityId == cbmodel.Id);
            foreach (var i in cinfos)
            {
                i.UniversityId = target.Id;
            }
            return _context.SaveChanges();
        }
    }
}

