using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Flora.GetEspeciesFlora
{
    public interface IGetEspeciesFlora
    {
        Task<EspeciesFloraOutput> Execute(string filtro, int pagina, int rpp);
    }
}
