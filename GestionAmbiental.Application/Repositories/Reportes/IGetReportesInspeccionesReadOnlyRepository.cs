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
        Task<List<Dimension>> GetNInspecciones(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetInspeccionesXFecha(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string fecuencia);
        Task<List<List<Dimension>>> GetObservacionesXFecha(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string frecuencia,EstadosInspeccion? estado);
        Task<List<Dimension>> GetNObservaciones(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado);
        Task<List<Dimension>> GetObsMAPvsSSO(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado);
        Task<List<List<Dimension>>> GetObsMAPvsSSO_Estado(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado);
        Task<List<Dimension>> GetBPMAPvsSSO(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado);
        Task<List<List<Dimension>>> GetBPMAPvsSSO_Estado(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado);
        Task<List<Dimension>> GetObsNivelRiesgoMA(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado);
        Task<List<Dimension>> GetObsNivelRiesgoSSO(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado);
        Task<List<Dimension>> GetObsClasificacionMA(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado);
        Task<List<Dimension>> GetObsClasificacionSSO(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado);
        Task<List<List<Dimension>>> GetObsTipoCondicionxNivelRiesgoMA(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado);
        Task<List<List<Dimension>>> GetObsTipoCondicionxNivelRiesgoSSO(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado);
        Task<List<Domain.Inspecciones.InspeccionCabecera>> GetDetalleObsxEstado(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion estado,bool buenaPractica);
    }
}
