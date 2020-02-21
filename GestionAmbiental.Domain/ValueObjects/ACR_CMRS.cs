using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class ACR_CMRS
    {
        public int Id_ACR_CMRS { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public ACR_CMRS()
        {
        }

        public ACR_CMRS(int id_ACR_CMRS, string descripcion, string estado)
        {
            Id_ACR_CMRS = id_ACR_CMRS;
            Descripcion = descripcion;
            Estado = estado;
        }
    }
}
