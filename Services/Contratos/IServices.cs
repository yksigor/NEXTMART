using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contratos
{
    interface IServices
    {
        Task<List<T>> FindAllAsync<T>();

        Task<T> FindByIdAsync<T>(int id);
    }
}
