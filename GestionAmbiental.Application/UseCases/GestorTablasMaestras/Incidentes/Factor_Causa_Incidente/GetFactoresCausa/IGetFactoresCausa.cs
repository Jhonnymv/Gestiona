using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Factor_Causa_Incidente.GetFactoresCausa
{
    public interface IGetFactoresCausa
    {
        Task<FactoresCausaOutput> Execute(string filtro, int pagina, int rpp);
    }
}
