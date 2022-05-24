using Domain.Models;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;

namespace Repository.Data
{
    public class LojaDBContext : DbContext
    {
        //private readonly IConfiguration configuration;

        //public LojaDBContext(DbContextOptions<LojaDBContext> options, IConfiguration configuration) : base(options)
        //{
        //    this.configuration = configuration;
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var con = configuration.GetSection($"ConnectionStrings:Production").Value;

        //    optionsBuilder.UseSqlServer(con);
        //}

        public LojaDBContext(DbContextOptions<LojaDBContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<PessoaFisica> PessoasFisicas { get; set; }
        public DbSet<PessoaJuridica> PessoasJuridicas { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Item> Itens { get; set; }
        public DbSet<Tarifa> Tarifas { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<ImagemProduto> ImagensProduto { get; set; }
        public DbSet<CategoriaProduto> CategoriaProdutos { get; set; }
        //public DbSet<Carteira> Carteiras { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasAlternateKey(c => c.Id);
            modelBuilder.Entity<PessoaFisica>().HasAlternateKey(c => c.Cpf);
            modelBuilder.Entity<PessoaJuridica>().HasAlternateKey(c => c.Cnpj);
            modelBuilder.Entity<Venda>().HasAlternateKey(c => c.Id);
            modelBuilder.Entity<Produto>().HasAlternateKey(c => c.Id);
            modelBuilder.Entity<Item>().HasAlternateKey(c => c.Id);
            modelBuilder.Entity<Tarifa>().HasAlternateKey(c => c.Id);
            modelBuilder.Entity<Endereco>().HasAlternateKey(c => c.Id);
            modelBuilder.Entity<CategoriaProduto>().HasAlternateKey(c => c.Id);
            modelBuilder.Entity<ImagemProduto>().HasAlternateKey(c => c.Id);
            //modelBuilder.Entity<Carteira>().HasAlternateKey(c => c.Id);
        }
    }
}
