﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository.Data;

namespace Repository.Migrations
{
    [DbContext(typeof(LojaDBContext))]
    [Migration("20190428025432_CArrinho")]
    partial class CArrinho
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Models.CategoriaProduto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descricao");

                    b.Property<int>("IdComerciante");

                    b.Property<int?>("LojaId");

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("LojaId");

                    b.ToTable("CATEGORIAPRODUTOS");
                });

            modelBuilder.Entity("Domain.Models.Endereco", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Bairro")
                        .IsRequired();

                    b.Property<string>("CEP")
                        .IsRequired();

                    b.Property<string>("Complemento");

                    b.Property<string>("Logradouro")
                        .IsRequired();

                    b.Property<string>("Municipio");

                    b.Property<string>("Numero")
                        .IsRequired();

                    b.Property<int>("UF");

                    b.HasKey("Id");

                    b.ToTable("ENDERECOS");
                });

            modelBuilder.Entity("Domain.Models.ImagemProduto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CaminhoImagem")
                        .IsRequired();

                    b.Property<int>("IdProduto");

                    b.Property<string>("NomeImagem");

                    b.Property<int?>("ProdutoId");

                    b.Property<string>("UrlImagem");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoId");

                    b.ToTable("IMAGEMPRODUTO");
                });

            modelBuilder.Entity("Domain.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProdutoId")
                        .IsConcurrencyToken();

                    b.Property<int>("Quantidade");

                    b.Property<int>("ValorUnidade");

                    b.Property<int>("VendaId")
                        .IsConcurrencyToken();

                    b.HasKey("Id");

                    b.HasIndex("ProdutoId");

                    b.HasIndex("VendaId");

                    b.ToTable("ITENS");
                });

            modelBuilder.Entity("Domain.Models.PessoaFisica", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Cpf")
                        .IsRequired();

                    b.Property<DateTime>("DataNascimento");

                    b.Property<int>("IdEndereco");

                    b.Property<int>("IdUsuario");

                    b.Property<string>("NomeCompleto")
                        .IsRequired();

                    b.Property<double>("Saldo");

                    b.Property<int>("Sexo");

                    b.Property<string>("Telefone")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAlternateKey("Cpf");

                    b.HasIndex("IdEndereco");

                    b.HasIndex("IdUsuario");

                    b.ToTable("PESSOASFISICAS");
                });

            modelBuilder.Entity("Domain.Models.PessoaJuridica", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Cnpj")
                        .IsRequired();

                    b.Property<int>("IdEndereco");

                    b.Property<int>("IdUsuario");

                    b.Property<string>("NomeFantasia")
                        .IsRequired();

                    b.Property<string>("RazaoSocial")
                        .IsRequired();

                    b.Property<double>("Saldo");

                    b.Property<string>("Segmento")
                        .IsRequired();

                    b.Property<string>("Telefone")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAlternateKey("Cnpj");

                    b.HasIndex("IdEndereco");

                    b.HasIndex("IdUsuario");

                    b.ToTable("PESSOASJURIDICAS");
                });

            modelBuilder.Entity("Domain.Models.Produto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoriaId");

                    b.Property<int?>("ComercianteId");

                    b.Property<string>("Descricao")
                        .IsRequired();

                    b.Property<int>("IdCategoria");

                    b.Property<int>("IdComerciante");

                    b.Property<string>("Marca")
                        .IsRequired();

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<double>("Preco");

                    b.Property<double>("QuantidadeEstoque");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("ComercianteId");

                    b.ToTable("PRODUTOS");
                });

            modelBuilder.Entity("Domain.Models.Tarifa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Codigo")
                        .IsRequired();

                    b.Property<double>("Porcentagem");

                    b.Property<double>("ValorMaximo");

                    b.HasKey("Id");

                    b.ToTable("TARIFAS");
                });

            modelBuilder.Entity("Domain.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("RepitaPassword")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<int>("TipoUsuario");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("USUARIOS");
                });

            modelBuilder.Entity("Domain.Models.Venda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DataVenda");

                    b.Property<string>("IdCarrinho");

                    b.Property<int>("IdUsuario");

                    b.Property<int>("StatusVenda");

                    b.Property<double>("ValorTotal");

                    b.HasKey("Id");

                    b.HasIndex("IdUsuario");

                    b.ToTable("VENDAS");
                });

            modelBuilder.Entity("Domain.Models.CategoriaProduto", b =>
                {
                    b.HasOne("Domain.Models.PessoaJuridica", "Loja")
                        .WithMany()
                        .HasForeignKey("LojaId");
                });

            modelBuilder.Entity("Domain.Models.ImagemProduto", b =>
                {
                    b.HasOne("Domain.Models.Produto")
                        .WithMany("ImagensProduto")
                        .HasForeignKey("ProdutoId");
                });

            modelBuilder.Entity("Domain.Models.Item", b =>
                {
                    b.HasOne("Domain.Models.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Models.Venda")
                        .WithMany("Itens")
                        .HasForeignKey("VendaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.Models.PessoaFisica", b =>
                {
                    b.HasOne("Domain.Models.Endereco", "Endereco")
                        .WithMany()
                        .HasForeignKey("IdEndereco")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.Models.PessoaJuridica", b =>
                {
                    b.HasOne("Domain.Models.Endereco", "Endereco")
                        .WithMany()
                        .HasForeignKey("IdEndereco")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.Models.Produto", b =>
                {
                    b.HasOne("Domain.Models.CategoriaProduto", "Categoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId");

                    b.HasOne("Domain.Models.PessoaJuridica", "Comerciante")
                        .WithMany()
                        .HasForeignKey("ComercianteId");
                });

            modelBuilder.Entity("Domain.Models.Venda", b =>
                {
                    b.HasOne("Domain.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
