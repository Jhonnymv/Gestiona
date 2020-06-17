using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.ImplementarAccionMA
{
    public class ImplementarAccionMA : IImplementarAccionMA
    {
        private readonly IEditarInvestigacionFinalReadOnlyRepository _editarInvestigacionFinalReadOnlyRepository;

        public ImplementarAccionMA(IEditarInvestigacionFinalReadOnlyRepository editarInvestigacionFinalReadOnlyRepository)
        {
            _editarInvestigacionFinalReadOnlyRepository = editarInvestigacionFinalReadOnlyRepository;
        }

        public async Task<int> Execute(int IdAccion, List<string> evidencias, string comentario)
        {
            var input = new Accion_Incidente_Investigacion();
            input.Id_Accion_Incidente_Investigacion = IdAccion;
            input.Evidencia = JsonConvert.SerializeObject(evidencias);
            input.Comentarios_Implementacion = comentario;
            input.Fecha_Implementacion = DateTime.Now;
            input.Comentarios_Verificacion = comentario;
            input.Fecha_Verificacion = DateTime.Now;
            input.Estado = EstadosIncidentes.Aprobado.ToString();

            return await _editarInvestigacionFinalReadOnlyRepository.ImplementarAccionCorrectivaInvestigacion(input);
        }
    }
}
