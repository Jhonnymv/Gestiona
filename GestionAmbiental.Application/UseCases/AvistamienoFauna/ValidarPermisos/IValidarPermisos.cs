using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.ValidarPermisos
{
    public interface IValidarPermisos
    {
        Task<bool> Editar(string UsuarioRed, int IdUsuarioAvistamiento);
    }
}
