using GestionAmbiental.Domain.Incidentes.Items;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.ReportePreliminar
{
    public class Reporte_Preliminar_Incidente
    {
        public int Id_Reporte_Preliminar { get; set; }
        public int Num_Trabajadores { get; set; }
        public string Razon_Social_Contratista { get; set; }
        public string RUC_Contratista { get; set; }
        public string Actividad_Contratista { get; set; }
        public string Domicilio_Contratista { get; set; }
        public int Num_Trabajadores_Contratista { get; set; }
        public Tipo_Incidente Tipo_Incidente { get; set; }
        public Nivel_Incidente Nivel_Incidente { get; set; }
        public DateTime Fecha_Incidente { get; set; }
        public string Producto_Derramado { get; set; }
        public double Cantidad_Derramado { get; set; }
        public Unidad_Medida Unidad_Cantidad_Derramado { get; set; }
        public double Area_Afectada { get; set; }
        public Unidad_Medida Unidad_Area_Afectada { get; set; }
        public string Lugar_Incidente { get; set; }
        public string Equipo_Involucrado { get; set; }
        public Componente_Ambiental Componente_Ambiental { get; set; }
        public string Descripcion_Incidente { get; set; }
        public TipoCondicion Causa_Incidente { get; set; }
        public string Otra_Causa_Incidente { get; set; }
        public Gerencia Gerencia_Responsable { get; set; }
        public string Comentario_Accion { get; set; }
        public Usuario Supervisor_MA { get; set; }
        public string Evidencia { get; set; }
        public string Estado { get; set; }
    }
}
