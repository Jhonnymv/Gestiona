using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido.EditarResiduoSolido
{
    public interface IEditarResiduoSolido
    {
        Task Execute(int Id_Residuo_Solido,int Id_Tipo_Residuo, string Descripcion,string Estado);
    }
}
