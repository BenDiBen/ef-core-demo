using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfCoreDemo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name_First = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name_Last = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name_MiddleNames = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Addresses_PostalInternal_Suburb = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Addresses_PostalInternal_Street_FirstLine = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Addresses_PostalInternal_Street_SecondLine = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Addresses_PostalInternal_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Addresses_PostalInternal_PostalCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Addresses_PostalInternal_Province = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Addresses_Residential_Suburb = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Addresses_Residential_Street_FirstLine = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Addresses_Residential_Street_SecondLine = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Addresses_Residential_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Addresses_Residential_PostalCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Addresses_Residential_Province = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ContactDetails_PhoneNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    ContactDetails_Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactDetails_WorkNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    ContactDetails_AlternateNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    DemographicInfo_DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    DemographicInfo_Gender = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    DemographicInfo_PreferredLanguage = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    MarketingPreferences_AcceptsMarketingEmails = table.Column<bool>(type: "bit", nullable: false),
                    MarketingPreferences_AcceptsSmsNotifications = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OverdraftLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.UniqueConstraint("AK_Accounts_Number", x => x.Number);
                    table.ForeignKey(
                        name: "FK_Accounts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DebitedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreditedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DebtorReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditorReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Requested = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Processed = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_CreditedAccountId",
                        column: x => x.CreditedAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_DebitedAccountId",
                        column: x => x.DebitedAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CustomerId",
                table: "Accounts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreditedAccountId",
                table: "Transactions",
                column: "CreditedAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DebitedAccountId",
                table: "Transactions",
                column: "DebitedAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
