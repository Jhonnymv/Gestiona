using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun
{
    public class NombreComunOutput
    {
        public int IdNombreComunFauna { get; set; }
        public string Descripcion { get; set; }
        public int IdTipoFauna { get; set; }
        public string TipoFauna { get; set; }
        public string Foto { get; set; }
        public string Estado { get; set; }

        public NombreComunOutput()
        {
        }

        public NombreComunOutput(Domain.AvistamientoFauna.Items.NombreComunFauna nombreComunFauna)
        {
            IdNombreComunFauna = nombreComunFauna.IdNombreComunFauna;
            Descripcion = nombreComunFauna.Descripcion;
            IdTipoFauna = nombreComunFauna.TipoFauna.IdTipoFauna;
            TipoFauna = nombreComunFauna.TipoFauna.Descripcion;
            Foto = nombreComunFauna.Foto;
            Estado = nombreComunFauna.Estado;
        }
    }
}
