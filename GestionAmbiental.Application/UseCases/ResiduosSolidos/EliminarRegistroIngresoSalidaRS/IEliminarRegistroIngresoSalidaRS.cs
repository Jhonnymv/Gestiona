using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.EliminarRegistroIngresoSalidaRS
{
    public interface IEliminarRegistroIngresoSalidaRS
    {
        Task Execute(int Id_Ingreso_Salida_RS, string usuario_red);
    }
}
