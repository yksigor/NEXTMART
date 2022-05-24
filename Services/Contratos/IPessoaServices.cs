using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contratos
{
    public interface IPessoaServices
    {
        PessoaFisica GetPessoaFisicaByIdUsuario(int idUsuario);
        Endereco GetEndereco(int Id);
        Task<PessoaFisica> GetPessoaFisicaAsync(int id);
        Task<PessoaFisica> GetPessoaFisicaByIdUsuarioAsync(int idUsuario);
        Task<PessoaJuridica> GetPessoaJuridicaByIdAsync(int id);
        Task<PessoaFisica> UpdatePessoaFisicaAsync(PessoaFisica pessoaFisica);
        Task<PessoaJuridica> UpdatePessoaJuridicaAsync(PessoaJuridica pessoaJuridica);
        Task<PessoaJuridica> GetPessoaJuridicaAsync(int id);
        Task PagarComerciantes(List<Pagamento> pagamentos);
        Task<bool> CobrarConsumidor(Venda venda);
    }
}
