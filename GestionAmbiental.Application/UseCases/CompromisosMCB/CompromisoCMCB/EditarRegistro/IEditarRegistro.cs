using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.CompromisoCMCB.EditarRegistro
{
    public interface IEditarRegistro
    {
        Task Execute(int Id,
            string Codigo,
            int? Id_Criterio,
            string? Detalle_compromiso,
            int? Id_Etapa,
            string? Texto_compromiso,
            int? Id_Origen,
            string? Tomo,
            string? Capitulo,
            string? Seccion,
            string? Pagina,
            string? Resolucion,
            int? Id_Autoridad,
            DateTime? Fecha,
            int? Id_Compromiso,
            int? Id_Aspecto,
            int? Id_Componente,
            int? Id_Gerencia,
            int? Id_Area,
            string? Frecuencia,
            int? Id_Entidad,
            int? Id_Responsable1,
            int? Id_Responsable2,
            string? Frecuencia_reporte,
            string? Sustentos,
            string? Estado,
            int? Id_Aprueba,
            string? Medio_verificacion,

            string? Ano,
            string? Duplicado,
            int? Id_TipoCompromiso,
            string? Actividad,
            int? Id_ComponenteAux,
            int? Id_FactorAmbiental,
            string? Vigencia,
            string? Motivo,
            string? Comentario
            );
    }

}
