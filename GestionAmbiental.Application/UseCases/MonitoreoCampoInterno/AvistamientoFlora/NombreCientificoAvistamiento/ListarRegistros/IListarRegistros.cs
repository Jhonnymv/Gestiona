using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento.ListarRegistros
{
    public interface IListarRegistros
    {
        Task<NombreCientificosAvistamientoOuput> Execute(int pagina, int rpp);
        Task<NombreCientificoAvistamientoOuput> BuscarPorID(int idAvistamiento);

        Task<NombreCientificosAvistamientoOuput> BuscarPorIdAvistamiento(int idAvistamiento);
    }
}
