using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class UniversityDAL : BaseDAL, IUniversityDAL
    {
        public UniversityDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <returns></returns>
        public List<University> GetList(string name)
        {
            var list = GetListData();
            if (!string.IsNullOrEmpty(name))
                list = list.Where(x => x.Name.Contains(name));

            return list.ToList();

        }
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <returns></returns>
        public University GetUniversity(int id)
        {
            return _context.University.FirstOrDefault(x => x.Id == id); 
        }
        /// <summary>
        /// 检验学校名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GetName(string name)
        {
            return _context.University.Any(x => x.Name == name);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="university"></param>
        /// <returns></returns>
        public int Add(University university)
        {
            var unty = _context.University.OrderByDescending(x => x.Id).FirstOrDefault();
            //university.Id = (unty == null || unty.Id < InitID) ? InitID : unty.Id + 1;
            university.CreateTime = DateTime.Now;
            _context.University.Add(university);
            return _context.SaveChanges();

        }
        /// <summary>
        /// 根据学校名称检索学校id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int Getbyname(string name)
        {
            var universityid = _context.University.FirstOrDefault(x => x.Name == name) == null ? 0 : _context.University.FirstOrDefault(x => x.Name == name).Id;
            return universityid;
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
        /// 修改学校信息
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="university"></param>        
        /// <returns></returns>
        public int ChangeInfo(int ID, University university)
        {
            var data = _context.University.FirstOrDefault(x => x.Id == ID);
            data.Image = university.Image;
            data.Intro = university.Intro;
            return _context.SaveChanges();
        }
        /// <summary>
        /// 修改学校信息
        /// </summary>
        /// <param name="university"></param>
        /// <returns></returns>
        public int ChangeInfo(University university)
        {
            _context.University.Update(university);
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
                var university = _context.University.FirstOrDefault(x => x.Id == id);
                Utils.WriteInfoLog("University:Delete" + university.ToJson());
                _context.University.Remove(university);
                return _context.SaveChanges();
            }
            return 0;
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
            return _context.University.Where(x => 1 == 1);
        }

        /// <summary>
        /// 合并
        /// </summary>
        /// <param name="cbrows"></param>
        /// <param name="targetid"></param>
        /// <returns></returns>
        public int Combine(List<University> cbrows, int targetid)
        {
            var target = _context.University.FirstOrDefault(x => x.Id == targetid);

            foreach (var item in cbrows)
            {
                if (item.Id != targetid)
                {
                    _context.University.Remove(item);
                    var data = _context.University.FirstOrDefault(x => x.Id == item.Id);
                    Utils.WriteInfoLog("University:CombineDel" + data.ToJson());
                    var classes = _context.Class.Where(x => x.UniversityId == item.Id);
                    foreach (var cs in classes)
                    {
                        cs.UniversityId = target.Id;
                        cs.University = target.Name;

                    }

                    var cinfos = _context.ClassInfoContent.Where(x => x.UniversityId == item.Id);
                    foreach (var cs in cinfos)
                    {
                        cs.UniversityId = target.Id;
                    }
                }
            }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
            return _context.SaveChanges();
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int Import(List<University> ls)
        {
            _context.University.AddRange(ls);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 查询当前导入数据的最大
        /// </summary>
        /// <returns></returns>
        public int GetMaxId()
        {
            var max = _context.University.OrderByDescending(x => x.Id).FirstOrDefault();
            return max == null ? 0 : max.Id;
        }
    }
}
