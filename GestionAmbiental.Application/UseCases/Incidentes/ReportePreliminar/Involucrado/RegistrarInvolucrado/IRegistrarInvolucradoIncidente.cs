using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Involucrado.RegistrarInvolucrado
{
    public interface IRegistrarInvolucradoIncidente
    {
        Task<int> Execute(int IdReporte, string tipo, int usuario, string nobmres, string apellidos, string correo, string puesto, string celular, string turno);
    }
}
