using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Factor_Causa_Incidente.GetFactorCausa
{
    public interface IGetFactorCausa
    {
        Task<FactorCausaOutput> Execute(int id);
    }
}
