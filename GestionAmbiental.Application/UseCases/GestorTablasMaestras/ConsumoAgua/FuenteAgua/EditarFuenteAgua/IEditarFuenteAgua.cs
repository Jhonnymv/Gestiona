using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.FuenteAgua.EditarFuenteAgua
{
    public interface IEditarFuenteAgua
    {
        Task Execute(int id, string descripcion,string coordenadas, int idOrigen, string estado);
    }
}
