using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Verificacion.AprobarObservacion
{
    public interface IAprobarObservacion
    {
        Task Execute(int IdRAC, string Comentario_Verificacion, string usuario_red);
    }
}
