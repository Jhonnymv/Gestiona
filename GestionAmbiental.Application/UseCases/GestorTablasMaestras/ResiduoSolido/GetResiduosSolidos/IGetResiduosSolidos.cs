using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido.GetResiduosSolidos
{
    public interface IGetResiduosSolidos
    {
        Task<ResiduosSolidosOutput> Execute(string filtro, int pagina, int rpp);
    }
}
