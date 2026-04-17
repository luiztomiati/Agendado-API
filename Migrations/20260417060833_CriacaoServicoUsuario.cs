using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agendado.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoServicoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServicoUsuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServicoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServicosId = table.Column<Guid>(type: "uuid", nullable: false),
                    DtInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DtAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicoUsuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicoUsuarios_Servicos_ServicosId",
                        column: x => x.ServicosId,
                        principalTable: "Servicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicoUsuarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServicoUsuarios_ServicosId",
                table: "ServicoUsuarios",
                column: "ServicosId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoUsuarios_UsuarioId",
                table: "ServicoUsuarios",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServicoUsuarios");
        }
    }
}
