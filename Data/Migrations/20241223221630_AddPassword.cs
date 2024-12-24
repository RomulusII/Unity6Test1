using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Chest_ChestId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_UnitJobs_UnitJobsId",
                table: "Units");

            migrationBuilder.AlterColumn<int>(
                name: "UnitJobsId",
                table: "Units",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ChestId",
                table: "Units",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCommunicationTime",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Chest_ChestId",
                table: "Units",
                column: "ChestId",
                principalTable: "Chest",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_UnitJobs_UnitJobsId",
                table: "Units",
                column: "UnitJobsId",
                principalTable: "UnitJobs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Chest_ChestId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_UnitJobs_UnitJobsId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "LastCommunicationTime",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Players");

            migrationBuilder.AlterColumn<int>(
                name: "UnitJobsId",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChestId",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Chest_ChestId",
                table: "Units",
                column: "ChestId",
                principalTable: "Chest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_UnitJobs_UnitJobsId",
                table: "Units",
                column: "UnitJobsId",
                principalTable: "UnitJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
