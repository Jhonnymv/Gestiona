using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.EliminarVencidos
{
    public interface IEliminarVencidos
    {
        Task Execute(string fechaInicio, string fechaFin);
    }
}
