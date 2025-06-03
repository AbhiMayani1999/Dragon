using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dragon.Data.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigConnection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatabaseType = table.Column<byte>(type: "tinyint", nullable: false),
                    TenantCode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Server = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Database = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    User = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigConnection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigDomainConnect",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigDomainConnect", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigEmailConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HostUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    EnableSsl = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    NetKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetPassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigEmailConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HrmsCompany",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Proprietor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSelf = table.Column<bool>(type: "bit", nullable: false),
                    GstNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LutArn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pancard = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HrmsCompany", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformComponentProperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsGeneric = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformComponentProperty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformComponentStructure",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsGeneric = table.Column<bool>(type: "bit", nullable: false),
                    CanBeParent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformComponentStructure", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformComponentStructureSubComponent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildComponentId = table.Column<int>(type: "int", nullable: true),
                    ParentComponentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformComponentStructureSubComponent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformKeyGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsGeneric = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformKeyGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformNavigation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentNavigationId = table.Column<int>(type: "int", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortIndex = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformNavigation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformNavigation_PlatformNavigation_ParentNavigationId",
                        column: x => x.ParentNavigationId,
                        principalTable: "PlatformNavigation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlatformStructurePropertyToStructure",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildStructureId = table.Column<int>(type: "int", nullable: true),
                    StructurePropertyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformStructurePropertyToStructure", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformStructureSubStructure",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildStructureId = table.Column<int>(type: "int", nullable: true),
                    ParentStructureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformStructureSubStructure", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformUserType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformUserType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigDomainSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tagline = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DomainConnectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigDomainSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfigDomainSetting_ConfigDomainConnect_DomainConnectId",
                        column: x => x.DomainConnectId,
                        principalTable: "ConfigDomainConnect",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HrmsCompanyBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IfscCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SwiftCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Iban = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HrmsCompanyBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HrmsCompanyBank_HrmsCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "HrmsCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HrmsCompanyDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HrmsCompanyDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HrmsCompanyDepartment_HrmsCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "HrmsCompany",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HrmsCompanyInvoice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SameStateInvoice = table.Column<bool>(type: "bit", nullable: false),
                    InternationalInvoice = table.Column<bool>(type: "bit", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<float>(type: "real", nullable: false),
                    TaxAmount = table.Column<float>(type: "real", nullable: false),
                    TotalInWords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxPercentage = table.Column<float>(type: "real", nullable: false),
                    SgstAmount = table.Column<float>(type: "real", nullable: false),
                    CgstAmount = table.Column<float>(type: "real", nullable: false),
                    SgstPercentage = table.Column<float>(type: "real", nullable: false),
                    CgstPercentage = table.Column<float>(type: "real", nullable: false),
                    PaymentTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromCompanyId = table.Column<int>(type: "int", nullable: true),
                    ToCompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HrmsCompanyInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HrmsCompanyInvoice_HrmsCompany_FromCompanyId",
                        column: x => x.FromCompanyId,
                        principalTable: "HrmsCompany",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HrmsCompanyInvoice_HrmsCompany_ToCompanyId",
                        column: x => x.ToCompanyId,
                        principalTable: "HrmsCompany",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlatformComponentStructureProperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsMultiple = table.Column<bool>(type: "bit", nullable: false),
                    ComponentStructureId = table.Column<int>(type: "int", nullable: true),
                    ComponentPropertyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformComponentStructureProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformComponentStructureProperty_PlatformComponentProperty_ComponentPropertyId",
                        column: x => x.ComponentPropertyId,
                        principalTable: "PlatformComponentProperty",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlatformComponentStructureProperty_PlatformComponentStructure_ComponentStructureId",
                        column: x => x.ComponentStructureId,
                        principalTable: "PlatformComponentStructure",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlatformStructure",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComponentStructureId = table.Column<int>(type: "int", nullable: true),
                    ShortIndex = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformStructure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformStructure_PlatformComponentStructure_ComponentStructureId",
                        column: x => x.ComponentStructureId,
                        principalTable: "PlatformComponentStructure",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlatformKeyStore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformKeyStore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformKeyStore_PlatformKeyGroup_KeyGroupId",
                        column: x => x.KeyGroupId,
                        principalTable: "PlatformKeyGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUniversal = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformUser_PlatformUserType_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "PlatformUserType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformUserTypeNavigation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCreate = table.Column<bool>(type: "bit", nullable: false),
                    IsEdit = table.Column<bool>(type: "bit", nullable: false),
                    IsView = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformUserTypeNavigation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformUserTypeNavigation_PlatformUserType_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "PlatformUserType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HrmsEmployee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Salutation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ctc = table.Column<int>(type: "int", nullable: false),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFullTime = table.Column<bool>(type: "bit", nullable: false),
                    IsGenerateDocuments = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RelievingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProfileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pancard = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HrmsEmployee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HrmsEmployee_HrmsCompanyDepartment_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "HrmsCompanyDepartment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HrmsCompanyInvoiceService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    RatePerMonth = table.Column<float>(type: "real", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HrmsCompanyInvoiceService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HrmsCompanyInvoiceService_HrmsCompanyInvoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "HrmsCompanyInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformStructureProperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentStructurePropertyId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StructureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformStructureProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformStructureProperty_PlatformComponentStructureProperty_ComponentStructurePropertyId",
                        column: x => x.ComponentStructurePropertyId,
                        principalTable: "PlatformComponentStructureProperty",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlatformStructureProperty_PlatformStructure_StructureId",
                        column: x => x.StructureId,
                        principalTable: "PlatformStructure",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlatformUserNavigation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCreate = table.Column<bool>(type: "bit", nullable: false),
                    IsEdit = table.Column<bool>(type: "bit", nullable: false),
                    IsView = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformUserNavigation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformUserNavigation_PlatformUser_UserId",
                        column: x => x.UserId,
                        principalTable: "PlatformUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HrmsEmployeeAppraisal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RevisedDesignation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppraisalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevisedCtc = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HrmsEmployeeAppraisal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HrmsEmployeeAppraisal_HrmsEmployee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "HrmsEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HrmsEmployeeBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IfscCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SwiftCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Iban = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HrmsEmployeeBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HrmsEmployeeBank_HrmsEmployee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "HrmsEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HrmsEmployeeDocument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HrmsEmployeeDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HrmsEmployeeDocument_HrmsEmployee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "HrmsEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HrmsEmployeePunching",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PunchTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PunchType = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HrmsEmployeePunching", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HrmsEmployeePunching_HrmsEmployee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "HrmsEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HrmsEmployeeSalary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkingDays = table.Column<int>(type: "int", nullable: false),
                    Leaves = table.Column<int>(type: "int", nullable: false),
                    FixSalary = table.Column<int>(type: "int", nullable: false),
                    Bonus = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HrmsEmployeeSalary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HrmsEmployeeSalary_HrmsEmployee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "HrmsEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigDomainSetting_DomainConnectId",
                table: "ConfigDomainSetting",
                column: "DomainConnectId");

            migrationBuilder.CreateIndex(
                name: "IX_HrmsCompanyBank_CompanyId",
                table: "HrmsCompanyBank",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_HrmsCompanyDepartment_CompanyId",
                table: "HrmsCompanyDepartment",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_HrmsCompanyInvoice_FromCompanyId",
                table: "HrmsCompanyInvoice",
                column: "FromCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_HrmsCompanyInvoice_ToCompanyId",
                table: "HrmsCompanyInvoice",
                column: "ToCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_HrmsCompanyInvoiceService_InvoiceId",
                table: "HrmsCompanyInvoiceService",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_HrmsEmployee_DepartmentId",
                table: "HrmsEmployee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_HrmsEmployeeAppraisal_EmployeeId",
                table: "HrmsEmployeeAppraisal",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HrmsEmployeeBank_EmployeeId",
                table: "HrmsEmployeeBank",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HrmsEmployeeDocument_EmployeeId",
                table: "HrmsEmployeeDocument",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HrmsEmployeePunching_EmployeeId",
                table: "HrmsEmployeePunching",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HrmsEmployeeSalary_EmployeeId",
                table: "HrmsEmployeeSalary",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformComponentStructureProperty_ComponentPropertyId",
                table: "PlatformComponentStructureProperty",
                column: "ComponentPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformComponentStructureProperty_ComponentStructureId",
                table: "PlatformComponentStructureProperty",
                column: "ComponentStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformKeyStore_KeyGroupId",
                table: "PlatformKeyStore",
                column: "KeyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformNavigation_ParentNavigationId",
                table: "PlatformNavigation",
                column: "ParentNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformStructure_ComponentStructureId",
                table: "PlatformStructure",
                column: "ComponentStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformStructureProperty_ComponentStructurePropertyId",
                table: "PlatformStructureProperty",
                column: "ComponentStructurePropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformStructureProperty_StructureId",
                table: "PlatformStructureProperty",
                column: "StructureId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUser_UserTypeId",
                table: "PlatformUser",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUserNavigation_UserId",
                table: "PlatformUserNavigation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUserTypeNavigation_UserTypeId",
                table: "PlatformUserTypeNavigation",
                column: "UserTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigConnection");

            migrationBuilder.DropTable(
                name: "ConfigDomainSetting");

            migrationBuilder.DropTable(
                name: "ConfigEmailConfig");

            migrationBuilder.DropTable(
                name: "HrmsCompanyBank");

            migrationBuilder.DropTable(
                name: "HrmsCompanyInvoiceService");

            migrationBuilder.DropTable(
                name: "HrmsEmployeeAppraisal");

            migrationBuilder.DropTable(
                name: "HrmsEmployeeBank");

            migrationBuilder.DropTable(
                name: "HrmsEmployeeDocument");

            migrationBuilder.DropTable(
                name: "HrmsEmployeePunching");

            migrationBuilder.DropTable(
                name: "HrmsEmployeeSalary");

            migrationBuilder.DropTable(
                name: "PlatformComponentStructureSubComponent");

            migrationBuilder.DropTable(
                name: "PlatformKeyStore");

            migrationBuilder.DropTable(
                name: "PlatformNavigation");

            migrationBuilder.DropTable(
                name: "PlatformStructureProperty");

            migrationBuilder.DropTable(
                name: "PlatformStructurePropertyToStructure");

            migrationBuilder.DropTable(
                name: "PlatformStructureSubStructure");

            migrationBuilder.DropTable(
                name: "PlatformUserNavigation");

            migrationBuilder.DropTable(
                name: "PlatformUserTypeNavigation");

            migrationBuilder.DropTable(
                name: "ConfigDomainConnect");

            migrationBuilder.DropTable(
                name: "HrmsCompanyInvoice");

            migrationBuilder.DropTable(
                name: "HrmsEmployee");

            migrationBuilder.DropTable(
                name: "PlatformKeyGroup");

            migrationBuilder.DropTable(
                name: "PlatformComponentStructureProperty");

            migrationBuilder.DropTable(
                name: "PlatformStructure");

            migrationBuilder.DropTable(
                name: "PlatformUser");

            migrationBuilder.DropTable(
                name: "HrmsCompanyDepartment");

            migrationBuilder.DropTable(
                name: "PlatformComponentProperty");

            migrationBuilder.DropTable(
                name: "PlatformComponentStructure");

            migrationBuilder.DropTable(
                name: "PlatformUserType");

            migrationBuilder.DropTable(
                name: "HrmsCompany");
        }
    }
}
