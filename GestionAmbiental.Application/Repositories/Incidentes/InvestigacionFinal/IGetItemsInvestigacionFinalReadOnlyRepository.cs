using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal
{
    public interface IGetItemsInvestigacionFinalReadOnlyRepository
    {
        Task<List<Producto_Derramado_Incidente>> GetProductosDerramados(int IdInvestigacion);
        Task<List<Involucrado_Incidente_Investigacion>> GetInvolucradosInvestigacion(int IdInvestigacion);
        Task<List<Accion_Incidente_Investigacion>> GetAccionesInvestigacion(int IdInvestigacion);
        Task<List<Causa_Incidente_Investigacion>> GetCausasInvestigacion(int IdInvestigacion);
        Task<Producto_Derramado_Incidente> GetProductoDerramado(int IdProducto);
        Task<Accion_Incidente_Investigacion> GetAccionInvestigacion(int IdAccionCorrectiva);
        Task<Causa_Incidente_Investigacion> GetCausaInvestigacion(int IdCausaIncidente);
        Task<Involucrado_Incidente_Investigacion> GetInvolucradoInvestigacion(int IdInvolucradoIncidente);
    }
}
