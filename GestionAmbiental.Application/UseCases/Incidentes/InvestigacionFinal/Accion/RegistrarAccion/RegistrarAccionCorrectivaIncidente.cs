using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.RegistrarAccion
{
    public class RegistrarAccionCorrectivaIncidente : IRegistrarAccionCorrectivaIncidenteInvestigacion
    {
        private readonly IRegistrarInvestigacionFinalReadOnlyRepository _registrarInvestigacionFinalReadOnlyRepository;

        public RegistrarAccionCorrectivaIncidente(IRegistrarInvestigacionFinalReadOnlyRepository registrarInvestigacionFinalReadOnlyRepository)
        {
            _registrarInvestigacionFinalReadOnlyRepository = registrarInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarInvestigacionFinalReadOnlyRepository));
        }

        public async Task<int> Execute(int IdInvestigacion,string tipoAccion, string AccionInmediata, int IdUsuario, int IdUsuario2, string Responsable, string Fecha, string Estatus)
        {
            var fechaAccion = Convert.ToDateTime(Fecha);

            var accion = new Accion_Incidente_Investigacion();
            accion.Id_Investigacion_Final = IdInvestigacion;
            accion.Accion_Inmediata = AccionInmediata;
            accion.Tipo_Accion = tipoAccion;
            if (IdUsuario != 0)
                accion.Id_Usuario = IdUsuario;
            if (IdUsuario2 != 0)
                accion.Id_Usuario2 = IdUsuario2;
            accion.Responsable = Responsable;
            accion.Fecha = fechaAccion;
            accion.Estatus = Estatus;

            return await _registrarInvestigacionFinalReadOnlyRepository.RegistrarAccionCorrectivaInvestigacion(accion);
        }
    }
}
