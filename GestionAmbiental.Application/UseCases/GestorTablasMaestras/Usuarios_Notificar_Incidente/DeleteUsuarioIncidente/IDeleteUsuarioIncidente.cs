using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Incidente.DeleteUsuarioIncidente
{
    public interface IDeleteUsuarioIncidente
    {
        Task Execute(int Id_Usuario);
    }
}
