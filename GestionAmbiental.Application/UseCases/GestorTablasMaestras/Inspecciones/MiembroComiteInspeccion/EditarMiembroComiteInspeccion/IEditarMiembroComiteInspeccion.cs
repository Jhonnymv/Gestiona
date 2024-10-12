using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.MiembroComiteInspeccion.EditarMiembroComiteInspeccion
{
    public interface IEditarMiembroComiteInspeccion
    {
        Task Execute(int Id, string Nombres, string Apellidos, string Estado);
    }
}
