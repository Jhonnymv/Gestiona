using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Involucrado.EditarInvolucrado
{
    public interface IEditarInvolucradoIncidente
    {
        Task<int> Execute(int IdInvolucrado, int IdInvestigacion, string tipo, string nombres, string apellidos, int usuario, string DocIdentidad, int Edad, string PuestoTrabajo, string Empresa);
    }
}
