
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.RegistrarInspeccion
{
    public interface IRegistrarInspeccionObservacion
    {
        
        Task<int> Execute(
            int Id_Inspeccion_Cabecera, string Tipo_Inspeccion, int Asesor_MA_Seguridad, string Descripcion_Condicion, int Id_Tipo_Condicion, 
            List<string> Evidencia_Condicion, string Nivel_Riesgo_Ambiental, string Descripcion_Accion,
            int Id_Responsable_Marcobre, string Responsable_Contratista,string Fecha_Programada,string usuario_red);
    }
}
