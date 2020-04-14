using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarParte2
{
    public interface IRegistrarParte2
    {
        Task<int> Execute(int Id_Reporte_Preliminar, int Id_Gerencia_Responsable, List<string> Evidencia, string Comentario_Accion, int Id_Supervisor_MA, string Opcion);
    }
}
