using Agendado.Data;
using Agendado.Interface.Repository;
using Agendado.Model;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Agendado.Repository
{
    public class ServicoRepository : IServicoRepository
    {
        private readonly AppDbContext _context;

        public ServicoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SalvarServicoAsync(Servicos servico)
        {
            _context.Add(servico);
            _context.SaveChanges();
        }
        public async Task UpdateServicoAsync(Servicos servico)
        {
            _context.Update(servico);
            _context.SaveChanges();
        }
        public async Task<Servicos?> GetServicoByIdAsync(Guid id)
        {
            return _context.Servicos.FirstOrDefault(f => f.Id == id);
        }
        public async Task DeleteServicoAsync(Servicos servico)
        {
            _context.Remove(servico);
            _context.SaveChanges();
        }
    }
}
