using GestionAmbiental.Application.Repositories.Notificaciones;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.ObservacionImplementada
{
    public class GetObservacionImplementada : IGetObservacionImplementada
    {
        private readonly IGetInspeccionCabeceraInfoReadOnlyRepository _getInspeccionCabeceraInfoReadOnlyRepository;
        private readonly IGetInspeccionDetalleInfoReadOnlyRepository _getInspeccionDetalleInfoReadOnlyRepository;

        public GetObservacionImplementada(IGetInspeccionCabeceraInfoReadOnlyRepository getInspeccionCabeceraInfoReadOnlyRepository, IGetInspeccionDetalleInfoReadOnlyRepository getInspeccionDetalleInfoReadOnlyRepository)
        {
            _getInspeccionCabeceraInfoReadOnlyRepository = getInspeccionCabeceraInfoReadOnlyRepository ?? throw new ArgumentNullException(nameof(getInspeccionCabeceraInfoReadOnlyRepository));
            _getInspeccionDetalleInfoReadOnlyRepository = getInspeccionDetalleInfoReadOnlyRepository ?? throw new ArgumentNullException(nameof(getInspeccionDetalleInfoReadOnlyRepository));
        }

        public async Task<ObservacionImplementadaOutput> Execute(int IdObservacion)
        {
            var observacion = await _getInspeccionDetalleInfoReadOnlyRepository.GetInspeccionDetalleImplementacion(IdObservacion);
            var cabecera = await _getInspeccionCabeceraInfoReadOnlyRepository.GetInspeccionCabecera(observacion.IdInspeccionCabecera);

            var output = new ObservacionImplementadaOutput();
            output.Codigo = cabecera.Codigo;
            output.IdInspeccionCabecera = cabecera.IdCabecera;
            output.IdInspeccionDetalle = observacion.IdInspeccionDetalle;
            output.AreaEjecutora = cabecera.Gerencia;
            output.Empresa = cabecera.Empresa;
            output.AreaInspeccionada = cabecera.AreaInspeccionada;
            output.TipoInspeccion = cabecera.TipoInspeccion;
            output.FechaInspeccion = cabecera.FechaInspeccion.ToString();
            output.ResponsableLugar = cabecera.ResponsableArea.Nombres + " " + cabecera.ResponsableArea.Apellidos;
            output.ResponsableInspeccion = cabecera.ResponsableInspeccion;
            output.Participantes = cabecera.Participantes;
            output.Objetivo = cabecera.Objetivo;

            output.TipoObservacion = observacion.TipoObservacion;
            output.Asesor = observacion.Asesor.Nombres + " " + observacion.Asesor.Apellidos;
            output.DescripcionCondicion = observacion.DescripcionCondicion;
            output.TipoCondicion = observacion.TipoCondicion.Descripcion;
            output.DescripcionAccion = observacion.DescripcionAccion;
            if (observacion.EvidenciaCondicion != null)
            {
                var evidencias_condicion = JArray.Parse(observacion.EvidenciaCondicion);
                var listEvidencias_condicion = new List<string>();
                foreach (var evidencia in evidencias_condicion)
                {
                    listEvidencias_condicion.Add((string)evidencia);
                }
                output.EvidenciaCondicion = listEvidencias_condicion;
            }
            output.NivelRiesgo = observacion.NivelRiesgo;
            output.DescripcionAccion = observacion.DescripcionAccion;
            output.ResponsableMarcobre = observacion.ResponsableMarcobre.Nombres + " " + observacion.ResponsableMarcobre.Apellidos;
            output.ResponsableContratista = observacion.ResponsableContratista;
            output.FechaProgramada = observacion.FechaProgramada;
            output.Estado = observacion.Estado;

            output.ComentarioImplementacion = observacion.ComentarioImplementacion;
            output.FechaCierre = observacion.FechaCierre.Value.ToString("dd/MM/yyyy");
            if (observacion.EvidenciaImplementacion != null)
            {
                var evidencia_implementacion = JArray.Parse(observacion.EvidenciaImplementacion);
                var listEvidencias_implementacion = new List<string>();
                foreach (var evidencia in evidencia_implementacion)
                {
                    listEvidencias_implementacion.Add((string)evidencia);
                }
                output.EvidenciaImplementacion = listEvidencias_implementacion;
            }


            //emails
            output.EmailAsesor = observacion.Asesor.Email;

            return output;
        }
    }
}
