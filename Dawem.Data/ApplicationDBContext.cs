using Dawem.Domain.Entities;
using Dawem.Domain.Entities.Attendances;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Localization;
using Dawem.Domain.Entities.Lookups;
using Dawem.Domain.Entities.Others;
using Dawem.Domain.Entities.Permissions;
using Dawem.Domain.Entities.Providers;
using Dawem.Domain.Entities.Requests;
using Dawem.Domain.Entities.Schedules;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Generic;
using Dawem.Translations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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

            #region Handle All decimal Precisions

            var allDecimalProperties = builder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?));

            foreach (var property in allDecimalProperties)
            {
                property.SetPrecision(30);
                property.SetScale(20);
            }

            #endregion

            builder.Entity<Translation>().HasIndex(x => new { x.KeyWord, x.Lang }).IsUnique();
            builder.Entity<MyUser>(entity => { entity.ToTable(name: nameof(MyUser) + LeillaKeys.S).HasKey(x => x.Id); });
            builder.Entity<UserRole>(entity => { entity.ToTable(name: nameof(UserRole) + LeillaKeys.S); });
            builder.Entity<UserClaim>(entity => { entity.ToTable(nameof(UserClaim) + LeillaKeys.S); });
            builder.Entity<UserLogIn>(entity => { entity.ToTable(nameof(UserLogIn) + LeillaKeys.S); });
            builder.Entity<UserToken>(entity => { entity.ToTable(nameof(UserToken) + LeillaKeys.S); });
            builder.Entity<RoleClaim>(entity => { entity.ToTable(nameof(RoleClaim) + LeillaKeys.S); });
            builder.Entity<Role>(entity => { entity.ToTable(nameof(Role) + LeillaKeys.S); });
            builder.Entity<UserBranch>().HasOne(p => p.User).WithMany(b => b.UserBranches).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PermissionScreen>()
         .HasOne(p => p.Permission)
         .WithMany(b => b.PermissionScreens)
         .HasForeignKey(p => p.PermissionId)
         .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PermissionScreenAction>()
        .HasOne(p => p.PermissionScreen)
        .WithMany(b => b.PermissionScreenActions)
        .HasForeignKey(p => p.PermissionScreenId)
        .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<RequestAttachment>()
         .HasOne(p => p.Request)
         .WithMany(b => b.RequestAttachments)
         .HasForeignKey(p => p.RequestId)
         .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<RequestAssignment>()
         .HasOne(p => p.Request)
         .WithOne(b => b.RequestAssignment)
         .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RequestTask>()
         .HasOne(p => p.Request)
         .WithOne(b => b.RequestTask)
         .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<RequestTaskEmployee>()
        .HasOne(p => p.RequestTask)
        .WithMany(b => b.TaskEmployees)
        .HasForeignKey(p => p.RequestTaskId)
        .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<RequestJustification>()
         .HasOne(p => p.Request)
         .WithOne(b => b.RequestJustification)
         .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<RequestPermission>()
         .HasOne(p => p.Request)
         .WithOne(b => b.RequestPermission)
         .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<RequestVacation>()
         .HasOne(p => p.Request)
         .WithOne(b => b.RequestVacation)
         .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Company>()
                .HasIndex(u => u.IdentityCode)
                .IsUnique();

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


            builder.Entity<EmployeeAttendanceCheck>()
           .HasOne(p => p.EmployeeAttendance)
           .WithMany(b => b.EmployeeAttendanceChecks)
           .HasForeignKey(p => p.EmployeeAttendanceId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ScheduleDay>()
                .HasOne(p => p.Schedule)
                .WithMany(b => b.ScheduleDays)
                .HasForeignKey(p => p.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SchedulePlanEmployee>()
               .HasOne(p => p.SchedulePlan)
               .WithOne(b => b.SchedulePlanEmployee)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SchedulePlanGroup>()
               .HasOne(p => p.SchedulePlan)
               .WithOne(b => b.SchedulePlanGroup)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SchedulePlanDepartment>()
               .HasOne(p => p.SchedulePlan)
               .WithOne(b => b.SchedulePlanDepartment)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SchedulePlanLogEmployee>()
               .HasOne(p => p.SchedulePlanLog)
               .WithMany(b => b.SchedulePlanLogEmployees)
               .HasForeignKey(p => p.SchedulePlanLogId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<GroupEmployee>()
               .HasOne(p => p.Group)
               .WithMany(b => b.GroupEmployees)
               .HasForeignKey(p => p.GroupId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ZoneDepartment>()
              .HasOne(p => p.Department)
              .WithMany(b => b.Zones)
              .HasForeignKey(p => p.DepartmentId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DepartmentManagerDelegator>()
           .HasOne(p => p.Department)
           .WithMany(b => b.ManagerDelegators)
           .HasForeignKey(p => p.DepartmentId)
           .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<FingerprintEnforcementNotifyWay>()
         .HasOne(p => p.FingerprintEnforcement)
         .WithMany(b => b.FingerprintEnforcementNotifyWays)
         .HasForeignKey(p => p.FingerprintEnforcementId)
         .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<FingerprintEnforcementEmployee>()
          .HasOne(p => p.FingerprintEnforcement)
          .WithMany(b => b.FingerprintEnforcementEmployees)
          .HasForeignKey(p => p.FingerprintEnforcementId)
          .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<FingerprintEnforcementGroup>()
         .HasOne(p => p.FingerprintEnforcement)
         .WithMany(b => b.FingerprintEnforcementGroups)
         .HasForeignKey(p => p.FingerprintEnforcementId)
         .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<FingerprintEnforcementDepartment>()
         .HasOne(p => p.FingerprintEnforcement)
         .WithMany(b => b.FingerprintEnforcementDepartments)
         .HasForeignKey(p => p.FingerprintEnforcementId)
         .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<FingerprintEnforcementAction>()
         .HasOne(p => p.FingerprintEnforcement)
         .WithMany(b => b.FingerprintEnforcementActions)
         .HasForeignKey(p => p.FingerprintEnforcementId)
         .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<FingerprintEnforcementDepartment>()
        .HasOne(p => p.Company)
        .WithMany()
        .HasForeignKey(p => p.FingerprintEnforcementId)
        .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<FingerprintEnforcementEmployee>()
        .HasOne(p => p.Company)
        .WithMany()
        .HasForeignKey(p => p.FingerprintEnforcementId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FingerprintEnforcementGroup>()
        .HasOne(p => p.Company)
        .WithMany()
        .HasForeignKey(p => p.FingerprintEnforcementId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FingerprintEnforcementAction>()
       .HasOne(p => p.Company)
       .WithMany()
       .HasForeignKey(p => p.FingerprintEnforcementId)
       .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FingerprintEnforcementAction>()
      .HasOne(p => p.NonComplianceAction)
      .WithMany()
      .HasForeignKey(p => p.FingerprintEnforcementId)
      .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FingerprintEnforcementNotifyWay>()
      .HasOne(p => p.Company)
      .WithMany()
      .HasForeignKey(p => p.FingerprintEnforcementId)
      .OnDelete(DeleteBehavior.Restrict);




            builder.Entity<Department>()
           .HasMany(d => d.Employees)
           .WithOne(e => e.Department)
           .HasForeignKey(e => e.DepartmentId);



            builder.Entity<ShiftWorkingTime>()
           .Property(e => e.CheckInTime)
           .HasConversion(
               v => v.ToTimeSpan(),
               v => TimeOnly.FromTimeSpan(v)
           );
            builder.Entity<ShiftWorkingTime>()
          .Property(e => e.CheckOutTime)
          .HasConversion(
              v => v.ToTimeSpan(),
              v => TimeOnly.FromTimeSpan(v)

          );

            builder.Entity<EmployeeAttendanceCheck>()
          .Property(e => e.Time)
          .HasConversion(
              v => v.ToTimeSpan(),
              v => TimeOnly.FromTimeSpan(v)
          );

            builder.Entity<EmployeeAttendance>()
         .Property(e => e.ShiftCheckInTime)
         .HasConversion(
             v => v.ToTimeSpan(),
             v => TimeOnly.FromTimeSpan(v)
         );
            builder.Entity<EmployeeAttendance>()
          .Property(e => e.ShiftCheckOutTime)
          .HasConversion(
              v => v.ToTimeSpan(),
              v => TimeOnly.FromTimeSpan(v)

          );

        }


        public DbSet<SchedulePlan> SchedulePlans { get; set; }
        public DbSet<SchedulePlanEmployee> SchedulePlanEmployees { get; set; }
        public DbSet<SchedulePlanGroup> SchedulePlanGroups { get; set; }
        public DbSet<SchedulePlanDepartment> SchedulePlanDepartments { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<SchedulePlanLog> SchedulePlanBackgroundJobLogs { get; set; }
        public DbSet<SchedulePlanLogEmployee> SchedulePlanBackgroundJobLogEmployees { get; set; }
        public DbSet<ScheduleDay> ScheduleDays { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<GroupEmployee> GroupEmployees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<AssignmentType> AssignmentTypes { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<HolidayType> HolidayTypes { get; set; }
        public DbSet<JustificationType> JustificationTypes { get; set; }
        public DbSet<VacationType> VacationTypes { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }
        public DbSet<ShiftWorkingTime> ShiftWorkingTimes { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<PermissionLog> ScreenPermissionLogs { get; set; }
        public DbSet<MyUser> MyUser { get; set; }
        public DbSet<FingerprintDevice> FingerprintDevices { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }
        public DbSet<EmployeeAttendanceCheck> EmployeeAttendanceChecks { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionScreen> PermissionScreens { get; set; }
        public DbSet<PermissionScreenAction> PermissionScreenActions { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<VacationBalance> VacationsBalances { get; set; }
        public DbSet<RequestAssignment> RequestAssignments { get; set; }
        public DbSet<RequestJustification> RequestJustifications { get; set; }
        public DbSet<RequestPermission> RequestPermissions { get; set; }
        public DbSet<RequestVacation> RequestVacations { get; set; }
        public DbSet<RequestTask> RequestTasks { get; set; }
        public DbSet<RequestTaskEmployee> RequestTaskEmployees { get; set; }
        public DbSet<RequestAttachment> RequestAttachments { get; set; }
        public DbSet<DepartmentManagerDelegator> DepartmentManagerDelegators { get; set; }
        public DbSet<ZoneDepartment> ZoneDepartments { get; set; }
        public DbSet<ZoneGroup> ZoneGroups { get; set; }
        public DbSet<ZoneEmployee> ZoneEmployees { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Holiday> Holidays  { get; set; }
        public DbSet<NotificationStore> NotificationStores { get; set; }





    }
}