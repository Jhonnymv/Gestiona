using GestionAmbiental.Domain.Incidentes.FlashReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.RegistrarDesdeFlashReport
{
    public interface IRegistrarDesdeFlashReport
    {
        Task<int> Execute(
            int Id_Flash_Report,
            string Codigo,
            int Id_Nivel_Incidente,
            string Lugar_Incidente,
            string Descripcion_Incidente,
            int Id_Gerencia_Responsable,
            int Id_Supervisor_MCB,
            DateTime Fecha_Incidente,
            int Id_Componente_Ambiental,
            double Cantidad_Derramado,
            int Id_Unidad_Cantidad_Derramado,
            int Id_Producto_Incidente,
            int Id_Registrado_Por);
    }
}
