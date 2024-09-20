using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cafe.Dal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersDataTg",
                columns: table => new
                {
                    Chat_Id = table.Column<long>(type: "bigint", nullable: false),
                    Count = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDataTg", x => x.Chat_Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersDataTg");
        }
    }
}
