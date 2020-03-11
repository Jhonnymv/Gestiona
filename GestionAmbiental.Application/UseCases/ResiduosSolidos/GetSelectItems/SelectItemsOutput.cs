using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.GetSelectItems
{
    public class SelectItemsOutput
    {
        public List<Empresa> ItemsEmpresa { get; set; }
        public List<ACR_CMRS> ItemsACR_CMRS { get; set; }
        public List<EO_RS> ItemsEO_RS { get; set; }
        public List<Ingreso_Salida> ItemsIngresoSalida { get; set; }
        public List<Motivo_Ingreso_Salida> ItemsMotivoIngresoSalida { get; set; }
        public List<Residuo_Solido> ItemsResiduoSolido { get; set; }
        public List<Tipo_Residuo> ItemsTipoResiduo { get; set; }
        public List<Usuario> ItemsUsuarios { get; set; }
        public List<Usuario> ItemsAsesoresMA { get; set; }
        public List<Responsable_ARC_CRMS> ItemsResponsableACR_CMRS { get; set; }
        public List<Gerencia> ItemsGerencia { get; set; }
    }
}
