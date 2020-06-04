using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Fauna.GetEspeciesFauna
{
    public interface IGetEspeciesFauna
    {
        Task<EspeciesFaunasOutput> Execute(string filtro, int pagina, int rpp);
    }
}
