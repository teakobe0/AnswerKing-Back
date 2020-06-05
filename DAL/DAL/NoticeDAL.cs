using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class NoticeDAL : BaseDAL, INoticeDAL
    {

        public NoticeDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        public List<Notice> GetList()
        {
            return GetListData().ToList();
        }

        public class Notice_v:Notice
        {
            
            public string Sendname { get; set; }
        }
        /// <summary>
        /// 根据客户id检索通知消息
        /// </summary>
        /// <returns></returns>
        public List<Notice_v> GetList(int clientid)
        {
            List<Notice_v> ls = new List<Notice_v>();
            Notice_v nv = null;
            var list = _context.Notice.Where(x => x.ReceiveId == clientid && x.IsRead == 0);
            foreach (var item in list)
            {
                nv = new Notice_v();
                nv = Utils.TransReflection<Notice, Notice_v>(item);
                if (item.SendId == 0 && item.CreateBy != "0")
                {
                    nv.Sendname = "系统客服";
                }
                else
                {
                    nv.Sendname = _context.Client.FirstOrDefault(x => x.Id == item.SendId).Name;
                }
                ls.Add(nv);
            }
            return ls;

        }
        private IQueryable<Notice> GetListData()
        {
            return _context.Notice.Where(x => x.IsDel == false);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="comment_v"></param>
        /// <returns></returns>
        public int Add(Comment_v comment_v)
        {
            Notice notice = new Notice();
            notice.CreateTime = DateTime.Now;
            notice.SendId = comment_v.ClientId;
            var cpid = int.Parse(comment_v.ParentId.Substring(comment_v.ParentId.LastIndexOf(",") + 1));
            var clientid = _context.Comment.FirstOrDefault(x => x.Id == cpid).ClientId;
            notice.ReceiveId = clientid;
            notice.ContentsUrl = comment_v.contenturl;
            _context.Notice.Add(notice);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 修改通知状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ChangeStatus(int id)
        {
            var notice = _context.Notice.FirstOrDefault(x => x.Id == id);
            notice.IsRead = 1;
            return _context.SaveChanges();

        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public int Add(Notice notice)
        {

            notice.CreateTime = DateTime.Now;
            _context.Notice.Add(notice);
            return _context.SaveChanges();
        }
    }
}
