using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento.ListarRegistros
{
    public interface IListarRegistros
    {
        Task<UsoAvistamientosOuput> Execute(int pagina, int rpp);
        Task<UsoAvistamientoOuput> BuscarPorID(int idAvistamiento);

        Task<UsoAvistamientosOuput> BuscarPorIdAvistamiento(int idAvistamiento);
    }
}
