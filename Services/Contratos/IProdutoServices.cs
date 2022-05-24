using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contratos
{
    public interface IProdutoServices
    {
        List<Produto> GetProdutos(int idComerciante);
        List<Produto> GetProdutosByCategoria(int idCategoria);
        List<Produto> GetProdutos();
        Produto GetProduto(int idProduto);
        Produto GetProduto(int idProduto, int idComerciante);
        Produto InsertOrUpdateProduto(int? idProduto, Produto produto);
        CategoriaProduto InsertOrUpdateCategoria(int? idcategoria, CategoriaProduto categoria);
        CategoriaProduto GetCategoriaProduto(int idCategoria);
        CategoriaProduto GetCategoriaProdutoByIdComerciante(int idComerciante);
        PessoaJuridica GetComercianteProduto(int idComerciante);
        bool DeleteProduto(int idComerciante, int idProduto);
        bool DeleteCategoria(int idComerciante, int idCategoria);
        Task<Produto> GetProdutoAsync(int Id);
        Task<List<Produto>> GetProdutosAsync();
        Task<List<Produto>> GetProdutosAsync(int idComerciante);
        List<ImagemProduto> GetImagensProduto(int idProduto);
        Task<List<ImagemProduto>> GetImagensProdutoAsync(int idProduto);
        Task<List<CategoriaProduto>> GetCategoriaProdutosAsync(int idComerciante);
        Task<List<CategoriaProduto>> GetCategoriaProdutosAsync();
        List<ImagemProduto> InsertOrUpdateProduto(int value);
        Task AtualizarEstoque(List<Item> produtos);
        Task Restock(List<Item> itens);
        Task<bool> RemoverImagem(int id, string wwrootPath);
        string RemoverArquivo(string caminho);
    }
}
