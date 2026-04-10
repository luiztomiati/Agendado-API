using Agendado.Data;
using Agendado.Dto;
using Agendado.Interface;
using Agendado.Model;
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
    }
}
