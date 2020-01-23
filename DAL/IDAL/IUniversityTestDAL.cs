using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IUniversityTestDAL
    {
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        List<UniversityTest> GetList();
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        UniversityTest GetUniversityTest(int id);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="university"></param>
        /// <returns></returns>
        UniversityTest Add(UniversityTest universityTest);
        /// <summary>
        /// 根据客户id检索学校
        /// </summary>
        /// <returns></returns>
        List<UniversityTest> GetList(int clientId);
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        int Edit(UniversityTest ut);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
       /// <summary>
       /// 根据学校名称检索
       /// </summary>
       /// <param name="name"></param>
       /// <returns></returns>
        List<UniversityTest> GetList(string name);
        /// <summary>
        /// 查询学校名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool GetName(string name, int id);
       

    }
}
