using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contratos
{
    public interface IUsuarioServices
    {
        Task<List<Usuario>> FindAllAsync();
        Task<Usuario> FindByIdAsync(int id);
        Task InsertAsync(Usuario usuario);
        Task RemoveAsync(int id);
        Task UpdateAsync(Usuario usuario);
    }
}
