using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.CompromisosMCB
{
    public class GerenciaUsuarioCMCB
    {
        public int Id { get; set; }
        public GerenciaCMCB Gerencia { get; set; }

        public Usuario Usuario { get; set; }

    }
}
