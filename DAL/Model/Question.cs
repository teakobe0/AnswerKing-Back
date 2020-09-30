using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Model
{
    /// <summary>
    /// 问题表
    /// </summary>
    public class Question : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Number { get; set; } //问题编号
        public int Type { get; set; } //问题类别
        public string Title { get; set; }//标题
        public string Content { get; set; }//内容
        public string Img { get; set; }//图片
        public DateTime EndTime { get; set; }//截止时间
        public int Currency { get; set; }//货币
        public int Answerer { get; set; }//回答者
        public string Evaluate { get; set; }//评价
        public int Sign { get; set; }//标记 5:好评，1：差评,3：超时
        public string Memo { get; set; }//备注（退款原因）
        public bool IsAudit { get; set; }//是否审核 0：未审核，1：已审核
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
        public int Status { get; set; }//状态 1:保存 2：正在竞拍 3：已选竞拍者，4：已回答，5：申请客服，6：已完成,7:已关闭

    }
}

