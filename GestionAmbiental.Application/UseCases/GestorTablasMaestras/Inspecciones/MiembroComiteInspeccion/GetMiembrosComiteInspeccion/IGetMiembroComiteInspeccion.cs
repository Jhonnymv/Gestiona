using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.MiembroComiteInspeccion.GetMiembrosComiteInspeccion
{
    public interface IGetMiembrosComiteInspeccion
    {
        Task<MiembrosComiteInspeccionOutput> Execute(string filtro, int pagina, int rpp);
    }
}
