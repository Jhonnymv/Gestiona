using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Factor_Causa_Incidente.AgregarFactorCausa
{
    public interface IAgregarFactorCausa
    {
        Task<int> Execute(string descripcion, string tipo);
    }
}
