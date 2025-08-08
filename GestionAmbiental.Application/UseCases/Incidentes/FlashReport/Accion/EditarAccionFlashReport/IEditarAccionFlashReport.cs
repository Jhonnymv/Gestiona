using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.Accion.EditarAccionFlashReport
{
    public interface IEditarAccionFlashReport
    {
        Task<int> Execute(
            int Id_Accion_Flash_Report,
            int Id_Flash_Report,
            string Descripcion,
            int Id_Usuario,
            string Responsable,
            DateTime Fecha,
            string Estatus,
            string Estado
            );
    }
}
