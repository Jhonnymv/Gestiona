using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.AmpliarFechaAccion
{
    public class AmpliarFechaAccion : IAmpliarFechaAccion
    {
        private readonly IEditarInvestigacionFinalReadOnlyRepository _editarInvestigacionFinalReadOnlyRepository;

        public AmpliarFechaAccion(IEditarInvestigacionFinalReadOnlyRepository editarInvestigacionFinalReadOnlyRepository)
        {
            _editarInvestigacionFinalReadOnlyRepository = editarInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarInvestigacionFinalReadOnlyRepository));
        }

        public async Task<int> Execute(int IdAccion, string Fecha)
        {
            var fechaAccion = Convert.ToDateTime(Fecha);

            var accion = new Accion_Incidente_Investigacion();
            accion.Id_Accion_Incidente_Investigacion = IdAccion;
            accion.Fecha = fechaAccion;
            accion.Estado = EstadosIncidentes.Ampliado.ToString();

            return await _editarInvestigacionFinalReadOnlyRepository.AmpliarFechaAccion(accion);
        }
    }
}
