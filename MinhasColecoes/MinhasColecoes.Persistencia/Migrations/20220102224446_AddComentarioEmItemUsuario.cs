using Microsoft.EntityFrameworkCore.Migrations;

namespace MinhasColecoes.Persistencia.Migrations
{
    public partial class AddComentarioEmItemUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comentario",
                table: "ItensUsuario",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comentario",
                table: "ItensUsuario");
        }
    }
}
