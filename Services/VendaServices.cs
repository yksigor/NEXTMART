using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Services.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class VendaServices : IVendaServices
    {
        private readonly LojaDBContext context;
        private readonly IProdutoServices produtoServices;
        private readonly IItemServices itemServices;
        private readonly IPessoaServices pessoaServices;
        private readonly IUsuarioServices usuarioServices;

        public VendaServices(LojaDBContext context, IProdutoServices produtoServices, IItemServices itemServices, IPessoaServices pessoaServices, IUsuarioServices usuarioServices)
        {
            this.context = context;
            this.produtoServices = produtoServices;
            this.itemServices = itemServices;
            this.pessoaServices = pessoaServices;
            this.usuarioServices = usuarioServices;
        }

        public void EditItem(int idItem)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Venda>> GetAllVendas()
        {
            var venda = context.Vendas.ToList();
            foreach (var m in venda)
            {
                m.Itens = await itemServices.GetItensVendaAsync(m.Id);
                m.Usuario = await usuarioServices.FindByIdAsync(m.IdUsuario);
            }
            return venda;
        }

        public async Task<Venda> GetVendaByIdAsync(int Id)
        {
            var venda = await context.Vendas.FirstOrDefaultAsync(a => a.Id == Id);
            venda.Itens = await itemServices.GetItensVendaAsync(venda.Id);
            venda.Usuario = await usuarioServices.FindByIdAsync(venda.IdUsuario);
            return venda;
        }

        public Venda GetVendaById(int Id)
        {
            var venda = context.Vendas.FirstOrDefault(a => a.Id == Id);
            venda.Itens = GetItensVenda(venda.Id);
            return venda;
        }

        public List<Venda> GetVendasByIdUsuario(int idUsuario)
        {
            var vendas = context.Vendas.Where(a => a.IdUsuario == idUsuario).ToList();

            var consumidor = pessoaServices.GetPessoaFisicaByIdUsuario(idUsuario);

            vendas.ForEach(venda =>
            {
                venda.Itens = GetItensVenda(venda.Id).ToList();
                venda.Consumidor = consumidor;
            });

            return vendas;
        }

        public Venda InsertItem(int idProduto, int Quantidade, Venda venda)
        {
            var item = venda.Itens?.FirstOrDefault(p => p.ProdutoId == idProduto);
            var produto = produtoServices.GetProduto(idProduto);

            if (item == null)
            {
                item = new Item()
                {
                    Produto = produto,
                    VendaId = venda.Id,
                    Quantidade = Quantidade,
                    ValorUnidade = produto.Preco
                };

                var i = context.Itens.Add(item);
                context.SaveChanges();
            }
            else
            {
                item.Quantidade++;
                context.Itens.Update(item);
                context.SaveChanges();
            }
            return GetVendaById(venda.Id);
        }

        public bool RemoveItem(int idItem)
        {
            throw new NotImplementedException();
        }

        public List<Item> GetItensVenda(int idVenda)
        {
            var itens = context.Itens.Where(a => a.VendaId == idVenda).ToList();

            itens.ForEach(item =>
            {
                item.Produto = produtoServices.GetProduto(item.ProdutoId);
            });

            return itens;
        }

        public Venda CreateVenda(Venda venda)
        {
            var v = context.Vendas.Add(venda);
            venda.Itens.ForEach(i =>
            {
                i.VendaId = v.Entity.Id;
            });
            context.AddRange(venda.Itens);
            context.SaveChanges();
            return v.Entity;
        }

        public async Task<bool> RemoveAsync(int Id)
        {
            var venda = await GetVendaByIdAsync(Id);

            context.Vendas.Remove(venda);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Item> UpdateItemAsync(Item item)
        {
            item = await itemServices.UpdateAsync(item);
            return item;
        }

        public async Task<bool> RemoveItemAsync(int Id)
        {
            return await itemServices.RemoveAsync(Id);
        }

        public async Task<Venda> UpdateVendaAsync(Venda venda)
        {
            var v = await context.Vendas.FirstOrDefaultAsync(a => a.Id == venda.Id);
            v.StatusVenda = venda.StatusVenda;
            await context.SaveChangesAsync();
            return v;
        }

        public async Task<Venda> PagarAsync(Venda venda)
        {
            if (await pessoaServices.CobrarConsumidor(venda))
            {
                venda.StatusVenda = StatusVenda.Concluida;
                await UpdateVendaAsync(venda);
                await pessoaServices.PagarComerciantes(venda.GetListaPagamentos());

                return venda;
            }
            else
            {
                return venda;
            }
        }

        public async Task<List<Item>> ItensVendidos(PessoaJuridica pessoa)
        {
            var itens = await context.Itens.Where(a => a.Produto.IdComerciante == pessoa.Id).ToListAsync();

            itens.ForEach(item =>
            {
                item.Produto = produtoServices.GetProduto(item.ProdutoId);
                //item.Venda = ObterVendaComConsumidor(item.VendaId);
            });

            return itens;
        }

        public Venda ObterVendaComConsumidor(int idVenda)
        {
            var venda = context.Vendas.FirstOrDefault(a => a.Id == idVenda);
            venda.Consumidor = context.PessoasFisicas.FirstOrDefault(a => a.IdUsuario == venda.IdUsuario);

            return venda;
        }
    }
}