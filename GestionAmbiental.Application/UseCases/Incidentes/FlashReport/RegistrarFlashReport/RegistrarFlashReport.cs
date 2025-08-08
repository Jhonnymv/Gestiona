using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.Incidentes.FlashReport;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Enum;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.RegistrarFlashReport
{
    internal class RegistrarFlashReport : IRegistrarFlashReport
    {
        private readonly IFlashReportReadOnlyRepository _flashReportReadOnlyRepository;
		private readonly IRegistrarReportePreliminarReadOnlyRepository _registrarReportePreliminarReadOnlyRepository;
		
		private readonly ReportePreliminar.RegistrarDesdeFlashReport.IRegistrarDesdeFlashReport _registrarMenorDesdeFlashReport;
        private readonly InvestigacionFinal.RegistrarDesdeFlashReport.IRegistrarDesdeFlashReport _registrarMayorDesdeFlashReport;

        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public RegistrarFlashReport(IFlashReportReadOnlyRepository flashReportReadOnlyRepository, IRegistrarReportePreliminarReadOnlyRepository registrarReportePreliminarReadOnlyRepository, ReportePreliminar.RegistrarDesdeFlashReport.IRegistrarDesdeFlashReport registrarMenorDesdeFlashReport, InvestigacionFinal.RegistrarDesdeFlashReport.IRegistrarDesdeFlashReport registrarMayorDesdeFlashReport, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _flashReportReadOnlyRepository = flashReportReadOnlyRepository ?? throw new ArgumentNullException(nameof(flashReportReadOnlyRepository));
            _registrarReportePreliminarReadOnlyRepository = registrarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarReportePreliminarReadOnlyRepository));
            _registrarMenorDesdeFlashReport = registrarMenorDesdeFlashReport ?? throw new ArgumentNullException(nameof(registrarMenorDesdeFlashReport));
            _registrarMayorDesdeFlashReport = registrarMayorDesdeFlashReport ?? throw new ArgumentNullException(nameof(registrarMayorDesdeFlashReport));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task<int> Execute(int Id_Gerencia, int Id_Empresa, string Lugar, DateTime Fecha_Hora, int Nivel_Incidente, int Componente_Ambiental_Afectado, int Id_Producto_Incidente, float Cantidad_Derramada, int Id_Unidad_Cantidad_Derramada, string Descripcion, string Registro_Fotografico, string Archivos_Registro_Fotografico, int Id_Supervisor_Marcobre, string Supervisor_Contratista, string Cargo_Supervisor_Marcobre, string Cargo_Supervisor_Contratista, string Estado, string Opcion, string usuario_red)
        {
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            var flashReport = new Domain.Incidentes.FlashReport.Flash_Report
            {
				Id_Gerencia = Id_Gerencia,
                Id_Empresa = Id_Empresa,
                Lugar = Lugar,
                Fecha_Hora = Fecha_Hora,
                Id_Nivel_Incidente = Nivel_Incidente,
                Id_Componente_Ambiental = Componente_Ambiental_Afectado,
                Id_Producto_Incidente = Id_Producto_Incidente,
                Cantidad_Derramada = Cantidad_Derramada,
                Id_Unidad_Cantidad_Derramada = Id_Unidad_Cantidad_Derramada,
                Descripcion = Descripcion,
                Registro_Fotografico = Registro_Fotografico,
				Archivos_Registro_Fotografico = Archivos_Registro_Fotografico,
				Id_Supervisor_Marcobre = Id_Supervisor_Marcobre,
                Supervisor_Contratista = Supervisor_Contratista,
                Cargo_Supervisor_Marcobre = Cargo_Supervisor_Marcobre,
                Cargo_Supervisor_Contratista = Cargo_Supervisor_Contratista,
                Estado = Estado,
                Id_Registrado_Por = usuario.IdUsuario
            };
			flashReport.Codigo = "TBD";
			switch (Opcion)
			{
				case "GuardarEstado":
					flashReport.Estado = EstadosIncidentes.Pendiente.ToString();
					break;
				case "Registrar":
					flashReport.Estado = EstadosIncidentes.Registrado.ToString();
					break;
				case "Finalizar":
					flashReport.Estado = EstadosIncidentes.Registrado.ToString();
					var codigoAnteriorFR = await _flashReportReadOnlyRepository.GetUltimoCodigo();
					var codigoAnteriorRP = await _registrarReportePreliminarReadOnlyRepository.GetUltimoCodigo();

					if (Convert.ToInt32(codigoAnteriorFR.Remove(0, 3))> Convert.ToInt32(codigoAnteriorRP.Remove(0, 3)))
						flashReport.Codigo = "IA-" + (Convert.ToInt32(codigoAnteriorFR.Remove(0, 3)) + 1).ToString().PadLeft(7, '0');
					else
						flashReport.Codigo = "IA-" + (Convert.ToInt32(codigoAnteriorRP.Remove(0, 3)) + 1).ToString().PadLeft(7, '0');

					flashReport.Estado = EstadosIncidentes.Difundido.ToString();
					if (flashReport.Id_Nivel_Incidente == 6 || flashReport.Id_Nivel_Incidente == 1 || flashReport.Id_Nivel_Incidente == 2)
					{
                        await _registrarMenorDesdeFlashReport.Execute(
                        flashReport.Id_Flash_Report,
                        flashReport.Codigo,
                        flashReport.Id_Nivel_Incidente,
                        flashReport.Id_Gerencia,
                        flashReport.Fecha_Hora,
                        flashReport.Id_Componente_Ambiental,
                        flashReport.Cantidad_Derramada,
                        flashReport.Id_Unidad_Cantidad_Derramada,
                        flashReport.Lugar,
                        flashReport.Descripcion,
                        flashReport.Id_Producto_Incidente,
                        flashReport.Id_Supervisor_Marcobre,
                        flashReport.Id_Registrado_Por.Value);
                    }
                    else
                        await _registrarMayorDesdeFlashReport.Execute(
                        flashReport.Id_Flash_Report,
						flashReport.Codigo,
						flashReport.Id_Nivel_Incidente,
                        flashReport.Lugar,
                        flashReport.Descripcion,
                        flashReport.Id_Gerencia,
                        flashReport.Id_Supervisor_Marcobre,
                        flashReport.Fecha_Hora,
						flashReport.Id_Componente_Ambiental,
						flashReport.Cantidad_Derramada,
						flashReport.Id_Unidad_Cantidad_Derramada,
                        flashReport.Id_Producto_Incidente,
                        flashReport.Id_Registrado_Por.Value);
                    break;
				default:
					flashReport.Estado = null;
					break;
			}
			return await _flashReportReadOnlyRepository.RegistrarFlashReport(flashReport);
        }
    }
}
