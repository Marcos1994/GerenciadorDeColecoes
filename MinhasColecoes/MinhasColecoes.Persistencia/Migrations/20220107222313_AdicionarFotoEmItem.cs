using Microsoft.EntityFrameworkCore.Migrations;

namespace MinhasColecoes.Persistencia.Migrations
{
    public partial class AdicionarFotoEmItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "Itens",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Itens");
        }
    }
}
