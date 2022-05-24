using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class nullIdUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VENDAS_USUARIOS_IdUsuario",
                table: "VENDAS");

            migrationBuilder.AlterColumn<int>(
                name: "IdUsuario",
                table: "VENDAS",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_VENDAS_USUARIOS_IdUsuario",
                table: "VENDAS",
                column: "IdUsuario",
                principalTable: "USUARIOS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VENDAS_USUARIOS_IdUsuario",
                table: "VENDAS");

            migrationBuilder.AlterColumn<int>(
                name: "IdUsuario",
                table: "VENDAS",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VENDAS_USUARIOS_IdUsuario",
                table: "VENDAS",
                column: "IdUsuario",
                principalTable: "USUARIOS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
