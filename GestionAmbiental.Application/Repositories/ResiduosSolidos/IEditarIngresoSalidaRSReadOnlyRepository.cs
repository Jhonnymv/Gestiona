using GestionAmbiental.Domain.ResiduosSolidos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.ResiduosSolidos
{
    public interface IEditarIngresoSalidaRSReadOnlyRepository
    {
        Task EditarIngresoSalidaRS(Ingreso_Salida_RS ingreso_Salida_RS,int Id_Editado_Por);
        Task EliminarRegistroIngresoSalidaRS(int Id_Ingreso_Salida_RS, int Id_Editado_Por);
    }
}
