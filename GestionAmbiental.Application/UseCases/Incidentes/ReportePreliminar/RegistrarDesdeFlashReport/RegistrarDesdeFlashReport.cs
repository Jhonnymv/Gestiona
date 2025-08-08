using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarDesdeFlashReport
{
    internal class RegistrarDesdeFlashReport : IRegistrarDesdeFlashReport
    {
        private readonly IRegistrarReportePreliminarReadOnlyRepository _registrarReportePreliminarReadOnlyRepository;

        public RegistrarDesdeFlashReport(IRegistrarReportePreliminarReadOnlyRepository registrarReportePreliminarReadOnlyRepository)
        {
            _registrarReportePreliminarReadOnlyRepository = registrarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarReportePreliminarReadOnlyRepository));
        }

        public async Task<int> Execute(int Id_Flash_Report, string Codigo, int Id_Nivel_Incidente, int Id_Gerencia, DateTime Fecha_Incidente, int Id_Componente_Ambiental, double Cantidad_Derramado, int Id_Unidad_Cantidad_Derramado, string Lugar_Incidente, string Descripcion_Incidente, int Id_Producto_Derramado, int Id_Supervisor_Marcobre, int Id_Registrado_Por)
        {
            return await _registrarReportePreliminarReadOnlyRepository.RegistrarReportePreliminarDesdeFlashReport(new Domain.Incidentes.ReportePreliminar.Reporte_Preliminar_Incidente
            {                
                Codigo = Codigo,
                Nivel_Incidente = new Domain.Incidentes.Items.Nivel_Incidente { Id_Nivel_Incidente = Id_Nivel_Incidente },
                Fecha_Incidente = Fecha_Incidente,
                Producto_Derramado = Id_Producto_Derramado.ToString(),
                Gerencia_Responsable = new Domain.ValueObjects.Gerencia { IdGerencia = Id_Gerencia},
                //Producto_Derramado = Id_Componente_Ambiental,
                Componente_Ambiental = new Domain.Incidentes.Items.Componente_Ambiental { Id_Componente_Ambiental = Id_Componente_Ambiental },
                Cantidad_Derramado = Cantidad_Derramado,
                Unidad_Cantidad_Derramado = new Domain.Incidentes.Items.Unidad_Medida { Id_Unidad_Medida = Id_Unidad_Cantidad_Derramado },
                Lugar_Incidente = Lugar_Incidente,
                Descripcion_Incidente = Descripcion_Incidente,
                Supervisor_MA = new Domain.ValueObjects.Usuario { IdUsuario = Id_Supervisor_Marcobre},
                Id_Registrado_Por = Id_Registrado_Por
            }, Id_Flash_Report);
        }
    }
}
