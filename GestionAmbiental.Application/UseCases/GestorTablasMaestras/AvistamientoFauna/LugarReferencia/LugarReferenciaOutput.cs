using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.LugarReferencia
{
    public class LugarReferenciaOutput
    {
        public int IdLugarReferencia { get; set; }
        public string Descripcion { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Estado { get; set; }
        public LugarReferenciaOutput()
        {
        }
        public LugarReferenciaOutput(Domain.AvistamientoFauna.Items.LugarReferencia lugarReferencia)
        {
            IdLugarReferencia = lugarReferencia.IdLugarReferencia;
            Descripcion = lugarReferencia.Descripcion;
            Latitud = lugarReferencia.Latitud;
            Longitud = lugarReferencia.Longitud;
            Estado = lugarReferencia.Estado;
        }
    }   
}
