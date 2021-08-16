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
            var cabecera = await _getInspeccionCabeceraInfoReadOnlyRepository.GetInspeccionCabecera(observacion.IdInspeccionCabecera);
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
            output.IdInspeccionCabecera = cabecera.IdCabecera;
            output.IdInspeccionDetalle = observacion.IdInspeccionDetalle;
            output.Codigo = cabecera.Codigo;
            output.GerenciaInspeccionada = cabecera.GerenciaInspeccionada;
            output.AreaInspeccionada = cabecera.AreaInspeccionada;
            output.FechaInspeccion = cabecera.FechaInspeccion.ToString();
            output.ResponsableLugar = cabecera.ResponsableArea.Nombres +" " + cabecera.ResponsableArea.Apellidos;
            output.Participantes = cabecera.Participantes;
            output.Objetivo = cabecera.Objetivo;
            output.DescripcionCondicion = observacion.DescripcionCondicion;
            output.DescripcionActoCondicion = observacion.DescripcionActoCondicion;
            output.TipoCondicion = observacion.TipoCondicion?.Descripcion;
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
            output.ResponsableMarcobre = observacion.ResponsableMarcobre.Nombres+ " "+ observacion.ResponsableMarcobre.Apellidos;
            output.ResponsableContratista = observacion.ResponsableContratista;
            output.FechaProgramada = observacion.FechaProgramada;
            output.Estado = observacion.Estado;

            //emails
            output.EmailResponsableMarcobre = observacion.ResponsableMarcobre.Email;

            return output;

        }
    }
}
