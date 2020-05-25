using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Involucrado.EditarInvolucrado
{
    public interface IEditarInvolucradoIncidente
    {
        Task<int> Execute(int IdInvolucrado, int IdReporte, string tipo, int usuario, string nobmres, string apellidos, string correo, string puesto, string celular, string turno);
    }
}
