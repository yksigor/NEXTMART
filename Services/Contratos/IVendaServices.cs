using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contratos
{
    public interface IVendaServices
    {
        Venda InsertItem(int idProduto, int Quantidade, Venda venda);
        Task<Venda> GetVendaByIdAsync(int Id);
        Venda GetVendaById(int Id);
        List<Venda> GetVendasByIdUsuario(int idUsuario);
        Task <List<Venda>> GetAllVendas();
        bool RemoveItem(int idItem);
        void EditItem(int idItem);
        List<Item> GetItensVenda(int idVenda);
        Venda CreateVenda(Venda venda);
        Task<bool> RemoveAsync(int Id);
        Task<Item> UpdateItemAsync(Item item);
        Task<bool> RemoveItemAsync(int Id);
        Task<Venda> UpdateVendaAsync(Venda venda);
        Task<Venda> PagarAsync(Venda venda);
        Task<List<Item>> ItensVendidos(PessoaJuridica pessoa);
        Venda ObterVendaComConsumidor(int idVenda);
    }
}
