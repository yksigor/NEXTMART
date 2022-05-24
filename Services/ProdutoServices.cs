using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Services.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;

namespace Services
{
    public class ProdutoServices : IProdutoServices
    {
        public readonly LojaDBContext context;
        public readonly IPessoaServices pessoaServices;

        public ProdutoServices(LojaDBContext context, IPessoaServices pessoaServices)
        {
            this.context = context;
            this.pessoaServices = pessoaServices;
        }

        public bool DeleteCategoria(int idComerciante, int idCategoria)
        {
            var categoria = context.CategoriaProdutos.FirstOrDefault(c => c.Id.Equals(idCategoria) && c.IdComerciante.Equals(idComerciante));

            if (categoria is null) return false;

            context.CategoriaProdutos.Remove(categoria);

            try
            {
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteProduto(int idComerciante, int idProduto)
        {
            var produto = context.Produtos.FirstOrDefault(c => c.Id.Equals(idProduto) && c.IdComerciante.Equals(idComerciante));

            if (produto is null) return false;

            context.Produtos.Remove(produto);

            try
            {
                context.SaveChanges();
                return true;
            }catch
            {
                return false;
            }
        }

        public CategoriaProduto GetCategoriaProduto(int idCategoria)
        {
            return context.CategoriaProdutos.FirstOrDefault(c => c.Id == idCategoria);
        }

        public CategoriaProduto GetCategoriaProdutoByIdComerciante(int idComerciante)
        {
            return context.CategoriaProdutos.FirstOrDefault(c => c.IdComerciante == idComerciante);
        }

        public PessoaJuridica GetComercianteProduto(int idComerciante)
        {
            return context.PessoasJuridicas.FirstOrDefault(c => c.Id == idComerciante);
        }

        public async Task<List<CategoriaProduto>> GetCategoriaProdutosAsync(int idComerciante)
        {
            return await context.CategoriaProdutos.Where(a => a.IdComerciante == idComerciante).ToListAsync();
        }

        public async Task<List<CategoriaProduto>> GetCategoriaProdutosAsync()
        {
            return await context.CategoriaProdutos.ToListAsync();
        }

        public List<ImagemProduto> GetImagensProduto(int idProduto)
        {
            return context.ImagensProduto.Where(a => a.IdProduto == idProduto).ToList();
        }

        public async Task<List<ImagemProduto>> GetImagensProdutoAsync(int idProduto)
        {
            return await context.ImagensProduto.Where(a => a.IdProduto == idProduto).ToListAsync();
        }

        public Produto GetProduto(int idProduto)
        {
            var produto = context.Produtos.FirstOrDefault(c => c.Id.Equals(idProduto));

            produto.Comerciante = pessoaServices.GetPessoaJuridicaAsync(produto.IdComerciante).Result;
            produto.ImagensProduto = context.ImagensProduto.Where(i => i.IdProduto == produto.Id).ToList();
            produto.ImagensProduto.ForEach(s =>
            {
                s.CaminhoImagem = string.Concat("/uploads/", s.NomeImagem);
            });

            return produto;
        }

        public Produto GetProduto(int idProduto, int idComerciante)
        {
            var produto = context.Produtos.FirstOrDefault(c => c.Id.Equals(idProduto) && c.IdComerciante.Equals(idComerciante));

            produto.ImagensProduto = context.ImagensProduto.Where(i => i.IdProduto == produto.Id).ToList();
            produto.ImagensProduto.ForEach(s =>
            {
                s.CaminhoImagem = string.Concat("/uploads/", s.NomeImagem);
            });

            return produto;
        }

        public async Task<Produto> GetProdutoAsync(int Id)
        {
            var produto = await context.Produtos.FirstOrDefaultAsync(a => a.Id == Id);
            if (produto is null) return null;
            produto.ImagensProduto = GetImagensProduto(produto.Id);
            produto.Categoria = GetCategoriaProduto(produto.IdCategoria);
            produto.Comerciante = GetComercianteProduto(produto.IdComerciante);
            return produto;
        }

        public List<Produto> GetProdutos(int idComerciante)
        {
            var produtos = context.Produtos.Where(c => c.IdComerciante.Equals(idComerciante)).ToList();

            produtos.ForEach(a =>
            {
                a.ImagensProduto = context.ImagensProduto.Where(i => i.IdProduto == a.Id).ToList();
                a.ImagensProduto.ForEach(s =>
                {
                    s.CaminhoImagem = string.Concat("/uploads/", s.NomeImagem);
                });
            });

            return produtos;
        }

        public List<Produto> GetProdutosByCategoria(int idCategoria)
        {
            var produtos = context.Produtos.Where(c => c.IdCategoria.Equals(idCategoria)).ToList();

            produtos.ForEach(a =>
            {
                a.ImagensProduto = context.ImagensProduto.Where(i => i.IdProduto == a.Id).ToList();
                a.ImagensProduto.ForEach(s =>
                {
                    s.CaminhoImagem = string.Concat("/uploads/", s.NomeImagem);
                });
            });

            return produtos;
        }

        public List<Produto> GetProdutos()
        {
            var produtos = context.Produtos.Where(p => p.Comerciante.StatusComerciante == StatusComerciante.Ativo).ToList();

            produtos.ForEach(a =>
            {
                a.ImagensProduto = context.ImagensProduto.Where(i => i.IdProduto == a.Id).ToList();
                a.ImagensProduto.ForEach(s =>
                {
                    s.CaminhoImagem = string.Concat("/uploads/", s.NomeImagem);
                });
            });

            return produtos;
        }

        public async Task<List<Produto>> GetProdutosAsync()
        {

            List<Produto> produtos = await context.Produtos.Join(context.PessoasJuridicas, p => p.IdComerciante, c => c.Id, (p, c) => new { p, c })
                .Where(s => s.c.StatusComerciante == StatusComerciante.Ativo)
                .Select(a => a.p).ToListAsync();

            produtos.ForEach(p =>
            {
                p.ImagensProduto = GetImagensProduto(p.Id);
            });

            return produtos;
        }

        public async Task<List<Produto>> GetProdutosAsync(int idComerciante)
        {
            return await context.Produtos.Where(a => a.IdComerciante == idComerciante).ToListAsync();
        }

        public CategoriaProduto InsertOrUpdateCategoria(int? idcategoria, CategoriaProduto categoria)
        {
            CategoriaProduto c;
            if (idcategoria != 0)
            {
                c = context.CategoriaProdutos.FirstOrDefault(m => m.Id.Equals(idcategoria.Value));

                c.Descricao = categoria.Descricao;
                c.IdComerciante = categoria.IdComerciante;
                c.Nome = categoria.Nome;

                context.CategoriaProdutos.Update(c);
            }
            else
            {
                c = context.CategoriaProdutos.Add(categoria).Entity;
            }

            context.SaveChanges();

            return c;
        }

        public Produto InsertOrUpdateProduto(int? idProduto, Produto produto)
        {
            Produto p;
            if (idProduto != 0)
            {
                p = context.Produtos.FirstOrDefault(c => c.Id.Equals(idProduto.Value));

                p.IdCategoria = produto.IdCategoria;
                p.IdComerciante = produto.IdComerciante;
                p.Nome = produto.Nome;
                p.Preco = produto.Preco;
                p.QuantidadeEstoque = produto.QuantidadeEstoque;
                p.Descricao = produto.Descricao;
                p.Marca = produto.Marca;

                context.Produtos.Update(p);
            } else
            {
                p = context.Produtos.Add(produto).Entity;
            }
            
            context.SaveChanges();

            return p;
        }

        public List<ImagemProduto> InsertOrUpdateProduto(int value)
        {
            throw new NotImplementedException();
        }

        public async Task AtualizarEstoque(List<Item> itens)
        {
            foreach (var item in itens)
            {
                (await context.Produtos
                    .FirstOrDefaultAsync(a => a.Id == item.ProdutoId))
                    .QuantidadeEstoque -= item.Quantidade;
            }

            await context.SaveChangesAsync();
        }

        public async Task Restock(List<Item> itens)
        {
            foreach (var item in itens)
            {
                (await context.Produtos
                    .FirstOrDefaultAsync(a => a.Id == item.ProdutoId))
                    .QuantidadeEstoque += item.Quantidade;
            }

            await context.SaveChangesAsync();
        }

        public async Task<bool> RemoverImagem(int id, string wwwrootPath)
        {
            var imagem = await context.ImagensProduto.FirstOrDefaultAsync(a => a.Id == id);
            if(imagem != null)
            {
                if(RemoverArquivo(imagem.CaminhoFisico(wwwrootPath)) == "OK")
                {
                    context.ImagensProduto.Remove(imagem);
                    await context.SaveChangesAsync();

                    return true;
                }
                return false;
            }

            return false;
        }

        public string RemoverArquivo(string caminho)
        {

            FileInfo fi = new FileInfo(caminho);
            try
            {
                fi.Delete();
                return "OK";
            }
            catch (IOException e)
            {
                return e.Message;
            }
        }        
    }
}