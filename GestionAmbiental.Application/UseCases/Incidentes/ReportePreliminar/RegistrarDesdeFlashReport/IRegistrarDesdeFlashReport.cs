using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarDesdeFlashReport
{
	public interface IRegistrarDesdeFlashReport
	{
		Task<int> Execute(
			int Id_Flash_Report,
            string Codigo,
			int Id_Nivel_Incidente,
			int Id_Gerencia,
			DateTime Fecha_Incidente,
            int Id_Componente_Ambiental,
			double Cantidad_Derramado,
			int Id_Unidad_Cantidad_Derramado,
			string Lugar_Incidente,
			string Descripcion_Incidente,
			int Id_Producto_Derramado,
            int Id_Supervisor_Marcobre,
            int Id_Registrado_Por);
    }
}
