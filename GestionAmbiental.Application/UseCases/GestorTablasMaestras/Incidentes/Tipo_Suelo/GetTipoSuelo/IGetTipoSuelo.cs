using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Suelo.GetTipoSuelo
{
    public interface IGetTipoSuelo
    {
        Task<TipoSueloOutput> Execute(int id);
    }
}
