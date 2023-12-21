using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.Persistence
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        

        //Db Sets or tables
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseContent> CourseContents { get; set; }
        public virtual DbSet<Domains> Domains { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeCourse> EmployeeCourses { get; set; }
        public virtual DbSet<EmployeeQuiz> EmployeeQuizzes { get; set; }
        public virtual DbSet<EmployeeQuizAnswer> EmployeeQuizAnswers { get; set; }
        public virtual DbSet<EmployeeSubDomain> EmployeeSubDomains { get; set; }
        public virtual DbSet<FileBank> FileBanks { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<QuestionBank> QuestionBanks { get; set; }
        public virtual DbSet<Quiz> Quizes { get; set; }
        public virtual DbSet<QuizOption> QuizOptions { get; set; } 
        public virtual DbSet<QuizQuestion> QuizQuestions { get; set; }
        public virtual DbSet<Report> Reports { get; set; } 
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SubDomain> SubDomains { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataBaseContext).Assembly);

            modelBuilder
                .Entity<EmployeeCourse>(
                    eb =>
                    {
                        eb.HasNoKey();
                    });

            modelBuilder.Entity<EmployeeQuizAnswer>(entity =>
            {
                entity.HasNoKey();

                modelBuilder.Entity<EmployeeQuizAnswer>()
                   .HasOne(c => c.EmployeeQuiz)
                   .WithMany()
                   .HasForeignKey(c => c.EmployeeQuiz_Id)
                   .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<EmployeeQuizAnswer>()
                    .HasOne(c => c.QuizQuestions)
                    .WithMany()
                    .HasForeignKey(c => c.QuizQuestion_Id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder
                .Entity<EmployeeSubDomain>(
                    eb =>
                    {
                        eb.HasNoKey();
                    });

            modelBuilder.Entity<Course>(entity =>
            {

                modelBuilder.Entity<Course>()
                   .HasOne(c => c.Company)
                   .WithMany(t => t.Courses)
                   .HasForeignKey(c => c.Company_Id)
                   .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<Course>()
                    .HasOne(c => c.SubDomain)
                    .WithMany(t => t.Courses)
                    .HasForeignKey(c => c.SubDomain_Id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Group>(entity =>
            {

                modelBuilder.Entity<Group>()
                   .HasOne(c => c.Employees)
                   .WithMany(t => t.Groups)
                   .HasForeignKey(c => c.Employees_Id)
                   .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<CourseContent>(entity =>
            {

                modelBuilder.Entity<CourseContent>()
                   .HasOne(c => c.Courses)
                   .WithMany(t => t.CourseContents)
                   .HasForeignKey(c => c.Courses_Id)
                   .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<EmployeeSubDomain>(entity =>
            {

                modelBuilder.Entity<EmployeeSubDomain>()
                   .HasOne(c => c.SubDomain)
                   .WithMany()
                   .HasForeignKey(c => c.SubDomain_Id)
                   .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<EmployeeSubDomain>()
                    .HasOne(c => c.Employee)
                    .WithMany()
                    .HasForeignKey(c => c.Employee_Id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EmployeeCourse>(entity =>
            {

                modelBuilder.Entity<EmployeeCourse>()
                   .HasOne(c => c.Group)
                   .WithMany()
                   .HasForeignKey(c => c.Group_Id)
                   .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<EmployeeCourse>()
                    .HasOne(c => c.Employee)
                    .WithMany()
                    .HasForeignKey(c => c.Employee_Id)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<EmployeeCourse>()
                   .HasOne(c => c.Courses)
                   .WithMany()
                   .HasForeignKey(c => c.Courses_Id)
                   .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                modelBuilder.Entity<Quiz>()
                    .HasOne(c => c.SubDomain)
                    .WithMany(t => t.Quizes)
                    .HasForeignKey(c => c.SubDomain_Id)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<Quiz>()
                   .HasOne(c => c.Courses)
                   .WithMany(t => t.Quizes)
                   .HasForeignKey(c => c.Courses_Id)
                   .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<Quiz>()
                   .HasOne(c => c.Company)
                   .WithMany(t => t.Quizzes)
                   .HasForeignKey(c => c.Company_Id)
                   .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<EmployeeQuiz>(entity =>
            {
                modelBuilder.Entity<EmployeeQuiz>()
                    .HasOne(c => c.Employee)
                    .WithMany(t => t.EmployeeQuizzes)
                    .HasForeignKey(c => c.Employee_Id)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<EmployeeQuiz>()
                   .HasOne(c => c.Quiz)
                   .WithMany(t=>t.EmployeeQuizes)
                   .HasForeignKey(c => c.Quiz_Id)
                   .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<EmployeeQuiz>()
                   .HasOne(c => c.Group)
                   .WithMany(t=>t.EmployeeQuizzes)
                   .HasForeignKey(c => c.Group_Id)
                   .OnDelete(DeleteBehavior.Restrict);

            });
        }
    }
}
