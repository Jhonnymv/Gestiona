using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Domain.ValueObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Inspecciones
{
    public class InspeccionDetalleOutput
    {
        public int Id_Inspeccion_Cabecera { get; set; }
        public int Id_Inspeccion_Detalle { get; set; }
        public string Descripcion_Condicion { get; set; }
        public string Tipo_Condicion { get; set; }
        public List<string> Evidencia_Condicion { get; set; }
        public string Nivel_Riesgo_Ambiental { get; set; }
        public string Descripcion_Accion { get; set; }
        public string Responsable_Marcobre { get; set; }
        public string Responsable_Contratista { get; set; }
        public DateTime Fecha_Programada { get; set; }
        public List<string> Evidencia_Inspeccion { get; set; }
        public string Comentario_Inspeccion { get; set; }
        public DateTime? Fecha_Cierre { get; set; }
        public DateTime? Fecha_Verificacion_Inspector { get; set; }
        public string Comentario_Inspector { get; set; }
        public DateTime? Fecha_Verificacion_MA { get; set; }
        public string Comentario_MA { get; set; }
        public string Estado { get; set; }

        public InspeccionDetalleOutput (InspeccionDetalle detalle)
        {
            Id_Inspeccion_Cabecera = detalle.Id_Inspeccion_Cabecera;
            Id_Inspeccion_Detalle = detalle.Id_Inspeccion_Detalle;
            Descripcion_Condicion = detalle.Descripcion_Condicion;
            Tipo_Condicion = detalle.Tipo_Condicion.Descripcion;

            var evidencias = JArray.Parse(detalle.Evidencia_Condicion);
            var listEvidencias = new List<string>();
            foreach(var evidencia in evidencias)
            {
                listEvidencias.Add((string)evidencia);
            }
            Evidencia_Condicion = listEvidencias;
            Nivel_Riesgo_Ambiental = detalle.Nivel_Riesgo_Ambiental;
            Descripcion_Accion = detalle.Descripcion_Accion;
            Responsable_Marcobre = detalle.Responsable_Marcobre.Nombres +" " + detalle.Responsable_Marcobre.Apellidos;
            Responsable_Contratista = detalle.Responsable_Contratista;
            Fecha_Programada = detalle.Fecha_Programada;

            if (detalle.Evidencia_Inspeccion!=null)
            {
                var evidencias_inspeccion = JArray.Parse(detalle.Evidencia_Inspeccion);
                var listEvidencias_inspeccion = new List<string>();
                foreach (var evidencia in evidencias_inspeccion)
                {
                    listEvidencias_inspeccion.Add((string)evidencia);
                }
            }
            Comentario_Inspeccion = detalle.Comentario_Inspeccion != null ? detalle.Comentario_Inspeccion:"";
            Fecha_Cierre = detalle.Fecha_Cierre;
            Fecha_Verificacion_Inspector = detalle.Fecha_Verificacion_Inspector;
            Comentario_Inspector = detalle.Comentario_Inspector != null ? detalle.Comentario_Inspector : "";
            Fecha_Verificacion_MA = detalle.Fecha_Verificacion_MA;
            Comentario_MA = detalle.Comentario_MA != null ? detalle.Comentario_MA : "";

            Estado = detalle.Estado;
        }
    }

}
