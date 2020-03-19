using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.EO_RS.GetEO_RSs
{
    public interface IGetEO_RSs
    {
        Task<EO_RSsOutput> Execute(string filtro, int pagina, int rpp);
    }
}
