using Agendado.Application.DTOs;
using Agendado.Domain.Entities;
using Agendado.Domain.Model;
using Agendado.Interface.Repository;
using Agendado.Interface.Service;
using Agendado.Repository;
using System.Security.Claims;

namespace Agendado.Service
{
    public class AtendimentoService : IAtendimentoService
    {
        private readonly IEmpresaAtendimentoRepository _empresaAtendimentoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAtendimentoRepository _atendimentoRepository;


        public AtendimentoService(IEmpresaAtendimentoRepository empresaAtendimentoRepository, IUsuarioRepository usuarioRepository, IHttpContextAccessor httpContextAccessor, IAtendimentoRepository atendimentoRepository)
        {
            _empresaAtendimentoRepository = empresaAtendimentoRepository;
            _usuarioRepository = usuarioRepository;
            _httpContextAccessor = httpContextAccessor;
            _atendimentoRepository = atendimentoRepository;
        }

        public async Task HorarioAtendimentoPadrao(Usuario usuario) {
            var atendimentoPadrao = _empresaAtendimentoRepository.GetByEmpresaId(usuario.EmpresaId) ?? throw new Exception("Não foi encontrado horario de funcionamento da empresa");
            var horarios = atendimentoPadrao.Select(h => new Atendimento
            {
                DiaSemana = h.DiaSemana,
                HoraInicio = h.HoraInicio,
                HoraFim = h.HoraFim,
                UsuarioId = usuario.Id
            }).ToList();
            usuario.Atendimentos = horarios;
        }

        public async Task CriarHorarioAtendimentoEmpresa(List<DadosHorarioEmpresa> dados, Empresa empresa)
        {
            if (!dados.Any()) throw new Exception("Horarios de funcionamento deve estar preenchido");
            
            var horarios = dados.Select(h => new EmpresaFuncionamento
            {
                DiaSemana = h.DiaSemana,
                HoraInicio = h.HoraInicio,
                HoraFim = h.HoraFim,
            }).ToList();

            foreach (var horario in horarios)
            {
                if (horario.HoraInicio > horario.HoraFim)
                {
                    throw new Exception("Horário de abertura não pode ser maior que o fechamento");
                }
            }
            empresa.Horarios = horarios;
        }
        public async Task CriarAtendimentoUsuario(Empresa empresa, Usuario usuario)
        {
            var horarios = empresa.Horarios.Select(s => new Atendimento
            {
                HoraInicio = s.HoraInicio,
                HoraFim = s.HoraFim,
                DiaSemana = s.DiaSemana,
                UsuarioId = usuario.Id
            }).ToList();
            usuario.Atendimentos = horarios;
        }

        public async Task EditarHorarioAtendimento(UsuarioHorarioAtendimentoDTO dados)
        {
            var userId = _httpContextAccessor.HttpContext?.User
               .FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? throw new Exception("Usuário não encontrado");

            var usuarioLogado = await _usuarioRepository.GetIdentityUserAsync(userId)
                ?? throw new Exception("Usuário logado não encontrado");

            var atendimentoEmpresa = _empresaAtendimentoRepository.GetByEmpresaId(usuarioLogado.EmpresaId) ?? throw
            new Exception("Atendimento não encontrado");

            List<Guid> ids = dados.Atendimentos.Select(s => s.Id).ToList();
            var atendimentoAtual = _atendimentoRepository.GetListById(ids);

            await ValidarHorarioAtendimento(dados.Atendimentos, atendimentoEmpresa);

            foreach (var item in atendimentoAtual)
            {

                var db = dados.Atendimentos.First(f => f.Id == item.Id);
                item.HoraInicio = db.HoraInicio;
                item.HoraFim = db.HoraFim;
            }
            _atendimentoRepository.Save();
        }
        private async Task ValidarHorarioAtendimento(List<AtendimentoDTO> dados, List<EmpresaFuncionamento> atendimentoEmpresa)
        {
            if (dados.Count == 0) throw new Exception("Horários de atendimento precisam estar preenchidos");

            foreach (var atendimentoUsuario in dados)
            {
                var diaAtendimentoEmpresa = atendimentoEmpresa.FirstOrDefault(f => f.DiaSemana == atendimentoUsuario.DiaSemana) == null;
                if (diaAtendimentoEmpresa)
                {
                    throw new Exception("Empresa não possui atendimento neste dia");
                }
                if (diaAtendimentoEmpresa && atendimentoEmpresa
                    .FirstOrDefault(f => f.HoraInicio > atendimentoUsuario.HoraInicio
                    || f.HoraFim < atendimentoUsuario.HoraFim) == null)
                {
                    throw new Exception("O horário de atendimento do usuário deve estar dentro do horário de funcionamento da empresa.");
                }
            }
        }
    }
}
