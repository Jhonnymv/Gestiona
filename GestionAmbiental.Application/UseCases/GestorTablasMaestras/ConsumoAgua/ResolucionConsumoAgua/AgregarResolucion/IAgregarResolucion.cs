using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.ResolucionConsumoAgua.AgregarResolucion
{
    public interface IAgregarResolucion
    {
        Task<int> Execute(string descripcion,string anio,string tipoUso, string claseDerecho, int idFuente, double volumenOtorgado, double? limiteDiario, double? limiteSemanal, double? limiteMensual, string fechaVencimiento, double? caudal, string actividad);
    }
}
