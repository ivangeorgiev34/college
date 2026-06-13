using CollegeNBU.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CollegeNBU.Data.Data;

public class CollegeDbContext : IdentityDbContext<ApplicationUser>
{
    public CollegeDbContext(DbContextOptions<CollegeDbContext> options)
        : base(options)
    {
    }

    public DbSet<College> Colleges => Set<College>();
    public DbSet<Faculty> Faculties => Set<Faculty>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Rector> Rectors => Set<Rector>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<StudentCourse> StudentCourses => Set<StudentCourse>();
    public DbSet<SemesterProgram> SemesterPrograms => Set<SemesterProgram>();
    public DbSet<SemesterProgramCourse> SemesterProgramCourses => Set<SemesterProgramCourse>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ConfigureStudentCourse(builder);
        ConfigureSemesterProgramCourse(builder);
        ConfigureDepartmentHead(builder);

        // Course relations
        builder.Entity<Course>()
            .HasOne(c => c.Teacher)
            .WithMany(t => t.Courses)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Course>()
            .HasOne(c => c.Department)
            .WithMany(d => d.Courses)
            .HasForeignKey(c => c.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Teacher -> Department
        builder.Entity<Teacher>()
            .HasOne(t => t.Department)
            .WithMany(d => d.Teachers)
            .HasForeignKey(t => t.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Attendance relations
        builder.Entity<Attendance>()
            .HasOne(a => a.Teacher)
            .WithMany(t => t.Attendances)
            .HasForeignKey(a => a.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Attendance>()
            .HasOne(a => a.Student)
            .WithMany(s => s.Attendances)
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Attendance>()
            .HasOne(a => a.Course)
            .WithMany(c => c.Attendances)
            .HasForeignKey(a => a.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        // Grade relations
        builder.Entity<Grade>()
            .HasOne(g => g.Teacher)
            .WithMany()
            .HasForeignKey(g => g.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureStudentCourse(ModelBuilder builder)
    {
        builder.Entity<StudentCourse>()
            .HasKey(sc => new { sc.StudentId, sc.CourseId });

        builder.Entity<StudentCourse>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.StudentCourses)
            .HasForeignKey(sc => sc.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<StudentCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.StudentCourses)
            .HasForeignKey(sc => sc.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureSemesterProgramCourse(ModelBuilder builder)
    {
        builder.Entity<SemesterProgramCourse>()
            .HasKey(x => new { x.SemesterProgramId, x.CourseId });

        builder.Entity<SemesterProgramCourse>()
            .HasOne(x => x.SemesterProgram)
            .WithMany(p => p.Courses)
            .HasForeignKey(x => x.SemesterProgramId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<SemesterProgramCourse>()
            .HasOne(x => x.Course)
            .WithMany(c => c.SemesterPrograms)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureDepartmentHead(ModelBuilder builder)
    {
        builder.Entity<Department>()
            .HasOne(d => d.HeadTeacher)
            .WithOne()
            .HasForeignKey<Department>(d => d.HeadTeacherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}