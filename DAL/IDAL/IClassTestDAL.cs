using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IClassTestDAL
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        List<ClassTest> GetList();
        /// <summary>s
        /// 新增课程
        /// </summary>
        /// <param name="clat"></param>
        /// <returns></returns>
        ClassTest Add(ClassTest clat);
        /// <summary>
        /// 根据条件检索
        /// </summary>
        /// <param name="universityTestId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        List<ClassTest> GetList(int universityTestId, string name);
        /// <summary>
        /// 根据课程id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ClassTest GeClassTest(int id);
        /// <summary>
        /// 根据客户id检索课程
        /// </summary>
        /// <returns></returns>
        List<ClassTest> GetList(int clientId);
        /// <summary>
        /// 编辑课程
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        int Edit(ClassTest cla);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        /// 查询同一学校的该课程名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool GetName(int universityTestId, string name, int id);


    }
}
