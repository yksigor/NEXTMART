using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contratos
{
    public interface IPessoaJuridicaServices
    {
        Task<List<PessoaJuridica>> FindAllAsync();
        Task<PessoaJuridica> FindByIdAsync(int idComerciante);
        Task<PessoaJuridica> GetPessoaJuridicaByIdUsuarioAsync(int idUsuario);
        Task InsertAsync(PessoaJuridica pessoaJuridica);
        Task RemoveAsync(int idComerciante);
        Task UpdateAsync(PessoaJuridica pessoaJuridica);
    }
}
