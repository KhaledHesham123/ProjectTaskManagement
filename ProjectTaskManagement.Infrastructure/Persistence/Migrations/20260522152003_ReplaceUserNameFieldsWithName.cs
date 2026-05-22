using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTaskManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceUserNameFieldsWithName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "First_Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Last_Name",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Full_Name",
                table: "AspNetUsers",
                newName: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Permissions_Name",
                table: "Permissions",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_Permission",
                table: "UserPermissions",
                column: "Permission");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissions_Permissions_Permission",
                table: "UserPermissions",
                column: "Permission",
                principalTable: "Permissions",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_Permissions_Permission",
                table: "UserPermissions");

            migrationBuilder.DropIndex(
                name: "IX_UserPermissions_Permission",
                table: "UserPermissions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Permissions_Name",
                table: "Permissions");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUsers",
                newName: "Full_Name");

            migrationBuilder.AddColumn<string>(
                name: "First_Name",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Last_Name",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
