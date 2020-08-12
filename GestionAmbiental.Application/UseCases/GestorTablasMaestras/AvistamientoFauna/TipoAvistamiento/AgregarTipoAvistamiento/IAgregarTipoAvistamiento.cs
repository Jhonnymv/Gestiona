using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoAvistamiento.AgregarTipoAvistamiento
{
    public interface IAgregarTipoAvistamiento
    {
        Task<int> Execute(string descripcion);
    }
}
