using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Reportes
{
    public interface IGetReportesInspeccionesReadOnlyRepository
    {
        Task<List<Dimension>> GetNInspecciones(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, int usuarioInvolucrado, string tipoObservacion);
        Task<List<Dimension>> GetInspeccionesXFecha(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string fecuencia, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion);
        Task<List<List<Dimension>>> GetObservacionesXFecha(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string frecuencia, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion);
        Task<List<Dimension>> GetNObservaciones(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion);
        Task<List<Dimension>> GetObsMAPvsSSO(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion);
        Task<List<List<Dimension>>> GetObsMAPvsSSO_Estado(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion);
        Task<List<Dimension>> GetBPMAPvsSSO(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion);
        Task<List<List<Dimension>>> GetBPMAPvsSSO_Estado(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion);
        Task<List<Dimension>> GetObsNivelRiesgoMA(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion);
        Task<List<Dimension>> GetObsNivelRiesgoSSO(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion);
        Task<List<Dimension>> GetObsClasificacionMA(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion);
        Task<List<Dimension>> GetObsClasificacionSSO(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion);
        Task<List<List<Dimension>>> GetObsTipoCondicionxNivelRiesgoMA(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion);
        Task<List<List<Dimension>>> GetObsTipoCondicionxNivelRiesgoSSO(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion);
        Task<List<Domain.Inspecciones.InspeccionCabecera>> GetDetalleObsxEstado(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion estado,bool buenaPractica, int usuarioInvolucrado, string tipoObservacion);
        Task<List<List<Dimension>>> GetGerenciaInspeccionadaxEstadoObs(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion);
    }
}
