using Agendado.Application.Dto;
using Agendado.Data;
using Agendado.Domain.Model;
using Agendado.Interface.Repository;
using Agendado.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

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
            await _context.SaveChangesAsync();
        }
        public async Task UpdateServicoAsync(Servicos servico)
        {
            _context.Update(servico);
            await _context.SaveChangesAsync();
        }
        public async Task<Servicos?> GetServicoByIdAsync(Guid id)
        {
            return await _context.Servicos.FirstOrDefaultAsync(f => f.Id == id);
        }
        public async Task DeleteServicoAsync(Servicos servico)
        {
            _context.Remove(servico);
            await _context.SaveChangesAsync();
        }

        public async Task<ResultadoPagincao<DadosServico>> GetServicosAsync(Guid empresaId, int page, int qtdPag)
        {
            var query = _context.Servicos.Where(f => f.EmpresaId == empresaId);

            var total = await query.CountAsync();

            var servicos = await query
                .Skip((page - 1) * qtdPag)
                .Take(qtdPag)
                .Select(s => new DadosServico
                (
                    s.Nome,
                    s.Descricao,
                    s.TempoDuracao,
                    s.Valor
                 
                )).ToListAsync();

            return new ResultadoPagincao<DadosServico>
            {
                Items = servicos,
                Page = page,
                QtdPage = qtdPag
            };
        }
    }
}
