using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_RS.GetUsuarios_RS
{
    public interface IGetUsuarios_RS
    {
        Task<UsuariosOutput> Execute(string filtro, int pagina, int rpp);
    }
}
