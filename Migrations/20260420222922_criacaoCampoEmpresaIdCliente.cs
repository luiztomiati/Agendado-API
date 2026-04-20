using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agendado.Migrations
{
    /// <inheritdoc />
    public partial class criacaoCampoEmpresaIdCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmpresaId",
                table: "Cliente",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_EmpresaId",
                table: "Cliente",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Empresas_EmpresaId",
                table: "Cliente",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Empresas_EmpresaId",
                table: "Cliente");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_EmpresaId",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Cliente");
        }
    }
}
