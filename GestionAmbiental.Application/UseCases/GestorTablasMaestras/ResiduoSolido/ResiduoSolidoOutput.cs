using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido
{
    public class ResiduoSolidoOutput
    {
        public int Id_Residuo_Solido { get; set; }
        public int Id_Tipo_Residuo { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public ResiduoSolidoOutput()
        {
        }
        public ResiduoSolidoOutput(Domain.ValueObjects.Residuo_Solido residuo_solido)
        {
            Id_Residuo_Solido = residuo_solido.Id_Residuo_Solido;
            Id_Tipo_Residuo = residuo_solido.Tipo_Residuo.Id_Tipo_Residuo;
            Descripcion = residuo_solido.Descripcion;
            Estado = residuo_solido.Estado;
        }
    }
}
