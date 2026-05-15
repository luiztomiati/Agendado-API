using Agendado.Data;
using Agendado.Domain.Entities;
using Agendado.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Agendado.Repository
{
    public class EmpresaAtendimentoRepository : IEmpresaAtendimentoRepository
    {
        private readonly AppDbContext _context;

        public EmpresaAtendimentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<EmpresaFuncionamento> GetByEmpresaId(Guid empresaId)
        {
            return _context.EmpresaFuncionamentos
                .Where(w => w.EmpresaId == empresaId)
                .ToList();
        }
        public List<EmpresaFuncionamento> GetById(List<Guid> id)
        {
            return _context.EmpresaFuncionamentos.Where(w => id.Contains(w.Id)).ToList();
        }
    }
}
