using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Repository.Data;
using Services.Contratos;

namespace Services
{
    public class ItemServices: IItemServices
    {
        private readonly LojaDBContext _context;
        private readonly IProdutoServices produtoServices;

        public ItemServices(LojaDBContext context, IProdutoServices produtoServices)
        {
            _context = context;
            this.produtoServices = produtoServices;
        }

        public async Task<List<Item>> FindAllAsync()
        {
            return await _context.Itens.ToListAsync();
        }

        public async Task<Item> FindByIdAsync(int id)
        {
            var item = await _context.Itens.FirstOrDefaultAsync(p => p.Id == id);
            var venda = await _context.Vendas.FindAsync(item.VendaId);
            return venda != null ? item : null;
        }

        public async Task<List<Item>> GetItensVendaAsync(int idVenda)
        {
            var itens = await _context.Itens.Where(i => i.VendaId == idVenda).ToListAsync();

            foreach(var item in itens)
            {
                item.Produto = await produtoServices.GetProdutoAsync(item.ProdutoId);
            }

            return itens;
        }

        public async Task<bool> InsertAsync(Item item)
        {
            var venda = await _context.Vendas.FindAsync(item.VendaId);
            if (venda == null) return false;

            _context.Itens.Add(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var obj = await _context.Itens.FirstOrDefaultAsync(a => a.Id == id);
            if (obj == null) return false;

            _context.Itens.Remove(obj);
            var x = await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Item> UpdateAsync(Item item)
        {
            var i = await _context.Itens.FirstOrDefaultAsync(p => p.Id == item.Id);
            if (i == null) return null;

            i.ProdutoId = item.ProdutoId;
            i.Quantidade = item.Quantidade;
            i.ValorUnidade = item.ValorUnidade;

            _context.Itens.Update(i);
            await _context.SaveChangesAsync();
            return i;
        }
    }
}
