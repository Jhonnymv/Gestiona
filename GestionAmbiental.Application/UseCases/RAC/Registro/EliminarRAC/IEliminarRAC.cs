using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Registro.EliminarRAC
{
    public interface IEliminarRAC
    {
        Task Execute(int idRAC, string usuario_red);
    }
}
