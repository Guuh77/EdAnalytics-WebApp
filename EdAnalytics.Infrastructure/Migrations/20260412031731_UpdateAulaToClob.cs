using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EdAnalytics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAulaToClob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Conteudo",
                table: "Aulas");

            migrationBuilder.AddColumn<string>(
                name: "Conteudo",
                table: "Aulas",
                type: "CLOB",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Conteudo",
                table: "Aulas");

            migrationBuilder.AddColumn<string>(
                name: "Conteudo",
                table: "Aulas",
                type: "NVARCHAR2(2000)",
                nullable: false,
                defaultValue: "");
        }
    }
}
