using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.LugarReferencia.GetLugaresReferencia
{
    public interface IGetLugaresReferencia
    {
        Task<LugaresReferenciaOutput> Execute(string filtro, int pagina, int rpp);
    }
}

