using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.FuenteAgua.AgregarFuenteAgua
{
    public interface IAgregarFuenteAgua
    {
        Task<int> Execute(string descripcion, string coordenadas, int idOrigen);
    }
}
