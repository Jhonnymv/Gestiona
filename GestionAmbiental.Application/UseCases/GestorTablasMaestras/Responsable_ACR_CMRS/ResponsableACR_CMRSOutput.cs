using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS
{
    public class ResponsableACR_CMRSOutput
    {
        public int Id_Responsable_ACR_CRMS { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Estado { get; set; }
        public ResponsableACR_CMRSOutput()
        {

        }
        public ResponsableACR_CMRSOutput(Domain.ValueObjects.Responsable_ARC_CRMS responsable_ARC_CRMS)
        {
            Id_Responsable_ACR_CRMS = responsable_ARC_CRMS.Id_Responsable_ACR_CRMS;
            Nombres = responsable_ARC_CRMS.Nombres;
            Apellidos = responsable_ARC_CRMS.Apellidos;
            Estado = responsable_ARC_CRMS.Estado;
        }
    }
}
