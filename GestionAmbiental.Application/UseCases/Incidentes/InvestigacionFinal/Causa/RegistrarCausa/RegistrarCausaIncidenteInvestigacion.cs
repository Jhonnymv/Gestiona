using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Causa.RegistrarCausa
{
    public class RegistrarCausaIncidenteInvestigacion : IRegistrarCausaIncidenteInvestigacion
    {
        private readonly IRegistrarInvestigacionFinalReadOnlyRepository _registrarInvestigacionFinalReadOnlyRepository;

        public RegistrarCausaIncidenteInvestigacion(IRegistrarInvestigacionFinalReadOnlyRepository registrarInvestigacionFinalReadOnlyRepository)
        {
            _registrarInvestigacionFinalReadOnlyRepository = registrarInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarInvestigacionFinalReadOnlyRepository));
        }

        public async Task<int> Execute(int Id_Investigacion, int Id_Acto_Condicion, string Tipo_Acto_Condicion,string Descripcion)
        {
            var causa = new Causa_Incidente_Investigacion();
            causa.Id_Investigacion_Final = Id_Investigacion;
            causa.Tipo_Acto_Condicion = Tipo_Acto_Condicion;
            switch (Tipo_Acto_Condicion)
            {
                case "Acto Subestándar": causa.Id_Acto_Condicion = Id_Acto_Condicion; break;
                case "Condición Subestándar": causa.Id_Acto_Condicion = Id_Acto_Condicion; break;
                case "Factor Personal": causa.Id_Factor_Causa_Incidente = Id_Acto_Condicion; break;
                case "Factor Laboral": causa.Id_Factor_Causa_Incidente = Id_Acto_Condicion; break;
            }
            causa.Descripcion = Descripcion;
            return await _registrarInvestigacionFinalReadOnlyRepository.RegistrarCausaIncidenteInvestigacion(causa);
        }
    }
}
