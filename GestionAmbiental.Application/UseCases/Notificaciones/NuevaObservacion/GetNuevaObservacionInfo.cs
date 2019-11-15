using GestionAmbiental.Application.Repositories.Notificaciones;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.NuevaObservacion
{
    public class GetNuevaObservacionInfo : IGetNuevaObservacionInfo
    {
        private readonly IGetInspeccionCabeceraInfoReadOnlyRepository _getInspeccionCabeceraInfoReadOnlyRepository;
        private readonly IGetInspeccionDetalleInfoReadOnlyRepository _getInspeccionDetalleInfoReadOnlyRepository;

        public GetNuevaObservacionInfo(IGetInspeccionCabeceraInfoReadOnlyRepository getInspeccionCabeceraInfoReadOnlyRepository, IGetInspeccionDetalleInfoReadOnlyRepository getInspeccionDetalleInfoReadOnlyRepository)
        {
            _getInspeccionCabeceraInfoReadOnlyRepository = getInspeccionCabeceraInfoReadOnlyRepository ?? throw new ArgumentNullException(nameof(getInspeccionCabeceraInfoReadOnlyRepository));
            _getInspeccionDetalleInfoReadOnlyRepository = getInspeccionDetalleInfoReadOnlyRepository ?? throw new ArgumentNullException(nameof(getInspeccionDetalleInfoReadOnlyRepository));
        }

        public async Task<NuevaObservacionOutput> Execute(int IdObservacion)
        {
            var observacion = await _getInspeccionDetalleInfoReadOnlyRepository.GetInspeccionDetalle(IdObservacion);
            var cabecera = await _getInspeccionCabeceraInfoReadOnlyRepository.GetInspeccionCabecera(observacion.Id_Inspeccion_Cabecera);
            /*
        public string Tipo_Condicion { get; set; }
        public List<string> Evidencia_Condicion { get; set; }
        public string Nivel_Riesgo_Ambiental { get; set; }
        public string Descripcion_Accion { get; set; }
        public string Responsable_Marcobre { get; set; }
        public string Responsable_Contratista { get; set; }
        public DateTime Fecha_Programada { get; set; }
        public string Estado { get; set; }
             */

            var output = new NuevaObservacionOutput();
            output.Id_Inspeccion_Cabecera = cabecera.IdCabecera;
            output.Id_Inspeccion_Detalle = observacion.Id_Inspeccion_Detalle;
            output.Codigo = cabecera.Codigo;
            output.Gerencia_Inspeccionada_Marcobre = cabecera.GerenEjecMar;
            output.Empresa_Contratista_Marcobre = cabecera.EmpContratMarc;
            output.Gerencia_Inspeccionada_EPCM = cabecera.GerenEjecEPCM;
            output.Empresa_Contratista_EPCM = cabecera.EmpContratEPCM;
            output.Lugar_Inspeccionado = cabecera.LugarInspec;
            output.Fecha_Inspeccion = cabecera.FechaInspec.ToString();
            output.Responsable_Lugar = cabecera.Responsable_Lugar.Nombres +" " + cabecera.Responsable_Lugar.Apellidos;
            output.Inspectores = cabecera.Inspectores;
            output.Objetivo = cabecera.Objetivo;
            output.Descripcion_Condicion = observacion.Descripcion_Condicion;
            output.Tipo_Condicion = observacion.Tipo_Condicion.Descripcion;
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
            output.Responsable_Marcobre = observacion.Responsable_Marcobre.Nombres+ " "+ observacion.Responsable_Marcobre.Apellidos;
            output.Responsable_Contratista = observacion.Responsable_Contratista;
            output.Fecha_Programada = observacion.Fecha_Programada;
            output.Estado = observacion.Estado;

            //emails
            output.Email_Responsable_Marcobre = observacion.Responsable_Marcobre.Email;

            return output;

        }
    }
}
