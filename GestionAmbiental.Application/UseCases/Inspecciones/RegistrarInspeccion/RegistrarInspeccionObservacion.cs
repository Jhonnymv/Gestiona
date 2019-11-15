using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.RegistrarInspeccion
{
    public class RegistrarInspeccionObservacion : IRegistrarInspeccionObservacion
    {
        private readonly IRegistarInspeccionReadOnlyRepository _registarInspeccionReadOnlyRepository;

        public RegistrarInspeccionObservacion(IRegistarInspeccionReadOnlyRepository registarInspeccionReadOnlyRepository)
        {
            _registarInspeccionReadOnlyRepository = registarInspeccionReadOnlyRepository ?? throw new ArgumentNullException(nameof(registarInspeccionReadOnlyRepository));
        }

        public async Task<int> Execute(
            int Id_Inspeccion_Cabecera, string Tipo_Inspeccion, int Asesor_MA_Seguridad, string Descripcion_Condicion, int Id_Tipo_Condicion, List<string> Evidencia_Condicion, 
            string Nivel_Riesgo_Ambiental, string Descripcion_Accion, int Id_Responsable_Marcobre, string Responsable_Contratista, 
            string Fecha_Programada)
        {
            var evidencia = JsonConvert.SerializeObject(Evidencia_Condicion);

            var detalle = new NuevaInspeccionDetalle
            {
                Id_Inspeccion_Cabecera = Id_Inspeccion_Cabecera,
                Tipo_Inspeccion = Tipo_Inspeccion,
                Id_Asesor_MA_Seguridad = Asesor_MA_Seguridad,
                Descripcion_Condicion = Descripcion_Condicion,
                Id_Tipo_Condicion = Id_Tipo_Condicion,
                Nivel_Riesgo_Ambiental = Nivel_Riesgo_Ambiental,
                Evidencia_Condicion = evidencia,
                Descripcion_Accion = Descripcion_Accion,
                Id_Responsable_Marcobre = Id_Responsable_Marcobre,
                Responsable_Contratista = Responsable_Contratista,
                Fecha_Programada = Convert.ToDateTime(Fecha_Programada)
            };

            return await _registarInspeccionReadOnlyRepository.RegistrarInspeccionDetalle(detalle);
        }
    }
}
