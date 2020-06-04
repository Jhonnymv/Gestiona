using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Fauna.GetEspecieFauna
{
    public interface IGetEspecieFauna
    {
        Task<EspecieFaunaOutput> Execute(int id);
    }
}
