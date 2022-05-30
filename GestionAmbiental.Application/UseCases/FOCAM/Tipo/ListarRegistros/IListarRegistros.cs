using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.FOCAM.Tipo.ListarRegistros
{
    public interface IListarRegistros
    {
        Task<TiposOuput> Execute(int pagina, int rpp);
        Task<TipoOuput> BuscarById(int idTipo);

        Task<TiposOuput> SelectItems();
    }
}
