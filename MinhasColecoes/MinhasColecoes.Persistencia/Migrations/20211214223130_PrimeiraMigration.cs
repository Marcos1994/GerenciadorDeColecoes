using Microsoft.EntityFrameworkCore.Migrations;

namespace MinhasColecoes.Persistencia.Migrations
{
    public partial class PrimeiraMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Colecoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdColecaoMaior = table.Column<int>(type: "int", nullable: true),
                    IdDono = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Publica = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colecoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Colecoes_Colecoes_IdColecaoMaior",
                        column: x => x.IdColecaoMaior,
                        principalTable: "Colecoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Colecoes_Usuarios_IdDono",
                        column: x => x.IdDono,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ColecoesUsuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdColecao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColecoesUsuario", x => new { x.IdColecao, x.IdUsuario });
                    table.ForeignKey(
                        name: "FK_ColecoesUsuario_Colecoes_IdColecao",
                        column: x => x.IdColecao,
                        principalTable: "Colecoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ColecoesUsuario_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Itens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdColecao = table.Column<int>(type: "int", nullable: false),
                    Original = table.Column<bool>(type: "bit", nullable: false),
                    IdOriginal = table.Column<int>(type: "int", nullable: true),
                    IdDonoParticular = table.Column<int>(type: "int", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Itens_Colecoes_IdColecao",
                        column: x => x.IdColecao,
                        principalTable: "Colecoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Itens_Itens_IdOriginal",
                        column: x => x.IdOriginal,
                        principalTable: "Itens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Itens_Usuarios_IdDonoParticular",
                        column: x => x.IdDonoParticular,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensUsuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdItem = table.Column<int>(type: "int", nullable: false),
                    Relacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensUsuario", x => new { x.IdItem, x.IdUsuario });
                    table.ForeignKey(
                        name: "FK_ItensUsuario_Itens_IdItem",
                        column: x => x.IdItem,
                        principalTable: "Itens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensUsuario_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Colecoes_IdColecaoMaior",
                table: "Colecoes",
                column: "IdColecaoMaior");

            migrationBuilder.CreateIndex(
                name: "IX_Colecoes_IdDono",
                table: "Colecoes",
                column: "IdDono");

            migrationBuilder.CreateIndex(
                name: "IX_ColecoesUsuario_IdUsuario",
                table: "ColecoesUsuario",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Itens_IdColecao",
                table: "Itens",
                column: "IdColecao");

            migrationBuilder.CreateIndex(
                name: "IX_Itens_IdDonoParticular",
                table: "Itens",
                column: "IdDonoParticular");

            migrationBuilder.CreateIndex(
                name: "IX_Itens_IdOriginal",
                table: "Itens",
                column: "IdOriginal",
                unique: true,
                filter: "[IdOriginal] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ItensUsuario_IdUsuario",
                table: "ItensUsuario",
                column: "IdUsuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColecoesUsuario");

            migrationBuilder.DropTable(
                name: "ItensUsuario");

            migrationBuilder.DropTable(
                name: "Itens");

            migrationBuilder.DropTable(
                name: "Colecoes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
