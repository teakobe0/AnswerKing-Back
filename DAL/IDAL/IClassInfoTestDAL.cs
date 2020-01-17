using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IClassInfoTestDAL
    {  /// <summary>
       /// 查询列表
       /// </summary>
       /// <returns></returns> 
        List<ClassInfoTest> GetList();
        /// <summary>
        /// 新增课程
        /// </summary>
        /// <param name="cit"></param>
        /// <returns></returns>
        ClassInfoTest Add(ClassInfoTest cit);
        /// <summary>
        /// 根据课程资料id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <returns></returns>
        ClassInfoTest GetClassInfoTest(int id);
        /// <summary>
        /// 根据客户id检索课程资料
        /// </summary>
        /// <returns></returns>
        List<ClassInfoTest> GetList(int clientId);
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="cit"></param>
        /// <returns></returns>
        int Edit(ClassInfoTest cit);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
    }  
}
