using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Test.NuevoRegistro
{
    public interface INuevoRegistro
    {
        Task<int> Execute(string nombre, int? numero, string evidencia);
    }
}
