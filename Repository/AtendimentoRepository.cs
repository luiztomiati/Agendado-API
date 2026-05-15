using Agendado.Data;
using Agendado.Domain.Model;
using Agendado.Interface.Repository;

namespace Agendado.Repository
{
    public class AtendimentoRepository : IAtendimentoRepository
    {
        private readonly AppDbContext _context;

        public AtendimentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        } 
        public List<Atendimento> GetListById(List<Guid> ids)
        {
            return _context.Atendimentos.Where(w => ids.Contains(w.Id)).ToList();
        }
    }
}
