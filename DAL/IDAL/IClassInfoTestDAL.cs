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
        /// 根据课程id检索
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        List<ClassInfoTest> GetLs(int classid);
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
        /// <summary>
        /// 查询该客户是否创建过该课程的订单
        /// </summary>
        /// <param name="clientid"></param>
        /// <param name="classtestid"></param>
        /// <returns></returns>
        ClassInfoTest GetClassInfoTest(int clientid, int classtestid);
        /// <summary>
        /// 根据课程资料单号查询
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
         List<ClassInfoTest> GetListByno(int no, int status);
        /// <summary>
        /// 更改题库集状态
        /// </summary>
        /// <param name="cit"></param>
        /// <returns></returns>
        int Change(int id);
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="classInfoTest"></param>
        /// <returns></returns>
        int Audit(ClassInfoTest classInfoTest);
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
        int AddImportData(List<ClassInfoTest> ls);
        /// <summary>
        /// 修改课程资料有用、没用
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="classInfoId"></param>
        /// <param name="type"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        int Change(int clientId, int classInfoId, string type, int check, DateTime? time);
        int GetClients();
        ClassInfoTest GetRandomClassInfo();
        ClassInfoTest GetRandom();
        /// <summary>
        /// 修改课程资料
        /// </summary>
        /// <param name="classInfo"></param>
        /// <returns></returns>
        int ChangeClassInfo(ClassInfoTest classInfoTest);
    }  
}
