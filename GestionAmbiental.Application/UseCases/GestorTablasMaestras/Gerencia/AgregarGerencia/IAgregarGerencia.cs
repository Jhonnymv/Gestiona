using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gerencia.AgregarGerencia
{
    public interface IAgregarGerencia
    {
        Task<int> Execute(string descripcion);
    }
}
