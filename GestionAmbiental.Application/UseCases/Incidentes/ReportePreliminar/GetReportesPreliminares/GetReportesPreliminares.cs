using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetReportesPreliminares
{
    public class GetReportesPreliminares : IGetReportesPreliminares
    {
        private readonly IGetReportePreliminarReadOnlyRepository _getReportePreliminarReadOnlyRepository;

        public GetReportesPreliminares(IGetReportePreliminarReadOnlyRepository getReportePreliminarReadOnlyRepository)
        {
            _getReportePreliminarReadOnlyRepository = getReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportePreliminarReadOnlyRepository));
        }
        public async Task<ReportesPreliminaresOutput> Execute(string fecha, int id_Usuario, int id_Gerencia, int pagina, int rpp)
        {
            DateTime? fechaFiltro = null;
            if (fecha != null)
                fechaFiltro = Convert.ToDateTime(fecha);
            var data = await _getReportePreliminarReadOnlyRepository.GetReportes(fechaFiltro,id_Usuario,id_Gerencia);

            var output = new ReportesPreliminaresOutput();
            output.Reportes = new List<ReportePreliminarOutput>();
            output.NReportes = data.Count;

            data = data.Skip((pagina - 1) * rpp).Take(rpp).ToList();
            foreach (var item in data)
            {
                var outputItem = new ReportePreliminarOutput(item);
                //outputItem.Id_Reporte_Preliminar = item.Id_Reporte_Preliminar;
                //outputItem.Codigo = item.Codigo;
                //outputItem.Num_Trabajadores = item.Num_Trabajadores;
                //outputItem.Razon_Social_Contratista = item.Razon_Social_Contratista;
                //outputItem.RUC_Contratista = item.RUC_Contratista;
                //outputItem.Actividad_Contratista = item.Actividad_Contratista;
                //outputItem.Domicilio_Contratista = item.Domicilio_Contratista;
                //outputItem.Num_Trabajadores_Contratista = item.Num_Trabajadores_Contratista;
                //if (item.Tipo_Incidente != null)
                //{
                //    outputItem.Id_Tipo_Incidente = item.Tipo_Incidente.Id_Tipo_Incidente;
                //    outputItem.Tipo_Incidente = item.Tipo_Incidente.Descripcion;
                //}
                //if (item.Nivel_Incidente != null)
                //{
                //    outputItem.Id_Nivel_Incidente = item.Nivel_Incidente.Id_Nivel_Incidente;
                //    outputItem.Nivel_Incidente = item.Nivel_Incidente.Descripcion;
                //}                    
                //outputItem.Fecha_Incidente = item.Fecha_Incidente;
                //outputItem.Producto_Derramado = item.Producto_Derramado;
                //outputItem.Cantidad_Derramado = item.Cantidad_Derramado;
                //if (item.Unidad_Cantidad_Derramado != null)
                //{
                //    outputItem.Id_Unidad_Cantidad_Derramado = item.Unidad_Cantidad_Derramado.Id_Unidad_Medida;
                //    outputItem.Unidad_Cantidad_Derramado = item.Unidad_Cantidad_Derramado.Descripcion;
                //}
                
                //    outputItem.Area_Afectada = item.Area_Afectada;
                //if (item.Unidad_Area_Afectada != null)
                //{
                //    outputItem.Id_Unidad_Area_Afectada = item.Unidad_Area_Afectada.Id_Unidad_Medida;
                //    outputItem.Unidad_Area_Afectada = item.Unidad_Area_Afectada.Descripcion;
                //}
                   
                //outputItem.Lugar_Incidente = item.Lugar_Incidente;
                //outputItem.Equipo_Involucrado = item.Equipo_Involucrado;
                //if (item.Componente_Ambiental != null)
                //{
                //    outputItem.Id_Componente_Ambiental = item.Componente_Ambiental.Id_Componente_Ambiental;
                //    outputItem.Componente_Ambiental = item.Componente_Ambiental.Descripcion;
                //}                    
                //outputItem.Descripcion_Incidente = item.Descripcion_Incidente;
                //if (item.Causa_Incidente != null)
                //{
                //    outputItem.Id_Causa_Incidente = item.Causa_Incidente.IdTipoCondicion;
                //    outputItem.Causa_Incidente = item.Causa_Incidente.Descripcion;
                //}
                //outputItem.Otra_Causa_Incidente = item.Otra_Causa_Incidente;
                //if (item.Gerencia_Responsable != null)
                //{
                //    outputItem.Id_Gerencia_Responsable = item.Gerencia_Responsable.IdGerencia;
                //    outputItem.Gerencia_Responsable = item.Gerencia_Responsable.Descripcion;
                //}
                //outputItem.Comentario_Accion = item.Comentario_Accion;
                //if (item.Supervisor_MA != null)
                //{
                //    outputItem.Id_Supervisor_MA = item.Supervisor_MA.IdUsuario;
                //    outputItem.Supervisor_MA = item.Supervisor_MA.Nombres + " " + item.Supervisor_MA.Apellidos;
                //}
                //outputItem.Evidencia = item.Evidencia;
                //outputItem.Estado = item.Estado;

                output.Reportes.Add(outputItem);
            }           

            return output;
        }
    }
}
