using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.GetEvidenciasFlashReport
{
	public interface IGetEvidenciasFlashReport
	{
		Task<string> Execute(int IdFlashReport);
	}
}
