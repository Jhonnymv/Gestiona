using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Involucrado.EliminarInvolucrado
{
    public interface IEliminarInvolucradoIncidente
    {
        Task Execute(int IdInvolucradoIncidente);
    }
}
