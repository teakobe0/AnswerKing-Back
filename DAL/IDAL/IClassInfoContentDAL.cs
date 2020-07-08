using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IClassInfoContentDAL
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        List<ClassInfoContent> GetList();
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        int Add(ClassInfoContent classInfoContent,out string id);
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="cict"></param>
        /// <returns></returns>
        int Edit(ClassInfoContent cict);
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
        List<ClassInfoContent> GetLs(int clientId, int classInfoId);
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
        ClassInfoContent GetClassInfoContent(int id);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        ///  根据课程资料id检索
        /// </summary>
        /// <param name="classinfotestid"></param>
        /// <param name="status"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <param name="PageTotal"></param>
        /// <returns></returns>
        List<ClassInfoContent> GetListbycinid(int classinfotestid, int status, int pagenum, int pagesize, out int PageTotal);
        /// <summary>
        /// 审核、取消审核
        /// </summary>
        /// <param name="classInfoContent"></param>
        /// <returns></returns>
        int Audit(ClassInfoContent classInfoContent);
        /// <summary>
        /// 检索下一个未审核答案
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ClassInfoContent GetNext(int id);
        /// <summary>
        /// 查询该订单是否存在未审核的答案
        /// </summary>
        /// <param name="classInfoTestId"></param>
        /// <returns></returns>
        bool GetNoAudit(int classInfoTestId);
        /// <summary>
        /// 获取当前导入数据的最大
        /// </summary>
        /// <returns></returns>
        int GetImportMaxid();
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        int AddImportData(List<ClassInfoContent> ls);
        /// <summary>
        /// 根据题库集id，周名称检索类型
        /// </summary>
        /// <param name="classInfoId"></param>
        /// <param name="weekName"></param>
        /// <returns></returns>
        List<ClassInfoContent> Types(int classInfoId, int weekName);
        /// <summary>
        /// 查询未识别的图片集
        /// </summary>
        /// <returns></returns>
        List<ClassInfoContent> ImgLs();
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        int Update(List<ClassInfoContent> ls);
    }
}

