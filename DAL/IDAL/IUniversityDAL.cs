using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IUniversityDAL
    {
        /// <summary>
        /// 查询列表 带条件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<University> GetList(string name);
        /// <summary>
        /// 根据学校id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        University GetUniversity(int id);
        /// <summary>
        /// 隐藏学校
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Hide(int id);
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        List<University> GetList();
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="university"></param>
        /// <returns></returns>
        int Add(University university);
        /// <summary>
        /// 检验学校名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool GetName(string name, int id);
        /// <summary>
        /// 修改学校信息
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="university"></param>
        /// <returns></returns>
        int ChangeInfo(int ID, University university);
        /// <summary>
        /// 修改学校信息
        /// </summary>
        /// <param name="university"></param>
        /// <returns></returns>
        int ChangeInfo(University university);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        /// 合并数据
        /// </summary>
        /// <param name="cbrows"></param>
        /// <param name="targetid"></param>
        /// <returns></returns>
        int Combine(List<University> cbrows, int targetid);
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        int Import(List<University> ls);
        /// <summary>
        /// 获取当前导入数据的最大
        /// </summary>
        /// <returns></returns>
        int GetMaxId();
        /// <summary>
        /// 根据学校名称检索学校id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int Getbyname(string name);
        /// <summary>
        /// 根据国家和州/省份检索学校
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<University> GetByCountry(string name, string state);
        University GetUniversity(string name);
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        int DelImg(int id, string img);
    }
}
