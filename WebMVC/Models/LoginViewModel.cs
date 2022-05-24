using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models
{
    public class LoginViewModel
    {
        public List<TipoUsuario> ListaTipos => Enum.GetValues(typeof(TipoUsuario)).Cast<TipoUsuario>()
                .Where(a => a != TipoUsuario.Administrador && a != TipoUsuario.Entregador).ToList();

        public PessoaFisica PessoaFisica { get; set; }
        public PessoaJuridica PessoaJuridica { get; set; }
        public Usuario Usuario { get; set; }
        public Endereco Endereco { get; set; }
    }
}
