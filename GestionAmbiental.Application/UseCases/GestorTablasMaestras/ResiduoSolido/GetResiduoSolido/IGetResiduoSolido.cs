using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido.GetResiduoSolido
{
    public interface IGetResiduoSolido
    {
        Task<ResiduoSolidoOutput> Execute(int idResiduoSolido);
    }
}
