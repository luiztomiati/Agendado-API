using Agendado.Application.DTOs;
using Agendado.Domain.Model;

namespace Agendado.Interface.Service
{
    public interface IAtendimentoService
    {
        Task CriarHorarioAtendimentoEmpresa(List<DadosHorarioEmpresa> dados, Empresa empresa);
        Task HorarioAtendimentoPadrao(Usuario usuario);
        Task CriarAtendimentoUsuario(Empresa empresa, Usuario usuario);
        Task EditarHorarioAtendimento(UsuarioHorarioAtendimentoDTO dados);
    }
}
