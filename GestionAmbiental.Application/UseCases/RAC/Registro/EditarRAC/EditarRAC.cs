using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.RAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Registro.EditarRAC
{
    public class EditarRAC : IEditarRAC
    {
        private readonly IEditarObservacionRACReadOnlyRepository _editarObservacionRACReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public EditarRAC(IEditarObservacionRACReadOnlyRepository editarObservacionRACReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _editarObservacionRACReadOnlyRepository = editarObservacionRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarObservacionRACReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(int Id_RAC, string Nombres_Apellidos, int? Id_Gerencia, int? Id_Empresa, string Otra_Empresa, int Id_Empresa_Obs, int Id_Gerencia_Obs, string Ubicacion, string Fecha, string SSO_MA, string Clasificacion, string NivelRiesgo,
            int Id_Tipo_Condicion, string Descripcion_Acto_Cond, string Medida_Control, string PAC, int Id_Responsable_Cierre, string Fecha_Cierre, int Id_Supervisor, string Evidencia_Obs, string Evidencia_Cierre, string Comentario_Cierre, string Comentario_Verificacion,string usuarioRed)
        {
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuarioRed);
            var input = new Domain.RAC.RAC
            {
                Id_RAC = Id_RAC,
                Nombres_Apellidos = Nombres_Apellidos,
                Id_Gerencia = Id_Gerencia,
                Id_Empresa = Id_Empresa,
                Otra_Empresa = Otra_Empresa,
                Id_Empresa_Obs = Id_Empresa_Obs,
                Id_Gerencia_Obs = Id_Gerencia_Obs,
                Ubicacion = Ubicacion,
                SSO_MA = SSO_MA,
                Clasificacion = Clasificacion,
                NivelRiesgo = NivelRiesgo,
                Id_Tipo_Condicion = Id_Tipo_Condicion,
                Descripcion_Acto_Cond = Descripcion_Acto_Cond,
                Medida_Control = Medida_Control,
                PAC = PAC,
                Id_Responsable_Cierre = Id_Responsable_Cierre,
                Id_Supervisor = Id_Supervisor,
                Evidencia_Obs = Evidencia_Obs,
                Evidencia_Cierre = Evidencia_Cierre,
                Comentario_Cierre = Comentario_Cierre,
                Comentario_Verificacion = Comentario_Verificacion
            };
            if (Fecha != null)
                input.Fecha = Convert.ToDateTime(Fecha);
            if (Fecha_Cierre != null)
                input.Fecha_Cierre = Convert.ToDateTime(Fecha_Cierre);
            await _editarObservacionRACReadOnlyRepository.EditarRAC(input,usuario.IdUsuario);
        }
    }
}
