using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Avistamiento_Fauna.DeleteUsuario
{
    public interface IDeleteUsuario
    {
        Task Execute(int Id_Usuario);
    }
}
