using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Test.EditarRegistro
{
    public interface IEditarRegistro
    {
        Task Execute(int id, string nombre, int? numero, string evidencia);
    }
}
