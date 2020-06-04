using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Unidad_Medida.GetUnidadMedida
{
    public interface IGetUnidadMedida
    {
        Task<UnidadMedidaOutput> Execute(int id);
    }
}
