using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoAvistamiento.GetTiposAvistamiento
{
    public interface IGetTiposAvistamiento
    {
        Task<TiposAvistamientoOutput> Execute(string filtro, int pagina, int rpp);
    }
}
