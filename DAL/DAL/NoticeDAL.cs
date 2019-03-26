using DAL.IDAL;
using DAL.Model;
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
        /// <summary>
        /// 根据客户id检索通知消息
        /// </summary>
        /// <returns></returns>
        public List<Notice> GetList(int clientid)
        {
            var list = GetListData();
            list = list.Where(x => x.RecipientId == clientid&&x.IsRead==0);
            return list.ToList();

        }
        private IQueryable<Notice> GetListData()
        {
            return _context.Notice.Where(x => 1 == 1);
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
            notice.InitiatorId = comment_v.ClientId;
            var cpid = int.Parse(comment_v.ParentId.Substring(comment_v.ParentId.LastIndexOf(",") + 1));
            var clientid = _context.Comment.FirstOrDefault(x => x.Id == cpid).ClientId;
            notice.RecipientId = clientid;
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
    }
}
