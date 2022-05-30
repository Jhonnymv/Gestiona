using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociadaAvistamiento.ListarRegistros
{
    public interface IListarRegistros
    {
        Task<FaunasAsociadasAvistamientosOuput> Execute(int pagina, int rpp);
        Task<faunaAsociadaAvistamientoOuput> BuscarPorID(int idAvistamiento);

        Task<FaunasAsociadasAvistamientosOuput> BuscarPorIdAvistamiento(int idAvistamiento);
    }
}
