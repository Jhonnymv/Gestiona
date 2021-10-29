using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.EditarObservacion
{
    public class EditarObservacion : IEditarObservacion
    {
        private readonly IEditarInspeccionReadOnlyRepository _editarInspeccionReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public EditarObservacion(IEditarInspeccionReadOnlyRepository editarInspeccionReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _editarInspeccionReadOnlyRepository = editarInspeccionReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarInspeccionReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(int Id_Inspeccion_Cabecera,int Id_Observacion, string Tipo_Inspeccion, int Asesor_MA_Seguridad, string Descripcion_Acto_Condicion, string Descripcion_Condicion,
            int? Id_Tipo_Condicion, List<string> Evidencia_Condicion, string Nivel_Riesgo_Ambiental, string Descripcion_Accion, 
            int Id_Responsable_Marcobre, string Responsable_Contratista, string Correo_Contratista, string Fecha_Programada, string usuario_red,
            bool Cambio_Fecha_Vencida)
        {
            var evidencia = JsonConvert.SerializeObject(Evidencia_Condicion);

            var detalle = new NuevaInspeccionDetalle
            {
                IdInspeccionCabecera = Id_Inspeccion_Cabecera,
                IdInspeccionDetalle = Id_Observacion,
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
                //Estado = EstadosInspeccion.Registrado.ToString()                
            };

            if (Fecha_Programada!=null && Fecha_Programada.Length > 0)
                detalle.FechaProgramada = Convert.ToDateTime(Fecha_Programada);

            if (Cambio_Fecha_Vencida)
                detalle.Estado = EstadosInspeccion.Ampliado.ToString();

            var Id_Usuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuario_red);
            await _editarInspeccionReadOnlyRepository.EditarInspeccionDetalle(detalle, Id_Usuario);
        }
    }
}
