using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarParte2
{
    public class RegistrarParte2 : IRegistrarParte2
    {
        private readonly IRegistrarReportePreliminarReadOnlyRepository _registrarReportePreliminarReadOnlyRepository;

        public RegistrarParte2(IRegistrarReportePreliminarReadOnlyRepository registrarReportePreliminarReadOnlyRepository)
        {
            _registrarReportePreliminarReadOnlyRepository = registrarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarReportePreliminarReadOnlyRepository));
        }

        public async Task<int> Execute(int Id_Reporte_Preliminar, int Id_Gerencia_Responsable, List<string> Evidencia, string Comentario_Accion, int Id_Supervisor_MA, string Opcion)
        {
            var input = new Domain.Incidentes.ReportePreliminar.Reporte_Preliminar_Incidente();

            switch (Opcion)
            {
                case "Completar":
                    input.Estado = EstadosIncidentes.Registrado.ToString();
                    var codigoAnterior = await _registrarReportePreliminarReadOnlyRepository.GetUltimoCodigo();
                    input.Codigo = (Convert.ToInt32(codigoAnterior.Remove(0, 3)) + 1).ToString().PadLeft(5, '0');
                    break;
                case "GuardarEstado":
                    input.Estado = EstadosIncidentes.Pendiente.ToString();
                    input.Codigo = "TBD";
                    break;
            }

            input.Id_Reporte_Preliminar = Id_Reporte_Preliminar;
            if (Id_Gerencia_Responsable != 0)
            {
                input.Gerencia_Responsable = new Domain.ValueObjects.Gerencia();
                input.Gerencia_Responsable.IdGerencia = Id_Gerencia_Responsable;
            }

            input.Evidencia = JsonConvert.SerializeObject(Evidencia);
            input.Comentario_Accion = Comentario_Accion;

            if(Id_Supervisor_MA != 0)
            {
                input.Supervisor_MA = new Domain.ValueObjects.Usuario();
                input.Supervisor_MA.IdUsuario = Id_Supervisor_MA;
            }

            return await _registrarReportePreliminarReadOnlyRepository.RegistrarReportePreliminarParte2(input);
        }
    }
}
