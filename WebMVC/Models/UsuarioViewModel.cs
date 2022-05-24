using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models
{
    public class UsuarioViewModel : Usuario
    {
        public List<TipoUsuario> ListaTipos => Enum.GetValues(typeof(TipoUsuario)).Cast<TipoUsuario>()
                .Where(a => a != Domain.Models.TipoUsuario.Administrador).ToList();
    }
}
