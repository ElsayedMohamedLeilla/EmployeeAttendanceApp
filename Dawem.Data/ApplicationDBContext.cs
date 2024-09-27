using Dawem.Domain.Entities;
using Dawem.Domain.Entities.Attendances;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Domain.Entities.Dawem;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Localization;
using Dawem.Domain.Entities.Lookups;
using Dawem.Domain.Entities.Others;
using Dawem.Domain.Entities.Permissions;
using Dawem.Domain.Entities.Providers;
using Dawem.Domain.Entities.Requests;
using Dawem.Domain.Entities.Schedules;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Domain.Entities.Summons;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Domain.RealTime.Firebase;
using Dawem.Models.DTOs.Dawem.Generic;
using Dawem.Translations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Dawem");

            base.OnModelCreating(modelBuilder);


            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                   .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetProperty(nameof(IBaseEntity.AddedDate)) != null)
                {
                    var entityTypeBuilder = modelBuilder.Entity(entityType.ClrType);
                    entityTypeBuilder
                        .Property(nameof(IBaseEntity.AddedDate))
                        .HasDefaultValueSql(LeillaKeys.GetDateSQL);
                }
            }

            #region Handle All decimal Precisions

            var allDecimalProperties = modelBuilder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?));

            foreach (var property in allDecimalProperties)
            {
                property.SetPrecision(30);
                property.SetScale(20);
            }

            #endregion

            modelBuilder.Entity<Translation>().HasIndex(x => new { x.KeyWord, x.Lang }).IsUnique();
            modelBuilder.Entity<MyUser>(entity => { entity.ToTable(name: nameof(MyUser) + LeillaKeys.S).HasKey(x => x.Id); });
            modelBuilder.Entity<UserRole>(entity => { entity.ToTable(name: nameof(UserRole) + LeillaKeys.S); });
            modelBuilder.Entity<UserClaim>(entity => { entity.ToTable(nameof(UserClaim) + LeillaKeys.S); });
            modelBuilder.Entity<UserLogIn>(entity => { entity.ToTable(nameof(UserLogIn) + LeillaKeys.S); });
            modelBuilder.Entity<UserToken>(entity => { entity.ToTable(nameof(UserToken) + LeillaKeys.S); });
            modelBuilder.Entity<RoleClaim>(entity => { entity.ToTable(nameof(RoleClaim) + LeillaKeys.S); });
            modelBuilder.Entity<Role>(entity => { entity.ToTable(nameof(Role) + LeillaKeys.S); });

            modelBuilder.Entity<Company>().Property(c => c.IdentityCode)
              .UseCollation("SQL_Latin1_General_CP1_CS_AS");

            modelBuilder.Entity<NotificationTranslation>().
                HasOne(p => p.Notification).
                WithMany(b => b.NotificationTranslations).
                HasForeignKey(p => p.NotificationId).
                OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<NotificationEmployee>().
                HasOne(p => p.Notification).
                WithMany(b => b.NotificationEmployees).
                HasForeignKey(p => p.NotificationId).
                OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<UserBranch>().HasOne(p => p.User).
                WithMany(b => b.UserBranches).HasForeignKey(p => p.UserId).
                OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserResponsibility>().
                HasOne(p => p.User).
                WithMany(b => b.UserResponsibilities).
                HasForeignKey(p => p.UserId).
                OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlanNameTranslation>().
                HasOne(p => p.Plan).
                WithMany(b => b.NameTranslations).
                HasForeignKey(p => p.PlanId).
                OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompanyBranch>()
             .HasOne(p => p.Company)
             .WithMany(b => b.CompanyBranches)
             .HasForeignKey(p => p.CompanyId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompanyIndustry>()
         .HasOne(p => p.Company)
         .WithMany(b => b.CompanyIndustries)
         .HasForeignKey(p => p.CompanyId)
         .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SummonLog>()
         .HasOne(p => p.Summon)
         .WithMany(b => b.SummonLogs)
         .HasForeignKey(p => p.SummonId)
         .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SummonLogSanction>()
         .HasOne(p => p.SummonLog)
         .WithMany(b => b.SummonLogSanctions)
         .HasForeignKey(p => p.SummonLogId)
         .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PermissionScreen>()
         .HasOne(p => p.Permission)
         .WithMany(b => b.PermissionScreens)
         .HasForeignKey(p => p.PermissionId)
         .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PermissionScreenAction>()
        .HasOne(p => p.PermissionScreen)
        .WithMany(b => b.PermissionScreenActions)
        .HasForeignKey(p => p.PermissionScreenId)
        .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<RequestAttachment>()
         .HasOne(p => p.Request)
         .WithMany(b => b.RequestAttachments)
         .HasForeignKey(p => p.RequestId)
         .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<RequestAssignment>()
         .HasOne(p => p.Request)
         .WithOne(b => b.RequestAssignment)
         .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RequestTask>()
         .HasOne(p => p.Request)
         .WithOne(b => b.RequestTask)
         .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<RequestTaskEmployee>()
        .HasOne(p => p.RequestTask)
        .WithMany(b => b.TaskEmployees)
        .HasForeignKey(p => p.RequestTaskId)
        .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<RequestJustification>()
         .HasOne(p => p.Request)
         .WithOne(b => b.RequestJustification)
         .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<RequestPermission>()
         .HasOne(p => p.Request)
         .WithOne(b => b.RequestPermission)
         .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<RequestVacation>()
         .HasOne(p => p.Request)
         .WithOne(b => b.RequestVacation)
         .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Company>()
                .HasIndex(u => u.IdentityCode)
                .IsUnique();

            modelBuilder.Entity<Translation>()
                .Property(p => p.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Translation>()
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<Translation>()
               .Property(p => p.IsActive)
               .HasDefaultValue(true);


            modelBuilder.Entity<UserRole>().HasOne(p => p.Role)
                   .WithMany(r => r.UserRoles)
                   .HasForeignKey(p => p.RoleId)
                   .IsRequired();

            modelBuilder.Entity<UserRole>().HasOne(p => p.User)
                   .WithMany(r => r.UserRoles)
                   .HasForeignKey(p => p.UserId)
            .IsRequired();


            modelBuilder.Entity<EmployeeAttendanceCheck>()
           .HasOne(p => p.EmployeeAttendance)
           .WithMany(b => b.EmployeeAttendanceChecks)
           .HasForeignKey(p => p.EmployeeAttendanceId)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ScheduleDay>()
                .HasOne(p => p.Schedule)
                .WithMany(b => b.ScheduleDays)
                .HasForeignKey(p => p.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SchedulePlanEmployee>()
               .HasOne(p => p.SchedulePlan)
               .WithOne(b => b.SchedulePlanEmployee)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SchedulePlanLog>()
               .HasOne(p => p.SchedulePlan)
               .WithMany(b => b.SchedulePlanLogs)
               .HasForeignKey(p => p.SchedulePlanId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SchedulePlanGroup>()
               .HasOne(p => p.SchedulePlan)
               .WithOne(b => b.SchedulePlanGroup)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SchedulePlanDepartment>()
               .HasOne(p => p.SchedulePlan)
               .WithOne(b => b.SchedulePlanDepartment)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SchedulePlanLogEmployee>()
               .HasOne(p => p.SchedulePlanLog)
               .WithMany(b => b.SchedulePlanLogEmployees)
               .HasForeignKey(p => p.SchedulePlanLogId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupEmployee>()
               .HasOne(p => p.Group)
               .WithMany(b => b.GroupEmployees)
               .HasForeignKey(p => p.GroupId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ZoneDepartment>()
              .HasOne(p => p.Department)
              .WithMany(b => b.Zones)
              .HasForeignKey(p => p.DepartmentId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DepartmentManagerDelegator>()
           .HasOne(p => p.Department)
           .WithMany(b => b.ManagerDelegators)
           .HasForeignKey(p => p.DepartmentId)
           .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<SummonNotifyWay>()
         .HasOne(p => p.Summon)
         .WithMany(b => b.SummonNotifyWays)
         .HasForeignKey(p => p.SummonId)
         .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SummonEmployee>()
          .HasOne(p => p.Summon)
          .WithMany(b => b.SummonEmployees)
          .HasForeignKey(p => p.SummonId)
          .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SummonGroup>()
         .HasOne(p => p.Summon)
         .WithMany(b => b.SummonGroups)
         .HasForeignKey(p => p.SummonId)
         .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SummonDepartment>()
         .HasOne(p => p.Summon)
         .WithMany(b => b.SummonDepartments)
         .HasForeignKey(p => p.SummonId)
         .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SummonSanction>()
         .HasOne(p => p.Summon)
         .WithMany(b => b.SummonSanctions)
         .HasForeignKey(p => p.SummonId)
         .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SummonSanction>()
         .HasOne(p => p.Sanction)
         .WithMany(b => b.SummonSanctions)
         .HasForeignKey(p => p.SanctionId)
         .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NotificationUserFCMToken>().
                HasOne(p => p.NotificationUser).
                WithMany(b => b.NotificationUserFCMTokens).
                HasForeignKey(p => p.NotificationUserId).
                OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuItemNameTranslation>().
                HasOne(p => p.MenuItem).
                WithMany(b => b.MenuItemNameTranslations).
                HasForeignKey(p => p.MenuItemId).
                OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuItemAction>().
                HasOne(p => p.MenuItem).
                WithMany(b => b.MenuItemActions).
                HasForeignKey(p => p.MenuItemId).
                OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlanScreen>().
                HasOne(p => p.Plan).
                WithMany(b => b.PlanScreens).
                HasForeignKey(p => p.PlanId).
                OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<SummonDepartment>()
        .HasOne(p => p.Company)
        .WithMany()
        .HasForeignKey(p => p.CompanyId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SummonEmployee>()
        .HasOne(p => p.Company)
        .WithMany()
        .HasForeignKey(p => p.CompanyId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SummonGroup>()
        .HasOne(p => p.Company)
        .WithMany()
        .HasForeignKey(p => p.CompanyId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SummonSanction>()
       .HasOne(p => p.Company)
       .WithMany()
       .HasForeignKey(p => p.CompanyId)
       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SummonNotifyWay>()
      .HasOne(p => p.Company)
      .WithMany()
      .HasForeignKey(p => p.CompanyId)
      .OnDelete(DeleteBehavior.Restrict);

            #region Handle Indexes

            modelBuilder.Entity<Country>().
                HasIndex(p => p.TimeZoneToUTC);

            modelBuilder.Entity<Notification>().
                HasIndex(p => p.NotificationType);
            modelBuilder.Entity<Notification>().
                HasIndex(p => p.HelperNumber);
            modelBuilder.Entity<Notification>().
                HasIndex(p => p.HelperDate);

            modelBuilder.Entity<Summon>().
                HasIndex(p => p.StartDateAndTimeUTC);
            modelBuilder.Entity<Summon>().
                HasIndex(p => p.EndDateAndTimeUTC);

            modelBuilder.Entity<ShiftWorkingTime>().
                HasIndex(p => p.CheckInTime);
            modelBuilder.Entity<ShiftWorkingTime>().
                HasIndex(p => p.CheckOutTime);
            modelBuilder.Entity<ShiftWorkingTime>().
                HasIndex(p => p.AllowedMinutes);
            modelBuilder.Entity<ShiftWorkingTime>().
                HasIndex(p => p.IsTwoDaysShift);


            modelBuilder.Entity<EmployeeAttendance>().
                HasIndex(p => p.LocalDate);
            modelBuilder.Entity<EmployeeAttendance>().
                HasIndex(p => p.ShiftCheckInTime);
            modelBuilder.Entity<EmployeeAttendance>().
                HasIndex(p => p.ShiftCheckOutTime);
            modelBuilder.Entity<EmployeeAttendance>().
                HasIndex(p => p.FingerPrintStatus);


            modelBuilder.Entity<EmployeeAttendanceCheck>().
                HasIndex(p => p.FingerPrintDate);
            modelBuilder.Entity<EmployeeAttendanceCheck>().
                HasIndex(p => p.FingerPrintDateUTC);
            modelBuilder.Entity<EmployeeAttendanceCheck>().
                HasIndex(p => p.FingerPrintType);

            modelBuilder.Entity<ScheduleDay>().
                HasIndex(p => p.WeekDay);


            #endregion

            modelBuilder.Entity<Department>()
           .HasMany(d => d.Employees)
           .WithOne(e => e.Department)
           .HasForeignKey(e => e.DepartmentId);

            #region Add Index To All CompanyId And Name In All Tables

            var allIsDeletedEntities = modelBuilder.Model.GetEntityTypes()
                     .Where(entity => entity.GetProperties().Any(p => p.Name == nameof(Employee.IsDeleted)));

            foreach (var entityType in allIsDeletedEntities)
            {
                var isDeleted = entityType?.GetProperty(nameof(Employee.IsDeleted));
                if (entityType != null && isDeleted != null)
                {
                    entityType.AddIndex(isDeleted, LeillaKeys.IsDeleted)
                    .IsUnique = false;
                }
            }

            #endregion

            #region Add Index To All CompanyId And Name In All Tables

            var allNameEntities = modelBuilder.Model.GetEntityTypes()
                     .Where(entity => entity.GetProperties().Any(p => p.Name == nameof(Employee.CompanyId)) &&
                     entity.GetProperties().Any(p => p.Name == nameof(Employee.Name)) &&
                     entity.GetProperties().Any(p => p.Name == nameof(BaseEntity.IsDeleted)));

            foreach (var entityType in allNameEntities)
            {
                var companyId = entityType?.GetProperty(nameof(Employee.CompanyId));
                var name = entityType?.GetProperty(nameof(Employee.Name));
                var isDeleted = entityType?.GetProperty(nameof(BaseEntity.IsDeleted));

                if (entityType != null && companyId != null && name != null && isDeleted != null)
                {
                    entityType.AddIndex(new List<IMutableProperty> { companyId, name, isDeleted }, LeillaKeys.UniqueIndexCompanyIdNameIsDeleted)
                    .IsUnique = true;
                }
            }

            #endregion

            #region Add Index To All CompanyId Code All Tables

            var allCodeEntities = modelBuilder.Model.GetEntityTypes()
                     .Where(entity => entity.GetProperties().Any(p => p.Name == nameof(Employee.CompanyId)) &&
                     entity.GetProperties().Any(p => p.Name == nameof(Employee.Code)) &&
                     entity.GetProperties().Any(p => p.Name == nameof(BaseEntity.IsDeleted)));

            foreach (var entityType in allCodeEntities)
            {
                var compoanyId = entityType?.GetProperty(nameof(Employee.CompanyId));
                var code = entityType?.GetProperty(nameof(Employee.Code));
                var isDeleted = entityType?.GetProperty(nameof(BaseEntity.IsDeleted));

                if (entityType != null && compoanyId != null && code != null && isDeleted != null)
                {
                    entityType.AddIndex(new List<IMutableProperty> { compoanyId, code, isDeleted }, LeillaKeys.UniqueIndexCompanyIdCodeIsDeleted)
                    .IsUnique = true;
                }
            }

            #endregion

            #region Handle All String Max Length

            var allEntity = modelBuilder.Model.GetEntityTypes()
                .Where(e => e.ClrType != typeof(UserToken) &&
                e.ClrType != typeof(UserLogIn) &&
                e.ClrType != typeof(UserLoginInfo));

            var allStringPropertiesWithMobile = allEntity
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(string)
                     && (p.Name.Contains(LeillaKeys.Mobile)
                     || p.Name.Contains(LeillaKeys.Phone)));

            foreach (var property in allStringPropertiesWithMobile)
            {
                property.SetMaxLength(20);
            }


            var allStringPropertiesWithOutNotesAndAddress = allEntity
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(string)
                     && p.Name != nameof(BaseEntity.Notes)
                     && p.Name != nameof(Employee.Address)
                     && p.Name != nameof(NotificationTranslation.Body)
                     && !p.Name.Contains(LeillaKeys.Mobile)
                     && !p.Name.Contains(LeillaKeys.Phone));

            foreach (var property in allStringPropertiesWithOutNotesAndAddress)
            {
                property.SetMaxLength(50);
            }

            var allStringPropertiesWithNotesAndAddress = allEntity
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(string)
                     && (p.Name == nameof(BaseEntity.Notes) || p.Name == nameof(Employee.Address)));

            foreach (var property in allStringPropertiesWithNotesAndAddress)
            {
                property.SetMaxLength(200);
            }

            var allStringPropertiesWithFullMessage = allEntity
                    .SelectMany(t => t.GetProperties())
                    .Where(p => p.ClrType == typeof(string)
                    && (p.Name == nameof(NotificationTranslation.Body)));

            foreach (var property in allStringPropertiesWithFullMessage)
            {
                property.SetMaxLength(500);
            }

            var allStringPropertiesWithFileOrImageName = allEntity
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(string)
                     && (p.Name.Contains(LeillaKeys.ProfileImageName)
                     || p.Name.Contains(LeillaKeys.FileName)
                     || p.Name.Contains(LeillaKeys.LogoImageName)));

            foreach (var property in allStringPropertiesWithFileOrImageName)
            {
                property.SetMaxLength(250);
            }

            modelBuilder.Entity<Employee>()
                .Property(e => e.FingerprintMobileCode)
                .HasMaxLength(100);

            modelBuilder.Entity<Translation>()
                .Property(e => e.KeyWord)
                .HasMaxLength(250);

            modelBuilder.Entity<Translation>()
                .Property(e => e.TranslationText)
                .HasMaxLength(250);

            modelBuilder.Entity<NotificationUserFCMToken>()
                .Property(e => e.FCMToken)
                .HasMaxLength(250);

            modelBuilder.Entity<MyUser>()
                .Property(e => e.PasswordHash)
                .HasMaxLength(250);

            modelBuilder.Entity<MyUser>()
                .Property(e => e.SecurityStamp)
                .HasMaxLength(250);

            #endregion

            #region Computed Columns

            modelBuilder.Entity<EmployeeAttendance>().
                Property(e => e.TotalWorkingHours).
                HasComputedColumnSql("dbo.TotalWorkingHours(Id)");

            modelBuilder.Entity<EmployeeAttendance>().
                Property(e => e.TotalLateArrivalsHours).
                HasComputedColumnSql("dbo.TotalLateArrivalsHours(Id)");

            modelBuilder.Entity<EmployeeAttendance>().
                Property(e => e.TotalEarlyDeparturesHours).
                HasComputedColumnSql("dbo.TotalEarlyDeparturesHours(Id)");

            modelBuilder.Entity<EmployeeAttendance>().
                Property(e => e.TotalOverTimeHours).
                HasComputedColumnSql("dbo.TotalOverTimeHours(Id)");

            modelBuilder.Entity<EmployeeAttendance>().
                Property(e => e.TotalBreakHours).
                HasComputedColumnSql("dbo.TotalBreakHours(Id)");

            modelBuilder.Entity<EmployeeAttendance>().
                Property(e => e.CheckInDateTime).
                HasComputedColumnSql("dbo.CheckInDateTime(Id)");

            modelBuilder.Entity<EmployeeAttendance>().
                Property(e => e.CheckOutDateTime).
                HasComputedColumnSql("dbo.CheckOutDateTime(Id)");


            modelBuilder.Entity<EmployeeAttendance>().
                ToTable(tbl => tbl.HasTrigger("dbo.TotalWorkingHours(Id)"));

            modelBuilder.Entity<EmployeeAttendance>().
                ToTable(tbl => tbl.HasTrigger("dbo.TotalLateArrivalsHours(Id)"));

            modelBuilder.Entity<EmployeeAttendance>().
                ToTable(tbl => tbl.HasTrigger("dbo.TotalEarlyDeparturesHours(Id)"));

            modelBuilder.Entity<EmployeeAttendance>().
                ToTable(tbl => tbl.HasTrigger("dbo.TotalOverTimeHours(Id)"));

            modelBuilder.Entity<EmployeeAttendance>().
                ToTable(tbl => tbl.HasTrigger("dbo.TotalBreakHours(Id)"));

            modelBuilder.Entity<EmployeeAttendance>().
                ToTable(tbl => tbl.HasTrigger("dbo.CheckInDateTime(Id)"));

            modelBuilder.Entity<EmployeeAttendance>().
                ToTable(tbl => tbl.HasTrigger("dbo.CheckOutDateTime(Id)"));

            #endregion

            #region Handle Query Filters

            // define your filter expression tree
            Expression<Func<BaseEntity, bool>> filterExpr = bm => !bm.IsDeleted;
            foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                // check if current entity type is child of BaseModel
                if (mutableEntityType.ClrType.IsAssignableTo(typeof(BaseEntity)))
                {
                    // modify expression to handle correct child type
                    var parameter = Expression.Parameter(mutableEntityType.ClrType);
                    var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
                    var lambdaExpression = Expression.Lambda(body, parameter);

                    // set filter
                    mutableEntityType.SetQueryFilter(lambdaExpression);
                }
            }

            #endregion
        }
        public DbSet<MenuItem> Screens { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Setting> DawemSettings { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanNameTranslation> PlanNameTranslations { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionLog> SubscriptionLogs { get; set; }
        public DbSet<SchedulePlan> SchedulePlans { get; set; }
        public DbSet<SchedulePlanEmployee> SchedulePlanEmployees { get; set; }
        public DbSet<SchedulePlanGroup> SchedulePlanGroups { get; set; }
        public DbSet<SchedulePlanDepartment> SchedulePlanDepartments { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<SchedulePlanLog> SchedulePlanBackgroundJobLogs { get; set; }
        public DbSet<SchedulePlanLogEmployee> SchedulePlanBackgroundJobLogEmployees { get; set; }
        public DbSet<ScheduleDay> ScheduleDays { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Responsibility> Responsibilities { get; set; }
        public DbSet<UserResponsibility> UserResponsibilities { get; set; }
        public DbSet<NotificationUser> FirebaseUsers { get; set; }
        public DbSet<NotificationUserFCMToken> FirebaseUserFCMTokens { get; set; }
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
        public DbSet<FingerprintTransaction> FingerprintTransactions { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<CompanyBranch> CompanyBranches { get; set; }
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
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<EmployeeOTP> EmployeeOTPs { get; set; }

        public DbSet<DefaultLookup> DefaultLookups { get; set; }
        public DbSet<DefaultLookupsNameTranslation> DefaultLookupsTranslations { get; set; }





    }
}