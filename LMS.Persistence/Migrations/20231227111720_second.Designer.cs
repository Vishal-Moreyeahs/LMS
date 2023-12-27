﻿// <auto-generated />
using System;
using LMS.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LMS.Persistence.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20231227111720_second")]
    partial class second
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("LMS.Domain.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AlternateContact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OfficialMailId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pincode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrimaryContact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrimaryMailId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("LMS.Domain.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Company_Id")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMandatory")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubDomain_Id")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Company_Id");

                    b.HasIndex("SubDomain_Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("LMS.Domain.Models.CourseContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Courses_Id")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Media")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sequence")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Courses_Id");

                    b.ToTable("CourseContents");
                });

            modelBuilder.Entity("LMS.Domain.Models.Domains", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Company_Id")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Company_Id");

                    b.ToTable("Domains");
                });

            modelBuilder.Entity("LMS.Domain.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AlternateNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Company_Id")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PermanemtAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ReportingManager")
                        .HasColumnType("int");

                    b.Property<int>("Role_Id")
                        .HasColumnType("int");

                    b.Property<string>("TemporaryAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Company_Id");

                    b.HasIndex("Role_Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("LMS.Domain.Models.EmployeeCourse", b =>
                {
                    b.Property<int>("Courses_Id")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Employee_Id")
                        .HasColumnType("int");

                    b.Property<int?>("Group_Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasIndex("Courses_Id");

                    b.HasIndex("Employee_Id");

                    b.HasIndex("Group_Id");

                    b.ToTable("EmployeeCourses");
                });

            modelBuilder.Entity("LMS.Domain.Models.EmployeeQuiz", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Attempt")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Employee_Id")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Group_Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("Quiz_Id")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Employee_Id");

                    b.HasIndex("Group_Id");

                    b.HasIndex("Quiz_Id");

                    b.ToTable("EmployeeQuizes");
                });

            modelBuilder.Entity("LMS.Domain.Models.EmployeeQuizAnswer", b =>
                {
                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeQuiz_Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<int>("QuizQuestion_Id")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasIndex("EmployeeQuiz_Id");

                    b.HasIndex("QuizQuestion_Id");

                    b.ToTable("EmployeeQuizAnswers");
                });

            modelBuilder.Entity("LMS.Domain.Models.EmployeeSubDomain", b =>
                {
                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Employee_Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("SubDomain_Id")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasIndex("Employee_Id");

                    b.HasIndex("SubDomain_Id");

                    b.ToTable("EmployeeSubDomains");
                });

            modelBuilder.Entity("LMS.Domain.Models.FileBank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Company_Id")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Format")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Size")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Company_Id");

                    b.ToTable("FileBanks");
                });

            modelBuilder.Entity("LMS.Domain.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Employees_Id")
                        .HasColumnType("int");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Employees_Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("LMS.Domain.Models.QuestionBank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ImagePath")
                        .HasColumnType("varbinary(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsImageAttached")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubDomain_Id")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("SubDomain_Id");

                    b.ToTable("QuestionBanks");
                });

            modelBuilder.Entity("LMS.Domain.Models.Quiz", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Company_Id")
                        .HasColumnType("int");

                    b.Property<int>("Courses_Id")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMandatory")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PassingCriteria")
                        .HasColumnType("int");

                    b.Property<int>("RetakeCount")
                        .HasColumnType("int");

                    b.Property<int>("SubDomain_Id")
                        .HasColumnType("int");

                    b.Property<int>("Time")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Company_Id");

                    b.HasIndex("Courses_Id");

                    b.HasIndex("SubDomain_Id");

                    b.ToTable("Quizes");
                });

            modelBuilder.Entity("LMS.Domain.Models.QuizOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<bool>("IsImageAttached")
                        .HasColumnType("bit");

                    b.Property<string>("OptionValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionBank_Id")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("QuestionBank_Id");

                    b.ToTable("QuizOptions");
                });

            modelBuilder.Entity("LMS.Domain.Models.QuizQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("Mark")
                        .HasColumnType("int");

                    b.Property<int>("QuestionBank_Id")
                        .HasColumnType("int");

                    b.Property<int>("Quiz_Id")
                        .HasColumnType("int");

                    b.Property<int>("SequenceNo")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("QuestionBank_Id");

                    b.HasIndex("Quiz_Id");

                    b.ToTable("QuizQuestions");
                });

            modelBuilder.Entity("LMS.Domain.Models.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeQuiz_Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("ObtainMarks")
                        .HasColumnType("int");

                    b.Property<int>("Percentage")
                        .HasColumnType("int");

                    b.Property<bool>("ResultStatus")
                        .HasColumnType("bit");

                    b.Property<int>("TotalMarks")
                        .HasColumnType("int");

                    b.Property<int>("TotalNoOfQuestion")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeQuiz_Id");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("LMS.Domain.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("LMS.Domain.Models.SubDomain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Domain_Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Domain_Id");

                    b.ToTable("SubDomains");
                });

            modelBuilder.Entity("LMS.Domain.Models.Course", b =>
                {
                    b.HasOne("LMS.Domain.Models.Company", "Company")
                        .WithMany("Courses")
                        .HasForeignKey("Company_Id")
                        .IsRequired();

                    b.HasOne("LMS.Domain.Models.SubDomain", "SubDomain")
                        .WithMany("Courses")
                        .HasForeignKey("SubDomain_Id")
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("SubDomain");
                });

            modelBuilder.Entity("LMS.Domain.Models.CourseContent", b =>
                {
                    b.HasOne("LMS.Domain.Models.Course", "Courses")
                        .WithMany("CourseContents")
                        .HasForeignKey("Courses_Id")
                        .IsRequired();

                    b.Navigation("Courses");
                });

            modelBuilder.Entity("LMS.Domain.Models.Domains", b =>
                {
                    b.HasOne("LMS.Domain.Models.Company", "Company")
                        .WithMany("Domains")
                        .HasForeignKey("Company_Id")
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("LMS.Domain.Models.Employee", b =>
                {
                    b.HasOne("LMS.Domain.Models.Company", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("Company_Id")
                        .IsRequired();

                    b.HasOne("LMS.Domain.Models.Role", "Role")
                        .WithMany("Employees")
                        .HasForeignKey("Role_Id")
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("LMS.Domain.Models.EmployeeCourse", b =>
                {
                    b.HasOne("LMS.Domain.Models.Course", "Courses")
                        .WithMany()
                        .HasForeignKey("Courses_Id")
                        .IsRequired();

                    b.HasOne("LMS.Domain.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("Employee_Id")
                        .IsRequired();

                    b.HasOne("LMS.Domain.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("Group_Id")
                        .HasPrincipalKey("GroupId");

                    b.Navigation("Courses");

                    b.Navigation("Employee");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("LMS.Domain.Models.EmployeeQuiz", b =>
                {
                    b.HasOne("LMS.Domain.Models.Employee", "Employee")
                        .WithMany("EmployeeQuizes")
                        .HasForeignKey("Employee_Id")
                        .IsRequired();

                    b.HasOne("LMS.Domain.Models.Group", "Group")
                        .WithMany("EmployeeQuizes")
                        .HasForeignKey("Group_Id")
                        .HasPrincipalKey("GroupId");

                    b.HasOne("LMS.Domain.Models.Quiz", "Quiz")
                        .WithMany("EmployeeQuizes")
                        .HasForeignKey("Quiz_Id")
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Group");

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("LMS.Domain.Models.EmployeeQuizAnswer", b =>
                {
                    b.HasOne("LMS.Domain.Models.EmployeeQuiz", "EmployeeQuiz")
                        .WithMany()
                        .HasForeignKey("EmployeeQuiz_Id")
                        .IsRequired();

                    b.HasOne("LMS.Domain.Models.QuizQuestion", "QuizQuestions")
                        .WithMany()
                        .HasForeignKey("QuizQuestion_Id")
                        .IsRequired();

                    b.Navigation("EmployeeQuiz");

                    b.Navigation("QuizQuestions");
                });

            modelBuilder.Entity("LMS.Domain.Models.EmployeeSubDomain", b =>
                {
                    b.HasOne("LMS.Domain.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("Employee_Id")
                        .IsRequired();

                    b.HasOne("LMS.Domain.Models.SubDomain", "SubDomain")
                        .WithMany()
                        .HasForeignKey("SubDomain_Id")
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("SubDomain");
                });

            modelBuilder.Entity("LMS.Domain.Models.FileBank", b =>
                {
                    b.HasOne("LMS.Domain.Models.Company", "Company")
                        .WithMany("FileBanks")
                        .HasForeignKey("Company_Id")
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("LMS.Domain.Models.Group", b =>
                {
                    b.HasOne("LMS.Domain.Models.Employee", "Employees")
                        .WithMany("Groups")
                        .HasForeignKey("Employees_Id")
                        .IsRequired();

                    b.Navigation("Employees");
                });

            modelBuilder.Entity("LMS.Domain.Models.QuestionBank", b =>
                {
                    b.HasOne("LMS.Domain.Models.SubDomain", "SubDomain")
                        .WithMany("QuestionBanks")
                        .HasForeignKey("SubDomain_Id")
                        .IsRequired();

                    b.Navigation("SubDomain");
                });

            modelBuilder.Entity("LMS.Domain.Models.Quiz", b =>
                {
                    b.HasOne("LMS.Domain.Models.Company", "Company")
                        .WithMany("Quizes")
                        .HasForeignKey("Company_Id")
                        .IsRequired();

                    b.HasOne("LMS.Domain.Models.Course", "Courses")
                        .WithMany("Quizes")
                        .HasForeignKey("Courses_Id")
                        .IsRequired();

                    b.HasOne("LMS.Domain.Models.SubDomain", "SubDomain")
                        .WithMany("Quizes")
                        .HasForeignKey("SubDomain_Id")
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Courses");

                    b.Navigation("SubDomain");
                });

            modelBuilder.Entity("LMS.Domain.Models.QuizOption", b =>
                {
                    b.HasOne("LMS.Domain.Models.QuestionBank", "QuestionBank")
                        .WithMany("QuizOptions")
                        .HasForeignKey("QuestionBank_Id")
                        .IsRequired();

                    b.Navigation("QuestionBank");
                });

            modelBuilder.Entity("LMS.Domain.Models.QuizQuestion", b =>
                {
                    b.HasOne("LMS.Domain.Models.QuestionBank", "QuestionBank")
                        .WithMany("QuizQuestions")
                        .HasForeignKey("QuestionBank_Id")
                        .IsRequired();

                    b.HasOne("LMS.Domain.Models.Quiz", "Quiz")
                        .WithMany("QuizQuestions")
                        .HasForeignKey("Quiz_Id")
                        .IsRequired();

                    b.Navigation("QuestionBank");

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("LMS.Domain.Models.Report", b =>
                {
                    b.HasOne("LMS.Domain.Models.EmployeeQuiz", "EmployeeQuiz")
                        .WithMany("Reports")
                        .HasForeignKey("EmployeeQuiz_Id")
                        .IsRequired();

                    b.Navigation("EmployeeQuiz");
                });

            modelBuilder.Entity("LMS.Domain.Models.SubDomain", b =>
                {
                    b.HasOne("LMS.Domain.Models.Domains", "Domain")
                        .WithMany("SubDomains")
                        .HasForeignKey("Domain_Id")
                        .IsRequired();

                    b.Navigation("Domain");
                });

            modelBuilder.Entity("LMS.Domain.Models.Company", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("Domains");

                    b.Navigation("Employees");

                    b.Navigation("FileBanks");

                    b.Navigation("Quizes");
                });

            modelBuilder.Entity("LMS.Domain.Models.Course", b =>
                {
                    b.Navigation("CourseContents");

                    b.Navigation("Quizes");
                });

            modelBuilder.Entity("LMS.Domain.Models.Domains", b =>
                {
                    b.Navigation("SubDomains");
                });

            modelBuilder.Entity("LMS.Domain.Models.Employee", b =>
                {
                    b.Navigation("EmployeeQuizes");

                    b.Navigation("Groups");
                });

            modelBuilder.Entity("LMS.Domain.Models.EmployeeQuiz", b =>
                {
                    b.Navigation("Reports");
                });

            modelBuilder.Entity("LMS.Domain.Models.Group", b =>
                {
                    b.Navigation("EmployeeQuizes");
                });

            modelBuilder.Entity("LMS.Domain.Models.QuestionBank", b =>
                {
                    b.Navigation("QuizOptions");

                    b.Navigation("QuizQuestions");
                });

            modelBuilder.Entity("LMS.Domain.Models.Quiz", b =>
                {
                    b.Navigation("EmployeeQuizes");

                    b.Navigation("QuizQuestions");
                });

            modelBuilder.Entity("LMS.Domain.Models.Role", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("LMS.Domain.Models.SubDomain", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("QuestionBanks");

                    b.Navigation("Quizes");
                });
#pragma warning restore 612, 618
        }
    }
}
