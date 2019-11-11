using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Enum
{
    public enum EstadosInspeccion
    {
        Registrado = 1,
        Pendiente = 2,
        Implementado = 3,
        Vencido = 4,
        Ampliado = 5,
        Aprobado = 6,
        Rechazado = 7,
        AprobadoMA = 8,
        RechazadoMA = 9
    }
}
