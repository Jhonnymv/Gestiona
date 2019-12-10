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
            var cabecera = await _getInspeccionCabeceraInfoReadOnlyRepository.GetInspeccionCabecera(observacion.Id_Inspeccion_Cabecera);

            var output = new ObservacionImplementadaOutput();
            output.Id_Inspeccion_Cabecera = cabecera.IdCabecera;
            output.Id_Inspeccion_Detalle = observacion.Id_Inspeccion_Detalle;
            output.Area_Ejecutora = cabecera.GerenEjecMar;
            output.Empresa = cabecera.EmpContratMarc;
            output.Area_Inspeccionada = cabecera.LugarInspec;
            output.Tipo_Inspeccion = cabecera.Tipo_Inspeccion;
            output.Fecha_Inspeccion = cabecera.FechaInspec.ToString();
            output.Responsable_Lugar = cabecera.Responsable_Lugar.Nombres + " " + cabecera.Responsable_Lugar.Apellidos;
            output.ResponsableInspeccion = cabecera.Responsable_Inspeccion;
            output.Participantes = cabecera.Inspectores;
            output.Objetivo = cabecera.Objetivo;

            output.Tipo_Observacion = observacion.Tipo_Observacion;
            output.Asesor_SSO_MAP = observacion.Responsable_MA.Nombres + " " + observacion.Responsable_MA.Apellidos;
            output.Descripcion_Condicion = observacion.Descripcion_Condicion;
            output.Tipo_Condicion = observacion.Tipo_Condicion.Descripcion;
            output.Descripcion_Accion = observacion.Descripcion_Accion;
            if (observacion.Evidencia_Condicion != null)
            {
                var evidencias_condicion = JArray.Parse(observacion.Evidencia_Condicion);
                var listEvidencias_condicion = new List<string>();
                foreach (var evidencia in evidencias_condicion)
                {
                    listEvidencias_condicion.Add((string)evidencia);
                }
                output.Evidencia_Condicion = listEvidencias_condicion;
            }
            output.Nivel_Riesgo_Ambiental = observacion.Nivel_Riesgo_Ambiental;
            output.Descripcion_Accion = observacion.Descripcion_Accion;
            output.Responsable_Marcobre = observacion.Responsable_Marcobre.Nombres + " " + observacion.Responsable_Marcobre.Apellidos;
            output.Responsable_Contratista = observacion.Responsable_Contratista;
            output.Fecha_Programada = observacion.Fecha_Programada;
            output.Estado = observacion.Estado;

            output.Comentario_Implementacion = observacion.Comentario_Inspeccion;
            output.Fecha_Cierre = observacion.Fecha_Cierre.Value.ToString("dd/MM/yyyy");
            if (observacion.Evidencia_Inspeccion != null)
            {
                var evidencia_implementacion = JArray.Parse(observacion.Evidencia_Inspeccion);
                var listEvidencias_implementacion = new List<string>();
                foreach (var evidencia in evidencia_implementacion)
                {
                    listEvidencias_implementacion.Add((string)evidencia);
                }
                output.Evidencia_Implementacion = listEvidencias_implementacion;
            }


            //emails
            output.Email_Asesor_MAP_SSO = observacion.Responsable_MA.Email;

            return output;
        }
    }
}
