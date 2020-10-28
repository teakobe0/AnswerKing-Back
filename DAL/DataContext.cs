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
        public DbSet<Client> Client { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<ImportRecords> ImportRecords { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Notice> Notice { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<UseRecords> UseRecords { get; set; }
        public DbSet<Focus> Focus { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<UniversityCombine> UniversityCombine { get; set; }
        public DbSet<ClassCombine> ClassCombine { get; set; }
        public DbSet<Class> Class { get; set; }
        public DbSet<University> University { get; set; }
        public DbSet<ClassInfo> ClassInfo { get; set; }
        public DbSet<ClassInfoContent> ClassInfoContent { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Bidding> Bidding { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<IntegralRecords> IntegralRecords { get; set; }
        public DbSet<Favourite> Favourite { get; set; }
        public DbSet<ClientQuestionInfo> ClientQuestionInfo { get; set; }
    }
}
