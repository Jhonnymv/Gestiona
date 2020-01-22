using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorUsuarios.UpdateUsuarios
{
    public interface IUpdateUsuarios
    {
        Task Execute(string usuarios);
    }
}
