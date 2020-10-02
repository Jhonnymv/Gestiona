using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Consumo_Agua.AgregarUsuario
{
    public interface IAgregarUsuario
    {
        Task<int> Execute(int Id_Usuario_AD);
    }
}
