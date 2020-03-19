using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.MotivoIngresoSalida.AgregarMotivo
{
    public interface IAgregarMotivo
    {
        Task<int> Execute(string descripcion);
    }
}
