using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.MiembroComiteInspeccion.GetMiembroComiteInspeccion
{
    public interface IGetMiembroComiteInspeccion
    {
        Task<MiembroComiteInspeccionOutput> Execute(int id);
    }
}
