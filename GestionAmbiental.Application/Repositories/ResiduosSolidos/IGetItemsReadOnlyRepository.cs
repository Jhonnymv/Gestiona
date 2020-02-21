using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.ResiduosSolidos
{
    public interface IGetItemsReadOnlyRepository
    {
        Task<List<ACR_CMRS>> GetACR_CMRS();
        Task<List<EO_RS>> GetEO_RS();
        Task<List<Ingreso_Salida>> GetIngreso_Salida();
        Task<List<Motivo_Ingreso_Salida>> GetMotivos_Ingreso_Salida();
        Task<List<Residuo_Solido>> GetResiduos_Solidos();
        Task<List<Tipo_Residuo>> GetTipos_Residuo();
        Task<List<Usuario>> GetUsuarios();
    }
}
