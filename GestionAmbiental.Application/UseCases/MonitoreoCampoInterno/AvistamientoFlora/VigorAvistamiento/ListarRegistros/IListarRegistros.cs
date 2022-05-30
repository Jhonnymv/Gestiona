using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento.ListarRegistros
{
    public interface IListarRegistros
    {
        Task<vigorAvistamientosOuput> Execute(int pagina, int rpp);
        Task<VigorAvistamientoOuput> BuscarPorID(int idAvistamiento);

        Task<vigorAvistamientosOuput> BuscarPorIdAvistamiento(int idAvistamiento);
    }
}
