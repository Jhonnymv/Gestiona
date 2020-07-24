using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoFauna.EditarTipoFauna
{
    public interface IEditarTipoFauna
    {
        Task Execute(int IdTipoFauna, string Descripcion, string Estado);
    }
}
