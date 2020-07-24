using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun.AgregarNombreComun
{
    public interface IAgregarNombreComun
    {
        Task<int> Execute(string Descripcion, int IdTipoFauna, string Foto);
    }
}
