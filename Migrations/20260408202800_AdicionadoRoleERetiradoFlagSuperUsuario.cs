using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agendado.Migrations
{
    /// <inheritdoc />
    public partial class AdicionadoRoleERetiradoFlagSuperUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SuperUsuario",
                table: "Usuarios");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Usuarios");

            migrationBuilder.AddColumn<bool>(
                name: "SuperUsuario",
                table: "Usuarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
