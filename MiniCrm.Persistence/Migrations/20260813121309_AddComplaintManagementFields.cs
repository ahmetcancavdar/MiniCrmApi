using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCrm.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddComplaintManagementFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ========================================================
            // AdminResponse -> AdminNote
            // Eski veriyi kaybetmeden kolon adını değiştiriyoruz.
            // ========================================================

            migrationBuilder.RenameColumn(
                name: "AdminResponse",
                table: "Complaints",
                newName: "AdminNote");


            // ========================================================
            // AdminNote max length
            // Eski AdminResponse nvarchar(4000),
            // yeni model AdminNote nvarchar(2000).
            // ========================================================

            migrationBuilder.AlterColumn<string>(
                name: "AdminNote",
                table: "Complaints",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);


            // ========================================================
            // REJECT DATE
            // ========================================================

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedAtUtc",
                table: "Complaints",
                type: "datetime2",
                nullable: true);


            // ========================================================
            // STATUS INDEX
            // ========================================================

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_Status",
                table: "Complaints",
                column: "Status");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // ========================================================
            // STATUS INDEX
            // ========================================================

            migrationBuilder.DropIndex(
                name: "IX_Complaints_Status",
                table: "Complaints");


            // ========================================================
            // REJECT DATE
            // ========================================================

            migrationBuilder.DropColumn(
                name: "RejectedAtUtc",
                table: "Complaints");


            // ========================================================
            // AdminNote tekrar nvarchar(4000)
            // ========================================================

            migrationBuilder.AlterColumn<string>(
                name: "AdminNote",
                table: "Complaints",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);


            // ========================================================
            // AdminNote -> AdminResponse
            // ========================================================

            migrationBuilder.RenameColumn(
                name: "AdminNote",
                table: "Complaints",
                newName: "AdminResponse");
        }
    }
}