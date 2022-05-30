using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento.ListarRegistros
{
    public interface IListarRegistros
    {
        Task<FenologiasAvistamientosOuput> Execute(int pagina, int rpp);
        Task<FenologiaAvistamientoOuput> BuscarPorID(int idAvistamiento);

        Task<FenologiasAvistamientosOuput> BuscarPorIdAvistamiento(int idAvistamiento);
    }
}
