using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Contracts.Repositories;
using LMS.Domain.Models;
using LMS.Domain.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace LMS.Persistence
{
    public class DataBaseContext : DbContext
    {
        private readonly IAuthenticatedUserService _authenticatedUserServices;

        public DataBaseContext(DbContextOptions<DataBaseContext> options, IAuthenticatedUserService authenticatedUserService)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = true;
            _authenticatedUserServices = authenticatedUserService;
        }

        //Db Sets or tables
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseContent> CourseContents { get; set; }
        public virtual DbSet<Domains> Domains { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeCourse> EmployeeCourses { get; set; }
        public virtual DbSet<EmployeeQuiz> EmployeeQuizes { get; set; }
        public virtual DbSet<EmployeeQuizAnswer> EmployeeQuizAnswers { get; set; }
        public virtual DbSet<EmployeeSubDomain> EmployeeSubDomains { get; set; }
        public virtual DbSet<FileBank> FileBanks { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<QuestionBank> QuestionBanks { get; set; }
        public virtual DbSet<Quiz> Quizes { get; set; }
        public virtual DbSet<Option> Options { get; set; }
        public virtual DbSet<QuizQuestion> QuizQuestions { get; set; }
        public virtual DbSet<Report> Reports { get; set; } 
        public virtual DbSet<ResetPasswordVerification> ResetPasswordVerifications { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SubDomain> SubDomains { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var loggedInUser = _authenticatedUserServices.GetLoggedInUser();
            foreach (var entry in ChangeTracker.Entries<BaseEntityClass>())
            {
                entry.Entity.UpdatedDate = DateTime.UtcNow;
                entry.Entity.UpdatedBy = loggedInUser.Result.EmployeeId;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.CreatedBy = loggedInUser.Result.EmployeeId;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataBaseContext).Assembly);

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.Company_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SubDomain)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.SubDomain_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CourseContent>(entity =>
            {

                entity.HasOne(d => d.Courses)
                    .WithMany(p => p.CourseContents)
                    .HasForeignKey(d => d.Courses_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Domains>(entity =>
            {
                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Domains)
                    .HasForeignKey(d => d.Company_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Company_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Role_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<EmployeeCourse>(entity =>
            {
                entity.HasNoKey();

                entity.HasOne(d => d.Courses)
                    .WithMany()
                    .HasForeignKey(d => d.Courses_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Employee)
                    .WithMany()
                    .HasForeignKey(d => d.Employee_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Group)
                    .WithMany()
                    .HasForeignKey(d => d.Group_Id)
                    .HasPrincipalKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<EmployeeQuiz>(entity =>
            {
                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeQuizes)
                    .HasForeignKey(d => d.Employee_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.EmployeeQuizes)
                    .HasForeignKey(d => d.Group_Id)
                    .HasPrincipalKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.EmployeeQuizes)
                    .HasForeignKey(d => d.Quiz_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<EmployeeQuizAnswer>(entity =>
            {
                entity.HasNoKey();

                entity.HasOne(d => d.EmployeeQuiz)
                    .WithMany()
                    .HasForeignKey(d => d.EmployeeQuiz_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.QuizQuestions)
                    .WithMany()
                    .HasForeignKey(d => d.QuizQuestion_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<EmployeeSubDomain>(entity =>
            {
                entity.HasNoKey();

                entity.HasOne(d => d.Employee)
                    .WithMany()
                    .HasForeignKey(d => d.Employee_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SubDomain)
                    .WithMany()
                    .HasForeignKey(d => d.SubDomain_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<FileBank>(entity =>
            {
                entity.HasOne(d => d.Company)
                    .WithMany(p => p.FileBanks)
                    .HasForeignKey(d => d.Company_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasOne(d => d.Employees)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.Employees_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<QuestionBank>(entity =>
            {
                entity.HasOne(d => d.SubDomain)
                    .WithMany(p => p.QuestionBanks)
                    .HasForeignKey(d => d.SubDomain_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Quizes)
                    .HasForeignKey(d => d.Company_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Courses)
                    .WithMany(p => p.Quizes)
                    .HasForeignKey(d => d.Courses_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SubDomain)
                    .WithMany(p => p.Quizes)
                    .HasForeignKey(d => d.SubDomain_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Option>(entity =>
            {
                entity.HasOne(d => d.QuestionBank)
                    .WithMany(p => p.Options)
                    .HasForeignKey(d => d.QuestionBank_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<QuizQuestion>(entity =>
            {
                entity.HasOne(d => d.QuestionBank)
                    .WithMany(p => p.QuizQuestions)
                    .HasForeignKey(d => d.QuestionBank_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.QuizQuestions)
                    .HasForeignKey(d => d.Quiz_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasOne(d => d.EmployeeQuiz)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.EmployeeQuiz_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<SubDomain>(entity =>
            {
                entity.HasOne(d => d.Domain)
                    .WithMany(p => p.SubDomains)
                    .HasForeignKey(d => d.Domain_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

        }
    }
}
