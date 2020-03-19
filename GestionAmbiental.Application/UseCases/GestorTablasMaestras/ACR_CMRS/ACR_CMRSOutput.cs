using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ACR_CMRS
{
    public class ACR_CMRSOutput
    {
        public int Id_ACR_CMRS { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public ACR_CMRSOutput()
        {
        }

        public ACR_CMRSOutput(Domain.ValueObjects.ACR_CMRS acr_cmrs)
        {
            Id_ACR_CMRS = acr_cmrs.Id_ACR_CMRS;
            Descripcion = acr_cmrs.Descripcion;
            Estado = acr_cmrs.Estado;
        }
    }
}
