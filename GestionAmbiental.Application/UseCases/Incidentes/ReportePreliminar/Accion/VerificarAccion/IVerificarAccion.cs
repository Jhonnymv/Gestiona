using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.VerificarAccion
{
    public interface IVerificarAccion
    {
        Task<int> Execute(int IdAccion, string Opcion,string Estatus, string Comentario);
    }
}
