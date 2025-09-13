using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.EliminarEvidencias
{
    public interface IEliminarEvidencia
    {
        Task<bool> Execute(int IdReporte, string NombreArchivo, string usuario_red);
    }
}
