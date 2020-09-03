using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.LugarReferencia.AgregarLugarReferencia
{
    public interface IAgregarLugarReferencia
    {
        Task<int> Execute(string descripcion, string latitud, string longitud);
    }
}
