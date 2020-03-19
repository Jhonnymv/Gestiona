using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.MotivoIngresoSalida
{
    public class MotivoOutput
    {
        public int Id_Motivo { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public MotivoOutput()
        {
        }
        public MotivoOutput(Domain.ValueObjects.Motivo_Ingreso_Salida motivo)
        {
            Id_Motivo = motivo.Id_Motivo;
            Descripcion = motivo.Descripcion;
            Estado = motivo.Estado;
        }

    }
}
