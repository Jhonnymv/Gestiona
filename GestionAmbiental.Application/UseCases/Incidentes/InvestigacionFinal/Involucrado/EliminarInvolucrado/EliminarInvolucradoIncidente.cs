using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Involucrado.EliminarInvolucrado
{
    public class EliminarInvolucradoIncidente : IEliminarInvolucradoIncidente
    {
        private readonly IEditarInvestigacionFinalReadOnlyRepository _editarReportePreliminarReadOnlyRepository;

        public EliminarInvolucradoIncidente(IEditarInvestigacionFinalReadOnlyRepository editarReportePreliminarReadOnlyRepository)
        {
            _editarReportePreliminarReadOnlyRepository = editarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarReportePreliminarReadOnlyRepository));
        }

        public async Task Execute(int IdInvolucradoIncidente)
        {
            var input = new Involucrado_Incidente_Investigacion();
            input.Id_Involucrado_Incidente_Investigacion = IdInvolucradoIncidente;
            input.Estado = EstadosIncidentes.Eliminado.ToString();
            await _editarReportePreliminarReadOnlyRepository.EditarInvolucradoInvestigacion(input);
        }
    }
}
