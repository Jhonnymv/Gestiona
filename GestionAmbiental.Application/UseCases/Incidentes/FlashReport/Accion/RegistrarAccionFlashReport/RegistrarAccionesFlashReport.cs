using GestionAmbiental.Application.Repositories.Incidentes.FlashReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.Accion.RegistrarAccionFlashReport
{
    internal class RegistrarAccionesFlashReport : IRegistrarAccionFlashReport
    {
        private readonly IAccionFlashReportReadOnlyRepository accionFlashReportReadOnlyRepository;

        public RegistrarAccionesFlashReport(IAccionFlashReportReadOnlyRepository accionFlashReportReadOnlyRepository)
        {
            this.accionFlashReportReadOnlyRepository = accionFlashReportReadOnlyRepository ?? throw new ArgumentNullException(nameof(accionFlashReportReadOnlyRepository));
        }

        public async Task<int> Execute(int Id_Flash_Report, string Descripcion, int Id_Usuario, string Responsable, DateTime Fecha, string Estatus, string Estado)
        {
            var input = new Domain.Incidentes.FlashReport.Accion_Flash_Report
            {
                Id_Flash_Report = Id_Flash_Report,
                Descripcion = Descripcion,
                Id_Usuario = Id_Usuario,
                Responsable = Responsable,
                Fecha = Fecha,
                Estatus = Estatus,
                Estado = Estado
            };
            return await accionFlashReportReadOnlyRepository.RegistrarAccionFlashReport(input);
        }
    }
}
