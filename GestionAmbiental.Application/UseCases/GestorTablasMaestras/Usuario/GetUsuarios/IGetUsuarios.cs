using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.GetUsuarios
{
    public interface IGetUsuarios
    {
        Task<UsuariosOutput> Execute(string filtro, int pagina, int rpp);
        Task<UsuariosOutput> Execute();
    }
}
