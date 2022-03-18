using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.SustentoCMCB.EditarRegistro
{
    public interface IEditarRegistro
    {
        Task Execute(int Id
        ,int Id_Compromiso
        ,string Fecha_limite
        ,string Fecha_entrega
        ,string Fecha_validacion
        ,int Id_Responsable1
        ,int Id_Responsable2
        ,int Id_Asesor
        ,string Estado
        ,string Sustento_cumplimiento
        ,string Detalle_compromiso
        ,string Evidencia
        ,string Comentario_implementacion
        ,string Comentario_verificacion
        ,string Medio_verificacion);
        Task ExecuteImplementar(int Id
        ,string Fecha_entrega
        ,string Sustento_cumplimiento
        ,string Evidencia
        ,string Comentario_implementacion);
        Task ExecuteVerificar(int Id
        , int Id_Asesor
        , string Fecha_validacion
        ,string Comentario_verificacion
        , string Medio_verificacion
            );
    }
}
