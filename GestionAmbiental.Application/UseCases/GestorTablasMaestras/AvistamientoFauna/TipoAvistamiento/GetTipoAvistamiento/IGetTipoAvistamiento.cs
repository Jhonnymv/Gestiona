using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoAvistamiento.GetTipoAvistamiento
{
    public interface IGetTipoAvistamiento
    {
        Task<TipoAvistamientoOutput> Execute(int idTipoAvistamiento);
    }
}
