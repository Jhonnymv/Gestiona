using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Test.EliminarRegistro
{
    public interface IEliminarRegistro
    {
        Task Execute(int id);
    }
}
