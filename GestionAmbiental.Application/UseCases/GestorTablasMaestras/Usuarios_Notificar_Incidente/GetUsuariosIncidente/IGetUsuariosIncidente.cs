using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario_Notificar_Incidente;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Incidente.GetUsuariosIncidente
{
    public interface IGetUsuariosIncidente
    {
        Task<UsuariosOutput> Execute(string filtro, int pagina, int rpp);
    }
}
