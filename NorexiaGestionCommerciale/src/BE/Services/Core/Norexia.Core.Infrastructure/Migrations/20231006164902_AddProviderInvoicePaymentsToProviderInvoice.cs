using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Norexia.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderInvoicePaymentsToProviderInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "ProviderInvoicePayments",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: false),
                    ProviderInvoiceId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_ProviderInvoicePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderInvoicePayments_PaymentMean_PaymentMeanId",
                        column: x => x.PaymentMeanId,
                        principalSchema: "app",
                        principalTable: "PaymentMean",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProviderInvoicePayments_ProviderInvoices_ProviderInvoiceId",
                        column: x => x.ProviderInvoiceId,
                        principalSchema: "app",
                        principalTable: "ProviderInvoices",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "app",
                table: "Currency",
                columns: new[] { "Id", "Created", "CreatedBy", "IsDefault", "IsDeleted", "LastModified", "LastModifiedBy", "Name" },
                values: new object[] { new Guid("f3a7f024-7a5c-4b88-b40f-de26a1523eae"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, null, null, "MAD" });

            migrationBuilder.InsertData(
                schema: "app",
                table: "PaymentMean",
                columns: new[] { "Id", "Created", "CreatedBy", "IsDeleted", "LastModified", "LastModifiedBy", "Name" },
                values: new object[,]
                {
                    { new Guid("6a3e5148-dad3-4a27-afcf-65a4dc7001dc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, "Chèque" },
                    { new Guid("8e480f5a-e01a-49bf-a173-b98e1d79660d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, "Carte bancaire" },
                    { new Guid("c14558f1-e20a-42e1-9fee-68fadee9b1ac"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, "Virement" }
                });

            migrationBuilder.InsertData(
                schema: "app",
                table: "PaymentTerms",
                columns: new[] { "Id", "Created", "CreatedBy", "DepositInvoice", "DepositInvoiceDownPayment", "DepositInvoiceNegotiable", "IsDeleted", "LastModified", "LastModifiedBy", "MaturityDuration", "MaturityDurationNegotiable", "PaymentByInstallments", "PaymentByInstallmentsNegotiable", "PaymentByInstallmentsNumber", "PaymentOption" },
                values: new object[] { new Guid("6a776e74-305c-4fcc-a8f1-8d618a6bb014"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, 10, true, false, null, null, 30, true, true, true, 3, 0 });

            migrationBuilder.InsertData(
                schema: "app",
                table: "VAT",
                columns: new[] { "Id", "Created", "CreatedBy", "IsDefault", "IsDeleted", "LastModified", "LastModifiedBy", "Value" },
                values: new object[] { new Guid("be9194d2-4742-4ef6-a357-226f10920878"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, null, null, 0.02m });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderInvoicePayments_PaymentMeanId",
                schema: "app",
                table: "ProviderInvoicePayments",
                column: "PaymentMeanId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderInvoicePayments_ProviderInvoiceId",
                schema: "app",
                table: "ProviderInvoicePayments",
                column: "ProviderInvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProviderInvoicePayments",
                schema: "app");

            migrationBuilder.DeleteData(
                schema: "app",
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("f3a7f024-7a5c-4b88-b40f-de26a1523eae"));

            migrationBuilder.DeleteData(
                schema: "app",
                table: "PaymentMean",
                keyColumn: "Id",
                keyValue: new Guid("6a3e5148-dad3-4a27-afcf-65a4dc7001dc"));

            migrationBuilder.DeleteData(
                schema: "app",
                table: "PaymentMean",
                keyColumn: "Id",
                keyValue: new Guid("8e480f5a-e01a-49bf-a173-b98e1d79660d"));

            migrationBuilder.DeleteData(
                schema: "app",
                table: "PaymentMean",
                keyColumn: "Id",
                keyValue: new Guid("c14558f1-e20a-42e1-9fee-68fadee9b1ac"));

            migrationBuilder.DeleteData(
                schema: "app",
                table: "PaymentTerms",
                keyColumn: "Id",
                keyValue: new Guid("6a776e74-305c-4fcc-a8f1-8d618a6bb014"));

            migrationBuilder.DeleteData(
                schema: "app",
                table: "VAT",
                keyColumn: "Id",
                keyValue: new Guid("be9194d2-4742-4ef6-a357-226f10920878"));

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
        }
    }
}
