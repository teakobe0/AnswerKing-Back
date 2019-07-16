using DAL.DAL;
using DAL.IDAL;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Ioc
{
    public class DALDIRegister
    {
        public void DIRegister_DAL(IServiceCollection services)
        {
            services.AddTransient(typeof(IClientDAL), typeof(ClientDAL));
            services.AddTransient(typeof(IUserDAL), typeof(UserDAL));
            services.AddTransient(typeof(IClassInfoContentDAL), typeof(ClassInfoContentDAL));
            services.AddTransient(typeof(IClassDAL), typeof(ClassDAL));
            services.AddTransient(typeof(IClassInfoDAL), typeof(ClassInfoDAL));
            services.AddTransient(typeof(IClassWeekDAL), typeof(ClassWeekDAL));
            services.AddTransient(typeof(IClassWeekTypeDAL), typeof(ClassWeekTypeDAL));
            services.AddTransient(typeof(IUniversityDAL), typeof(UniversityDAL));
            services.AddTransient(typeof(IHomeDAL), typeof(HomeDAL));
            services.AddTransient(typeof(IAreaDAL), typeof(AreaDAL));
            services.AddTransient(typeof(IImportRecordsDAL), typeof(ImportRecordsDAL));
            services.AddTransient(typeof(ICommentDAL), typeof(CommentDAL));
            services.AddTransient(typeof(INoticeDAL), typeof(NoticeDAL));
            services.AddTransient(typeof(IOrderDAL), typeof(OrderDAL));
            services.AddTransient(typeof(IUseRecordsDAL), typeof(UseRecordsDAL));
            services.AddTransient(typeof(IFocusDAL), typeof(FocusDAL));
            services.AddTransient(typeof(IFeedbackDAL), typeof(FeedbackDAL));
        }
    }
}
