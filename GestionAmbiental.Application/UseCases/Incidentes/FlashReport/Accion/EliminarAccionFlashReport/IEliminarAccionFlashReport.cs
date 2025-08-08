using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.Accion.EliminarAccionFlashReport
{
	public interface IEliminarAccionFlashReport
	{
		Task Execute(int IdAccion);
	}
}
