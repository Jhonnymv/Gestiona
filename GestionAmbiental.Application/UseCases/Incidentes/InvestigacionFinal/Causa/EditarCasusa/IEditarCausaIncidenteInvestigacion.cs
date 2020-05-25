using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Causa.EditarCasusa
{
    public interface IEditarCausaIncidenteInvestigacion
    {
        Task<int> Execute(int Id_Causa_Incidente, int Id_Investigacion, int Id_Acto_Condicion, string Tipo_Acto_Condicion, int Id_Factor_Causa, string Descripcion);
    }
}
