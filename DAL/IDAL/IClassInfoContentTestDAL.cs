using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IClassInfoContentTestDAL
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        List<ClassInfoContentTest> GetList();
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        int Add(ClassInfoContentTest classInfoContentTest);
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="cict"></param>
        /// <returns></returns>
        int Edit(ClassInfoContentTest cict);
        /// <summary>
        /// 根据订单id检索周
        /// </summary>
        /// <param name="classInfoTestId"></param>
        /// <returns></returns>
        object GetWeek(int classInfoTestId);
        /// <summary>
        /// 根据客户id,订单id检索答案
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="classInfoTestId"></param>
        /// <returns></returns>
        List<ClassInfoContentTest> GetLs(int clientId, int classInfoTestId);
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        int DelImg(int id, string img);
        /// <summary>
        /// 根据答案id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ClassInfoContentTest GeClassInfoContentTest(int id);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
    }
}

