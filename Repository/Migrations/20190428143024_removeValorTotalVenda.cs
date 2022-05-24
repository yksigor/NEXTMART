using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class removeValorTotalVenda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorTotal",
                table: "VENDAS");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ValorTotal",
                table: "VENDAS",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
