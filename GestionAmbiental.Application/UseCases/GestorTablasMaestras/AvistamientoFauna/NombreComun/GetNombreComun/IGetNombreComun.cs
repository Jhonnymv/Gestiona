using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun.GetNombreComun
{
    public interface IGetNombreComun
    {
        Task<NombreComunOutput> Execute(int idNombreComun);
    }
}
