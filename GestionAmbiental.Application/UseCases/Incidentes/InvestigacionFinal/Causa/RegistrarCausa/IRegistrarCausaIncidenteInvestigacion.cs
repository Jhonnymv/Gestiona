using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Causa.RegistrarCausa
{
    public interface IRegistrarCausaIncidenteInvestigacion
    {
        Task<int> Execute(int Id_Investigacion, int Id_Acto_Condicion, string Tipo_Acto_Condicion,string Descripcion);
    }
}
