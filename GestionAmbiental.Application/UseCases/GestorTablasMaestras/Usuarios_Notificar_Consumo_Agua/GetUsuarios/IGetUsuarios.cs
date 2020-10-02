using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Consumo_Agua.GetUsuarios
{
    public interface IGetUsuarios
    {
        Task<UsuariosOutput> Execute(string filtro, int pagina, int rpp);
    }
}
