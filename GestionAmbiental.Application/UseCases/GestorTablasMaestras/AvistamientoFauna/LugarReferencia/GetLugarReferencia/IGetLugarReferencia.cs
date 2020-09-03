using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.LugarReferencia.GetLugarReferencia
{
    public interface IGetLugarReferencia
    {
        Task<LugarReferenciaOutput> Execute(int id);
    }
}
