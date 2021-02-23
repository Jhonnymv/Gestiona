using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Enum
{
    public enum EstadosDocumentosSGA
    {
        Pendiente = 1,
        Registrado = 2,
        EnRevision = 3,
        EnAprobacion = 4,
        Firmas = 5,
        Aprobado = 6,
        Superado = 7
    }
}
