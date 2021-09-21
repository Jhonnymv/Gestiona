using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.EditarAdminInvestigacion
{
    public class EditarAdminInvestigacion : IEditarAdminInvestigacion
    {
        private readonly IEditarInvestigacionFinalReadOnlyRepository _editarInvestigacionFinalReadOnlyRepository;

        public EditarAdminInvestigacion(IEditarInvestigacionFinalReadOnlyRepository editarInvestigacionFinalReadOnlyRepository)
        {
            _editarInvestigacionFinalReadOnlyRepository = editarInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarInvestigacionFinalReadOnlyRepository));
        }

        public async Task<int> Execute(int IdInvestigacion, int IdSupervisor, List<string> Evidencias_Papel, List<string> Evidencias_Persona, List<string> Evidencias_Posicion, List<string> Evidencias_Partes,string Nombres_Archivos)
        {
            var input_investigacion = new Investigacion_Final_Incidente();
            input_investigacion.Id_Investigacion_Final = IdInvestigacion;
            input_investigacion.Supervisor_MA = new Domain.ValueObjects.Usuario { IdUsuario = IdSupervisor };
            input_investigacion.Evidencia_Papel = JsonConvert.SerializeObject(Evidencias_Papel);
            input_investigacion.Evidencia_Persona = JsonConvert.SerializeObject(Evidencias_Persona);
            input_investigacion.Evidencia_Parte = JsonConvert.SerializeObject(Evidencias_Partes);
            input_investigacion.Evidencia_Posicion = JsonConvert.SerializeObject(Evidencias_Posicion);
            input_investigacion.NombresArchivos = Nombres_Archivos;

            return await _editarInvestigacionFinalReadOnlyRepository.EditarAdminInvestigacion(input_investigacion);
        }
    }
}
