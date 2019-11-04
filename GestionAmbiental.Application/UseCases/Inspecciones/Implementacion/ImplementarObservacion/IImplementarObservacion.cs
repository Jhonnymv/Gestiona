using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ImplementarObservacion
{
    public interface IImplementarObservacion
    {
        Task Execute(int IdObservacion,List<string> Evidencia_Inspeccion, string Comentario);
    }
}
