using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Causa.EditarCasusa
{
    public class EditarCausaIncidenteInvestigacion : IEditarCausaIncidenteInvestigacion
    {
        private readonly IEditarInvestigacionFinalReadOnlyRepository _editarInvestigacionFinalReadOnlyRepository;

        public EditarCausaIncidenteInvestigacion(IEditarInvestigacionFinalReadOnlyRepository editarInvestigacionFinalReadOnlyRepository)
        {
            _editarInvestigacionFinalReadOnlyRepository = editarInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarInvestigacionFinalReadOnlyRepository));
        }

        public async Task<int> Execute(int Id_Causa_Incidente, int Id_Investigacion, int Id_Acto_Condicion, string Tipo_Acto_Condicion, int Id_Factor_Causa, string Descripcion)
        {
            var input = new Causa_Incidente_Investigacion();
            input.Id_Causa_Incidente_Investigacion = Id_Causa_Incidente;
            input.Id_Investigacion_Final = Id_Investigacion;
            switch (Tipo_Acto_Condicion)
            {
                case "Acto Subestándar": input.Id_Acto_Condicion = Id_Acto_Condicion; break;
                case "Condición Subestándar": input.Id_Acto_Condicion = Id_Acto_Condicion; break;
                case "Factor Personal": input.Id_Factor_Causa_Incidente = Id_Factor_Causa; break;
                case "Factor Laboral": input.Id_Factor_Causa_Incidente = Id_Factor_Causa; break;
            }
            input.Descripcion = Descripcion;
            input.Estado = EstadosIncidentes.Registrado.ToString();
            return await _editarInvestigacionFinalReadOnlyRepository.EditarCausaIncidenteInvestigacion(input);
        }
    }
}
