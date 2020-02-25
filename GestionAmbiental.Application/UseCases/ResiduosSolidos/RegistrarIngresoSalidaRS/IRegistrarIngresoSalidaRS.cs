using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.RegistrarIngresoSalidaRS
{
    public interface IRegistrarIngresoSalidaRS
    {
        Task<int> Execute(int arc_cmrs,int ingreso_salida,int motivo,int residuo_solido,string fecha,string responsable,double cantidad,int empresa);
    }
}
