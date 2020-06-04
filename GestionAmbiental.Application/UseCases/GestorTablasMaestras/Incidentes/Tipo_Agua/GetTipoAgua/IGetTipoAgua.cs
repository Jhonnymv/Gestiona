using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Agua.GetTipoAgua
{
    public interface IGetTipoAgua
    {
        Task<TipoAguaOutput> Execute(int id);
    }
}
