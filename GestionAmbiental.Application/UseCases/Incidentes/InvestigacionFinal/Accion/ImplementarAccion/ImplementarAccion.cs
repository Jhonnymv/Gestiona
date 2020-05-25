using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.EditarAccion;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.ImplementarAccion
{
    public class ImplementarAccion : IImplementarAccion
    {
        private readonly IEditarInvestigacionFinalReadOnlyRepository _editarInvestigacionFinalReadOnlyRepository;

        public ImplementarAccion(IEditarInvestigacionFinalReadOnlyRepository editarInvestigacionFinalReadOnlyRepository)
        {
            _editarInvestigacionFinalReadOnlyRepository = editarInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarInvestigacionFinalReadOnlyRepository));
        }

        public async Task<int> Execute(int IdAccion,List<string> evidencias, string comentario)
        {
            var input = new Accion_Incidente_Investigacion();
            input.Id_Accion_Incidente_Investigacion = IdAccion;
            input.Evidencia = JsonConvert.SerializeObject(evidencias);
            input.Comentarios_Implementacion = comentario;
            input.Fecha_Implementacion = DateTime.Now;
            input.Estado = EstadosIncidentes.Implementado.ToString();

            return await _editarInvestigacionFinalReadOnlyRepository.ImplementarAccionCorrectivaInvestigacion(input);
        }
    }
}
