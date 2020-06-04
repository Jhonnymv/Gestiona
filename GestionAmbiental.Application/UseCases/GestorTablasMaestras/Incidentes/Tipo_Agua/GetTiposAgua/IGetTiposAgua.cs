using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Agua.GetTiposAgua
{
    public interface IGetTiposAgua
    {
        Task<TiposAguaOutput> Execute(string filtro, int pagina, int rpp);
    }
}
