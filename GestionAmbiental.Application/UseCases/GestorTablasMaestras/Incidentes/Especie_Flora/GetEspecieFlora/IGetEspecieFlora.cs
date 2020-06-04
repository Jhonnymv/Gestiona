using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Flora.GetEspecieFlora
{
    public interface IGetEspecieFlora
    {
        Task<EspecieFloraOutput> Execute(int id);
    }
}
