using Agendado.Data;
using Agendado.Domain.Model;
using Agendado.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Agendado.Repository
{

    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly AppDbContext _context;

        public EmpresaRepository(AppDbContext context)
        {
            this._context = context;
        }

        public void CriarEmpresa (Empresa empresa)
        {
            _context.Add(empresa);
        }
        public async Task<Empresa?> GetEmpresaByIdAsync(Guid id)
        {
            return await _context.Empresas.FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
