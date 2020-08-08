using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna
{
    public class AvistamientoFaunaOutput
    {
        public int IdAvistamientoFauna { get; set; }
        public string Codigo { get; set; }
        public string[] Fotos { get; set; }
        public DateTime Fecha_Avistamiento { get; set; }
        public string Ubicacion { get; set; }
        public int Cantidad { get; set; }
        public int IdNombreComun { get; set; }
        public string NombreComun { get; set; }
        public int IdTipoAvistamiento { get; set; }
        public string TipoAvistamiento { get; set; }
        public int IdTipoFauna { get; set; }
        public string TipoFauna { get; set; }
        public int IdEstadoFauna { get; set; }
        public string EstadoFauna { get; set; }
        public string Lugar_Referencia { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public int IdEmpresa { get; set; }
        public string Empresa { get; set; }
        public int IdAreaTrabajo { get; set; }
        public string AreaTrabajo { get; set; }
        public string NFotocheck { get; set; }
        public string Descripcion_Accion { get; set; }
        public string[] Evidencia_Accion { get; set; }
        public int IdSupervisor { get; set; }
        public string Supervisor { get; set; }
        public string Estado { get; set; }

        public AvistamientoFaunaOutput(Domain.AvistamientoFauna.AvistamientoFauna avistamiento)
        {
            IdAvistamientoFauna = avistamiento.IdAvistamientoFauna;
            Codigo = avistamiento.Codigo;
            Fecha_Avistamiento = avistamiento.Fecha_Avistamiento;
            Ubicacion = avistamiento.Ubicacion;
            Cantidad = avistamiento.Cantidad;
            if (avistamiento.NombreComun != null)
            {
                IdNombreComun = avistamiento.NombreComun.IdNombreComunFauna;
                NombreComun = avistamiento.NombreComun.Descripcion;
                IdTipoFauna = avistamiento.NombreComun.TipoFauna.IdTipoFauna;
                TipoFauna = avistamiento.NombreComun.TipoFauna.Descripcion;
            }
            if (avistamiento.TipoAvistamiento != null)
            {
                IdTipoAvistamiento = avistamiento.TipoAvistamiento.IdTipoAvistamiento;
                TipoAvistamiento = avistamiento.TipoAvistamiento.Descripcion;
            }
            if (avistamiento.EstadoFauna != null)
            {
                IdEstadoFauna = avistamiento.EstadoFauna.IdEstadoFauna;
                EstadoFauna = avistamiento.EstadoFauna.Descripcion;
            }
            Lugar_Referencia = avistamiento.Lugar_Referencia;
            IdUsuario = avistamiento.Usuario.IdUsuario;
            Usuario = avistamiento.Usuario.Nombres + avistamiento.Usuario.Apellidos;
            if (avistamiento.Empresa != null)
            {
                IdEmpresa = avistamiento.Empresa.IdEmpresa;
                Empresa = avistamiento.Empresa.RazonSocial;
            }
            if (avistamiento.AreaTrabajo != null)
            {
                IdAreaTrabajo = avistamiento.AreaTrabajo.IdAreaTrabajo;
                AreaTrabajo = avistamiento.AreaTrabajo.Descripcion;
            }
            NFotocheck = avistamiento.NFotocheck;
            Descripcion_Accion = avistamiento.Descripcion_Accion;
            if (avistamiento.Supervisor != null)
            {
                IdSupervisor = avistamiento.Supervisor.IdUsuario;
                Supervisor = avistamiento.Supervisor.Nombres + avistamiento.Supervisor.Apellidos;
            }
            Estado = avistamiento.Estado;

            //Fotos
            if (avistamiento.Fotos != null)
            {
                var fotos = JArray.Parse(avistamiento.Fotos);
                if (fotos.Count > 0)
                {
                    var listaFotos = new List<string>();
                    foreach (var foto in fotos)
                    {
                        listaFotos.Add((string)foto);
                    }
                    Fotos = listaFotos.ToArray();
                }
            }
            //Evidencia
            if (avistamiento.Evidencia_Accion != null)
            {
                var evidencias = JArray.Parse(avistamiento.Evidencia_Accion);
                if (evidencias.Count > 0)
                {
                    var listaEvidencias = new List<string>();
                    foreach (var evidencia in evidencias)
                    {
                        listaEvidencias.Add((string)evidencia);
                    }
                    Evidencia_Accion = listaEvidencias.ToArray();
                }
            }
        }
    }
}
