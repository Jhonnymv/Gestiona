using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConductorRS.GetConductoresRS
{
    public interface IGetConductoresRS
    {
        Task<ConductoresRSOutput> Execute(string filtro, int pagina, int rpp);
    }
}
