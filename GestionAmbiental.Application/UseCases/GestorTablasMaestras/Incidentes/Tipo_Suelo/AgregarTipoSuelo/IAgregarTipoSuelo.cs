using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Suelo.AgregarTipoSuelo
{
    public interface IAgregarTipoSuelo
    {
        Task<int> Execute(string descripcion);
    }
}
