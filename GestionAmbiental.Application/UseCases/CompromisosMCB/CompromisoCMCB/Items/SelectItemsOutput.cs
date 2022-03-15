using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.CompromisoCMCB.Items
{
    public class SelectItemsOutput
    {
        public List<Domain.CompromisosMCB.AreaCMCB> ItemsArea { get; set; }
        public List<Domain.CompromisosMCB.AspectoCMCB> ItemsAspecto { get; set; }
        public List<Domain.CompromisosMCB.AutoridadCMCB> ItemsAutoridad { get; set; }
        public List<Domain.CompromisosMCB.ComponenteCMCB> ItemsComponente { get; set; }
        public List<Domain.CompromisosMCB.CriterioCMCB> ItemsCriterio { get; set; }
        public List<Domain.CompromisosMCB.EtapaCMCB> ItemsEtapa { get; set; }
        public List<Domain.CompromisosMCB.GerenciaCMCB> ItemsGerencia { get; set; }
        public List<Domain.CompromisosMCB.GerenciaUsuarioCMCB> ItemsGerenciaUsuario { get; set; }
        public List<Domain.CompromisosMCB.MedioVerificacionCMCB> ItemsMedioVerificacion { get; set; }
        public List<Domain.CompromisosMCB.OrigenCMCB> ItemsOrigen { get; set; }

        public List<Domain.ValueObjects.Usuario> ItemsUsuario { get; set; }
    }
}
