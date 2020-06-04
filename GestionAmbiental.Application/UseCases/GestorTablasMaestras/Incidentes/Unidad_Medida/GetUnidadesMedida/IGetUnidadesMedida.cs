using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Unidad_Medida.GetUnidadesMedida
{
    public interface IGetUnidadesMedida
    {
        Task<UnidadesMedidaOutput> Execute(string filtro, int pagina, int rpp);
    }
}
