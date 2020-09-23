using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.ResolucionConsumoAgua.EditarResolucion
{
    public interface IEditarResolucion
    {
        Task Execute(int id, string descripcion, string anio, string tipoUso, string claseDerecho, int idFuente, double volumenOtorgado, string actividad, string estado);
    }
}
