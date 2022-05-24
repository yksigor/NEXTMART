using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Services.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class PessoaJuridicaServices : IPessoaJuridicaServices
    {
        private readonly LojaDBContext _context;

        public PessoaJuridicaServices(LojaDBContext context)
        {
            _context = context;
        }

        public async Task<List<PessoaJuridica>> FindAllAsync()
        {
            return await _context.PessoasJuridicas.ToListAsync();
        }

        public async Task<PessoaJuridica> FindByIdAsync(int id)
        {
            return await _context.PessoasJuridicas.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PessoaJuridica> GetPessoaJuridicaByIdUsuarioAsync(int idUsuario)
        {
            return await _context.PessoasJuridicas.FirstOrDefaultAsync(m => m.IdUsuario == idUsuario);
        }

        public async Task InsertAsync(PessoaJuridica pessoaJuridica)
        {
            _context.Add(pessoaJuridica);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.PessoasJuridicas.FindAsync(id);
            _context.PessoasJuridicas.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PessoaJuridica pessoaJuridica)
        {
            var pj = _context.PessoasJuridicas.FirstOrDefault(p => p.Id == pessoaJuridica.Id);

            pj.Cnpj = pessoaJuridica.Cnpj;

            _context.PessoasJuridicas.Update(pj);

            await _context.SaveChangesAsync();
        }
    }
}
