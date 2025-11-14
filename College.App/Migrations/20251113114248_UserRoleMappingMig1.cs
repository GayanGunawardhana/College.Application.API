using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace College.App.Migrations
{
    /// <inheritdoc />
    public partial class UserRoleMappingMig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "Users",
                newName: "UserTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserTypeId",
                table: "Users",
                newName: "UserType");
        }
    }
}
