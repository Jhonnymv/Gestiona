using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.EO_RS.GetEO_RS
{
    public interface IGetEO_RS
    {
        Task<EO_RSOutput> Execute(int idEORS);
    }
}
