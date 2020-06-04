using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Incidente.AgregarUsuarioIncidente
{
    public interface IAgregarUsuarioIncidente
    {
        Task<int> Execute(int Id_Usuario_AD);
    }
}
