using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora.ListarRegistros
{
    public interface IListarRegistros
    {
        Task<AvistamientosFloraOuput> Execute(int pagina, int rpp);
        Task<AvistamientoFloraOuput> BuscarPorID(int idAvistamiento);
    }
}
