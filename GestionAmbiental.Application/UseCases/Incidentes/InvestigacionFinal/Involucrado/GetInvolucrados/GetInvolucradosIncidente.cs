using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Involucrado.GetInvolucrados
{
    public class GetInvolucradosIncidente : IGetInvolucradosIncidente
    {
        private readonly IGetItemsInvestigacionFinalReadOnlyRepository _getItemsInvestigacionFinalReadOnlyRepository;

        public GetInvolucradosIncidente(IGetItemsInvestigacionFinalReadOnlyRepository getItemsInvestigacionFinalReadOnlyRepository)
        {
            _getItemsInvestigacionFinalReadOnlyRepository = getItemsInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsInvestigacionFinalReadOnlyRepository));
        }

        public async Task<List<InvolucradoIncidenteOutput>> Execute(int IdInvestigacion)
        {
            var items = await _getItemsInvestigacionFinalReadOnlyRepository.GetInvolucradosInvestigacion(IdInvestigacion);
            items = items.OrderBy(x => x.Tipo_Involucrado).ToList();
            var output = new List<InvolucradoIncidenteOutput>();
            foreach (var item in items)
            {
                var outputItem = new InvolucradoIncidenteOutput();
                outputItem.Id_Involucrado_Incidente = item.Id_Involucrado_Incidente_Investigacion;
                outputItem.Id_Investigacion = item.Id_Investigacion_Final;
                outputItem.Tipo_Involucrado = item.Tipo_Involucrado;
                outputItem.Id_Usuario = item.Id_Usuario;
                outputItem.Nombres = item.Nombres;
                outputItem.Apellidos = item.Apellidos;
                outputItem.DocIdentidad = item.Doc_Identidad;
                outputItem.Edad = item.Edad;
                outputItem.Email = item.Email;
                outputItem.Puesto_Trabajo = item.Puesto_Trabajo;
                outputItem.Empresa = item.Empresa;
                outputItem.Celular = item.Celular;
                outputItem.Turno = item.Turno;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }
    }
}
