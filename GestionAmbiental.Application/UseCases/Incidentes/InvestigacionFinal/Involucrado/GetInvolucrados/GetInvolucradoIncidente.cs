using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Involucrado.GetInvolucrados
{
    public class GetInvolucradoIncidente : IGetInvolucradoIncidente
    {
        private readonly IGetItemsInvestigacionFinalReadOnlyRepository _getItemsInvestigacionFinalReadOnlyRepository;

        public GetInvolucradoIncidente(IGetItemsInvestigacionFinalReadOnlyRepository getItemsInvestigacionFinalReadOnlyRepository)
        {
            _getItemsInvestigacionFinalReadOnlyRepository = getItemsInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsInvestigacionFinalReadOnlyRepository));
        }

        public async Task<InvolucradoIncidenteOutput> Execute(int IdInvolucrado)
        {
            var involucrado = await _getItemsInvestigacionFinalReadOnlyRepository.GetInvolucradoInvestigacion(IdInvolucrado);

            var output = new InvolucradoIncidenteOutput();
            output.Id_Involucrado_Incidente = involucrado.Id_Involucrado_Incidente_Investigacion;
            output.Id_Investigacion = involucrado.Id_Investigacion_Final;
            output.Tipo_Involucrado = involucrado.Tipo_Involucrado;
            output.Id_Usuario = involucrado.Id_Usuario;
            output.Nombres = involucrado.Nombres;
            output.Apellidos = involucrado.Apellidos;
            output.DocIdentidad = involucrado.Doc_Identidad;
            output.Edad = involucrado.Edad;
            output.Email = involucrado.Email;
            output.Puesto_Trabajo = involucrado.Puesto_Trabajo;
            output.Empresa = involucrado.Empresa;
            output.Celular = involucrado.Celular;
            output.Turno = involucrado.Turno;
            output.Estado = involucrado.Estado;
            return output;
        }
    }
}
