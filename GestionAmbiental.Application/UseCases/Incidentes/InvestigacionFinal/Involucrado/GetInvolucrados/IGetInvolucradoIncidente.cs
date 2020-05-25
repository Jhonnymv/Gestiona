using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Involucrado.GetInvolucrados
{
    public interface IGetInvolucradoIncidente
    {
        Task<InvolucradoIncidenteOutput> Execute(int IdInvolucrado);
    }
}
