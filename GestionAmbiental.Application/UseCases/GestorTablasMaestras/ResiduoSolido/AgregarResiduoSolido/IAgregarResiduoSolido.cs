using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido.AgregarResiduoSolido
{
    public interface IAgregarResiduoSolido
    {
        Task<int> Execute(int Id_Tipo_Residuo, string Descripcion);
    }
}
