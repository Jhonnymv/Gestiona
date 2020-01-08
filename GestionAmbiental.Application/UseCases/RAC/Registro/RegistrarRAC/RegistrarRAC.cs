using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.RAC;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Registro.RegistrarRAC
{
    public class RegistrarRAC : IRegistrarRAC
    {
        private readonly IRegistrarRACReadOnlyRepository _registrarRACReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public RegistrarRAC(IRegistrarRACReadOnlyRepository registrarRACReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _registrarRACReadOnlyRepository = registrarRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarRACReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task<int> Execute(string nombres_apellidos, int gerencia, int empresa,
            string otra_emp, int empresa_obs, int gerencia_obs, string ubicacion,
            string fecha, string hora, string sso_ma, string clasificacion,
            string nivel_riesgo, int tipo_condicion, string descripcion, string medida,
            string pac, int responsable_cierre, string fecha_cierre, int supervisor, string evidencia,string usuario_red)
        {
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            var reporte = new Domain.RAC.RAC
            {
                Id_Registrado_Por = usuario.IdUsuario,
                Codigo = "CODIGO",
                Nombres_Apellidos = nombres_apellidos,
                Id_Gerencia = gerencia,
                Id_Empresa = empresa,
                Otra_Empresa = otra_emp,
                Id_Empresa_Obs = empresa_obs,
                Id_Gerencia_Obs = gerencia_obs,
                Ubicacion = ubicacion,
                Fecha = Convert.ToDateTime(fecha + " " + hora),
                SSO_MA = sso_ma,
                Clasificacion = clasificacion,
                NivelRiesgo = nivel_riesgo,
                Id_Tipo_Condicion = tipo_condicion,
                Descripcion_Acto_Cond = descripcion,
                Medida_Control = medida,
                PAC = pac,
                Id_Responsable_Cierre = responsable_cierre,
                Fecha_Cierre = Convert.ToDateTime(fecha_cierre),
                Id_Supervisor = supervisor,
                Evidencia_Obs = evidencia,
                Estado = EstadosRAC.Registrado.ToString()
            };
            return await _registrarRACReadOnlyRepository.RegistrarRAC(reporte);
        }
    }
}
