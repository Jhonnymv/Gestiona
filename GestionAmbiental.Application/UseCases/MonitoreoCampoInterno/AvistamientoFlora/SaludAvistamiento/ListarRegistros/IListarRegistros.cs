using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento.ListarRegistros
{
    public interface IListarRegistros
    {
        Task<SaludAvistamientosOuput> Execute(int pagina, int rpp);
        Task<SaludAvistamientoOuput> BuscarPorID(int idAvistamiento);

        Task<SaludAvistamientosOuput> BuscarPorIdAvistamiento(int idAvistamiento);
    }
}
