using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.MiembroComiteInspeccion.AgregarMiembroComiteInspeccion
{
    public interface IAgregarMiembroComiteInspeccion
    {
        Task<int> Execute(string nombre,string apellido);
    }
}
