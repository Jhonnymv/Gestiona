using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.SustentoCMCB.NuevoRegistro
{
    public interface INuevoRegistro
    {
        Task<int> Execute(int Id_Compromiso
        , string Fecha_limite
        , int Id_Responsable1
        , int Id_Responsable2
        , string Detalle_compromiso
        , string Medio_verificacion);
    }
}
