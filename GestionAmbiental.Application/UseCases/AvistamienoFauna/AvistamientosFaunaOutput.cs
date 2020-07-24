using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna
{
    public class AvistamientosFaunaOutput
    {
        public List<AvistamientoFaunaOutput> Avistamientos { get; set; }
        public int NAvistamientos { get; set; }
    }
}
