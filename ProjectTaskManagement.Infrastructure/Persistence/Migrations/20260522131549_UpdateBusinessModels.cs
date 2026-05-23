using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTaskManagement.Infrastructure.Persistence.Migrations
{
    public partial class UpdateBusinessModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_Project_Id",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Assigned_To_User_Id",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "End_Date",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Modified_At",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Owner_User_Id",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "Project_Id",
                table: "Tasks",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "Modified_By",
                table: "Tasks",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "Modified_At",
                table: "Tasks",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "Is_Deleted",
                table: "Tasks",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Due_Date",
                table: "Tasks",
                newName: "DueDate");

            migrationBuilder.RenameColumn(
                name: "Created_By",
                table: "Tasks",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "Tasks",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_Project_Id",
                table: "Tasks",
                newName: "IX_Tasks_ProjectId");

            migrationBuilder.RenameColumn(
                name: "Start_Date",
                table: "Projects",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "Modified_By",
                table: "Projects",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "Is_Deleted",
                table: "Projects",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Created_By",
                table: "Projects",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "Projects",
                newName: "CreatedAt");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Tasks",
                newName: "Project_Id");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "Tasks",
                newName: "Modified_By");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Tasks",
                newName: "Modified_At");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Tasks",
                newName: "Is_Deleted");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Tasks",
                newName: "Due_Date");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Tasks",
                newName: "Created_By");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Tasks",
                newName: "Created_At");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                newName: "IX_Tasks_Project_Id");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "Projects",
                newName: "Modified_By");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Projects",
                newName: "Start_Date");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Projects",
                newName: "Is_Deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Projects",
                newName: "Created_By");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Projects",
                newName: "Created_At");

            migrationBuilder.AddColumn<string>(
                name: "Assigned_To_User_Id",
                table: "Tasks",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "End_Date",
                table: "Projects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified_At",
                table: "Projects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner_User_Id",
                table: "Projects",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_Project_Id",
                table: "Tasks",
                column: "Project_Id",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
