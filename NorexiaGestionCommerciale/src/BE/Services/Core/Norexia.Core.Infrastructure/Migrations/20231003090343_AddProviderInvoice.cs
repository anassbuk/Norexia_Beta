using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Norexia.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "ClassKeys",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerCategory",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deliverer",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Tel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ServiceProvider = table.Column<string>(type: "text", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliverer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Families",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentFamilyId = table.Column<Guid>(type: "uuid", nullable: true),
                    Designation = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Families_Families_ParentFamilyId",
                        column: x => x.ParentFamilyId,
                        principalSchema: "app",
                        principalTable: "Families",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PaymentMean",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMean", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTerms",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaturityDuration = table.Column<int>(type: "integer", nullable: false),
                    MaturityDurationNegotiable = table.Column<bool>(type: "boolean", nullable: false),
                    DepositInvoice = table.Column<bool>(type: "boolean", nullable: false),
                    DepositInvoiceNegotiable = table.Column<bool>(type: "boolean", nullable: false),
                    DepositInvoiceDownPayment = table.Column<int>(type: "integer", nullable: false),
                    PaymentByInstallments = table.Column<bool>(type: "boolean", nullable: false),
                    PaymentByInstallmentsNegotiable = table.Column<bool>(type: "boolean", nullable: false),
                    PaymentByInstallmentsNumber = table.Column<int>(type: "integer", nullable: false),
                    PaymentOption = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTerms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceGroup",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductAvailability",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAvailability", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProviderCategory",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitTypes",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(125)", maxLength: 125, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VAT",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VAT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassValues",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: false),
                    ProductClassKeyId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassValues_ClassKeys_ProductClassKeyId",
                        column: x => x.ProductClassKeyId,
                        principalSchema: "app",
                        principalTable: "ClassKeys",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "text", nullable: false),
                    ClientType = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Tel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Fax = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Function = table.Column<string>(type: "text", nullable: true),
                    SocialReason = table.Column<string>(type: "text", nullable: true),
                    ICE = table.Column<string>(type: "text", nullable: true),
                    CompanyTel = table.Column<string>(type: "text", nullable: true),
                    CompanyFax = table.Column<string>(type: "text", nullable: true),
                    CompanyEmail = table.Column<string>(type: "text", nullable: true),
                    WebSite = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    CustomerCategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_CustomerCategory_CustomerCategoryId",
                        column: x => x.CustomerCategoryId,
                        principalSchema: "app",
                        principalTable: "CustomerCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "text", nullable: true),
                    LongDesignation = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ShortDesignation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2500)", maxLength: 2500, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    Action = table.Column<int>(type: "integer", nullable: true),
                    Barcode = table.Column<string>(type: "text", nullable: true),
                    ClassificationInfo_FamilyId = table.Column<Guid>(type: "uuid", nullable: true),
                    PurchaseInfo_Price = table.Column<decimal>(type: "numeric", nullable: true),
                    SellInfo_Currency = table.Column<string>(type: "text", nullable: true),
                    UnitInfo_IsBalance = table.Column<bool>(type: "boolean", nullable: false),
                    UnitInfo_IsDecimal = table.Column<bool>(type: "boolean", nullable: false),
                    StorageSupplyInfo_Quantity = table.Column<int>(type: "integer", nullable: true),
                    StorageSupplyInfo_MaximumStock = table.Column<int>(type: "integer", nullable: true),
                    StorageSupplyInfo_MinimumStock = table.Column<int>(type: "integer", nullable: true),
                    StorageSupplyInfo_SafetyStock = table.Column<int>(type: "integer", nullable: true),
                    StorageSupplyInfo_BatchSize = table.Column<int>(type: "integer", nullable: true),
                    StorageSupplyInfo_RetentionPeriod = table.Column<int>(type: "integer", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Families_ClassificationInfo_FamilyId",
                        column: x => x.ClassificationInfo_FamilyId,
                        principalSchema: "app",
                        principalTable: "Families",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Provider",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "text", nullable: false),
                    ProviderType = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Tel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Fax = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Function = table.Column<string>(type: "text", nullable: true),
                    SocialReason = table.Column<string>(type: "text", nullable: true),
                    ICE = table.Column<string>(type: "text", nullable: true),
                    CompanyTel = table.Column<string>(type: "text", nullable: true),
                    CompanyFax = table.Column<string>(type: "text", nullable: true),
                    CompanyEmail = table.Column<string>(type: "text", nullable: true),
                    WebSite = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    ProviderCategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Provider_ProviderCategory_ProviderCategoryId",
                        column: x => x.ProviderCategoryId,
                        principalSchema: "app",
                        principalTable: "ProviderCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UnitMeasurements",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(125)", maxLength: 125, nullable: false),
                    UnitTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitMeasurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitMeasurements_UnitTypes_UnitTypeId",
                        column: x => x.UnitTypeId,
                        principalSchema: "app",
                        principalTable: "UnitTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerAddress",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    AddressType = table.Column<int>(type: "integer", nullable: false),
                    StreetAdress = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ZipCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Region = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Localisation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Complement = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerAddress_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "app",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quotations",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "text", nullable: true),
                    QuotationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValidityDuration = table.Column<int>(type: "integer", nullable: true),
                    Responsable = table.Column<string>(type: "text", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Discount = table.Column<float>(type: "real", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PaymentTerms_MaturityDuration = table.Column<int>(type: "integer", nullable: true),
                    PaymentTerms_DepositInvoice = table.Column<bool>(type: "boolean", nullable: true),
                    PaymentTerms_DepositInvoiceDownPayment = table.Column<int>(type: "integer", nullable: true),
                    PaymentTerms_PaymentByInstallments = table.Column<bool>(type: "boolean", nullable: true),
                    PaymentTerms_PaymentByInstallmentsNumber = table.Column<int>(type: "integer", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveryMode = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotations_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "app",
                        principalTable: "Customer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductAssignedAvailabilities",
                schema: "app",
                columns: table => new
                {
                    ProductAvailabilityForeignKey = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductForeignKey = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAssignedAvailabilities", x => new { x.ProductAvailabilityForeignKey, x.ProductForeignKey });
                    table.ForeignKey(
                        name: "FK_ProductAssignedAvailabilities_ProductAvailability_ProductAv~",
                        column: x => x.ProductAvailabilityForeignKey,
                        principalSchema: "app",
                        principalTable: "ProductAvailability",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAssignedAvailabilities_Product_ProductForeignKey",
                        column: x => x.ProductForeignKey,
                        principalSchema: "app",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductClassValues",
                schema: "app",
                columns: table => new
                {
                    ClassValueForeignKey = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductForeignKey = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductClassValues", x => new { x.ClassValueForeignKey, x.ProductForeignKey });
                    table.ForeignKey(
                        name: "FK_ProductClassValues_ClassValues_ClassValueForeignKey",
                        column: x => x.ClassValueForeignKey,
                        principalSchema: "app",
                        principalTable: "ClassValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductClassValues_Product_ProductForeignKey",
                        column: x => x.ProductForeignKey,
                        principalSchema: "app",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImage",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImage_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "app",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductNote",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Note = table.Column<string>(type: "character varying(2500)", maxLength: 2500, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductNote_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "app",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SellingPrice",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PriceGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Margin = table.Column<double>(type: "double precision", nullable: true),
                    VAT = table.Column<int>(type: "integer", nullable: true),
                    Discount = table.Column<int>(type: "integer", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellingPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellingPrice_PriceGroup_PriceGroupId",
                        column: x => x.PriceGroupId,
                        principalSchema: "app",
                        principalTable: "PriceGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SellingPrice_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "app",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProviderAddress",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    AddressType = table.Column<int>(type: "integer", nullable: true),
                    StreetAdress = table.Column<string>(type: "text", nullable: true),
                    ZipCode = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Region = table.Column<string>(type: "text", nullable: true),
                    Localisation = table.Column<string>(type: "text", nullable: true),
                    Complement = table.Column<string>(type: "text", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderAddress_Provider_ProviderId",
                        column: x => x.ProviderId,
                        principalSchema: "app",
                        principalTable: "Provider",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "text", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Provider_ProviderId",
                        column: x => x.ProviderId,
                        principalSchema: "app",
                        principalTable: "Provider",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductUnitMeasurements",
                schema: "app",
                columns: table => new
                {
                    ProductForeignKey = table.Column<Guid>(type: "uuid", nullable: false),
                    UnitMeasurementForeignKey = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductUnitMeasurements", x => new { x.ProductForeignKey, x.UnitMeasurementForeignKey });
                    table.ForeignKey(
                        name: "FK_ProductUnitMeasurements_Product_ProductForeignKey",
                        column: x => x.ProductForeignKey,
                        principalSchema: "app",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductUnitMeasurements_UnitMeasurements_UnitMeasurementFor~",
                        column: x => x.UnitMeasurementForeignKey,
                        principalSchema: "app",
                        principalTable: "UnitMeasurements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleOrders",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationType = table.Column<int>(type: "integer", nullable: false),
                    Execution = table.Column<int>(type: "integer", nullable: false),
                    Reference = table.Column<string>(type: "text", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    SaleOrderOrigin = table.Column<int>(type: "integer", nullable: false),
                    QuotationId = table.Column<Guid>(type: "uuid", nullable: true),
                    Discount = table.Column<float>(type: "real", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    DeliveryMode = table.Column<int>(type: "integer", nullable: false),
                    SaleChannelId = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentTerms_MaturityDuration = table.Column<int>(type: "integer", nullable: true),
                    PaymentTerms_DepositInvoice = table.Column<bool>(type: "boolean", nullable: true),
                    PaymentTerms_DepositInvoiceDownPayment = table.Column<int>(type: "integer", nullable: true),
                    PaymentTerms_PaymentByInstallments = table.Column<bool>(type: "boolean", nullable: true),
                    PaymentTerms_PaymentByInstallmentsNumber = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleOrders_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "app",
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaleOrders_ProductAvailability_SaleChannelId",
                        column: x => x.SaleChannelId,
                        principalSchema: "app",
                        principalTable: "ProductAvailability",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaleOrders_Quotations_QuotationId",
                        column: x => x.QuotationId,
                        principalSchema: "app",
                        principalTable: "Quotations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuotationsLines",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellingPriceId = table.Column<Guid>(type: "uuid", nullable: true),
                    QuotationID = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: true),
                    Margin = table.Column<double>(type: "double precision", nullable: true),
                    VAT = table.Column<int>(type: "integer", nullable: true),
                    Discount = table.Column<int>(type: "integer", nullable: true),
                    Qty = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationsLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationsLines_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "app",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuotationsLines_Quotations_QuotationID",
                        column: x => x.QuotationID,
                        principalSchema: "app",
                        principalTable: "Quotations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuotationsLines_SellingPrice_SellingPriceId",
                        column: x => x.SellingPriceId,
                        principalSchema: "app",
                        principalTable: "SellingPrice",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProviderInvoices",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: false),
                    ProviderId = table.Column<Guid>(type: "uuid", nullable: true),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    CurrencyId = table.Column<Guid>(type: "uuid", nullable: true),
                    Discount = table.Column<float>(type: "real", nullable: true),
                    ProviderInvoiceOrigin = table.Column<int>(type: "integer", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    DirectCreationReason = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    DigitalInvoicePath = table.Column<string>(type: "text", nullable: true),
                    PaymentTerms_MaturityDuration = table.Column<int>(type: "integer", nullable: true),
                    PaymentTerms_DepositInvoice = table.Column<bool>(type: "boolean", nullable: true),
                    PaymentTerms_DepositInvoiceDownPayment = table.Column<int>(type: "integer", nullable: true),
                    PaymentTerms_PaymentByInstallments = table.Column<bool>(type: "boolean", nullable: true),
                    PaymentTerms_PaymentByInstallmentsNumber = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderInvoices_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "app",
                        principalTable: "Currency",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProviderInvoices_Provider_ProviderId",
                        column: x => x.ProviderId,
                        principalSchema: "app",
                        principalTable: "Provider",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProviderInvoices_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "app",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderLines",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: true),
                    VAT = table.Column<int>(type: "integer", nullable: true),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "app",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "app",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockEntries",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: false),
                    ProviderId = table.Column<Guid>(type: "uuid", nullable: true),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    StockEntryOrigin = table.Column<int>(type: "integer", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockEntries_Provider_ProviderId",
                        column: x => x.ProviderId,
                        principalSchema: "app",
                        principalTable: "Provider",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockEntries_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "app",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    SaleOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeliveryRef = table.Column<string>(type: "text", nullable: true),
                    CurrencyId = table.Column<Guid>(type: "uuid", nullable: true),
                    Discount = table.Column<float>(type: "real", nullable: true),
                    InvoiceOrigin = table.Column<int>(type: "integer", nullable: false),
                    InvoiceType = table.Column<int>(type: "integer", nullable: false),
                    PaymentTerms_MaturityDuration = table.Column<int>(type: "integer", nullable: true),
                    PaymentTerms_DepositInvoice = table.Column<bool>(type: "boolean", nullable: true),
                    PaymentTerms_DepositInvoiceDownPayment = table.Column<int>(type: "integer", nullable: true),
                    PaymentTerms_PaymentByInstallments = table.Column<bool>(type: "boolean", nullable: true),
                    PaymentTerms_PaymentByInstallmentsNumber = table.Column<int>(type: "integer", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveryStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveryEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    DirectCreationReason = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "app",
                        principalTable: "Currency",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoices_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "app",
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoices_SaleOrders_SaleOrderId",
                        column: x => x.SaleOrderId,
                        principalSchema: "app",
                        principalTable: "SaleOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SaleOrderLines",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellingPriceId = table.Column<Guid>(type: "uuid", nullable: true),
                    SaleOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: true),
                    Margin = table.Column<double>(type: "double precision", nullable: true),
                    VAT = table.Column<int>(type: "integer", nullable: true),
                    Discount = table.Column<int>(type: "integer", nullable: true),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleOrderLines_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "app",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleOrderLines_SaleOrders_SaleOrderId",
                        column: x => x.SaleOrderId,
                        principalSchema: "app",
                        principalTable: "SaleOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaleOrderLines_SellingPrice_SellingPriceId",
                        column: x => x.SellingPriceId,
                        principalSchema: "app",
                        principalTable: "SellingPrice",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalePayments",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: false),
                    SaleOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentMeanId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OperationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OperationNumber = table.Column<string>(type: "text", nullable: true),
                    Bank = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    AmountToBePaid = table.Column<double>(type: "double precision", nullable: true),
                    AmountToBePaidPercentage = table.Column<double>(type: "double precision", nullable: true),
                    AmountPaid = table.Column<double>(type: "double precision", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalePayments_PaymentMean_PaymentMeanId",
                        column: x => x.PaymentMeanId,
                        principalSchema: "app",
                        principalTable: "PaymentMean",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalePayments_SaleOrders_SaleOrderId",
                        column: x => x.SaleOrderId,
                        principalSchema: "app",
                        principalTable: "SaleOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProviderInvoiceLines",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProviderInvoiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    VAT = table.Column<int>(type: "integer", nullable: true),
                    Discount = table.Column<int>(type: "integer", nullable: true),
                    Qty = table.Column<int>(type: "integer", nullable: true),
                    ExpectedQty = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderInvoiceLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderInvoiceLines_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "app",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProviderInvoiceLines_ProviderInvoices_ProviderInvoiceId",
                        column: x => x.ProviderInvoiceId,
                        principalSchema: "app",
                        principalTable: "ProviderInvoices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockEntryLines",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    StockEntryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockEntryLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockEntryLines_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "app",
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockEntryLines_StockEntries_StockEntryId",
                        column: x => x.StockEntryId,
                        principalSchema: "app",
                        principalTable: "StockEntries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CreditNotes",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreditNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreditNoteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreditOrigin = table.Column<int>(type: "integer", nullable: false),
                    CreditAction = table.Column<int>(type: "integer", nullable: false),
                    Responsable = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Raison = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Note = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreditAmount = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditNotes_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "app",
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CreditNotes_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "app",
                        principalTable: "Invoices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    DelivererId = table.Column<Guid>(type: "uuid", nullable: true),
                    SaleOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeliveryOrigin = table.Column<int>(type: "integer", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PlannedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeliveryMode = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Situation = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deliveries_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "app",
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Deliveries_Deliverer_DelivererId",
                        column: x => x.DelivererId,
                        principalSchema: "app",
                        principalTable: "Deliverer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Deliveries_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "app",
                        principalTable: "Invoices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Deliveries_SaleOrders_SaleOrderId",
                        column: x => x.SaleOrderId,
                        principalSchema: "app",
                        principalTable: "SaleOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceLines",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellingPriceId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeliveryRef = table.Column<string>(type: "text", nullable: true),
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    VAT = table.Column<int>(type: "integer", nullable: false),
                    Discount = table.Column<int>(type: "integer", nullable: true),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    ExpectedQty = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceLines_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "app",
                        principalTable: "Invoices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceLines_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "app",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceLines_SellingPrice_SellingPriceId",
                        column: x => x.SellingPriceId,
                        principalSchema: "app",
                        principalTable: "SellingPrice",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InvoicePayments",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentMeanId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OperationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OperationNumber = table.Column<string>(type: "text", nullable: true),
                    Bank = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    AmountToBePaid = table.Column<double>(type: "double precision", nullable: true),
                    AmountToBePaidPercentage = table.Column<double>(type: "double precision", nullable: true),
                    AmountPaid = table.Column<double>(type: "double precision", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoicePayments_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "app",
                        principalTable: "Invoices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoicePayments_PaymentMean_PaymentMeanId",
                        column: x => x.PaymentMeanId,
                        principalSchema: "app",
                        principalTable: "PaymentMean",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditNoteLines",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellingPriceId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeliveryRef = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreditNoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    VAT = table.Column<int>(type: "integer", nullable: false),
                    Discount = table.Column<int>(type: "integer", nullable: true),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    ExpectedQty = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditNoteLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditNoteLines_CreditNotes_CreditNoteId",
                        column: x => x.CreditNoteId,
                        principalSchema: "app",
                        principalTable: "CreditNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditNoteLines_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "app",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditNoteLines_SellingPrice_SellingPriceId",
                        column: x => x.SellingPriceId,
                        principalSchema: "app",
                        principalTable: "SellingPrice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryLines",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellingPriceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeliveryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    ExpectedQty = table.Column<int>(type: "integer", nullable: true),
                    UnitPrice = table.Column<double>(type: "double precision", nullable: false),
                    Discount = table.Column<int>(type: "integer", nullable: true),
                    VAT = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryLines_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalSchema: "app",
                        principalTable: "Deliveries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeliveryLines_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "app",
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeliveryLines_SellingPrice_SellingPriceId",
                        column: x => x.SellingPriceId,
                        principalSchema: "app",
                        principalTable: "SellingPrice",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "app",
                table: "Currency",
                columns: new[] { "Id", "Created", "CreatedBy", "IsDefault", "IsDeleted", "LastModified", "LastModifiedBy", "Name" },
                values: new object[] { new Guid("26bad4d4-6575-4dd1-9aaa-8d84c51143f9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, null, null, "MAD" });

            migrationBuilder.InsertData(
                schema: "app",
                table: "PaymentMean",
                columns: new[] { "Id", "Created", "CreatedBy", "IsDeleted", "LastModified", "LastModifiedBy", "Name" },
                values: new object[,]
                {
                    { new Guid("0eb5afc3-56b8-4bfd-9e05-ae373b17be95"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, "Chèque" },
                    { new Guid("226cf844-31b0-47c1-b893-52240622ec48"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, "Carte bancaire" },
                    { new Guid("9f933b02-3984-444c-9e65-32910b55fc92"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, "Virement" }
                });

            migrationBuilder.InsertData(
                schema: "app",
                table: "PaymentTerms",
                columns: new[] { "Id", "Created", "CreatedBy", "DepositInvoice", "DepositInvoiceDownPayment", "DepositInvoiceNegotiable", "IsDeleted", "LastModified", "LastModifiedBy", "MaturityDuration", "MaturityDurationNegotiable", "PaymentByInstallments", "PaymentByInstallmentsNegotiable", "PaymentByInstallmentsNumber", "PaymentOption" },
                values: new object[] { new Guid("dcf10cc7-e493-4431-b8b2-5615f25c1912"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, 10, true, false, null, null, 30, true, true, true, 3, 0 });

            migrationBuilder.InsertData(
                schema: "app",
                table: "VAT",
                columns: new[] { "Id", "Created", "CreatedBy", "IsDefault", "IsDeleted", "LastModified", "LastModifiedBy", "Value" },
                values: new object[] { new Guid("76fa42f5-04bb-4d63-94af-fb625d4ded36"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, null, null, 0.02m });

            migrationBuilder.CreateIndex(
                name: "IX_ClassValues_ProductClassKeyId",
                schema: "app",
                table: "ClassValues",
                column: "ProductClassKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteLines_CreditNoteId",
                schema: "app",
                table: "CreditNoteLines",
                column: "CreditNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteLines_ProductId",
                schema: "app",
                table: "CreditNoteLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteLines_SellingPriceId",
                schema: "app",
                table: "CreditNoteLines",
                column: "SellingPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNotes_CustomerId",
                schema: "app",
                table: "CreditNotes",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNotes_InvoiceId",
                schema: "app",
                table: "CreditNotes",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerCategoryId",
                schema: "app",
                table: "Customer",
                column: "CustomerCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_CustomerId",
                schema: "app",
                table: "CustomerAddress",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_CustomerId",
                schema: "app",
                table: "Deliveries",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DelivererId",
                schema: "app",
                table: "Deliveries",
                column: "DelivererId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_InvoiceId",
                schema: "app",
                table: "Deliveries",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_SaleOrderId",
                schema: "app",
                table: "Deliveries",
                column: "SaleOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryLines_DeliveryId",
                schema: "app",
                table: "DeliveryLines",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryLines_ProductId",
                schema: "app",
                table: "DeliveryLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryLines_SellingPriceId",
                schema: "app",
                table: "DeliveryLines",
                column: "SellingPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_ParentFamilyId",
                schema: "app",
                table: "Families",
                column: "ParentFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_InvoiceId",
                schema: "app",
                table: "InvoiceLines",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_ProductId",
                schema: "app",
                table: "InvoiceLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_SellingPriceId",
                schema: "app",
                table: "InvoiceLines",
                column: "SellingPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePayments_InvoiceId",
                schema: "app",
                table: "InvoicePayments",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePayments_PaymentMeanId",
                schema: "app",
                table: "InvoicePayments",
                column: "PaymentMeanId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CurrencyId",
                schema: "app",
                table: "Invoices",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerId",
                schema: "app",
                table: "Invoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SaleOrderId",
                schema: "app",
                table: "Invoices",
                column: "SaleOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ClassificationInfo_FamilyId",
                schema: "app",
                table: "Product",
                column: "ClassificationInfo_FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedAvailabilities_ProductForeignKey",
                schema: "app",
                table: "ProductAssignedAvailabilities",
                column: "ProductForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_ProductClassValues_ProductForeignKey",
                schema: "app",
                table: "ProductClassValues",
                column: "ProductForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductId",
                schema: "app",
                table: "ProductImage",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductNote_ProductId",
                schema: "app",
                table: "ProductNote",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUnitMeasurements_UnitMeasurementForeignKey",
                schema: "app",
                table: "ProductUnitMeasurements",
                column: "UnitMeasurementForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_Provider_ProviderCategoryId",
                schema: "app",
                table: "Provider",
                column: "ProviderCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderAddress_ProviderId",
                schema: "app",
                table: "ProviderAddress",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderInvoiceLines_ProductId",
                schema: "app",
                table: "ProviderInvoiceLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderInvoiceLines_ProviderInvoiceId",
                schema: "app",
                table: "ProviderInvoiceLines",
                column: "ProviderInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderInvoices_CurrencyId",
                schema: "app",
                table: "ProviderInvoices",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderInvoices_ProviderId",
                schema: "app",
                table: "ProviderInvoices",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderInvoices_PurchaseOrderId",
                schema: "app",
                table: "ProviderInvoices",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_ProductId",
                schema: "app",
                table: "PurchaseOrderLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_PurchaseOrderId",
                schema: "app",
                table: "PurchaseOrderLines",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ProviderId",
                schema: "app",
                table: "PurchaseOrders",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_CustomerId",
                schema: "app",
                table: "Quotations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationsLines_ProductId",
                schema: "app",
                table: "QuotationsLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationsLines_QuotationID",
                schema: "app",
                table: "QuotationsLines",
                column: "QuotationID");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationsLines_SellingPriceId",
                schema: "app",
                table: "QuotationsLines",
                column: "SellingPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleOrderLines_ProductId",
                schema: "app",
                table: "SaleOrderLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleOrderLines_SaleOrderId",
                schema: "app",
                table: "SaleOrderLines",
                column: "SaleOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleOrderLines_SellingPriceId",
                schema: "app",
                table: "SaleOrderLines",
                column: "SellingPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleOrders_CustomerId",
                schema: "app",
                table: "SaleOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleOrders_QuotationId",
                schema: "app",
                table: "SaleOrders",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleOrders_SaleChannelId",
                schema: "app",
                table: "SaleOrders",
                column: "SaleChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_SalePayments_PaymentMeanId",
                schema: "app",
                table: "SalePayments",
                column: "PaymentMeanId");

            migrationBuilder.CreateIndex(
                name: "IX_SalePayments_SaleOrderId",
                schema: "app",
                table: "SalePayments",
                column: "SaleOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingPrice_PriceGroupId",
                schema: "app",
                table: "SellingPrice",
                column: "PriceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingPrice_ProductId",
                schema: "app",
                table: "SellingPrice",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockEntries_ProviderId",
                schema: "app",
                table: "StockEntries",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockEntries_PurchaseOrderId",
                schema: "app",
                table: "StockEntries",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockEntryLines_ProductId",
                schema: "app",
                table: "StockEntryLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockEntryLines_StockEntryId",
                schema: "app",
                table: "StockEntryLines",
                column: "StockEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitMeasurements_UnitTypeId",
                schema: "app",
                table: "UnitMeasurements",
                column: "UnitTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditNoteLines",
                schema: "app");

            migrationBuilder.DropTable(
                name: "CustomerAddress",
                schema: "app");

            migrationBuilder.DropTable(
                name: "DeliveryLines",
                schema: "app");

            migrationBuilder.DropTable(
                name: "InvoiceLines",
                schema: "app");

            migrationBuilder.DropTable(
                name: "InvoicePayments",
                schema: "app");

            migrationBuilder.DropTable(
                name: "PaymentTerms",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ProductAssignedAvailabilities",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ProductClassValues",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ProductImage",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ProductNote",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ProductUnitMeasurements",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ProviderAddress",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ProviderInvoiceLines",
                schema: "app");

            migrationBuilder.DropTable(
                name: "PurchaseOrderLines",
                schema: "app");

            migrationBuilder.DropTable(
                name: "QuotationsLines",
                schema: "app");

            migrationBuilder.DropTable(
                name: "SaleOrderLines",
                schema: "app");

            migrationBuilder.DropTable(
                name: "SalePayments",
                schema: "app");

            migrationBuilder.DropTable(
                name: "StockEntryLines",
                schema: "app");

            migrationBuilder.DropTable(
                name: "VAT",
                schema: "app");

            migrationBuilder.DropTable(
                name: "CreditNotes",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Deliveries",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ClassValues",
                schema: "app");

            migrationBuilder.DropTable(
                name: "UnitMeasurements",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ProviderInvoices",
                schema: "app");

            migrationBuilder.DropTable(
                name: "SellingPrice",
                schema: "app");

            migrationBuilder.DropTable(
                name: "PaymentMean",
                schema: "app");

            migrationBuilder.DropTable(
                name: "StockEntries",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Deliverer",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Invoices",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ClassKeys",
                schema: "app");

            migrationBuilder.DropTable(
                name: "UnitTypes",
                schema: "app");

            migrationBuilder.DropTable(
                name: "PriceGroup",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "app");

            migrationBuilder.DropTable(
                name: "PurchaseOrders",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Currency",
                schema: "app");

            migrationBuilder.DropTable(
                name: "SaleOrders",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Families",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Provider",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ProductAvailability",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Quotations",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ProviderCategory",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "app");

            migrationBuilder.DropTable(
                name: "CustomerCategory",
                schema: "app");
        }
    }
}
