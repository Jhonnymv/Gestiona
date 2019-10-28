using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Inspecciones
{
    public class InspeccionDetalle
    {
        public int Id_Inspeccion_Cabecera { get; set; }
        public int Id_Inspeccion_Detalle { get; set; }
        public string Descripcion_Condicion { get; set; }
        public Tipo_Condicion Tipo_Condicion { get; set; }
        public string Evidencia_Condicion { get; set; }
        public string Nivel_Riesgo_Ambiental { get; set; }
        public string Descripcion_Accion { get; set; }
        public Usuario Responsable_Marcobre { get; set; }
        public string Responsable_Contratista { get; set; }
        public DateTime Fecha_Programada { get; set; }

        public InspeccionDetalle(
            int id_Inspeccion_Cabecera, int id_Inspeccion_Detalle, string descripcion_Condicion, 
            Tipo_Condicion tipo_Condicion, string evidencia_Condicion, string nivel_Riesgo_Ambiental, 
            string descripcion_Accion, Usuario responsable_Marcobre, string responsable_Contratista, DateTime fecha_Programada)
        {
            Id_Inspeccion_Cabecera = id_Inspeccion_Cabecera;
            Id_Inspeccion_Detalle = id_Inspeccion_Detalle;
            Descripcion_Condicion = descripcion_Condicion;
            Tipo_Condicion = tipo_Condicion;
            Evidencia_Condicion = evidencia_Condicion;
            Nivel_Riesgo_Ambiental = nivel_Riesgo_Ambiental;
            Descripcion_Accion = descripcion_Accion;
            Responsable_Marcobre = responsable_Marcobre;
            Responsable_Contratista = responsable_Contratista;
            Fecha_Programada = fecha_Programada;
        }
    }
}
