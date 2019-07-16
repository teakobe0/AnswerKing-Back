using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
   public interface  IClassInfoContentDAL
    {
        /// <summary>
        /// 查询列表 带条件
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        List<ClassInfoContent> GetList(string searchText);
        /// <summary>
        /// 查询列表 带条件
        /// </summary>
        /// <param name="classweekid"></param>
        /// <returns></returns>
        List<ClassInfoContent> GetList(int classweekid);
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        List<ClassInfoContent> GetList();
        /// <summary>
        /// 查询url为空的数据
        /// </summary>
        /// <returns></returns>
        List<ClassInfoContent> Urls();
        /// <summary>
        /// 根据答案id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ClassInfoContent GetClassInfoContent(int id);
        /// <summary>
        /// 检索下一个未审核答案
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ClassInfoContent GetNext(int id);
        /// <summary>
        /// 新增答案
        /// </summary>
        /// <param name="classInfoContent"></param>
        /// <returns></returns>
        int Add(ClassInfoContent classInfoContent);
        /// <summary>
        /// 编辑答案
        /// </summary>
        /// <param name="classInfoContent"></param>
        /// <returns></returns>
        int ChangeInfo(int Id, ClassInfoContent classInfoContent);
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="classInfoContent"></param>
        /// <returns></returns>
        int Audit(ClassInfoContent classInfoContent);
        /// <summary>
        /// 编辑答案
        /// </summary>
        /// <param name="classInfoContent"></param>
        /// <returns></returns>
        int ChangeInfo(ClassInfoContent classInfoContent);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        /// 根据课程资料id检索
        /// </summary>
        /// <param name="classinfoid"></param>
        /// <returns></returns>
        object GetListbycinid(int classinfoid, int status, int pagenum,int pagesize, out int PageTotal);
        /// <summary>
        /// 获取当前导入数据的最大
        /// </summary>
        /// <returns></returns>
        int GetImportMaxid();
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        int DelImg(int id, string img);
        /// <summary>
        /// 根据每周课程类型id检索
        /// </summary>
        /// <param name="classweekid"></param>
        /// <returns></returns>
        List<ClassInfoContent> GetByTypeid(int classweektypeid,int id);
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
         int AddImportData(List<ClassInfoContent> ls);
    }
}
