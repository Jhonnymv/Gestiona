using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Implementacion.LevantarObservacion
{
    public interface ILevantarObservacionRAC
    {
        Task Execute(int IdRAC, string Evidencia_Cierre,string Comentario_Cierre, string usuario_red);
    }
}
