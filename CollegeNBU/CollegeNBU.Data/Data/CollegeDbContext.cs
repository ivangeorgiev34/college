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

    public DbSet<College> Colleges { get; set; } = null!;

    public DbSet<Faculty> Faculties { get; set; } = null!;

    public DbSet<Department> Departments { get; set; } = null!;

    public DbSet<Teacher> Teachers { get; set; } = null!;

    public DbSet<Student> Students { get; set; } = null!;

    public DbSet<Rector> Rectors { get; set; } = null!;

    public DbSet<Course> Courses { get; set; } = null!;

    public DbSet<Grade> Grades { get; set; } = null!;

    public DbSet<Attendance> Attendances { get; set; } = null!;

    public DbSet<StudentCourse> StudentCourses { get; set; } = null!;

    public DbSet<SemesterProgram> SemesterPrograms { get; set; } = null!;

    public DbSet<SemesterProgramCourse> SemesterProgramCourses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ConfigureStudentCourse(builder);
        ConfigureSemesterProgramCourse(builder);
        ConfigureDepartmentHead(builder);

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

            builder.Entity<Teacher>()
        .HasOne(t => t.Department)
        .WithMany(d => d.Teachers)
        .HasForeignKey(t => t.DepartmentId)
        .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureStudentCourse(ModelBuilder builder)
    {
        builder.Entity<StudentCourse>()
            .HasKey(sc => new
            {
                sc.StudentId,
                sc.CourseId
            });

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
            .HasKey(spc => new
            {
                spc.SemesterProgramId,
                spc.CourseId
            });

        builder.Entity<SemesterProgramCourse>()
            .HasOne(spc => spc.SemesterProgram)
            .WithMany(sp => sp.Courses)
            .HasForeignKey(spc => spc.SemesterProgramId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<SemesterProgramCourse>()
            .HasOne(spc => spc.Course)
            .WithMany(c => c.SemesterPrograms)
            .HasForeignKey(spc => spc.CourseId)
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