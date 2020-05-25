using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetInvestigacionesFinales
{
    public interface IGetInvestigacionesFinales
    {
        Task<InvestigacionesFinalesOutput> Execute(string fecha, int id_Usuario, int id_Gerencia,int id_Asesor, int pagina, int rpp);
    }
}
