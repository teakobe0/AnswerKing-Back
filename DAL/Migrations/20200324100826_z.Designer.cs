﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20200324100826_z")]
    partial class z
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DAL.Model.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsAudit");

                    b.Property<bool>("IsDel");

                    b.Property<int>("QuestionId");

                    b.Property<int>("RefId");

                    b.Property<int>("Sign");

                    b.HasKey("Id");

                    b.ToTable("Answer");
                });

            modelBuilder.Entity("DAL.Model.Area", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("ParentId");

                    b.Property<int>("RefId");

                    b.Property<string>("State");

                    b.HasKey("Id");

                    b.ToTable("Area");
                });

            modelBuilder.Entity("DAL.Model.Bidding", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("Currency");

                    b.Property<DateTime>("EndTime");

                    b.Property<bool>("IsDel");

                    b.Property<int>("QuestionId");

                    b.Property<int>("RefId");

                    b.HasKey("Id");

                    b.ToTable("Bidding");
                });

            modelBuilder.Entity("DAL.Model.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Difficulty");

                    b.Property<bool>("IsDel");

                    b.Property<string>("Memo");

                    b.Property<string>("Name");

                    b.Property<int>("OriginUniversityId");

                    b.Property<string>("Professor");

                    b.Property<int>("RefId");

                    b.Property<string>("University");

                    b.Property<int>("UniversityId");

                    b.HasKey("Id");

                    b.ToTable("Class");
                });

            modelBuilder.Entity("DAL.Model.ClassCombine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsDel");

                    b.Property<int>("OriginalId");

                    b.Property<int>("RefId");

                    b.Property<int>("TargetId");

                    b.HasKey("Id");

                    b.ToTable("ClassCombine");
                });

            modelBuilder.Entity("DAL.Model.ClassInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassId");

                    b.Property<int>("ClientId");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Evaluate");

                    b.Property<bool>("IsDel");

                    b.Property<int>("NoUse");

                    b.Property<int>("OriginClassId");

                    b.Property<int>("PageViews");

                    b.Property<int>("RefId");

                    b.Property<int>("TotalGrade");

                    b.Property<int>("Use");

                    b.HasKey("Id");

                    b.ToTable("ClassInfo");
                });

            modelBuilder.Entity("DAL.Model.ClassInfoContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassId");

                    b.Property<int>("ClassInfoId");

                    b.Property<int>("ClassWeekId");

                    b.Property<int>("ClassWeekTypeId");

                    b.Property<string>("Contents");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("CwtParentId");

                    b.Property<int>("IsAudit");

                    b.Property<int>("OriginClassId");

                    b.Property<int>("OriginUniversityId");

                    b.Property<int>("RefId");

                    b.Property<int>("Status");

                    b.Property<int>("UniversityId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("ClassInfoContent");
                });

            modelBuilder.Entity("DAL.Model.ClassInfoContentTest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassInfoTestId");

                    b.Property<int>("ClassTestId");

                    b.Property<int>("ClassWeek");

                    b.Property<string>("ClassWeekType");

                    b.Property<int>("ClientId");

                    b.Property<string>("Contents");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsAudit");

                    b.Property<bool>("IsDel");

                    b.Property<string>("Name");

                    b.Property<string>("NameUrl");

                    b.Property<int>("RefId");

                    b.Property<int>("UniversityTestId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("ClassInfoContentTest");
                });

            modelBuilder.Entity("DAL.Model.ClassInfoTest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassTestId");

                    b.Property<int>("ClientId");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsDel");

                    b.Property<string>("Name");

                    b.Property<int>("RefId");

                    b.HasKey("Id");

                    b.ToTable("ClassInfoTest");
                });

            modelBuilder.Entity("DAL.Model.ClassTest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsAudit");

                    b.Property<bool>("IsDel");

                    b.Property<string>("Name");

                    b.Property<string>("Professor");

                    b.Property<int>("RefId");

                    b.Property<int>("UniversityTestId");

                    b.HasKey("Id");

                    b.ToTable("ClassTest");
                });

            modelBuilder.Entity("DAL.Model.ClassWeek", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassInfoId");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("Grade");

                    b.Property<bool>("IsDel");

                    b.Property<int>("No");

                    b.Property<int>("RefId");

                    b.HasKey("Id");

                    b.ToTable("ClassWeek");
                });

            modelBuilder.Entity("DAL.Model.ClassWeekType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassWeekId");

                    b.Property<int>("ClassWeekTypeId");

                    b.Property<string>("ContentType");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("Grade");

                    b.Property<bool>("IsDel");

                    b.Property<int>("RefId");

                    b.HasKey("Id");

                    b.ToTable("ClassWeekType");
                });

            modelBuilder.Entity("DAL.Model.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Birthday");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<DateTime>("EffectiveDate");

                    b.Property<string>("Email");

                    b.Property<string>("Image");

                    b.Property<int>("Inviterid");

                    b.Property<bool>("IsDel");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("QQ");

                    b.Property<int>("RefId");

                    b.Property<string>("Role");

                    b.Property<string>("School");

                    b.Property<string>("Sex");

                    b.Property<string>("Tel");

                    b.HasKey("Id");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("DAL.Model.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassInfoId");

                    b.Property<int>("ClientId");

                    b.Property<string>("Contents");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsDel");

                    b.Property<string>("ParentId");

                    b.Property<int>("RefId");

                    b.HasKey("Id");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("DAL.Model.Feedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsDel");

                    b.Property<string>("Name");

                    b.Property<int>("RefId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Feedback");
                });

            modelBuilder.Entity("DAL.Model.Focus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CancelTime");

                    b.Property<int>("ClientId");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsDel");

                    b.Property<string>("Name");

                    b.Property<int>("RefId");

                    b.Property<int>("Type");

                    b.Property<string>("TypeId");

                    b.HasKey("Id");

                    b.ToTable("Focus");
                });

            modelBuilder.Entity("DAL.Model.ImportRecords", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("EndId");

                    b.Property<bool>("IsDel");

                    b.Property<int>("RefId");

                    b.Property<int>("StartId");

                    b.Property<string>("Table");

                    b.HasKey("Id");

                    b.ToTable("ImportRecords");
                });

            modelBuilder.Entity("DAL.Model.Notice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContentsUrl");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsDel");

                    b.Property<int>("IsRead");

                    b.Property<int>("ReceiveId");

                    b.Property<int>("RefId");

                    b.Property<int>("SendId");

                    b.HasKey("Id");

                    b.ToTable("Notice");
                });

            modelBuilder.Entity("DAL.Model.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Currency");

                    b.Property<bool>("IsDel");

                    b.Property<string>("Memo");

                    b.Property<string>("Name");

                    b.Property<string>("OrderNo");

                    b.Property<DateTime>("PayTime");

                    b.Property<int>("PayType");

                    b.Property<string>("Price");

                    b.Property<int>("RefId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("DAL.Model.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Answerer");

                    b.Property<string>("Content");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("Currency");

                    b.Property<DateTime>("EndTime");

                    b.Property<string>("Evaluate");

                    b.Property<bool>("IsAudit");

                    b.Property<bool>("IsDel");

                    b.Property<string>("Memo");

                    b.Property<int>("Number")
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("RefId");

                    b.Property<int>("Status");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("DAL.Model.University", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Image");

                    b.Property<string>("Intro");

                    b.Property<bool>("IsDel");

                    b.Property<int>("IsHide");

                    b.Property<string>("Name");

                    b.Property<int>("RefId");

                    b.Property<string>("State");

                    b.Property<string>("Website");

                    b.HasKey("Id");

                    b.ToTable("University");
                });

            modelBuilder.Entity("DAL.Model.UniversityCombine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsDel");

                    b.Property<int>("OriginalId");

                    b.Property<int>("RefId");

                    b.Property<int>("TargetId");

                    b.HasKey("Id");

                    b.ToTable("UniversityCombine");
                });

            modelBuilder.Entity("DAL.Model.UniversityTest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsAudit");

                    b.Property<bool>("IsDel");

                    b.Property<string>("Name");

                    b.Property<int>("RefId");

                    b.HasKey("Id");

                    b.ToTable("UniversityTest");
                });

            modelBuilder.Entity("DAL.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Email");

                    b.Property<string>("Group");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("QQ");

                    b.Property<int>("RefId");

                    b.Property<string>("Role");

                    b.Property<string>("Tel");

                    b.Property<bool>("isTerminated");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("DAL.Model.UseRecords", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Check");

                    b.Property<int>("ClassInfoId");

                    b.Property<int>("ClientId");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsDel");

                    b.Property<int>("RefId");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("UseRecords");
                });
#pragma warning restore 612, 618
        }
    }
}