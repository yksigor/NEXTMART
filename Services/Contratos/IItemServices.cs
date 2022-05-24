using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contratos
{
    public interface IItemServices
    {
        Task<List<Item>> FindAllAsync();
        Task<Item> FindByIdAsync(int id);
        Task<List<Item>> GetItensVendaAsync(int idVenda);
        Task<bool> InsertAsync(Item item);
        Task<bool> RemoveAsync(int id);
        Task<Item> UpdateAsync(Item item);
    }
}
