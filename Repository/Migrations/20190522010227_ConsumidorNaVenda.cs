using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class ConsumidorNaVenda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConsumidorId",
                table: "VENDAS",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VENDAS_ConsumidorId",
                table: "VENDAS",
                column: "ConsumidorId");

            migrationBuilder.AddForeignKey(
                name: "FK_VENDAS_PESSOASFISICAS_ConsumidorId",
                table: "VENDAS",
                column: "ConsumidorId",
                principalTable: "PESSOASFISICAS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VENDAS_PESSOASFISICAS_ConsumidorId",
                table: "VENDAS");

            migrationBuilder.DropIndex(
                name: "IX_VENDAS_ConsumidorId",
                table: "VENDAS");

            migrationBuilder.DropColumn(
                name: "ConsumidorId",
                table: "VENDAS");
        }
    }
}
