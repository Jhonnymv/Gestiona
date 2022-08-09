using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.CalidadAgua.ListarRegistros
{
    public interface IListarRegistros
    {
        Task<CalidadAguasOuput> Execute(int pagina, int rpp);
        Task<CalidadAguaOuput> BuscarPorId(int id);
    }
}
