using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.EstadoFauna.GetEstadosFauna
{
    public interface IGetEstadosFauna
    {
        Task<EstadosFaunaOutput> Execute(string filtro, int pagina, int rpp);
    }
}
