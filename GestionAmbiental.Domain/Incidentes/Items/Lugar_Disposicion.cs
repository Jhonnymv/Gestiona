using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.Items
{
    public class Lugar_Disposicion
    {
        public int? Id_Lugar_Disposicion { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public Lugar_Disposicion()
        {
        }

        public Lugar_Disposicion(int? id_Lugar_Disposicion, string descripcion, string estado)
        {
            Id_Lugar_Disposicion = id_Lugar_Disposicion;
            Descripcion = descripcion;
            Estado = estado;
        }
    }
}
