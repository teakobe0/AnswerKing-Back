﻿using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    /// <summary>
    /// 评论表数据访问层
    /// </summary>
    public class CommentDAL : BaseDAL, ICommentDAL
    {
        public CommentDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        public List<Comment> GetListPage(int type, int typeid)
        {
            var list = GetListData();
            if (type != 0)
            {
                list = list.Where(x => x.Type == type);
            }
            if (typeid != 0)
            {
                list = list.Where(x => x.TypeId == typeid);
            }
            return list.ToList();
        }
        /// <summary>
        /// 根据客户id检索评论
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        public List<Comment> GetListByClientid(int clientid)
        {
            var list = GetListData();
            if (clientid != 0)
            {
                list = list.Where(x => x.ClientId == clientid).OrderByDescending(x => x.Id);
            }
            return list.ToList();
        }
        private IQueryable<Comment> GetListData()
        {
            return _context.Comment.Where(x => x.IsDel == false);
        }
        /// <summary>
        /// 查询列表全部数据 根据条件
        /// </summary>
        /// <returns></returns>
        public List<Comment_v> GetList(int type, int typeid)
        {
            List<Comment_v> ls = new List<Comment_v>();
            Comment_v cv = null;

            var list = _context.Comment.Where(x => x.IsDel == false && x.Type == type).OrderBy(x => x.CreateTime).ToList();

            if (typeid != 0)
            {
                list = list.Where(x => x.TypeId == typeid).ToList();
                var parent = list.Where(x => x.ParentId == "0");
                foreach (var item in parent)
                {
                    cv = new Comment_v();
                    cv = Utils.TransReflection<Comment, Comment_v>(item);
                    var model = _context.Client.FirstOrDefault(x => x.Id == item.ClientId);
                    cv.name = model.Name;
                    cv.img = model.Image == null ? null : AppConfig.Configuration["imgurl"] + model.Image;
                    ls.Add(cv);

                    var son = list.Where(x => x.ParentId.Contains("," + item.Id) && x.ParentId.Substring(x.ParentId.LastIndexOf(",") + 1) == item.Id.ToString());
                    foreach (var ea in son)
                    {
                        cv = new Comment_v();
                        cv = Utils.TransReflection<Comment, Comment_v>(ea);
                        var reply = _context.Client.FirstOrDefault(x => x.Id == ea.ClientId);
                        cv.replyname = reply.Name;
                        cv.replyimg = reply.Image == null ? null : AppConfig.Configuration["imgurl"] + reply.Image;
                        var breply = _context.Client.FirstOrDefault(x => x.Id == item.ClientId);
                        cv.name = breply.Name;
                        cv.img = breply.Image == null ? null : AppConfig.Configuration["imgurl"] + breply.Image;
                        ls.Add(cv);
                        var sonreply = list.Where(x => x.ParentId.Contains("," + ea.Id + ",") ||
                         (x.ParentId.Contains("," + ea.Id) && x.ParentId.Substring(x.ParentId.LastIndexOf(",") + 1) == ea.Id.ToString()));
                        foreach (var cc in sonreply)
                        {
                            cv = new Comment_v();
                            cv = Utils.TransReflection<Comment, Comment_v>(cc);
                            var re = _context.Client.FirstOrDefault(x => x.Id == cc.ClientId);
                            cv.replyname = re.Name;
                            cv.replyimg = re.Image == null ? null : AppConfig.Configuration["imgurl"] + re.Image;
                            int cid = _context.Comment.FirstOrDefault(x => x.Id == int.Parse(cc.ParentId.Substring(cc.ParentId.LastIndexOf(",") + 1))).ClientId;
                            var b = _context.Client.FirstOrDefault(x => x.Id == cid);
                            cv.name = b.Name;
                            cv.img = b.Image == null ? null : AppConfig.Configuration["imgurl"] + b.Image;
                            ls.Add(cv);
                        }
                    }
                }
            }
            return ls;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public int Add(Comment comment)
        {

            comment.CreateTime = DateTime.Now;
            _context.Comment.Add(comment);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            var co = _context.Comment.FirstOrDefault(x => x.Id == id);
            var dellist = _context.Comment.Where(x =>
             x.ParentId.Contains("," + co.Id + ",")
            || (x.ParentId.Contains("," + co.Id) && x.ParentId.Substring(x.ParentId.LastIndexOf(",") + 1) == co.Id.ToString()));
            Utils.WriteInfoLog("Comment:Delete" + co.ToJson() + dellist.ToJson());
            foreach (var item in dellist)
            {
                item.IsDel = true;
            }
            _context.Comment.UpdateRange(dellist);
            co.IsDel = true;
            _context.Comment.Update(co);
            return _context.SaveChanges();
        }

    }

    public class Comment_v : Comment
    {
        public string replyname { get; set; }//回复人昵称
        public string replyimg { get; set; }//回复人图像
        public string name { get; set; }//被回复昵称
        public string img { get; set; }//被回复图像
        public string contentsid { get; set; } //内容id
    }
}
