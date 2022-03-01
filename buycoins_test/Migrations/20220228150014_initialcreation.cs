using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace buycoins_test.Migrations
{
    public partial class initialcreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserBankInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountName = table.Column<string>(type: "TEXT", nullable: false),
                    BankCode = table.Column<string>(type: "TEXT", nullable: false),
                    IsVerified = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccountNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBankInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBankInfos");
        }
    }
}
