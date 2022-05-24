using Domain.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contratos
{
    public interface ILoginServices
    {
        bool Validate(Usuario usuario);
        Usuario InsertUsuario(Usuario usuario);
        Usuario BuscarUsuario(Usuario usuario);
        Task<Usuario> GetUsuarioByUserAsync(User user);
        Usuario BuscarUsuarioPeloID(int id);
        Task<Usuario> BuscarUsuarioPeloIDAsync(int Id);
        Task<Pessoa> RetornaPessoaPeloUsuarioAsync(Usuario usuario);
        List<Claim> Autenticar(Usuario usuario);
        Task<Usuario> GetUsuarioLogadoAsync(ClaimsPrincipal User);
        Task<Pessoa> GetPessoaLogadaAsync(ClaimsPrincipal User);
    }
}
