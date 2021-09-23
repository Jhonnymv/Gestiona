using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.EditarReportePreliminar
{
    public interface IEditarReportePreliminar
    {
        Task Execute(
            int Id_Reporte_Preliminar,
            int Num_Trabajadores,
            string Razon_Social_Contratista,
            string RUC_Contratista,
            string Actividad_Contratista,
            string Domicilio_Contratista,
            int? Num_Trabajadores_Contratista,
            int Id_Tipo_Incidente,
            int Id_Nivel_Incidente,
            string Fecha_Incidente,
            string Hora_Incidente, string
            Producto_Derramado,
            double? Cantidad_Derramado,
            int Id_Unidad_Cantidad_Derramado,
            double? Area_Afectada,
            int Id_Unidad_Area_Afectada,
            string Lugar_Incidente,
            string Equipo_Involucrado,
            int Id_Componente_Ambiental,
            string Descripcion_Incidente,
            string Otra_Causa_Incidente,
            int Id_Gerencia_Responsable,
            List<string> Evidencia,
            string NombresEvidencias,
            string Comentario_Accion,
            int Id_Supervisor_MA,
            string Opcion);
    }
}
