using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorUsuarios.GetRolUsuario
{
    public interface IGetRolUsuario
    {
        Task<InspeccionRol?> Execute(string usuario_red);
    }
}
