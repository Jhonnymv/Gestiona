using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_RS.AgregarUsuario_RS
{
    public interface IAgregarUsuario_RS
    {
        Task<int> Execute(int Id_Usuario_AD);
    }
}
