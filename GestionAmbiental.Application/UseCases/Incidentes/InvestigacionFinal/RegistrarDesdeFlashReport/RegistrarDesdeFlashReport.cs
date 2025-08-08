using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.RegistrarDesdeFlashReport
{
    internal class RegistrarDesdeFlashReport : IRegistrarDesdeFlashReport
    {
        private readonly IRegistrarInvestigacionFinalReadOnlyRepository _registrarInvestigacionFinalReadOnlyRepository;

        public RegistrarDesdeFlashReport(IRegistrarInvestigacionFinalReadOnlyRepository registrarInvestigacionFinalReadOnlyRepository)
        {
            _registrarInvestigacionFinalReadOnlyRepository = registrarInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarInvestigacionFinalReadOnlyRepository));
        }

        public async Task<int> Execute(int Id_Flash_Report, string Codigo, int Id_Nivel_Incidente, string Lugar_Incidente, string Descripcion_Incidente, int Id_Gerencia_Responsable, int Id_Supervisor_MCB, DateTime Fecha_Incidente, int Id_Componente_Ambiental, double Cantidad_Derramado, int Id_Unidad_Cantidad_Derramado, int Id_Producto_Incidente, int Id_Registrado_Por)
        {
            return await _registrarInvestigacionFinalReadOnlyRepository.RegistrarDesdeFlashReport(new Domain.Incidentes.InvestigacionFinal.Investigacion_Final_Incidente
            {
                Codigo = Codigo,
                Nivel_Incidente = new Domain.Incidentes.Items.Nivel_Incidente { Id_Nivel_Incidente = Id_Nivel_Incidente },
                Lugar_Incidente = Lugar_Incidente,
                Fecha_Incidente = Fecha_Incidente,
                Descripcion_Incidente = Descripcion_Incidente,
                Gerencia_Responsable = new Domain.ValueObjects.Gerencia { IdGerencia = Id_Gerencia_Responsable},
                Supervisor_MA = new Domain.ValueObjects.Usuario { IdUsuario = Id_Supervisor_MCB }  ,
                Id_Registrado_Por = Id_Registrado_Por
            },Id_Flash_Report);
        }
    }
}
