using Domain.Models;
using Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Services.Contratos;

namespace Services
{
    public class LoginServices : ILoginServices
    {
        private readonly LojaDBContext _context;

        public LoginServices(LojaDBContext context)
        {
            _context = context;
        }

        public bool Validate(Usuario user)
        {
            if (BuscarUsuario(user) != null) return true;
            return false;
        }

        public Usuario InsertUsuario(Usuario usuario)
        {
            _context.Add(usuario);
            _context.SaveChanges();
            return _context.Usuarios.Last();
        }

        public Usuario BuscarUsuario(Usuario usuario)
        {
            return _context.Usuarios
                .FirstOrDefault(u => u.Password.Equals(usuario.Password) &&
                (u.Username.Equals(usuario.Username) || u.Email.Equals(usuario.Email)));
        }

        public async Task<Usuario> BuscarUsuarioPeloIDAsync(int Id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(a => a.Id == Id);
        }

        public async Task<Pessoa> RetornaPessoaPeloUsuarioAsync(Usuario usuario)
        {
            switch (usuario.TipoUsuario)
            {
                case TipoUsuario.Comerciante:
                    return await _context.PessoasJuridicas.FirstOrDefaultAsync(a => a.IdUsuario == usuario.Id);
                case TipoUsuario.Consumidor:
                    return await _context.PessoasJuridicas.FirstOrDefaultAsync(a => a.IdUsuario == usuario.Id);
                case TipoUsuario.Administrador:
                    return await _context.PessoasFisicas.FirstOrDefaultAsync(a => a.IdUsuario == usuario.Id);
                case TipoUsuario.Entregador:
                    return await _context.PessoasJuridicas.FirstOrDefaultAsync(a => a.IdUsuario == usuario.Id);
                default:
                    return null;
            }
        }

        public List<Claim> Autenticar(Usuario usuario)
        {
            var user = BuscarUsuario(usuario);

            //Criando uma Identidade e associando-a ao ambiente.
            return new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.TipoUsuario.ToString())
            };
        }

        public async Task<Usuario> GetUsuarioLogadoAsync(ClaimsPrincipal User)
        {
            return await BuscarUsuarioPeloIDAsync(Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
        }

        public Usuario BuscarUsuarioPeloID(int id)
        {
            return _context.Usuarios.FirstOrDefault(a => a.Id == id);
        }

        public async Task<Usuario> GetUsuarioByUserAsync(User user)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(a => a.Username.Equals(user.Username) && a.Password.Equals(user.Password));
        }

        public async Task<Pessoa> GetPessoaLogadaAsync(ClaimsPrincipal User)
        {
            var _user = await GetUsuarioLogadoAsync(User);

            Pessoa pessoa = await _context.PessoasFisicas.FirstOrDefaultAsync(a => a.Usuario.Id == _user.Id);

            pessoa = pessoa ?? await _context.PessoasJuridicas.FirstOrDefaultAsync(a => a.Usuario.Id == _user.Id);

            return pessoa;
        }
    }
}
