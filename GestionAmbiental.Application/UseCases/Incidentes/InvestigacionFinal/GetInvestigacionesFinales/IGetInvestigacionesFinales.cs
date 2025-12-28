using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetInvestigacionesFinales
{
    public interface IGetInvestigacionesFinales
    {
        Task<InvestigacionesFinalesOutput> Execute(int tipoIncidente, int nivelIncidente, int gerencia, int asesor, string fecha_inicio, string fecha_fin);
        Task<InvestigacionesFinalesOutput> Execute(string fecha_inicio, string fecha_fin, int[] gerencia, int[] titular, string[] estado, int[] responsable, int[] asesor, string codigo, int pagina, int rpp, string usuarioRed);
    }
}
