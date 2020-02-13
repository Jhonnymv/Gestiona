using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.EliminarObservacion
{
    public interface IEliminarObservacion
    {
        Task Execute(int IdObservacion, string usuario_red);
    }
}
