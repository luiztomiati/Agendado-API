using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agendado.Migrations
{
    /// <inheritdoc />
    public partial class AlteracaoTabelaEmpresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Empresas");

            migrationBuilder.RenameColumn(
                name: "Logradouro",
                table: "Empresas",
                newName: "Cep");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cep",
                table: "Empresas",
                newName: "Logradouro");

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
                name: "Estado",
                table: "Empresas",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
