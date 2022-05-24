using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repository.Data;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Services.Contratos;

namespace Services
{
    public class UsuarioServices : IUsuarioServices
    {
        private readonly LojaDBContext _context;

        public UsuarioServices(LojaDBContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> FindAllAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> FindByIdAsync(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task InsertAsync(Usuario usuario)
        {
            _context.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            // referencias: 
            //https://pt.stackoverflow.com/questions/175983/problema-de-fazer-update-com-1-campo-ef
            //https://social.msdn.microsoft.com/Forums/sqlserver/pt-BR/4d33e0dd-8d84-47f1-ac63-3b87eed94420/entity-framework-update-no-funciona?forum=adoptpt
            //_context.Entry(usuario).State = EntityState.Modified;

            var user = _context.Usuarios.FirstOrDefault(u => u.Id == usuario.Id);

            user.Username = usuario.Username;
            user.Password = usuario.Password;

            _context.Usuarios.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}