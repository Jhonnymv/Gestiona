using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetDetalleObsxEstado
{
    public class GetDetalleObsxEstado : IGetDetalleObsxEstado
    {
        private readonly IGetReportesInspeccionesReadOnlyRepository _getReportesInspeccionesReadOnlyRepository;
        public GetDetalleObsxEstado(IGetReportesInspeccionesReadOnlyRepository getReportesInspeccionesReadOnlyRepository)
        {
            _getReportesInspeccionesReadOnlyRepository = getReportesInspeccionesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesInspeccionesReadOnlyRepository));
        }
        public async Task<List<DetalleObsxEstadoOutput>> Execute(int empresa, int gerencia, int respLevObs, int Asesor, string FechaInicio, string FechaFin, string estado)
        {
            //Dar formato a fechas
            DateTime fInicio, fFinal;
            if (FechaInicio != null)
                fInicio = Convert.ToDateTime(FechaInicio);
            else
                fInicio = DateTime.MinValue;

            if (FechaFin != null)
                fFinal = Convert.ToDateTime(FechaFin + " 23:59:59");
            else
                fFinal = DateTime.MaxValue;
            //Obtener valores
            EstadosInspeccion enum_estado = EstadosInspeccion.Registrado;
            bool buenaPractica = false;
            switch (estado)
            {
                case "Reg": enum_estado = EstadosInspeccion.Registrado; break;
                case "Imp": enum_estado = EstadosInspeccion.Implementado; break;
                case "Ven": enum_estado = EstadosInspeccion.Vencido; break;
                case "Apr": enum_estado = EstadosInspeccion.Aprobado; break;
                case "Rec": enum_estado = EstadosInspeccion.Rechazado; break;
                case "Amp": enum_estado = EstadosInspeccion.Ampliado; break; 
                case "BImplementadas": enum_estado = EstadosInspeccion.Implementado; buenaPractica = true; break; 
                case "BAprobadas": enum_estado = EstadosInspeccion.Aprobado; buenaPractica = true; break; 
                case "BRechazadas": enum_estado = EstadosInspeccion.Rechazado; buenaPractica = true; break; 
            }      

            var data =  await _getReportesInspeccionesReadOnlyRepository.GetDetalleObsxEstado(empresa, gerencia, respLevObs, Asesor, fInicio, fFinal, enum_estado,buenaPractica);
            var output = new List<DetalleObsxEstadoOutput>();
            foreach(var item in data)
            {
                foreach(var subitem in item.Observaciones)
                {
                    var itemOutput = new DetalleObsxEstadoOutput();
                    itemOutput.Codigo = item.Codigo;
                    itemOutput.Id_Cabecera = item.IdCabecera;
                    itemOutput.Empresa = item.Empresa;
                    itemOutput.Gerencia = item.Gerencia;
                    itemOutput.Fecha_Inspeccion = item.FechaInspeccion.ToString("dd/MM/yyyy");
                    itemOutput.Tipo_Observacion = subitem.TipoObservacion;
                    itemOutput.Asesor = subitem.Asesor.Nombres + " " + subitem.Asesor.Apellidos;
                    itemOutput.Tipo_Condicion = subitem.TipoCondicion?.Descripcion;
                    itemOutput.Nivel_Riesgo = subitem.NivelRiesgo;
                    itemOutput.Responsable = subitem.ResponsableMarcobre.Nombres + " " + subitem.ResponsableMarcobre.Apellidos;
                    itemOutput.Fecha_Programada = subitem.FechaProgramada.HasValue? subitem.FechaProgramada.Value.ToString("dd/MM/yyyy"):"";
                    output.Add(itemOutput);
                }
            }
            return output;
        }
    }
}
