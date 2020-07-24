using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun.GetNombresComunes
{
    public interface IGetNombresComunes
    {
        Task<NombresComunesOutput> Execute(string filtro, int pagina, int rpp);
    }
}
