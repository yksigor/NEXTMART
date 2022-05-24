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
    public class PessoaServices : IPessoaServices
    {
        private readonly LojaDBContext context;
        private readonly ILoginServices loginServices;

        public PessoaServices(LojaDBContext context, ILoginServices loginServices)
        {
            this.context = context;
            this.loginServices = loginServices;
        }

        public async Task<bool> CobrarConsumidor(Venda venda)
        {
            var pessoa = await GetPessoaFisicaByIdUsuarioAsync(venda.IdUsuario);

            if (pessoa.Saldo >= venda.GetValorTotal())
            {
                pessoa.Saldo -= venda.GetValorTotal();
                pessoa = await UpdatePessoaFisicaAsync(pessoa);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Endereco GetEndereco(int Id)
        {
            return context.Enderecos.FirstOrDefault(a => a.Id == Id);
        }

        public async Task<PessoaFisica> GetPessoaFisicaAsync(int id)
        {
            var pessoa = await context.PessoasFisicas.FirstOrDefaultAsync(a => a.Id == id);
            pessoa.Endereco = GetEndereco(pessoa.IdEndereco);
            pessoa.Usuario = loginServices.BuscarUsuarioPeloID(pessoa.IdUsuario);
            return pessoa;
        }

        public PessoaFisica GetPessoaFisicaByIdUsuario(int idUsuario)
        {
            var pessoa = context.PessoasFisicas.FirstOrDefault(a => a.IdUsuario == idUsuario);
            pessoa.Endereco = GetEndereco(pessoa.IdEndereco);
            pessoa.Usuario = loginServices.BuscarUsuarioPeloID(pessoa.IdUsuario);
            return pessoa;
        }

        public async Task<PessoaFisica> GetPessoaFisicaByIdUsuarioAsync(int idUsuario)
        {
            var pessoa = await context.PessoasFisicas.FirstOrDefaultAsync(a => a.IdUsuario == idUsuario);
            pessoa.Endereco = await context.Enderecos.FirstOrDefaultAsync(a => a.Id == pessoa.IdEndereco);
            return pessoa;
        }

        public async Task<PessoaJuridica> GetPessoaJuridicaAsync(int id)
        {
            return await context.PessoasJuridicas.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<PessoaJuridica> GetPessoaJuridicaByIdAsync(int id)
        {
            return await context.PessoasJuridicas.FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task PagarComerciantes(List<Pagamento> pagamentos)
        {
            foreach (var pagamento in pagamentos)
            {
                (await context.PessoasJuridicas
                    .FirstOrDefaultAsync(a => a.Id == pagamento.IdComerciante))
                    .Saldo += pagamento.Valor;
            }

            await context.SaveChangesAsync();
        }

        public async Task<PessoaFisica> UpdatePessoaFisicaAsync(PessoaFisica pessoaFisica)
        {
            var pessoa = await context.PessoasFisicas.FirstOrDefaultAsync(a => a.Id == pessoaFisica.Id);
            pessoa.NomeCompleto = pessoaFisica.NomeCompleto;
            pessoa.Saldo = pessoaFisica.Saldo;
            return pessoa;
        }

        public async Task<PessoaJuridica> UpdatePessoaJuridicaAsync(PessoaJuridica pessoaJuridica)
        {
            var pessoa = await context.PessoasJuridicas.FirstOrDefaultAsync(a => a.Id == pessoaJuridica.Id);
            pessoa.Saldo = pessoaJuridica.Saldo;
            return pessoa;
        }
    }
}
