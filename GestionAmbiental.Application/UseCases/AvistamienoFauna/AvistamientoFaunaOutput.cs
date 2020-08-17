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
        public DateTime FechaAvistamiento { get; set; }
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
        public int IdLugarReferencia { get; set; }
        public string LugarReferencia { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string DescripcionAccion { get; set; }
        public string[] EvidenciaAccion { get; set; }
        public int IdSupervisor { get; set; }
        public string Supervisor { get; set; }
        public string Estado { get; set; }

        public AvistamientoFaunaOutput(Domain.AvistamientoFauna.AvistamientoFauna avistamiento)
        {
            IdAvistamientoFauna = avistamiento.IdAvistamientoFauna;
            Codigo = avistamiento.Codigo;
            FechaAvistamiento = avistamiento.FechaAvistamiento;
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
            if (avistamiento.LugarReferencia != null)
            {
                IdLugarReferencia = avistamiento.LugarReferencia.IdLugarReferencia;
                LugarReferencia = avistamiento.LugarReferencia.Descripcion;
            }
            IdUsuario = avistamiento.Usuario.IdUsuario;
            Usuario = avistamiento.Usuario.Nombres + avistamiento.Usuario.Apellidos;            
            DescripcionAccion = avistamiento.DescripcionAccion;
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
            if (avistamiento.EvidenciaAccion != null)
            {
                var evidencias = JArray.Parse(avistamiento.EvidenciaAccion);
                if (evidencias.Count > 0)
                {
                    var listaEvidencias = new List<string>();
                    foreach (var evidencia in evidencias)
                    {
                        listaEvidencias.Add((string)evidencia);
                    }
                    EvidenciaAccion = listaEvidencias.ToArray();
                }
            }
        }
    }
}
