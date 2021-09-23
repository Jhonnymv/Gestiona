using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoFauna.AgregarTipoFauna
{
    public interface IAgregarTipoFauna
    {
        Task<int> Execute(string Descripcion,string Foto);
    }
}
