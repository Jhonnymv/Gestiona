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
        public int IdInspeccionCabecera { get; set; }
        public int IdInspeccionDetalle { get; set; }
        public string TipoInspeccion { get; set; }
        public int IdAsesor { get; set; }
        public string AsesorMASeguridad { get; set; }
        public string DescripcionActoCondicion { get; set; }
        public string DescripcionCondicion { get; set; }
        public int? IdTipoCondicion { get; set; }
        public string TipoCondicion { get; set; }
        public List<string> EvidenciaCondicion { get; set; }
        public string NivelRiesgo { get; set; }
        public string DescripcionAccion { get; set; }
        public int IdResponsableMarcobre { get; set; }
        public string ResponsableMarcobre { get; set; }
        public int IdResponsableMarcobre2 { get; set; }
        public string ResponsableMarcobre2 { get; set; }
        public string ResponsableContratista { get; set; }
        public string CorreoContratista { get; set; }
        public DateTime? FechaProgramada { get; set; }
        public List<string> EvidenciaImplementacion { get; set; }
        public string ComentarioImplementacion { get; set; }
        public DateTime? FechaCierre { get; set; }
        public DateTime? FechaVerificacion { get; set; }
        public string ComentarioVerificacion { get; set; }
        public string Estado { get; set; }
        public bool PermisosAsesor { get; set; }
        public bool PermisosImplementar { get; set; }
        public bool PermisosAdmin { get; set; }

        public InspeccionDetalleOutput(InspeccionDetalle detalle,bool permisosImplentar)
        {
            IdInspeccionCabecera = detalle.IdInspeccionCabecera;
            IdInspeccionDetalle = detalle.IdInspeccionDetalle;
            TipoInspeccion = detalle.TipoObservacion;
            AsesorMASeguridad = detalle.Asesor.Nombres+ " " +detalle.Asesor.Apellidos;
            DescripcionActoCondicion = detalle.DescripcionActoCondicion;
            DescripcionCondicion = detalle.DescripcionCondicion;
            if (detalle.TipoCondicion != null)
            {
                TipoCondicion = detalle.TipoCondicion.Descripcion;
                IdTipoCondicion = detalle.TipoCondicion.IdTipoCondicion;
            }
            IdAsesor = detalle.Asesor != null ? detalle.Asesor.IdUsuario : 0;
            var evidencias = JArray.Parse(detalle.EvidenciaCondicion);
            EvidenciaCondicion = new List<string>();
            foreach (var evidencia in evidencias)
            {
                EvidenciaCondicion.Add((string)evidencia);
            }
            NivelRiesgo = detalle.NivelRiesgo;
            DescripcionAccion = detalle.DescripcionAccion;
            ResponsableMarcobre = detalle.ResponsableMarcobre.Nombres + " " + detalle.ResponsableMarcobre.Apellidos;
            ResponsableContratista = detalle.ResponsableContratista;
            CorreoContratista = detalle.CorreoContratista;
            IdResponsableMarcobre = detalle.ResponsableMarcobre.IdUsuario;
            FechaProgramada = detalle.FechaProgramada;

            EvidenciaImplementacion = new List<string>();
            if (detalle.EvidenciaImplementacion != null)
            {
                var evidencias_inspeccion = JArray.Parse(detalle.EvidenciaImplementacion);
               
                foreach (var evidencia in evidencias_inspeccion)
                {
                    EvidenciaImplementacion.Add((string)evidencia);
                }
            }
            ComentarioImplementacion = detalle.ComentarioImplementacion ?? "";
            FechaCierre = detalle.FechaCierre;
            FechaVerificacion = detalle.FechaVerificacion;
            ComentarioVerificacion = detalle.ComentarioVerificacion ?? "";

            Estado = detalle.Estado;
            PermisosImplementar = permisosImplentar;
            PermisosAsesor = false;
        }
        public InspeccionDetalleOutput(InspeccionDetalle detalle)
        {

            IdInspeccionCabecera = detalle.IdInspeccionCabecera;
            IdInspeccionDetalle = detalle.IdInspeccionDetalle;
            TipoInspeccion = detalle.TipoObservacion;
            AsesorMASeguridad = detalle.Asesor.Nombres + " " + detalle.Asesor.Apellidos;
            DescripcionActoCondicion = detalle.DescripcionActoCondicion;
            DescripcionCondicion = detalle.DescripcionCondicion;
            if (detalle.TipoCondicion != null)
            {
                TipoCondicion = detalle.TipoCondicion.Descripcion;
                IdTipoCondicion = detalle.TipoCondicion.IdTipoCondicion;
            }
            IdAsesor = detalle.Asesor.IdUsuario;
            var evidencias = JArray.Parse(detalle.EvidenciaCondicion);
            var listEvidencias = new List<string>();
            foreach (var evidencia in evidencias)
            {
                listEvidencias.Add((string)evidencia);
            }
            EvidenciaCondicion = listEvidencias;
            NivelRiesgo = detalle.NivelRiesgo;
            DescripcionAccion = detalle.DescripcionAccion;
            ResponsableMarcobre = detalle.ResponsableMarcobre.Nombres + " " + detalle.ResponsableMarcobre.Apellidos;
            IdResponsableMarcobre = detalle.ResponsableMarcobre.IdUsuario;
            if (detalle.ResponsableMarcobre2 != null)
            {
                ResponsableMarcobre2 = detalle.ResponsableMarcobre2.Nombres + " " + detalle.ResponsableMarcobre2.Apellidos;
                IdResponsableMarcobre2 = detalle.ResponsableMarcobre2.IdUsuario;
            }
            else{
                ResponsableMarcobre2 = "No Aplica";
                IdResponsableMarcobre2 = 0;
            }
            ResponsableContratista = detalle.ResponsableContratista;
            CorreoContratista = detalle.CorreoContratista;
            FechaProgramada = detalle.FechaProgramada;

            if (detalle.EvidenciaImplementacion != null)
            {
                var evidencias_inspeccion = JArray.Parse(detalle.EvidenciaImplementacion);
                var listEvidencias_inspeccion = new List<string>();
                foreach (var evidencia in evidencias_inspeccion)
                {
                    listEvidencias_inspeccion.Add((string)evidencia);
                }
                EvidenciaImplementacion = listEvidencias_inspeccion;
            }
            ComentarioImplementacion = detalle.ComentarioImplementacion ?? "";
            FechaCierre = detalle.FechaCierre;
            FechaVerificacion = detalle.FechaVerificacion;
            ComentarioVerificacion = detalle.ComentarioVerificacion ?? "";

            Estado = detalle.Estado;
        }
    }

}
