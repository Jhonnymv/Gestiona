using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.VerificarAccion
{
    public class VerificarAccion : IVerificarAccion
    {
        private readonly IEditarInvestigacionFinalReadOnlyRepository _editarInvestigacionFinalReadOnlyRepository;

        public VerificarAccion(IEditarInvestigacionFinalReadOnlyRepository editarInvestigacionFinalReadOnlyRepository)
        {
            _editarInvestigacionFinalReadOnlyRepository = editarInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarInvestigacionFinalReadOnlyRepository));
        }

        public async Task<int> Execute(int IdAccion, string Opcion, string Estatus, string Comentario)
        {
            var input = new Accion_Incidente_Investigacion();
            input.Id_Accion_Incidente_Investigacion = IdAccion;
            input.Estatus = Estatus;
            input.Comentarios_Verificacion = Comentario;
            input.Fecha_Verificacion = DateTime.Now;
            if (Opcion.Equals("Rechazar"))
                input.Estado = EstadosIncidentes.Rechazado.ToString();
            else if (Opcion.Equals("Aprobar"))
                input.Estado = EstadosIncidentes.Aprobado.ToString();

            return await _editarInvestigacionFinalReadOnlyRepository.VerificarAccionCorrectivaInvestigacion(input);
        }
    }
}
