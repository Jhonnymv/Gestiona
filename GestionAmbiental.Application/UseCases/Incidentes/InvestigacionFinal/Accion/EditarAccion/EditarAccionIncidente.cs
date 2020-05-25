using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.EditarAccion
{
    public class EditarAccionIncidente : IEditarAccionIncidenteInvestigacion
    {
        private readonly IEditarInvestigacionFinalReadOnlyRepository _editarInvestigacionFinalReadOnlyRepository;

        public EditarAccionIncidente(IEditarInvestigacionFinalReadOnlyRepository editarInvestigacionFinalReadOnlyRepository)
        {
            _editarInvestigacionFinalReadOnlyRepository = editarInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarInvestigacionFinalReadOnlyRepository));
        }

        public async Task<int> Execute(int IdAccion, int IdInvestigacion,string tipoAccion, int IdUsuario, string AccionInmediata, string Fecha, string Responsable, string Estatus)
        {
            var fechaAccion = Convert.ToDateTime(Fecha);

            var accion = new Accion_Incidente_Investigacion();
            accion.Id_Accion_Incidente_Investigacion = IdAccion;
            accion.Id_Investigacion_Final = IdInvestigacion;
            accion.Tipo_Accion = tipoAccion;
            accion.Accion_Inmediata = AccionInmediata;
            if (IdUsuario != 0)
                accion.Id_Usuario = IdUsuario;
            accion.Responsable = Responsable;
            accion.Fecha = fechaAccion;
            accion.Estatus = Estatus;
            accion.Estado = EstadosIncidentes.Registrado.ToString();

            return await _editarInvestigacionFinalReadOnlyRepository.EditarAccionCorrectivaInvestigacion(accion);
        }
    }
}
