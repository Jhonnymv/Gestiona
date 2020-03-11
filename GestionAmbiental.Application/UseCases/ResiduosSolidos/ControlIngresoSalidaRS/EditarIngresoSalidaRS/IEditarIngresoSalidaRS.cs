using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.EditarIngresoSalidaRS
{
    public interface IEditarIngresoSalidaRS
    {
        Task Execute(int id_ingreso_salida_rs, string opcion, int arc_cmrs, int ingreso_salida, int motivo, int residuo_solido, string fecha, int responsable, double cantidad, int empresa, int responsable_marc, string comentario, string usuario_red);
    }
}
