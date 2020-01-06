using GestionAmbiental.Application.Repositories.RAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Registro.RegistrarRAC
{
    public class RegistrarRAC : IRegistrarRAC
    {
        private readonly IRegistrarRACReadOnlyRepository _registrarRACReadOnlyRepository;

        public RegistrarRAC(IRegistrarRACReadOnlyRepository registrarRACReadOnlyRepository)
        {
            _registrarRACReadOnlyRepository = registrarRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarRACReadOnlyRepository));
        }

        public async Task<int> Execute(
            string codigo, string nombres_apellidos, int gerencia, int empresa,
            string otra_emp, int empresa_obs, int gerencia_obs, string ubicacion,
            string fecha, string hora, string sso_ma, string clasificacion,
            string nivel_riesgo, int tipo_condicion, string descripcion, string medida,
            string pac, int responsable_cierre, int supervisor, string evidencia)
        {
            var reporte = new Domain.RAC.RAC
            {
                Codigo = codigo,
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
                Id_Supervisor = supervisor,
                Evidencia_Obs = evidencia
            };
            return await _registrarRACReadOnlyRepository.RegistrarRAC(reporte);
        }
    }
}
