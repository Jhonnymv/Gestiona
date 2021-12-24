using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Test.ListarRegistros
{
    public interface IListarRegistros
    {
        Task<TestsOutput> Execute(int pagina, int rpp);
    }
}
