using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Unidad_Medida.EditarUnidadMedida
{
    public interface IEditarUnidadMedida
    {
        Task Execute(int id, string descripcion, string tipo, string estado);
    }
}
