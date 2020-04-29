using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IUniversityDAL
    {
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        List<University> GetList();
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        University GetUniversity(int id);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="university"></param>
        /// <returns></returns>
        University Add(University university);
        /// <summary>
        /// 根据客户id检索学校
        /// </summary>
        /// <returns></returns>
        List<University> GetList(int clientId);
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        int Edit(University ut);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        int DelImg(int id, string img);
       /// <summary>
       /// 根据学校名称检索
       /// </summary>
       /// <param name="name"></param>
       /// <returns></returns>
        List<University> GetList(string name);
        /// <summary>
        /// 查询学校名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool GetName(string name, int id);
        /// <summary>
        /// 根据学校名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        University GetUniversity(string name);
        /// <summary>
        /// 获取当前导入数据的最大id
        /// </summary>
        /// <returns></returns>
        int GetImportMaxid();
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        int AddImportData(List<University> ls);
        /// <summary>
        /// 根据国家和州/省份检索学校
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<University> GetByCountry(string name, string state);
    }
}
