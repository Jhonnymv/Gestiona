using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.EditarObservacion
{
    public interface IEditarObservacion
    {
        Task Execute(
            int Id_Inspeccion_Cabecera,
            int Id_Observacion,
            string Tipo_Inspeccion,
            int Asesor_MA_Seguridad,
            string Descripcion_Acto_Condicion,
            string Descripcion_Condicion,
            int? Id_Tipo_Condicion,
            List<string> Evidencia_Condicion,
            string Nivel_Riesgo_Ambiental, 
            string Descripcion_Accion,
            int Id_Responsable_Marcobre, 
            string Responsable_Contratista,
            string Correo_Contratista,
            string Fecha_Programada,
            string usuario_red,
            bool Cambio_Fecha_Vencida
            );
    }
}
