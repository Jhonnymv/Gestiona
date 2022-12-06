using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Domain.CompromisosMCB;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB
{
    public interface IGetItemsReadOnlyRepository
    {
        Task<List<Domain.CompromisosMCB.AreaCMCB>> GetAreas();
        Task<List<Domain.CompromisosMCB.AspectoCMCB>> GetAspectos();
        Task<List<Domain.CompromisosMCB.AutoridadCMCB>> GetAutoridades();
        Task<List<Domain.CompromisosMCB.ComponenteCMCB>> GetComponentes();
        Task<List<Domain.CompromisosMCB.CriterioCMCB>> GetCriterios();
        Task<List<Domain.CompromisosMCB.EtapaCMCB>> GetEtapas();
        Task<List<Domain.CompromisosMCB.GerenciaCMCB>> GetGerencias();
        Task<List<Domain.CompromisosMCB.GerenciaUsuarioCMCB>> GetGerenciasUsuario();
        Task<List<Domain.CompromisosMCB.MedioVerificacionCMCB>> GetMediosVerificacion();
        Task<List<Domain.CompromisosMCB.OrigenCMCB>> GetOrigenes();
        Task<List<Domain.ValueObjects.Usuario>> GetUsuarios();

        Task<List<Domain.CompromisosMCB.TipoCompromisoCMCB>> GetTipos();
        Task<List<Domain.CompromisosMCB.ComponenteCMCB>> GestComponentesAux();
        Task<List<Domain.CompromisosMCB.FactorAmbientalCMCB>> GetFactorAmbientals();
    }
}
