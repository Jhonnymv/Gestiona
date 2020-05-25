using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Involucrado.RegistrarInvolucrado
{
    public interface IRegistrarInvolucradoIncidente
    {
        Task<int> Execute(int IdInvestigacion, string tipo, int usuario, string nobmres, string apellidos, string docIdentidad, int edad, string puestoTrabajo, string empresa);
    }
}
