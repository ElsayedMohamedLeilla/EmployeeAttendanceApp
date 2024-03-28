using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        { }
        /// <inheritdoc />
        //protected override void Up(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.EnsureSchema(
        //        name: "Dawem");

        //    migrationBuilder.CreateTable(
        //        name: "Countries",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            NameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            NameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Iso = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Iso3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Dial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            CurrencyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            TimeZoneId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Order = table.Column<int>(type: "int", nullable: false),
        //            NationalityNameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            NationalityNameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            PhoneLength = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Countries", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "DawemSettings",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Type = table.Column<int>(type: "int", nullable: false),
        //            TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            GroupType = table.Column<int>(type: "int", nullable: false),
        //            GroupTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            ValueType = table.Column<int>(type: "int", nullable: false),
        //            ValueTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            String = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Integer = table.Column<int>(type: "int", nullable: true),
        //            Decimal = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_DawemSettings", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Languages",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            NativeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            ISO2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            ISO3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Order = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Languages", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Plans",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            MinNumberOfEmployees = table.Column<int>(type: "int", nullable: false),
        //            MaxNumberOfEmployees = table.Column<int>(type: "int", nullable: false),
        //            EmployeeCost = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: false),
        //            IsTrial = table.Column<bool>(type: "bit", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Plans", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Roles",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            NormalizedName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            ConcurrencyStamp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Roles", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Translations",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            KeyWord = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
        //            TransWords = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
        //            Lang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Translations", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Currencies",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            NameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            NameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Symbol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            CountryId = table.Column<int>(type: "int", nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Currencies", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Currencies_Countries_CountryId",
        //                column: x => x.CountryId,
        //                principalSchema: "Dawem",
        //                principalTable: "Countries",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Companies",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CountryId = table.Column<int>(type: "int", nullable: false),
        //            PreferredLanguageId = table.Column<int>(type: "int", nullable: true),
        //            IdentityCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            LogoImageName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
        //            WebSite = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            HeadquarterAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            HeadquarterLocation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            HeadquarterPostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            NumberOfEmployees = table.Column<int>(type: "int", nullable: false),
        //            TotalNumberOfEmployees = table.Column<int>(type: "int", nullable: false),
        //            ImportDefaultData = table.Column<bool>(type: "bit", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Companies", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Companies_Countries_CountryId",
        //                column: x => x.CountryId,
        //                principalSchema: "Dawem",
        //                principalTable: "Countries",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_Companies_Languages_PreferredLanguageId",
        //                column: x => x.PreferredLanguageId,
        //                principalSchema: "Dawem",
        //                principalTable: "Languages",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "PlanNameTranslations",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            PlanId = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
        //            LanguageId = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_PlanNameTranslations", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_PlanNameTranslations_Languages_LanguageId",
        //                column: x => x.LanguageId,
        //                principalSchema: "Dawem",
        //                principalTable: "Languages",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_PlanNameTranslations_Plans_PlanId",
        //                column: x => x.PlanId,
        //                principalSchema: "Dawem",
        //                principalTable: "Plans",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "RoleClaims",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            RoleId = table.Column<int>(type: "int", nullable: false),
        //            ClaimType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            ClaimValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_RoleClaims", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_RoleClaims_Roles_RoleId",
        //                column: x => x.RoleId,
        //                principalSchema: "Dawem",
        //                principalTable: "Roles",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "AssignmentTypes",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_AssignmentTypes", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_AssignmentTypes_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "CompanyAttachments",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            FileName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_CompanyAttachments", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_CompanyAttachments_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "CompanyBranches",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
        //            Location = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_CompanyBranches", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_CompanyBranches_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "CompanyIndustries",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_CompanyIndustries", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_CompanyIndustries_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "FingerprintDevices",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            PortNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            SerialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_FingerprintDevices", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_FingerprintDevices_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Holidays",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            DateType = table.Column<int>(type: "int", nullable: false),
        //            StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            IsSpecifiedByYear = table.Column<bool>(type: "bit", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Holidays", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Holidays_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "HolidayTypes",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_HolidayTypes", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_HolidayTypes_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "JobTitles",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_JobTitles", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_JobTitles_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "JustificationTypes",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_JustificationTypes", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_JustificationTypes_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "PermissionTypes",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_PermissionTypes", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_PermissionTypes_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Sanctions",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Type = table.Column<int>(type: "int", nullable: false),
        //            WarningMessage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Sanctions", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Sanctions_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Schedules",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Schedules", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Schedules_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "ShiftWorkingTimes",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            TimePeriod = table.Column<int>(type: "int", nullable: false),
        //            CheckInTime = table.Column<TimeSpan>(type: "time", nullable: false),
        //            CheckOutTime = table.Column<TimeSpan>(type: "time", nullable: false),
        //            AllowedMinutes = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_ShiftWorkingTimes", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_ShiftWorkingTimes_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Subscriptions",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            PlanId = table.Column<int>(type: "int", nullable: false),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            DurationInDays = table.Column<int>(type: "int", nullable: false),
        //            StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            Status = table.Column<int>(type: "int", nullable: false),
        //            RenewalCount = table.Column<int>(type: "int", nullable: false),
        //            FollowUpEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            NumberOfEmployees = table.Column<int>(type: "int", nullable: false),
        //            EmployeeCost = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: false),
        //            TotalAmount = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Subscriptions", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Subscriptions_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_Subscriptions_Plans_PlanId",
        //                column: x => x.PlanId,
        //                principalSchema: "Dawem",
        //                principalTable: "Plans",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Summons",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            ForType = table.Column<int>(type: "int", nullable: false),
        //            ForAllEmployees = table.Column<bool>(type: "bit", nullable: true),
        //            DateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            AllowedTime = table.Column<int>(type: "int", nullable: false),
        //            TimeType = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Summons", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Summons_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "TaskTypes",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_TaskTypes", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_TaskTypes_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "VacationTypes",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            DefaultType = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_VacationTypes", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_VacationTypes_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Zones",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Latitude = table.Column<double>(type: "float", nullable: false),
        //            Longitude = table.Column<double>(type: "float", nullable: false),
        //            Radius = table.Column<double>(type: "float", nullable: true),
        //            InsertedFromExcel = table.Column<bool>(type: "bit", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Zones", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Zones_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "SchedulePlans",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            ScheduleId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            SchedulePlanType = table.Column<int>(type: "int", nullable: false),
        //            DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SchedulePlans", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_SchedulePlans_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SchedulePlans_Schedules_ScheduleId",
        //                column: x => x.ScheduleId,
        //                principalSchema: "Dawem",
        //                principalTable: "Schedules",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "ScheduleDays",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            ScheduleId = table.Column<int>(type: "int", nullable: false),
        //            ShiftId = table.Column<int>(type: "int", nullable: true),
        //            WeekDay = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_ScheduleDays", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_ScheduleDays_Schedules_ScheduleId",
        //                column: x => x.ScheduleId,
        //                principalSchema: "Dawem",
        //                principalTable: "Schedules",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_ScheduleDays_ShiftWorkingTimes_ShiftId",
        //                column: x => x.ShiftId,
        //                principalSchema: "Dawem",
        //                principalTable: "ShiftWorkingTimes",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "SubscriptionLogs",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            SubscriptionId = table.Column<int>(type: "int", nullable: false),
        //            LogType = table.Column<int>(type: "int", nullable: false),
        //            LogTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SubscriptionLogs", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_SubscriptionLogs_Subscriptions_SubscriptionId",
        //                column: x => x.SubscriptionId,
        //                principalSchema: "Dawem",
        //                principalTable: "Subscriptions",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "SummonNotifyWays",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            SummonId = table.Column<int>(type: "int", nullable: false),
        //            NotifyWay = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SummonNotifyWays", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_SummonNotifyWays_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SummonNotifyWays_Summons_SummonId",
        //                column: x => x.SummonId,
        //                principalSchema: "Dawem",
        //                principalTable: "Summons",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "SummonSanctions",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            SummonId = table.Column<int>(type: "int", nullable: false),
        //            SanctionId = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SummonSanctions", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_SummonSanctions_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SummonSanctions_Sanctions_SanctionId",
        //                column: x => x.SanctionId,
        //                principalSchema: "Dawem",
        //                principalTable: "Sanctions",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SummonSanctions_Summons_SummonId",
        //                column: x => x.SummonId,
        //                principalSchema: "Dawem",
        //                principalTable: "Summons",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "SchedulePlanLogs",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            SchedulePlanId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            SchedulePlanType = table.Column<int>(type: "int", nullable: false),
        //            StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            FinishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SchedulePlanLogs", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_SchedulePlanLogs_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SchedulePlanLogs_SchedulePlans_SchedulePlanId",
        //                column: x => x.SchedulePlanId,
        //                principalSchema: "Dawem",
        //                principalTable: "SchedulePlans",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "DepartmentManagerDelegators",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            EmployeeId = table.Column<int>(type: "int", nullable: false),
        //            DepartmentId = table.Column<int>(type: "int", nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_DepartmentManagerDelegators", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Departments",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            ParentId = table.Column<int>(type: "int", nullable: true),
        //            ManagerId = table.Column<int>(type: "int", nullable: true),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            InsertedFromExcel = table.Column<bool>(type: "bit", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Departments", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Departments_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_Departments_Departments_ParentId",
        //                column: x => x.ParentId,
        //                principalSchema: "Dawem",
        //                principalTable: "Departments",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Employees",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            DepartmentId = table.Column<int>(type: "int", nullable: true),
        //            JobTitleId = table.Column<int>(type: "int", nullable: true),
        //            ScheduleId = table.Column<int>(type: "int", nullable: true),
        //            DirectManagerId = table.Column<int>(type: "int", nullable: true),
        //            MobileCountryId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            EmployeeNumber = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            MobileNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
        //            Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
        //            ProfileImageName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
        //            FingerprintMobileCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
        //            AllowChangeFingerprintMobileCode = table.Column<bool>(type: "bit", nullable: false),
        //            JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            AttendanceType = table.Column<int>(type: "int", nullable: false),
        //            EmployeeType = table.Column<int>(type: "int", nullable: false),
        //            InsertedFromExcel = table.Column<bool>(type: "bit", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Employees", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Employees_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_Employees_Countries_MobileCountryId",
        //                column: x => x.MobileCountryId,
        //                principalSchema: "Dawem",
        //                principalTable: "Countries",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_Employees_Departments_DepartmentId",
        //                column: x => x.DepartmentId,
        //                principalSchema: "Dawem",
        //                principalTable: "Departments",
        //                principalColumn: "Id");
        //            table.ForeignKey(
        //                name: "FK_Employees_Employees_DirectManagerId",
        //                column: x => x.DirectManagerId,
        //                principalSchema: "Dawem",
        //                principalTable: "Employees",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_Employees_JobTitles_JobTitleId",
        //                column: x => x.JobTitleId,
        //                principalSchema: "Dawem",
        //                principalTable: "JobTitles",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_Employees_Schedules_ScheduleId",
        //                column: x => x.ScheduleId,
        //                principalSchema: "Dawem",
        //                principalTable: "Schedules",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "SchedulePlanDepartments",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            SchedulePlanId = table.Column<int>(type: "int", nullable: false),
        //            DepartmentId = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SchedulePlanDepartments", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_SchedulePlanDepartments_Departments_DepartmentId",
        //                column: x => x.DepartmentId,
        //                principalSchema: "Dawem",
        //                principalTable: "Departments",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SchedulePlanDepartments_SchedulePlans_SchedulePlanId",
        //                column: x => x.SchedulePlanId,
        //                principalSchema: "Dawem",
        //                principalTable: "SchedulePlans",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "SummonDepartments",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            SummonId = table.Column<int>(type: "int", nullable: false),
        //            DepartmentId = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SummonDepartments", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_SummonDepartments_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SummonDepartments_Departments_DepartmentId",
        //                column: x => x.DepartmentId,
        //                principalSchema: "Dawem",
        //                principalTable: "Departments",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SummonDepartments_Summons_SummonId",
        //                column: x => x.SummonId,
        //                principalSchema: "Dawem",
        //                principalTable: "Summons",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "ZoneDepartments",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            DepartmentId = table.Column<int>(type: "int", nullable: true),
        //            ZoneId = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_ZoneDepartments", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_ZoneDepartments_Departments_DepartmentId",
        //                column: x => x.DepartmentId,
        //                principalSchema: "Dawem",
        //                principalTable: "Departments",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_ZoneDepartments_Zones_ZoneId",
        //                column: x => x.ZoneId,
        //                principalSchema: "Dawem",
        //                principalTable: "Zones",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "EmployeeAttendances",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            EmployeeId = table.Column<int>(type: "int", nullable: false),
        //            ScheduleId = table.Column<int>(type: "int", nullable: false),
        //            ShiftId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            LocalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            ShiftCheckInTime = table.Column<TimeSpan>(type: "time", nullable: false),
        //            ShiftCheckOutTime = table.Column<TimeSpan>(type: "time", nullable: false),
        //            AllowedMinutes = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_EmployeeAttendances", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_EmployeeAttendances_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_EmployeeAttendances_Employees_EmployeeId",
        //                column: x => x.EmployeeId,
        //                principalSchema: "Dawem",
        //                principalTable: "Employees",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_EmployeeAttendances_Schedules_ScheduleId",
        //                column: x => x.ScheduleId,
        //                principalSchema: "Dawem",
        //                principalTable: "Schedules",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_EmployeeAttendances_ShiftWorkingTimes_ShiftId",
        //                column: x => x.ShiftId,
        //                principalSchema: "Dawem",
        //                principalTable: "ShiftWorkingTimes",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Groups",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            ManagerId = table.Column<int>(type: "int", nullable: true),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Groups", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Groups_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_Groups_Employees_ManagerId",
        //                column: x => x.ManagerId,
        //                principalSchema: "Dawem",
        //                principalTable: "Employees",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "MyUsers",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: true),
        //            EmployeeId = table.Column<int>(type: "int", nullable: true),
        //            MobileCountryId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsAdmin = table.Column<bool>(type: "bit", nullable: false),
        //            BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Gender = table.Column<int>(type: "int", nullable: false),
        //            MobileNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
        //            ProfileImageName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
        //            Status = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            VerificationCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            VerificationCodeSendDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            NormalizedUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            NormalizedEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
        //            PasswordHash = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
        //            SecurityStamp = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
        //            ConcurrencyStamp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
        //            PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
        //            TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
        //            LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
        //            LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
        //            AccessFailedCount = table.Column<int>(type: "int", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_MyUsers", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_MyUsers_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_MyUsers_Countries_MobileCountryId",
        //                column: x => x.MobileCountryId,
        //                principalSchema: "Dawem",
        //                principalTable: "Countries",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_MyUsers_Employees_EmployeeId",
        //                column: x => x.EmployeeId,
        //                principalSchema: "Dawem",
        //                principalTable: "Employees",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "NotificationStores",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            EmployeeId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            IsRead = table.Column<bool>(type: "bit", nullable: false),
        //            Status = table.Column<int>(type: "int", nullable: false),
        //            ImageUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
        //            Priority = table.Column<int>(type: "int", nullable: false),
        //            NotificationType = table.Column<int>(type: "int", nullable: false),
        //            IsViewed = table.Column<bool>(type: "bit", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_NotificationStores", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_NotificationStores_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_NotificationStores_Employees_EmployeeId",
        //                column: x => x.EmployeeId,
        //                principalSchema: "Dawem",
        //                principalTable: "Employees",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "SchedulePlanEmployees",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            SchedulePlanId = table.Column<int>(type: "int", nullable: false),
        //            EmployeeId = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SchedulePlanEmployees", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_SchedulePlanEmployees_Employees_EmployeeId",
        //                column: x => x.EmployeeId,
        //                principalSchema: "Dawem",
        //                principalTable: "Employees",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SchedulePlanEmployees_SchedulePlans_SchedulePlanId",
        //                column: x => x.SchedulePlanId,
        //                principalSchema: "Dawem",
        //                principalTable: "SchedulePlans",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "SchedulePlanLogEmployees",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            SchedulePlanLogId = table.Column<int>(type: "int", nullable: false),
        //            EmployeeId = table.Column<int>(type: "int", nullable: false),
        //            OldScheduleId = table.Column<int>(type: "int", nullable: true),
        //            NewScheduleId = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SchedulePlanLogEmployees", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_SchedulePlanLogEmployees_Employees_EmployeeId",
        //                column: x => x.EmployeeId,
        //                principalSchema: "Dawem",
        //                principalTable: "Employees",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SchedulePlanLogEmployees_SchedulePlanLogs_SchedulePlanLogId",
        //                column: x => x.SchedulePlanLogId,
        //                principalSchema: "Dawem",
        //                principalTable: "SchedulePlanLogs",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_SchedulePlanLogEmployees_Schedules_NewScheduleId",
        //                column: x => x.NewScheduleId,
        //                principalSchema: "Dawem",
        //                principalTable: "Schedules",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SchedulePlanLogEmployees_Schedules_OldScheduleId",
        //                column: x => x.OldScheduleId,
        //                principalSchema: "Dawem",
        //                principalTable: "Schedules",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "SummonEmployees",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            SummonId = table.Column<int>(type: "int", nullable: false),
        //            EmployeeId = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SummonEmployees", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_SummonEmployees_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SummonEmployees_Employees_EmployeeId",
        //                column: x => x.EmployeeId,
        //                principalSchema: "Dawem",
        //                principalTable: "Employees",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SummonEmployees_Summons_SummonId",
        //                column: x => x.SummonId,
        //                principalSchema: "Dawem",
        //                principalTable: "Summons",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "SummonMissingLogs",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            EmployeeId = table.Column<int>(type: "int", nullable: false),
        //            SummonId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SummonMissingLogs", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_SummonMissingLogs_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SummonMissingLogs_Employees_EmployeeId",
        //                column: x => x.EmployeeId,
        //                principalSchema: "Dawem",
        //                principalTable: "Employees",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SummonMissingLogs_Summons_SummonId",
        //                column: x => x.SummonId,
        //                principalSchema: "Dawem",
        //                principalTable: "Summons",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "VacationBalances",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            EmployeeId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Year = table.Column<int>(type: "int", nullable: false),
        //            ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            DefaultVacationType = table.Column<int>(type: "int", nullable: false),
        //            Balance = table.Column<float>(type: "real", nullable: false),
        //            RemainingBalance = table.Column<float>(type: "real", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_VacationBalances", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_VacationBalances_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_VacationBalances_Employees_EmployeeId",
        //                column: x => x.EmployeeId,
        //                principalSchema: "Dawem",
        //                principalTable: "Employees",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "ZoneEmployees",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            EmployeeId = table.Column<int>(type: "int", nullable: true),
        //            ZoneId = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_ZoneEmployees", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_ZoneEmployees_Employees_EmployeeId",
        //                column: x => x.EmployeeId,
        //                principalSchema: "Dawem",
        //                principalTable: "Employees",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_ZoneEmployees_Zones_ZoneId",
        //                column: x => x.ZoneId,
        //                principalSchema: "Dawem",
        //                principalTable: "Zones",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "EmployeeAttendanceChecks",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            EmployeeAttendanceId = table.Column<int>(type: "int", nullable: false),
        //            SummonId = table.Column<int>(type: "int", nullable: true),
        //            ZoneId = table.Column<int>(type: "int", nullable: true),
        //            Time = table.Column<TimeSpan>(type: "time", nullable: false),
        //            Latitude = table.Column<double>(type: "float", nullable: false),
        //            Longitude = table.Column<double>(type: "float", nullable: false),
        //            IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            FingerPrintType = table.Column<int>(type: "int", nullable: false),
        //            RecognitionWay = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_EmployeeAttendanceChecks", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_EmployeeAttendanceChecks_EmployeeAttendances_EmployeeAttendanceId",
        //                column: x => x.EmployeeAttendanceId,
        //                principalSchema: "Dawem",
        //                principalTable: "EmployeeAttendances",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_EmployeeAttendanceChecks_Summons_SummonId",
        //                column: x => x.SummonId,
        //                principalSchema: "Dawem",
        //                principalTable: "Summons",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_EmployeeAttendanceChecks_Zones_ZoneId",
        //                column: x => x.ZoneId,
        //                principalSchema: "Dawem",
        //                principalTable: "Zones",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "GroupEmployees",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            EmployeeId = table.Column<int>(type: "int", nullable: false),
        //            GroupId = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_GroupEmployees", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_GroupEmployees_Employees_EmployeeId",
        //                column: x => x.EmployeeId,
        //                principalSchema: "Dawem",
        //                principalTable: "Employees",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_GroupEmployees_Groups_GroupId",
        //                column: x => x.GroupId,
        //                principalSchema: "Dawem",
        //                principalTable: "Groups",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "GroupManagerDelegators",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            EmployeeId = table.Column<int>(type: "int", nullable: false),
        //            GroupId = table.Column<int>(type: "int", nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_GroupManagerDelegators", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_GroupManagerDelegators_Employees_EmployeeId",
        //                column: x => x.EmployeeId,
        //                principalSchema: "Dawem",
        //                principalTable: "Employees",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_GroupManagerDelegators_Groups_GroupId",
        //                column: x => x.GroupId,
        //                principalSchema: "Dawem",
        //                principalTable: "Groups",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "SchedulePlanGroups",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            SchedulePlanId = table.Column<int>(type: "int", nullable: false),
        //            GroupId = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SchedulePlanGroups", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_SchedulePlanGroups_Groups_GroupId",
        //                column: x => x.GroupId,
        //                principalSchema: "Dawem",
        //                principalTable: "Groups",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SchedulePlanGroups_SchedulePlans_SchedulePlanId",
        //                column: x => x.SchedulePlanId,
        //                principalSchema: "Dawem",
        //                principalTable: "SchedulePlans",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "SummonGroups",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            SummonId = table.Column<int>(type: "int", nullable: false),
        //            GroupId = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SummonGroups", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_SummonGroups_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SummonGroups_Groups_GroupId",
        //                column: x => x.GroupId,
        //                principalSchema: "Dawem",
        //                principalTable: "Groups",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_SummonGroups_Summons_SummonId",
        //                column: x => x.SummonId,
        //                principalSchema: "Dawem",
        //                principalTable: "Summons",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "ZoneGroups",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            GroupId = table.Column<int>(type: "int", nullable: false),
        //            ZoneId = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_ZoneGroups", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_ZoneGroups_Groups_GroupId",
        //                column: x => x.GroupId,
        //                principalSchema: "Dawem",
        //                principalTable: "Groups",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_ZoneGroups_Zones_ZoneId",
        //                column: x => x.ZoneId,
        //                principalSchema: "Dawem",
        //                principalTable: "Zones",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "NotificationUsers",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            UserId = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_NotificationUsers", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_NotificationUsers_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_NotificationUsers_MyUsers_UserId",
        //                column: x => x.UserId,
        //                principalSchema: "Dawem",
        //                principalTable: "MyUsers",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "PermissionLogs",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Date = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            UserId = table.Column<int>(type: "int", nullable: false),
        //            ScreenCode = table.Column<int>(type: "int", nullable: false),
        //            ActionCode = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_PermissionLogs", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_PermissionLogs_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_PermissionLogs_MyUsers_UserId",
        //                column: x => x.UserId,
        //                principalSchema: "Dawem",
        //                principalTable: "MyUsers",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Permissions",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            ForType = table.Column<int>(type: "int", nullable: false),
        //            RoleId = table.Column<int>(type: "int", nullable: true),
        //            UserId = table.Column<int>(type: "int", nullable: true),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Permissions", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Permissions_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_Permissions_MyUsers_UserId",
        //                column: x => x.UserId,
        //                principalSchema: "Dawem",
        //                principalTable: "MyUsers",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_Permissions_Roles_RoleId",
        //                column: x => x.RoleId,
        //                principalSchema: "Dawem",
        //                principalTable: "Roles",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Requests",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            CompanyId = table.Column<int>(type: "int", nullable: false),
        //            EmployeeId = table.Column<int>(type: "int", nullable: false),
        //            DecisionUserId = table.Column<int>(type: "int", nullable: true),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Type = table.Column<int>(type: "int", nullable: false),
        //            IsNecessary = table.Column<bool>(type: "bit", nullable: false),
        //            ForEmployee = table.Column<bool>(type: "bit", nullable: false),
        //            Status = table.Column<int>(type: "int", nullable: false),
        //            DecisionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            Date = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            RejectReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Requests", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Requests_Companies_CompanyId",
        //                column: x => x.CompanyId,
        //                principalSchema: "Dawem",
        //                principalTable: "Companies",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_Requests_Employees_EmployeeId",
        //                column: x => x.EmployeeId,
        //                principalSchema: "Dawem",
        //                principalTable: "Employees",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_Requests_MyUsers_DecisionUserId",
        //                column: x => x.DecisionUserId,
        //                principalSchema: "Dawem",
        //                principalTable: "MyUsers",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "UserBranches",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            UserId = table.Column<int>(type: "int", nullable: false),
        //            BranchId = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_UserBranches", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_UserBranches_MyUsers_UserId",
        //                column: x => x.UserId,
        //                principalSchema: "Dawem",
        //                principalTable: "MyUsers",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "UserClaims",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            UserId = table.Column<int>(type: "int", nullable: false),
        //            ClaimType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            ClaimValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_UserClaims", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_UserClaims_MyUsers_UserId",
        //                column: x => x.UserId,
        //                principalSchema: "Dawem",
        //                principalTable: "MyUsers",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "UserLogIns",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
        //            ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
        //            ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
        //            UserId = table.Column<int>(type: "int", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_UserLogIns", x => new { x.LoginProvider, x.ProviderKey });
        //            table.ForeignKey(
        //                name: "FK_UserLogIns_MyUsers_UserId",
        //                column: x => x.UserId,
        //                principalSchema: "Dawem",
        //                principalTable: "MyUsers",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "UserRoles",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            UserId = table.Column<int>(type: "int", nullable: false),
        //            RoleId = table.Column<int>(type: "int", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
        //            table.ForeignKey(
        //                name: "FK_UserRoles_MyUsers_UserId",
        //                column: x => x.UserId,
        //                principalSchema: "Dawem",
        //                principalTable: "MyUsers",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_UserRoles_Roles_RoleId",
        //                column: x => x.RoleId,
        //                principalSchema: "Dawem",
        //                principalTable: "Roles",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "UserTokens",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            UserId = table.Column<int>(type: "int", nullable: false),
        //            LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
        //            Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
        //            Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
        //            table.ForeignKey(
        //                name: "FK_UserTokens_MyUsers_UserId",
        //                column: x => x.UserId,
        //                principalSchema: "Dawem",
        //                principalTable: "MyUsers",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "SummonMissingLogSanctions",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            SummonMissingLogId = table.Column<int>(type: "int", nullable: false),
        //            SummonSanctionId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            Done = table.Column<bool>(type: "bit", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SummonMissingLogSanctions", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_SummonMissingLogSanctions_SummonMissingLogs_SummonMissingLogId",
        //                column: x => x.SummonMissingLogId,
        //                principalSchema: "Dawem",
        //                principalTable: "SummonMissingLogs",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_SummonMissingLogSanctions_SummonSanctions_SummonSanctionId",
        //                column: x => x.SummonSanctionId,
        //                principalSchema: "Dawem",
        //                principalTable: "SummonSanctions",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "NotificationUserFCMTokens",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            NotificationUserId = table.Column<int>(type: "int", nullable: false),
        //            FCMToken = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
        //            DeviceType = table.Column<int>(type: "int", nullable: false),
        //            LastLogInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_NotificationUserFCMTokens", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_NotificationUserFCMTokens_NotificationUsers_NotificationUserId",
        //                column: x => x.NotificationUserId,
        //                principalSchema: "Dawem",
        //                principalTable: "NotificationUsers",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "PermissionScreens",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            PermissionId = table.Column<int>(type: "int", nullable: false),
        //            ScreenCode = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_PermissionScreens", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_PermissionScreens_Permissions_PermissionId",
        //                column: x => x.PermissionId,
        //                principalSchema: "Dawem",
        //                principalTable: "Permissions",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "RequestAssignments",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            RequestId = table.Column<int>(type: "int", nullable: false),
        //            AssignmentTypeId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            DateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_RequestAssignments", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_RequestAssignments_AssignmentTypes_AssignmentTypeId",
        //                column: x => x.AssignmentTypeId,
        //                principalSchema: "Dawem",
        //                principalTable: "AssignmentTypes",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_RequestAssignments_Requests_RequestId",
        //                column: x => x.RequestId,
        //                principalSchema: "Dawem",
        //                principalTable: "Requests",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "RequestAttachments",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            RequestId = table.Column<int>(type: "int", nullable: false),
        //            FileName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_RequestAttachments", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_RequestAttachments_Requests_RequestId",
        //                column: x => x.RequestId,
        //                principalSchema: "Dawem",
        //                principalTable: "Requests",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "RequestJustifications",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            RequestId = table.Column<int>(type: "int", nullable: false),
        //            JustificationTypeId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            DateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_RequestJustifications", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_RequestJustifications_JustificationTypes_JustificationTypeId",
        //                column: x => x.JustificationTypeId,
        //                principalSchema: "Dawem",
        //                principalTable: "JustificationTypes",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_RequestJustifications_Requests_RequestId",
        //                column: x => x.RequestId,
        //                principalSchema: "Dawem",
        //                principalTable: "Requests",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "RequestPermissions",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            RequestId = table.Column<int>(type: "int", nullable: false),
        //            PermissionTypeId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            DateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_RequestPermissions", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_RequestPermissions_PermissionTypes_PermissionTypeId",
        //                column: x => x.PermissionTypeId,
        //                principalSchema: "Dawem",
        //                principalTable: "PermissionTypes",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_RequestPermissions_Requests_RequestId",
        //                column: x => x.RequestId,
        //                principalSchema: "Dawem",
        //                principalTable: "Requests",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "RequestTasks",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            RequestId = table.Column<int>(type: "int", nullable: false),
        //            TaskTypeId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            DateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_RequestTasks", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_RequestTasks_Requests_RequestId",
        //                column: x => x.RequestId,
        //                principalSchema: "Dawem",
        //                principalTable: "Requests",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_RequestTasks_TaskTypes_TaskTypeId",
        //                column: x => x.TaskTypeId,
        //                principalSchema: "Dawem",
        //                principalTable: "TaskTypes",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "RequestVacations",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            RequestId = table.Column<int>(type: "int", nullable: false),
        //            VacationTypeId = table.Column<int>(type: "int", nullable: false),
        //            Code = table.Column<int>(type: "int", nullable: false),
        //            NumberOfDays = table.Column<int>(type: "int", nullable: false),
        //            BalanceBeforeRequest = table.Column<float>(type: "real", nullable: false),
        //            BalanceAfterRequest = table.Column<float>(type: "real", nullable: false),
        //            DateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_RequestVacations", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_RequestVacations_Requests_RequestId",
        //                column: x => x.RequestId,
        //                principalSchema: "Dawem",
        //                principalTable: "Requests",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_RequestVacations_VacationTypes_VacationTypeId",
        //                column: x => x.VacationTypeId,
        //                principalSchema: "Dawem",
        //                principalTable: "VacationTypes",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "PermissionScreenActions",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            PermissionScreenId = table.Column<int>(type: "int", nullable: false),
        //            ActionCode = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_PermissionScreenActions", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_PermissionScreenActions_PermissionScreens_PermissionScreenId",
        //                column: x => x.PermissionScreenId,
        //                principalSchema: "Dawem",
        //                principalTable: "PermissionScreens",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "RequestTaskEmployees",
        //        schema: "Dawem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            RequestTaskId = table.Column<int>(type: "int", nullable: false),
        //            EmployeeId = table.Column<int>(type: "int", nullable: false),
        //            AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
        //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            AddedApplicationType = table.Column<int>(type: "int", nullable: false),
        //            ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
        //            AddUserId = table.Column<int>(type: "int", nullable: true),
        //            ModifyUserId = table.Column<int>(type: "int", nullable: true),
        //            IsActive = table.Column<bool>(type: "bit", nullable: false),
        //            IsDeleted = table.Column<bool>(type: "bit", nullable: false),
        //            DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
        //            DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
        //            Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_RequestTaskEmployees", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_RequestTaskEmployees_Employees_EmployeeId",
        //                column: x => x.EmployeeId,
        //                principalSchema: "Dawem",
        //                principalTable: "Employees",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_RequestTaskEmployees_RequestTasks_RequestTaskId",
        //                column: x => x.RequestTaskId,
        //                principalSchema: "Dawem",
        //                principalTable: "RequestTasks",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "AssignmentTypes",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "AssignmentTypes",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Companies_CountryId",
        //        schema: "Dawem",
        //        table: "Companies",
        //        column: "CountryId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Companies_IdentityCode",
        //        schema: "Dawem",
        //        table: "Companies",
        //        column: "IdentityCode",
        //        unique: true,
        //        filter: "[IdentityCode] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Companies_PreferredLanguageId",
        //        schema: "Dawem",
        //        table: "Companies",
        //        column: "PreferredLanguageId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_CompanyAttachments_CompanyId",
        //        schema: "Dawem",
        //        table: "CompanyAttachments",
        //        column: "CompanyId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "CompanyBranches",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "CompanyIndustries",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Currencies_CountryId",
        //        schema: "Dawem",
        //        table: "Currencies",
        //        column: "CountryId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_DepartmentManagerDelegators_DepartmentId",
        //        schema: "Dawem",
        //        table: "DepartmentManagerDelegators",
        //        column: "DepartmentId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_DepartmentManagerDelegators_EmployeeId",
        //        schema: "Dawem",
        //        table: "DepartmentManagerDelegators",
        //        column: "EmployeeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Departments_ManagerId",
        //        schema: "Dawem",
        //        table: "Departments",
        //        column: "ManagerId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Departments_ParentId",
        //        schema: "Dawem",
        //        table: "Departments",
        //        column: "ParentId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "Departments",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "Departments",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_EmployeeAttendanceChecks_EmployeeAttendanceId",
        //        schema: "Dawem",
        //        table: "EmployeeAttendanceChecks",
        //        column: "EmployeeAttendanceId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_EmployeeAttendanceChecks_SummonId",
        //        schema: "Dawem",
        //        table: "EmployeeAttendanceChecks",
        //        column: "SummonId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_EmployeeAttendanceChecks_ZoneId",
        //        schema: "Dawem",
        //        table: "EmployeeAttendanceChecks",
        //        column: "ZoneId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_EmployeeAttendances_EmployeeId",
        //        schema: "Dawem",
        //        table: "EmployeeAttendances",
        //        column: "EmployeeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_EmployeeAttendances_ScheduleId",
        //        schema: "Dawem",
        //        table: "EmployeeAttendances",
        //        column: "ScheduleId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_EmployeeAttendances_ShiftId",
        //        schema: "Dawem",
        //        table: "EmployeeAttendances",
        //        column: "ShiftId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "EmployeeAttendances",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Employees_DepartmentId",
        //        schema: "Dawem",
        //        table: "Employees",
        //        column: "DepartmentId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Employees_DirectManagerId",
        //        schema: "Dawem",
        //        table: "Employees",
        //        column: "DirectManagerId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Employees_JobTitleId",
        //        schema: "Dawem",
        //        table: "Employees",
        //        column: "JobTitleId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Employees_MobileCountryId",
        //        schema: "Dawem",
        //        table: "Employees",
        //        column: "MobileCountryId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Employees_ScheduleId",
        //        schema: "Dawem",
        //        table: "Employees",
        //        column: "ScheduleId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "Employees",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "Employees",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "FingerprintDevices",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "FingerprintDevices",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_GroupEmployees_EmployeeId",
        //        schema: "Dawem",
        //        table: "GroupEmployees",
        //        column: "EmployeeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_GroupEmployees_GroupId",
        //        schema: "Dawem",
        //        table: "GroupEmployees",
        //        column: "GroupId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_GroupManagerDelegators_EmployeeId",
        //        schema: "Dawem",
        //        table: "GroupManagerDelegators",
        //        column: "EmployeeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_GroupManagerDelegators_GroupId",
        //        schema: "Dawem",
        //        table: "GroupManagerDelegators",
        //        column: "GroupId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Groups_ManagerId",
        //        schema: "Dawem",
        //        table: "Groups",
        //        column: "ManagerId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "Groups",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "Groups",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "Holidays",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "Holidays",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "HolidayTypes",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "HolidayTypes",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "JobTitles",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "JobTitles",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "JustificationTypes",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "JustificationTypes",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "EmailIndex",
        //        schema: "Dawem",
        //        table: "MyUsers",
        //        column: "NormalizedEmail");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_MyUsers_EmployeeId",
        //        schema: "Dawem",
        //        table: "MyUsers",
        //        column: "EmployeeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_MyUsers_MobileCountryId",
        //        schema: "Dawem",
        //        table: "MyUsers",
        //        column: "MobileCountryId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "MyUsers",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true,
        //        filter: "[CompanyId] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "MyUsers",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[CompanyId] IS NOT NULL AND [Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "UserNameIndex",
        //        schema: "Dawem",
        //        table: "MyUsers",
        //        column: "NormalizedUserName",
        //        unique: true,
        //        filter: "[NormalizedUserName] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_NotificationStores_EmployeeId",
        //        schema: "Dawem",
        //        table: "NotificationStores",
        //        column: "EmployeeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "NotificationStores",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_NotificationUserFCMTokens_NotificationUserId",
        //        schema: "Dawem",
        //        table: "NotificationUserFCMTokens",
        //        column: "NotificationUserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_NotificationUsers_CompanyId",
        //        schema: "Dawem",
        //        table: "NotificationUsers",
        //        column: "CompanyId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_NotificationUsers_UserId",
        //        schema: "Dawem",
        //        table: "NotificationUsers",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_PermissionLogs_CompanyId",
        //        schema: "Dawem",
        //        table: "PermissionLogs",
        //        column: "CompanyId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_PermissionLogs_UserId",
        //        schema: "Dawem",
        //        table: "PermissionLogs",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Permissions_RoleId",
        //        schema: "Dawem",
        //        table: "Permissions",
        //        column: "RoleId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Permissions_UserId",
        //        schema: "Dawem",
        //        table: "Permissions",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "Permissions",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_PermissionScreenActions_PermissionScreenId",
        //        schema: "Dawem",
        //        table: "PermissionScreenActions",
        //        column: "PermissionScreenId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_PermissionScreens_PermissionId",
        //        schema: "Dawem",
        //        table: "PermissionScreens",
        //        column: "PermissionId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "PermissionTypes",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "PermissionTypes",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_PlanNameTranslations_LanguageId",
        //        schema: "Dawem",
        //        table: "PlanNameTranslations",
        //        column: "LanguageId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_PlanNameTranslations_PlanId",
        //        schema: "Dawem",
        //        table: "PlanNameTranslations",
        //        column: "PlanId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_RequestAssignments_AssignmentTypeId",
        //        schema: "Dawem",
        //        table: "RequestAssignments",
        //        column: "AssignmentTypeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_RequestAssignments_RequestId",
        //        schema: "Dawem",
        //        table: "RequestAssignments",
        //        column: "RequestId",
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_RequestAttachments_RequestId",
        //        schema: "Dawem",
        //        table: "RequestAttachments",
        //        column: "RequestId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_RequestJustifications_JustificationTypeId",
        //        schema: "Dawem",
        //        table: "RequestJustifications",
        //        column: "JustificationTypeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_RequestJustifications_RequestId",
        //        schema: "Dawem",
        //        table: "RequestJustifications",
        //        column: "RequestId",
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_RequestPermissions_PermissionTypeId",
        //        schema: "Dawem",
        //        table: "RequestPermissions",
        //        column: "PermissionTypeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_RequestPermissions_RequestId",
        //        schema: "Dawem",
        //        table: "RequestPermissions",
        //        column: "RequestId",
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Requests_DecisionUserId",
        //        schema: "Dawem",
        //        table: "Requests",
        //        column: "DecisionUserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Requests_EmployeeId",
        //        schema: "Dawem",
        //        table: "Requests",
        //        column: "EmployeeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "Requests",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_RequestTaskEmployees_EmployeeId",
        //        schema: "Dawem",
        //        table: "RequestTaskEmployees",
        //        column: "EmployeeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_RequestTaskEmployees_RequestTaskId",
        //        schema: "Dawem",
        //        table: "RequestTaskEmployees",
        //        column: "RequestTaskId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_RequestTasks_RequestId",
        //        schema: "Dawem",
        //        table: "RequestTasks",
        //        column: "RequestId",
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_RequestTasks_TaskTypeId",
        //        schema: "Dawem",
        //        table: "RequestTasks",
        //        column: "TaskTypeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_RequestVacations_RequestId",
        //        schema: "Dawem",
        //        table: "RequestVacations",
        //        column: "RequestId",
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_RequestVacations_VacationTypeId",
        //        schema: "Dawem",
        //        table: "RequestVacations",
        //        column: "VacationTypeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_RoleClaims_RoleId",
        //        schema: "Dawem",
        //        table: "RoleClaims",
        //        column: "RoleId");

        //    migrationBuilder.CreateIndex(
        //        name: "RoleNameIndex",
        //        schema: "Dawem",
        //        table: "Roles",
        //        column: "NormalizedName",
        //        unique: true,
        //        filter: "[NormalizedName] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "Sanctions",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "Sanctions",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_ScheduleDays_ScheduleId",
        //        schema: "Dawem",
        //        table: "ScheduleDays",
        //        column: "ScheduleId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_ScheduleDays_ShiftId",
        //        schema: "Dawem",
        //        table: "ScheduleDays",
        //        column: "ShiftId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SchedulePlanDepartments_DepartmentId",
        //        schema: "Dawem",
        //        table: "SchedulePlanDepartments",
        //        column: "DepartmentId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SchedulePlanDepartments_SchedulePlanId",
        //        schema: "Dawem",
        //        table: "SchedulePlanDepartments",
        //        column: "SchedulePlanId",
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SchedulePlanEmployees_EmployeeId",
        //        schema: "Dawem",
        //        table: "SchedulePlanEmployees",
        //        column: "EmployeeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SchedulePlanEmployees_SchedulePlanId",
        //        schema: "Dawem",
        //        table: "SchedulePlanEmployees",
        //        column: "SchedulePlanId",
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SchedulePlanGroups_GroupId",
        //        schema: "Dawem",
        //        table: "SchedulePlanGroups",
        //        column: "GroupId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SchedulePlanGroups_SchedulePlanId",
        //        schema: "Dawem",
        //        table: "SchedulePlanGroups",
        //        column: "SchedulePlanId",
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SchedulePlanLogEmployees_EmployeeId",
        //        schema: "Dawem",
        //        table: "SchedulePlanLogEmployees",
        //        column: "EmployeeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SchedulePlanLogEmployees_NewScheduleId",
        //        schema: "Dawem",
        //        table: "SchedulePlanLogEmployees",
        //        column: "NewScheduleId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SchedulePlanLogEmployees_OldScheduleId",
        //        schema: "Dawem",
        //        table: "SchedulePlanLogEmployees",
        //        column: "OldScheduleId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SchedulePlanLogEmployees_SchedulePlanLogId",
        //        schema: "Dawem",
        //        table: "SchedulePlanLogEmployees",
        //        column: "SchedulePlanLogId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SchedulePlanLogs_SchedulePlanId",
        //        schema: "Dawem",
        //        table: "SchedulePlanLogs",
        //        column: "SchedulePlanId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "SchedulePlanLogs",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SchedulePlans_ScheduleId",
        //        schema: "Dawem",
        //        table: "SchedulePlans",
        //        column: "ScheduleId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "SchedulePlans",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "Schedules",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "Schedules",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "ShiftWorkingTimes",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "ShiftWorkingTimes",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SubscriptionLogs_SubscriptionId",
        //        schema: "Dawem",
        //        table: "SubscriptionLogs",
        //        column: "SubscriptionId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Subscriptions_PlanId",
        //        schema: "Dawem",
        //        table: "Subscriptions",
        //        column: "PlanId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "Subscriptions",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonDepartments_CompanyId",
        //        schema: "Dawem",
        //        table: "SummonDepartments",
        //        column: "CompanyId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonDepartments_DepartmentId",
        //        schema: "Dawem",
        //        table: "SummonDepartments",
        //        column: "DepartmentId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonDepartments_SummonId",
        //        schema: "Dawem",
        //        table: "SummonDepartments",
        //        column: "SummonId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonEmployees_CompanyId",
        //        schema: "Dawem",
        //        table: "SummonEmployees",
        //        column: "CompanyId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonEmployees_EmployeeId",
        //        schema: "Dawem",
        //        table: "SummonEmployees",
        //        column: "EmployeeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonEmployees_SummonId",
        //        schema: "Dawem",
        //        table: "SummonEmployees",
        //        column: "SummonId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonGroups_CompanyId",
        //        schema: "Dawem",
        //        table: "SummonGroups",
        //        column: "CompanyId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonGroups_GroupId",
        //        schema: "Dawem",
        //        table: "SummonGroups",
        //        column: "GroupId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonGroups_SummonId",
        //        schema: "Dawem",
        //        table: "SummonGroups",
        //        column: "SummonId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonMissingLogs_EmployeeId",
        //        schema: "Dawem",
        //        table: "SummonMissingLogs",
        //        column: "EmployeeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonMissingLogs_SummonId",
        //        schema: "Dawem",
        //        table: "SummonMissingLogs",
        //        column: "SummonId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "SummonMissingLogs",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonMissingLogSanctions_SummonMissingLogId",
        //        schema: "Dawem",
        //        table: "SummonMissingLogSanctions",
        //        column: "SummonMissingLogId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonMissingLogSanctions_SummonSanctionId",
        //        schema: "Dawem",
        //        table: "SummonMissingLogSanctions",
        //        column: "SummonSanctionId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonNotifyWays_CompanyId",
        //        schema: "Dawem",
        //        table: "SummonNotifyWays",
        //        column: "CompanyId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonNotifyWays_SummonId",
        //        schema: "Dawem",
        //        table: "SummonNotifyWays",
        //        column: "SummonId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "Summons",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonSanctions_CompanyId",
        //        schema: "Dawem",
        //        table: "SummonSanctions",
        //        column: "CompanyId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonSanctions_SanctionId",
        //        schema: "Dawem",
        //        table: "SummonSanctions",
        //        column: "SanctionId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SummonSanctions_SummonId",
        //        schema: "Dawem",
        //        table: "SummonSanctions",
        //        column: "SummonId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "TaskTypes",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "TaskTypes",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Translations_KeyWord_Lang",
        //        schema: "Dawem",
        //        table: "Translations",
        //        columns: new[] { "KeyWord", "Lang" },
        //        unique: true,
        //        filter: "[KeyWord] IS NOT NULL AND [Lang] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_UserBranches_UserId",
        //        schema: "Dawem",
        //        table: "UserBranches",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_UserClaims_UserId",
        //        schema: "Dawem",
        //        table: "UserClaims",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_UserLogIns_UserId",
        //        schema: "Dawem",
        //        table: "UserLogIns",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_UserRoles_RoleId",
        //        schema: "Dawem",
        //        table: "UserRoles",
        //        column: "RoleId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "VacationBalances",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_VacationBalances_EmployeeId",
        //        schema: "Dawem",
        //        table: "VacationBalances",
        //        column: "EmployeeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "VacationTypes",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "VacationTypes",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_ZoneDepartments_DepartmentId",
        //        schema: "Dawem",
        //        table: "ZoneDepartments",
        //        column: "DepartmentId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_ZoneDepartments_ZoneId",
        //        schema: "Dawem",
        //        table: "ZoneDepartments",
        //        column: "ZoneId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_ZoneEmployees_EmployeeId",
        //        schema: "Dawem",
        //        table: "ZoneEmployees",
        //        column: "EmployeeId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_ZoneEmployees_ZoneId",
        //        schema: "Dawem",
        //        table: "ZoneEmployees",
        //        column: "ZoneId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_ZoneGroups_GroupId",
        //        schema: "Dawem",
        //        table: "ZoneGroups",
        //        column: "GroupId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_ZoneGroups_ZoneId",
        //        schema: "Dawem",
        //        table: "ZoneGroups",
        //        column: "ZoneId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Code_IsDeleted",
        //        schema: "Dawem",
        //        table: "Zones",
        //        columns: new[] { "CompanyId", "Code", "IsDeleted" },
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Unique_CompanyId_Name_IsDeleted",
        //        schema: "Dawem",
        //        table: "Zones",
        //        columns: new[] { "CompanyId", "Name", "IsDeleted" },
        //        unique: true,
        //        filter: "[Name] IS NOT NULL");

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_DepartmentManagerDelegators_Departments_DepartmentId",
        //        schema: "Dawem",
        //        table: "DepartmentManagerDelegators",
        //        column: "DepartmentId",
        //        principalSchema: "Dawem",
        //        principalTable: "Departments",
        //        principalColumn: "Id",
        //        onDelete: ReferentialAction.Cascade);

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_DepartmentManagerDelegators_Employees_EmployeeId",
        //        schema: "Dawem",
        //        table: "DepartmentManagerDelegators",
        //        column: "EmployeeId",
        //        principalSchema: "Dawem",
        //        principalTable: "Employees",
        //        principalColumn: "Id",
        //        onDelete: ReferentialAction.Restrict);

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_Departments_Employees_ManagerId",
        //        schema: "Dawem",
        //        table: "Departments",
        //        column: "ManagerId",
        //        principalSchema: "Dawem",
        //        principalTable: "Employees",
        //        principalColumn: "Id");
        //}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Companies_CompanyId",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Companies_CompanyId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_JobTitles_Companies_CompanyId",
                schema: "Dawem",
                table: "JobTitles");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Companies_CompanyId",
                schema: "Dawem",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Countries_MobileCountryId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "CompanyAttachments",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "CompanyBranches",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "CompanyIndustries",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "DawemSettings",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "DepartmentManagerDelegators",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "EmployeeAttendanceChecks",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "FingerprintDevices",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "GroupEmployees",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "GroupManagerDelegators",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Holidays",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "HolidayTypes",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "NotificationStores",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "NotificationUserFCMTokens",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "PermissionLogs",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "PermissionScreenActions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "PlanNameTranslations",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "RequestAssignments",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "RequestAttachments",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "RequestJustifications",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "RequestPermissions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "RequestTaskEmployees",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "RequestVacations",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "ScheduleDays",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SchedulePlanDepartments",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SchedulePlanEmployees",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SchedulePlanGroups",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SchedulePlanLogEmployees",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SubscriptionLogs",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SummonDepartments",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SummonEmployees",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SummonGroups",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SummonMissingLogSanctions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SummonNotifyWays",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Translations",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "UserBranches",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "UserLogIns",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "VacationBalances",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "ZoneDepartments",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "ZoneEmployees",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "ZoneGroups",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "EmployeeAttendances",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "NotificationUsers",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "PermissionScreens",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "AssignmentTypes",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "JustificationTypes",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "PermissionTypes",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "RequestTasks",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "VacationTypes",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SchedulePlanLogs",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Subscriptions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SummonMissingLogs",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SummonSanctions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Zones",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "ShiftWorkingTimes",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Requests",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "TaskTypes",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SchedulePlans",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Plans",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Sanctions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Summons",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "MyUsers",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Companies",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "JobTitles",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Schedules",
                schema: "Dawem");
        }
    }
}
