using Dawem.Domain.Entities;
using Dawem.Domain.Entities.Attendance;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Localization;
using Dawem.Domain.Entities.Lookups;
using Dawem.Domain.Entities.Ohters;
using Dawem.Domain.Entities.Provider;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Generic;
using Dawem.Translations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection.Emit;

namespace Dawem.Data
{

    public static class DbContextHelper
    {
        public static DbContextOptions<ApplicationDBContext> GetDbContextOptions()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile(LeillaKeys.AppsettingsFile)
                .SetBasePath(Directory.GetCurrentDirectory()).Build();

            return new DbContextOptionsBuilder<ApplicationDBContext>()
                  .UseSqlServer(new SqlConnection(configuration.GetConnectionString(LeillaKeys.DawemConnectionString)), providerOptions => providerOptions.CommandTimeout(300))
                  .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).ConfigureWarnings(c => c.Log((RelationalEventId.CommandExecuting, LogLevel.Debug))).EnableSensitiveDataLogging(true).Options;
        }
    }

    public class ApplicationDBContext : IdentityDbContext<MyUser, Role, int, UserClaim,
        UserRole, UserLogIn, RoleClaim, UserToken>
    {

        public ApplicationDBContext() : base(DbContextHelper.GetDbContextOptions())
        {
            ChangeTracker.LazyLoadingEnabled = true;

        }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
          : base(options)
        {
        }

        public GeneralSetting GeneralSetting { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("Dawem");
            base.OnModelCreating(builder);
            foreach (var relationship in builder.Model.GetEntityTypes()
                   .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetProperty(nameof(IBaseEntity.AddedDate)) != null)
                {
                    var entityTypeBuilder = builder.Entity(entityType.ClrType);
                    entityTypeBuilder
                        .Property(nameof(IBaseEntity.AddedDate))
                        .HasDefaultValueSql(LeillaKeys.GetDateSQL);
                }
            }

            builder.Entity<Translation>().HasIndex(x => new { x.KeyWord, x.Lang }).IsUnique();
            builder.Entity<MyUser>(entity => { entity.ToTable(name: nameof(MyUser) + LeillaKeys.S).HasKey(x => x.Id); });
            builder.Entity<UserRole>(entity => { entity.ToTable(name: nameof(UserRole) + LeillaKeys.S); });
            builder.Entity<UserClaim>(entity => { entity.ToTable(nameof(UserClaim) + LeillaKeys.S); });
            builder.Entity<UserLogIn>(entity => { entity.ToTable(nameof(UserLogIn) + LeillaKeys.S); });
            builder.Entity<UserToken>(entity => { entity.ToTable(nameof(UserToken) + LeillaKeys.S); });
            builder.Entity<RoleClaim>(entity => { entity.ToTable(nameof(RoleClaim) + LeillaKeys.S); });
            builder.Entity<Role>(entity => { entity.ToTable(nameof(Role) + LeillaKeys.S); });
            builder.Entity<UserBranch>().HasOne(p => p.User).WithMany(b => b.UserBranches).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);




            builder.Entity<Translation>()
                .Property(p => p.IsActive)
                .HasDefaultValue(true);

            builder.Entity<Translation>()
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            builder.Entity<Translation>()
               .Property(p => p.IsActive)
               .HasDefaultValue(true);


            builder.Entity<UserRole>().HasOne(p => p.Role)
                   .WithMany(r => r.UserRoles)
                   .HasForeignKey(p => p.RoleId)
                   .IsRequired();

            builder.Entity<UserRole>().HasOne(p => p.User)
                   .WithMany(r => r.UserRoles)
                   .HasForeignKey(p => p.UserId)
            .IsRequired();


            builder.Entity<ScheduleDay>()
                .HasOne(p => p.Schedule)
                .WithMany(b => b.ScheduleDays)
                .HasForeignKey(p => p.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ShiftWorkingTime>()
           .Property(e => e.CheckInTime)
           .HasConversion(
               v => v.ToTimeSpan(),     // Convert TimeOnly to TimeSpan when saving to the database
               v => TimeOnly.FromTimeSpan(v)  // Convert TimeSpan to TimeOnly when reading from the database
           );
            builder.Entity<ShiftWorkingTime>()
          .Property(e => e.CheckOutTime)
          .HasConversion(
              v => v.ToTimeSpan(),     // Convert TimeOnly to TimeSpan when saving to the database
              v => TimeOnly.FromTimeSpan(v)  // Convert TimeSpan to TimeOnly when reading from the database
          );

        }
        public DbSet<Schedule> WeekAttendances { get; set; }
        public DbSet<ScheduleDay> WeekAttendanceShifts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<AssignmentType> AssignmentTypes { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<HolidayType> HolidayTypes { get; set; }
        public DbSet<JustificationsType> JustificationsTypes { get; set; }
        public DbSet<VacationsType> VacationsTypes { get; set; }
        public DbSet<PermissionsType> PermissionsTypes { get; set; }
        public DbSet<ShiftWorkingTime> ShiftWorkingTimes { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<ActionLog> ActionLogs { get; set; }
        public DbSet<MyUser> MyUser { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserScreenActionPermission> UserScreenActionPermissions { get; set; }
        public DbSet<Currency> Currencies { get; set; }
    }
}