using Agendado.Application.DTOs;
using Agendado.Data;
using Agendado.Interface.Repository;
using Agendado.Shared;
using Microsoft.EntityFrameworkCore;

namespace Agendado.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultadoPagincao<DadosCliente>> ListClientesAsync(Guid empresaId, int page, int qtdPag)
        {
            var query = _context.Cliente.Where(w => w.EmpresaId ==  empresaId);

            var quantidade = await query.CountAsync();

            var clientes = await query.Select(s => new DadosCliente (
                s.Nome,
                s.DDD,
                s.Telefone,
                s.Email
                )).Skip((page - 1) *  qtdPag).Take(qtdPag).ToListAsync();

            return new ResultadoPagincao<DadosCliente>
            {
                Items = clientes,
                Page = page,
                QtdPage = qtdPag,
                TotalCount = quantidade
            };
                
        }
    }
}
