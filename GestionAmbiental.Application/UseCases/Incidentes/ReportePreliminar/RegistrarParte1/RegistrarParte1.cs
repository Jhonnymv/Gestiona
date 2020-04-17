using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Incidentes.Items;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarParte1
{
    public class RegistrarParte1 : IRegistrarParte1
    {
        private readonly IRegistrarReportePreliminarReadOnlyRepository _registrarReportePreliminarReadOnlyRepository;

        public RegistrarParte1(IRegistrarReportePreliminarReadOnlyRepository registrarReportePreliminarReadOnlyRepository)
        {
            _registrarReportePreliminarReadOnlyRepository = registrarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarReportePreliminarReadOnlyRepository));
        }

        public async Task<int> Execute(
            int Num_Trabajadores,
            string Razon_Social_Contratista,
            string RUC_Contratista, 
            string Actividad_Contratista, 
            string Domicilio_Contratista, 
            int? Num_Trabajadores_Contratista, 
            int Id_Tipo_Incidente, 
            int Id_Nivel_Incidente,
            string Fecha_Incidente, 
            string Hora_Incidente, 
            string Producto_Derramado,
            double? Cantidad_Derramado,
            int Id_Unidad_Cantidad_Derramado,
            double? Area_Afectada, 
            int Id_Unidad_Area_Afectada, 
            string Lugar_Incidente, 
            string Equipo_Involucrado, 
            int Id_Componente_Ambiental, 
            string Descripcion_Incidente)
        {
            var reporteDomain = new Reporte_Preliminar_Incidente();

            reporteDomain.Codigo = "TBD";
            reporteDomain.Num_Trabajadores = Num_Trabajadores;
            reporteDomain.Razon_Social_Contratista = Razon_Social_Contratista;
            reporteDomain.RUC_Contratista = RUC_Contratista;
            reporteDomain.Actividad_Contratista = Actividad_Contratista;
            reporteDomain.Domicilio_Contratista = Domicilio_Contratista;
            reporteDomain.Num_Trabajadores_Contratista = Num_Trabajadores_Contratista;

            reporteDomain.Tipo_Incidente = new Tipo_Incidente();
            if (Id_Tipo_Incidente != 0)
                reporteDomain.Tipo_Incidente.Id_Tipo_Incidente = Id_Tipo_Incidente;

            reporteDomain.Nivel_Incidente = new Nivel_Incidente();
            if (Id_Nivel_Incidente != 0)
                reporteDomain.Nivel_Incidente.Id_Nivel_Incidente = Id_Nivel_Incidente;

            if (Fecha_Incidente !=null && Hora_Incidente != null)
            {
                var fechaIncidente = Convert.ToDateTime(Fecha_Incidente + " " + Hora_Incidente);
                reporteDomain.Fecha_Incidente = fechaIncidente;
            }

            reporteDomain.Producto_Derramado = Producto_Derramado;
            reporteDomain.Cantidad_Derramado = Cantidad_Derramado;

            reporteDomain.Unidad_Cantidad_Derramado = new Unidad_Medida();
            if (Id_Unidad_Cantidad_Derramado != 0)
                reporteDomain.Unidad_Cantidad_Derramado.Id_Unidad_Medida = Id_Unidad_Cantidad_Derramado;
                        
            reporteDomain.Area_Afectada = Area_Afectada;
            reporteDomain.Unidad_Area_Afectada = new Unidad_Medida();
            if (Id_Unidad_Area_Afectada != 0)
                reporteDomain.Unidad_Area_Afectada.Id_Unidad_Medida = Id_Unidad_Area_Afectada;

            reporteDomain.Lugar_Incidente = Lugar_Incidente;
            reporteDomain.Equipo_Involucrado = Equipo_Involucrado;

            reporteDomain.Componente_Ambiental = new Componente_Ambiental();
            if (Id_Componente_Ambiental != 0)
                reporteDomain.Componente_Ambiental.Id_Componente_Ambiental = Id_Componente_Ambiental;

            reporteDomain.Descripcion_Incidente = Descripcion_Incidente;
                        

            return await _registrarReportePreliminarReadOnlyRepository.RegistrarReportePreliminarParte1(reporteDomain);
        }
    }
}
