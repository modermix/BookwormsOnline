using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace BookwormsOnline.Migrations
{
    /// <inheritdoc />
    public partial class Models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreditCardNo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    MobileNo = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false),
                    BillingAddress = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    ShippingAddress = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProfileImage = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
