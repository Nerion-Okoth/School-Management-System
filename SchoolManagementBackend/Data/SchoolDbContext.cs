using Microsoft.EntityFrameworkCore;
using SchoolManagementBackend.Models;
using SchoolManagementSystem.Models;
using System;
using System.Threading.Tasks;

namespace SchoolManagementBackend.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }

        // DbSets for each entity
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring relationships
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<Profile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Relationship>()
                .HasKey(r => new { r.StudentId, r.ParentId });

            modelBuilder.Entity<Relationship>()
                .HasOne(r => r.Student)
                .WithMany(s => s.Relationships)
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Relationship>()
                .HasOne(r => r.Parent)
                .WithMany(p => p.Relationships)
                .HasForeignKey(r => r.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Course)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.CourseId)
                .IsRequired();

            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Courses)
                .WithOne(c => c.Teacher)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Class>()
                .HasOne(cl => cl.Teacher)
                .WithMany(t => t.Classes)
                .HasForeignKey(cl => cl.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Class)
                .WithMany(cl => cl.Students)
                .HasForeignKey(s => s.ClassId)
                .IsRequired();

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Course)
                .WithMany(c => c.Grades)
                .HasForeignKey(g => g.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Course)
                .WithMany(c => c.Attendances)
                .HasForeignKey(a => a.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data
            var adminUser = new User
            {
                UserId = 1,
                Username = "admin",
                Email = "admin@school.com",
                PasswordHash = "hashedpassword",
                Role = "Admin",
                CreatedAt = new DateTime(2023, 01, 01)
            };

            modelBuilder.Entity<User>().HasData(adminUser);

            modelBuilder.Entity<Profile>().HasData(
                new Profile
                {
                    ProfileId = 1,
                    UserId = adminUser.UserId,
                    Name = "Admin User",
                    Contact = "admin@school.com",
                    Address = "123 Admin St",
                    ImageUrl = "/images/admin.jpg"
                }
            );

            modelBuilder.Entity<Teacher>().HasData(
                new Teacher { Id = 1, FullName = "Mr. Williams", Email = "mrwilliams@school.com", PhoneNumber = "5551234567" }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "Algebra", Description = "Introduction to Algebra", CreditHours = 3, TeacherId = 1 }
            );

            modelBuilder.Entity<Class>().HasData(
                new Class { Id = 1, Name = "Grade 10 - Section A", TeacherId = 1, Description = "Mathematics-focused class" }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, FirstName = "Michael", LastName = "Johnson", ClassId = 1, Age = 15, CourseId = 1 }
            );

            modelBuilder.Entity<Grade>().HasData(
                new Grade { Id = 1, StudentId = 1, CourseId = 1, Score = 85.5 }
            );

            modelBuilder.Entity<Attendance>().HasData(
                new Attendance { Id = 1, StudentId = 1, CourseId = 1, Date = new DateTime(2023, 01, 01), IsPresent = true }
            );
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = DateTime.UtcNow;
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        entity.UpdatedAt = DateTime.UtcNow;
                    }
                }
            }
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = DateTime.UtcNow;
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        entity.UpdatedAt = DateTime.UtcNow;
                    }
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
