using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class CArrinho : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdCarrinho",
                table: "VENDAS",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusVenda",
                table: "VENDAS",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "IdUsuario",
                table: "VENDAS",
                nullable: true
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdCarrinho",
                table: "VENDAS");

            migrationBuilder.DropColumn(
                name: "StatusVenda",
                table: "VENDAS");
        }
    }
}
