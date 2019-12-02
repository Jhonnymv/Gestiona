using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Verificacion.VerificarObservacion
{
    public interface IVerificarObservacionMA
    {
        Task Execute(int IdObservacion, string Comentario, string Estado,string usuario_red);
    }
}
