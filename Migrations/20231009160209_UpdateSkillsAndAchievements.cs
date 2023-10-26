using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSkillsAndAchievements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Achievements_CVs_CVId",
                table: "Achievements");

            migrationBuilder.DropForeignKey(
                name: "FK_Achievements_Jobs_JobId",
                table: "Achievements");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_CVs_CVId",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Jobs_JobId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_CVId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Achievements_CVId",
                table: "Achievements");

            migrationBuilder.DropColumn(
                name: "CVId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CVId",
                table: "Achievements");

            migrationBuilder.AlterColumn<int>(
                name: "JobId",
                table: "Skills",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JobId",
                table: "Achievements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Achievements_Jobs_JobId",
                table: "Achievements",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Jobs_JobId",
                table: "Skills",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Achievements_Jobs_JobId",
                table: "Achievements");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Jobs_JobId",
                table: "Skills");

            migrationBuilder.AlterColumn<int>(
                name: "JobId",
                table: "Skills",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CVId",
                table: "Skills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "JobId",
                table: "Achievements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CVId",
                table: "Achievements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CVId",
                table: "Skills",
                column: "CVId");

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_CVId",
                table: "Achievements",
                column: "CVId");

            migrationBuilder.AddForeignKey(
                name: "FK_Achievements_CVs_CVId",
                table: "Achievements",
                column: "CVId",
                principalTable: "CVs",
                principalColumn: "CVId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Achievements_Jobs_JobId",
                table: "Achievements",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_CVs_CVId",
                table: "Skills",
                column: "CVId",
                principalTable: "CVs",
                principalColumn: "CVId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Jobs_JobId",
                table: "Skills",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId");
        }
    }
}
