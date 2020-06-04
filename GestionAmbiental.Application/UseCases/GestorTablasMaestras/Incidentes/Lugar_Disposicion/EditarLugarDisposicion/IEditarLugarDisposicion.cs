using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Lugar_Disposicion.EditarLugarDisposicion
{
    public interface IEditarLugarDisposicion
    {
        Task Execute(int id, string descripcion, string estado);
    }
}
