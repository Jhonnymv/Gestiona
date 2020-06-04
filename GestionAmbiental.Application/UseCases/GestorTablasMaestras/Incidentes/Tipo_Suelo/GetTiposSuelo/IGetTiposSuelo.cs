using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Suelo.GetTiposSuelo
{
    public interface IGetTiposSuelo
    {
        Task<TiposSueloOutput> Execute(string filtro, int pagina, int rpp);
    }
}
