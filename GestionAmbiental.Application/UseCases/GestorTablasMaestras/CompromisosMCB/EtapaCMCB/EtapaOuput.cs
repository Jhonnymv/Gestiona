using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.CompromisosMCB.EtapaCMCB
{
    public class EtapaOuput
    {
        public int Id { get; set; }
        public string Etapa { get; set; }
        public EtapaOuput(){}
        public EtapaOuput(Domain.CompromisosMCB.EtapaCMCB etapaCMCB)
        {
            Id = etapaCMCB.Id;
            Etapa = etapaCMCB.Etapa;
        }
    }
}
