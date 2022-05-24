using Domain.Models;
using Domain.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Contratos
{
    public interface ILoginServicesJWT
    {
        Token GerarToken(Usuario request);
    }
}
