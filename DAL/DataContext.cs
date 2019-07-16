using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Class> Class { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<ClassInfo> ClassInfo { get; set; }
        public DbSet<ClassWeek> ClassWeek { get; set; }
        public DbSet<ClassWeekType> ClassWeekType { get; set; }
        public DbSet<ClassInfoContent> ClassInfoContent { get; set; }
        public DbSet<University> University { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<ImportRecords> ImportRecords { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Notice> Notice { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<UseRecords> UseRecords { get; set; }
        public DbSet<Focus> Focus { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
    }
}
