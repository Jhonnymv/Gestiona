using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.LugarReferencia.EditarLugarReferencia
{
    public interface IEditarLugarReferencia
    {
        Task Execute(int idLugarReferencia, string descripcion, string latitud, string longitud, string estado);
    }
}
