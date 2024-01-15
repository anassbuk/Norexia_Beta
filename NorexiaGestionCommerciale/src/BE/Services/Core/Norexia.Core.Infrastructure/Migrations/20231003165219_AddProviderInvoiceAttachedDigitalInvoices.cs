using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Norexia.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderInvoiceAttachedDigitalInvoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "app",
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("26bad4d4-6575-4dd1-9aaa-8d84c51143f9"));

            migrationBuilder.DeleteData(
                schema: "app",
                table: "PaymentMean",
                keyColumn: "Id",
                keyValue: new Guid("0eb5afc3-56b8-4bfd-9e05-ae373b17be95"));

            migrationBuilder.DeleteData(
                schema: "app",
                table: "PaymentMean",
                keyColumn: "Id",
                keyValue: new Guid("226cf844-31b0-47c1-b893-52240622ec48"));

            migrationBuilder.DeleteData(
                schema: "app",
                table: "PaymentMean",
                keyColumn: "Id",
                keyValue: new Guid("9f933b02-3984-444c-9e65-32910b55fc92"));

            migrationBuilder.DeleteData(
                schema: "app",
                table: "PaymentTerms",
                keyColumn: "Id",
                keyValue: new Guid("dcf10cc7-e493-4431-b8b2-5615f25c1912"));

            migrationBuilder.DeleteData(
                schema: "app",
                table: "VAT",
                keyColumn: "Id",
                keyValue: new Guid("76fa42f5-04bb-4d63-94af-fb625d4ded36"));

            migrationBuilder.DropColumn(
                name: "DigitalInvoicePath",
                schema: "app",
                table: "ProviderInvoices");

            migrationBuilder.CreateTable(
                name: "AttachedDigitalInvoice",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Label = table.Column<string>(type: "text", nullable: false),
                    Path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ProviderInvoiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachedDigitalInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachedDigitalInvoice_ProviderInvoices_ProviderInvoiceId",
                        column: x => x.ProviderInvoiceId,
                        principalSchema: "app",
                        principalTable: "ProviderInvoices",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "app",
                table: "Currency",
                columns: new[] { "Id", "Created", "CreatedBy", "IsDefault", "IsDeleted", "LastModified", "LastModifiedBy", "Name" },
                values: new object[] { new Guid("5f4199e0-20a2-46f3-a64b-6d028dc5d798"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, null, null, "MAD" });

            migrationBuilder.InsertData(
                schema: "app",
                table: "PaymentMean",
                columns: new[] { "Id", "Created", "CreatedBy", "IsDeleted", "LastModified", "LastModifiedBy", "Name" },
                values: new object[,]
                {
                    { new Guid("54c62697-9fbe-48f4-8cc8-80fcab8caeda"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, "Carte bancaire" },
                    { new Guid("79b0ae9d-3aa0-4a8c-9884-d8fc4c6ac4ae"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, "Virement" },
                    { new Guid("9f6a4fba-4ae1-4ae6-bb6a-61cf84736095"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, "Chèque" }
                });

            migrationBuilder.InsertData(
                schema: "app",
                table: "PaymentTerms",
                columns: new[] { "Id", "Created", "CreatedBy", "DepositInvoice", "DepositInvoiceDownPayment", "DepositInvoiceNegotiable", "IsDeleted", "LastModified", "LastModifiedBy", "MaturityDuration", "MaturityDurationNegotiable", "PaymentByInstallments", "PaymentByInstallmentsNegotiable", "PaymentByInstallmentsNumber", "PaymentOption" },
                values: new object[] { new Guid("b69996f2-aaa6-4913-9f74-67bdd5491872"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, 10, true, false, null, null, 30, true, true, true, 3, 0 });

            migrationBuilder.InsertData(
                schema: "app",
                table: "VAT",
                columns: new[] { "Id", "Created", "CreatedBy", "IsDefault", "IsDeleted", "LastModified", "LastModifiedBy", "Value" },
                values: new object[] { new Guid("14148d5e-830d-418e-b1c8-00c357686a50"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, null, null, 0.02m });

            migrationBuilder.CreateIndex(
                name: "IX_AttachedDigitalInvoice_ProviderInvoiceId",
                schema: "app",
                table: "AttachedDigitalInvoice",
                column: "ProviderInvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachedDigitalInvoice",
                schema: "app");

            migrationBuilder.DeleteData(
                schema: "app",
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("5f4199e0-20a2-46f3-a64b-6d028dc5d798"));

            migrationBuilder.DeleteData(
                schema: "app",
                table: "PaymentMean",
                keyColumn: "Id",
                keyValue: new Guid("54c62697-9fbe-48f4-8cc8-80fcab8caeda"));

            migrationBuilder.DeleteData(
                schema: "app",
                table: "PaymentMean",
                keyColumn: "Id",
                keyValue: new Guid("79b0ae9d-3aa0-4a8c-9884-d8fc4c6ac4ae"));

            migrationBuilder.DeleteData(
                schema: "app",
                table: "PaymentMean",
                keyColumn: "Id",
                keyValue: new Guid("9f6a4fba-4ae1-4ae6-bb6a-61cf84736095"));

            migrationBuilder.DeleteData(
                schema: "app",
                table: "PaymentTerms",
                keyColumn: "Id",
                keyValue: new Guid("b69996f2-aaa6-4913-9f74-67bdd5491872"));

            migrationBuilder.DeleteData(
                schema: "app",
                table: "VAT",
                keyColumn: "Id",
                keyValue: new Guid("14148d5e-830d-418e-b1c8-00c357686a50"));

            migrationBuilder.AddColumn<string>(
                name: "DigitalInvoicePath",
                schema: "app",
                table: "ProviderInvoices",
                type: "text",
                nullable: true);

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
        }
    }
}
