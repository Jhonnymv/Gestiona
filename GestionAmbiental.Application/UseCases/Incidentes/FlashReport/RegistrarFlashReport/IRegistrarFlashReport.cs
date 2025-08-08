using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.RegistrarFlashReport
{
    public interface IRegistrarFlashReport
    {
        Task<int> Execute(
            int Id_Gerencia, 
            int Id_Empresa, 
            string Lugar,
            DateTime Fecha_Hora,
            int Nivel_Incidente, 
            int Id_Componente_Ambiental,
            int Id_Producto_Incidente,
            float Cantidad_Derramada,
            int Id_Unidad_Cantidad_Derramada,
            string Descripcion,
			 string Registro_Fotografico, 
             string Archivos_Registro_Fotografico,
			int Id_Supervisor_Marcobre,
            string Supervisor_Contratista,
            string Cargo_Supervisor_Marcobre,
            string Cargo_Supervisor_Contratista,
            string Estado,
            string Opcion,
            string usuario_red
            );
    }
}
