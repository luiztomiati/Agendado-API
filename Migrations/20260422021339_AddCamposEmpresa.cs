using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agendado.Migrations
{
    /// <inheritdoc />
    public partial class AddCamposEmpresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Empresas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Empresas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Logradouro",
                table: "Empresas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Uf",
                table: "Empresas",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Logradouro",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Uf",
                table: "Empresas");
        }
    }
}
