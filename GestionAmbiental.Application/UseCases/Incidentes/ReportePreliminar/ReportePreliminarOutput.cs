using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar
{
    public class ReportePreliminarOutput
    {
        public int Id_Reporte_Preliminar { get; set; }
        public string Codigo { get; set; }
        public int Num_Trabajadores { get; set; }
        public string Razon_Social_Contratista { get; set; }
        public string RUC_Contratista { get; set; }
        public string Actividad_Contratista { get; set; }
        public string Domicilio_Contratista { get; set; }
        public int? Num_Trabajadores_Contratista { get; set; }
        public int? Id_Tipo_Incidente { get; set; }
        public string Tipo_Incidente { get; set; }
        public int? Id_Nivel_Incidente { get; set; }
        public string Nivel_Incidente { get; set; }
        public DateTime? Fecha_Incidente { get; set; }
        public string Producto_Derramado { get; set; }
        public double? Cantidad_Derramado { get; set; }
        public int? Id_Unidad_Cantidad_Derramado { get; set; }
        public string Unidad_Cantidad_Derramado { get; set; }
        public double? Area_Afectada { get; set; }
        public int? Id_Unidad_Area_Afectada { get; set; }
        public string Unidad_Area_Afectada { get; set; }
        public string Lugar_Incidente { get; set; }
        public string Equipo_Involucrado { get; set; }
        public int? Id_Componente_Ambiental { get; set; }
        public string Componente_Ambiental { get; set; }
        public string Descripcion_Incidente { get; set; }
        public string Otra_Causa_Incidente { get; set; }
        public int Id_Gerencia_Responsable { get; set; }
        public string Gerencia_Responsable { get; set; }
        public string Comentario_Accion { get; set; }
        public int Id_Supervisor_MA { get; set; }
        public string Supervisor_MA { get; set; }
        public List<string> Evidencia { get; set; }
        public string Estado { get; set; }
        public int? Id_Registrado_Por { get; set; }
        public bool Investigacion { get; set; } = false;
        public bool PermisoEditar { get; set; } = false;
        public string Nombres_Archivos { get; set; }

        public int? NAccionesAprobadas { get; set; }
        public int? NAcciones { get; set; }
        public ReportePreliminarOutput()
        {
        }
        public ReportePreliminarOutput(Domain.Incidentes.ReportePreliminar.Reporte_Preliminar_Incidente reporte)
        {
            Id_Reporte_Preliminar = reporte.Id_Reporte_Preliminar;
            Codigo = reporte.Codigo;
            Num_Trabajadores = reporte.Num_Trabajadores;
            Razon_Social_Contratista = reporte.Razon_Social_Contratista;
            RUC_Contratista = reporte.RUC_Contratista;
            Actividad_Contratista = reporte.Actividad_Contratista;
            Domicilio_Contratista = reporte.Domicilio_Contratista;
            Num_Trabajadores_Contratista = reporte.Num_Trabajadores_Contratista;
            if (reporte.Tipo_Incidente != null)
            {
                Id_Tipo_Incidente = reporte.Tipo_Incidente.Id_Tipo_Incidente;
                Tipo_Incidente = reporte.Tipo_Incidente.Descripcion;
            }
            if (reporte.Nivel_Incidente != null)
            {
                Id_Nivel_Incidente = reporte.Nivel_Incidente.Id_Nivel_Incidente;
                Nivel_Incidente = reporte.Nivel_Incidente.Descripcion;
            }
            Fecha_Incidente = reporte.Fecha_Incidente;
            Producto_Derramado = reporte.Producto_Derramado;
            Cantidad_Derramado = reporte.Cantidad_Derramado;
            if (reporte.Unidad_Cantidad_Derramado != null)
            {
                Id_Unidad_Cantidad_Derramado = reporte.Unidad_Cantidad_Derramado.Id_Unidad_Medida;
                Unidad_Cantidad_Derramado = reporte.Unidad_Cantidad_Derramado.Descripcion;
            }

            Area_Afectada = reporte.Area_Afectada;
            if (reporte.Unidad_Area_Afectada != null)
            {
                Id_Unidad_Area_Afectada = reporte.Unidad_Area_Afectada.Id_Unidad_Medida;
                Unidad_Area_Afectada = reporte.Unidad_Area_Afectada.Descripcion;
            }

            Lugar_Incidente = reporte.Lugar_Incidente;
            Equipo_Involucrado = reporte.Equipo_Involucrado;
            if (reporte.Componente_Ambiental != null)
            {
                Id_Componente_Ambiental = reporte.Componente_Ambiental.Id_Componente_Ambiental;
                Componente_Ambiental = reporte.Componente_Ambiental.Descripcion;
            }
            Descripcion_Incidente = reporte.Descripcion_Incidente;
            
            Otra_Causa_Incidente = reporte.Otra_Causa_Incidente;
            if (reporte.Gerencia_Responsable != null)
            {
                Id_Gerencia_Responsable = reporte.Gerencia_Responsable.IdGerencia;
                Gerencia_Responsable = reporte.Gerencia_Responsable.Descripcion;
            }
            Comentario_Accion = reporte.Comentario_Accion;
            if (reporte.Supervisor_MA != null)
            {
                Id_Supervisor_MA = reporte.Supervisor_MA.IdUsuario;
                Supervisor_MA = reporte.Supervisor_MA.Nombres + " " + reporte.Supervisor_MA.Apellidos;
            }
            Evidencia = new List<string>();
            if (reporte.Evidencia != null)
            {
                var evidencias = JArray.Parse(reporte.Evidencia);

                foreach (var evidencia in evidencias)
                {
                    Evidencia.Add((string)evidencia);
                }
            }
            Estado = reporte.Estado;
            Id_Registrado_Por = reporte.Id_Registrado_Por;
            Nombres_Archivos = reporte.NombresArchivos;
            NAccionesAprobadas = reporte.NAccionesAprobadas;
            NAcciones = reporte.NAcciones;
        }
    }
}
