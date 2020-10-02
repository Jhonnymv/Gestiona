using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Consumo_Agua.DeleteUsuario
{
    public interface IDeleteUsuario
    {
        Task Execute(int Id_Usuario);
    }
}
