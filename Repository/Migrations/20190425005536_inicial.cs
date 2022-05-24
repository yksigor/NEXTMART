using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ENDERECOS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Logradouro = table.Column<string>(nullable: false),
                    Numero = table.Column<string>(nullable: false),
                    Complemento = table.Column<string>(nullable: true),
                    CEP = table.Column<string>(nullable: false),
                    UF = table.Column<int>(nullable: false),
                    Municipio = table.Column<string>(nullable: true),
                    Bairro = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENDERECOS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TARIFAS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(nullable: false),
                    Porcentagem = table.Column<double>(nullable: false),
                    ValorMaximo = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TARIFAS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "USUARIOS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: false),
                    RepitaPassword = table.Column<string>(maxLength: 20, nullable: false),
                    TipoUsuario = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIOS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PESSOASFISICAS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(nullable: false),
                    IdEndereco = table.Column<int>(nullable: false),
                    Telefone = table.Column<string>(nullable: false),
                    Saldo = table.Column<double>(nullable: false),
                    Cpf = table.Column<string>(nullable: false),
                    NomeCompleto = table.Column<string>(nullable: false),
                    DataNascimento = table.Column<DateTime>(nullable: false),
                    Sexo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PESSOASFISICAS", x => x.Id);
                    table.UniqueConstraint("AK_PESSOASFISICAS_Cpf", x => x.Cpf);
                    table.ForeignKey(
                        name: "FK_PESSOASFISICAS_ENDERECOS_IdEndereco",
                        column: x => x.IdEndereco,
                        principalTable: "ENDERECOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PESSOASFISICAS_USUARIOS_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PESSOASJURIDICAS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(nullable: false),
                    IdEndereco = table.Column<int>(nullable: false),
                    Telefone = table.Column<string>(nullable: false),
                    Saldo = table.Column<double>(nullable: false),
                    Cnpj = table.Column<string>(nullable: false),
                    RazaoSocial = table.Column<string>(nullable: false),
                    NomeFantasia = table.Column<string>(nullable: false),
                    Segmento = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PESSOASJURIDICAS", x => x.Id);
                    table.UniqueConstraint("AK_PESSOASJURIDICAS_Cnpj", x => x.Cnpj);
                    table.ForeignKey(
                        name: "FK_PESSOASJURIDICAS_ENDERECOS_IdEndereco",
                        column: x => x.IdEndereco,
                        principalTable: "ENDERECOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PESSOASJURIDICAS_USUARIOS_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VENDAS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(nullable: false),
                    DataVenda = table.Column<DateTime>(nullable: false),
                    ValorTotal = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VENDAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VENDAS_USUARIOS_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CATEGORIAPRODUTOS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdComerciante = table.Column<int>(nullable: false),
                    LojaId = table.Column<int>(nullable: true),
                    Nome = table.Column<string>(nullable: false),
                    Descricao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORIAPRODUTOS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CATEGORIAPRODUTOS_PESSOASJURIDICAS_LojaId",
                        column: x => x.LojaId,
                        principalTable: "PESSOASJURIDICAS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTOS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdComerciante = table.Column<int>(nullable: false),
                    ComercianteId = table.Column<int>(nullable: true),
                    IdCategoria = table.Column<int>(nullable: false),
                    CategoriaId = table.Column<int>(nullable: true),
                    Nome = table.Column<string>(nullable: false),
                    Marca = table.Column<string>(nullable: false),
                    Descricao = table.Column<string>(nullable: false),
                    Preco = table.Column<double>(nullable: false),
                    QuantidadeEstoque = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTOS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRODUTOS_CATEGORIAPRODUTOS_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "CATEGORIAPRODUTOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRODUTOS_PESSOASJURIDICAS_ComercianteId",
                        column: x => x.ComercianteId,
                        principalTable: "PESSOASJURIDICAS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IMAGEMPRODUTO",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdProduto = table.Column<int>(nullable: false),
                    NomeImagem = table.Column<string>(nullable: true),
                    CaminhoImagem = table.Column<string>(nullable: false),
                    ProdutoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMAGEMPRODUTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IMAGEMPRODUTO_PRODUTOS_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "PRODUTOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ITENS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Quantidade = table.Column<int>(nullable: false),
                    ValorUnidade = table.Column<int>(nullable: false),
                    VendaId = table.Column<int>(nullable: false),
                    ProdutoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITENS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ITENS_PRODUTOS_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "PRODUTOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ITENS_VENDAS_VendaId",
                        column: x => x.VendaId,
                        principalTable: "VENDAS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CATEGORIAPRODUTOS_LojaId",
                table: "CATEGORIAPRODUTOS",
                column: "LojaId");

            migrationBuilder.CreateIndex(
                name: "IX_IMAGEMPRODUTO_ProdutoId",
                table: "IMAGEMPRODUTO",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ITENS_ProdutoId",
                table: "ITENS",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ITENS_VendaId",
                table: "ITENS",
                column: "VendaId");

            migrationBuilder.CreateIndex(
                name: "IX_PESSOASFISICAS_IdEndereco",
                table: "PESSOASFISICAS",
                column: "IdEndereco");

            migrationBuilder.CreateIndex(
                name: "IX_PESSOASFISICAS_IdUsuario",
                table: "PESSOASFISICAS",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_PESSOASJURIDICAS_IdEndereco",
                table: "PESSOASJURIDICAS",
                column: "IdEndereco");

            migrationBuilder.CreateIndex(
                name: "IX_PESSOASJURIDICAS_IdUsuario",
                table: "PESSOASJURIDICAS",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUTOS_CategoriaId",
                table: "PRODUTOS",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUTOS_ComercianteId",
                table: "PRODUTOS",
                column: "ComercianteId");

            migrationBuilder.CreateIndex(
                name: "IX_VENDAS_IdUsuario",
                table: "VENDAS",
                column: "IdUsuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IMAGEMPRODUTO");

            migrationBuilder.DropTable(
                name: "ITENS");

            migrationBuilder.DropTable(
                name: "PESSOASFISICAS");

            migrationBuilder.DropTable(
                name: "TARIFAS");

            migrationBuilder.DropTable(
                name: "PRODUTOS");

            migrationBuilder.DropTable(
                name: "VENDAS");

            migrationBuilder.DropTable(
                name: "CATEGORIAPRODUTOS");

            migrationBuilder.DropTable(
                name: "PESSOASJURIDICAS");

            migrationBuilder.DropTable(
                name: "ENDERECOS");

            migrationBuilder.DropTable(
                name: "USUARIOS");
        }
    }
}
