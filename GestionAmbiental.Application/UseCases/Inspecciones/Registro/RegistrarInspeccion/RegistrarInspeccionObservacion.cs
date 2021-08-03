using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.RegistrarInspeccion
{
    public class RegistrarInspeccionObservacion : IRegistrarInspeccionObservacion
    {
        private readonly IRegistarInspeccionReadOnlyRepository _registarInspeccionReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public RegistrarInspeccionObservacion(IRegistarInspeccionReadOnlyRepository registarInspeccionReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _registarInspeccionReadOnlyRepository = registarInspeccionReadOnlyRepository ?? throw new ArgumentNullException(nameof(registarInspeccionReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task<int> Execute(
            int Id_Inspeccion_Cabecera, string Tipo_Inspeccion, int Asesor_MA_Seguridad, string Descripcion_Acto_Condicion, string Descripcion_Condicion, int? Id_Tipo_Condicion, List<string> Evidencia_Condicion,
            string Nivel_Riesgo_Ambiental, string Descripcion_Accion, int Id_Responsable_Marcobre, string Responsable_Contratista, string Correo_Contratista,
            string Fecha_Programada, string usuario_red)
        {
            var evidencia = JsonConvert.SerializeObject(Evidencia_Condicion);

            var detalle = new NuevaInspeccionDetalle
            {
                IdInspeccionCabecera = Id_Inspeccion_Cabecera,
                TipoInspeccion = Tipo_Inspeccion,
                IdAsesorMASeguridad = Asesor_MA_Seguridad,
                DescripcionActoCondicion = Descripcion_Acto_Condicion,
                DescripcionCondicion = Descripcion_Condicion,
                IdTipoCondicion = Id_Tipo_Condicion,
                NivelRiesgo = Nivel_Riesgo_Ambiental,
                EvidenciaCondicion = evidencia,
                DescripcionAccion = Descripcion_Accion,
                IdResponsableMarcobre = Id_Responsable_Marcobre,
                ResponsableContratista = Responsable_Contratista,
                CorreoContratista = Correo_Contratista,
            };
            if (Fecha_Programada != null && Fecha_Programada.Length > 0)
                detalle.FechaProgramada = Convert.ToDateTime(Fecha_Programada);
            var Id_Usuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuario_red);

            return await _registarInspeccionReadOnlyRepository.RegistrarInspeccionDetalle(detalle, Id_Usuario);
        }
    }
}
