using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Factor_Causa_Incidente
{
    public class FactorCausaOutput
    {
        public int Id_Factor_Causa_Incidente { get; set; }
        public string Descripcion { get; set; }
        public string Tipo_Factor { get; set; }
        public string Estado { get; set; }

        public FactorCausaOutput()
        {
        }
        public FactorCausaOutput(Domain.Incidentes.InvestigacionFinal.Factor_Causa_Incidente factor)
        {
            Id_Factor_Causa_Incidente = factor.Id_Factor_Causa_Incidente;
            Descripcion = factor.Descripcion;
            Tipo_Factor = factor.Tipo_Factor;
            Estado = factor.Estado;
        }
    }
}
