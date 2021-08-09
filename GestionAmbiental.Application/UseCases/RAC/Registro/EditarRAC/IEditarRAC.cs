using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Registro.EditarRAC
{
    public interface IEditarRAC
    {
        Task Execute(int Id_RAC, string Nombres_Apellidos, int? Id_Gerencia, int? Id_Empresa, string Otra_Empresa, int Id_Empresa_Obs, int Id_Gerencia_Obs, string Ubicacion, string Fecha, string SSO_MA, string Clasificacion, string NivelRiesgo,
            int Id_Tipo_Condicion, string Descripcion_Acto_Cond, string Medida_Control, string PAC, int Id_Responsable_Cierre, string Fecha_Cierre, int Id_Supervisor, string Evidencia_Obs, string Evidencia_Cierre, string Comentario_Cierre, string Comentario_Verificacion, string usuarioRed
        );
    }
}
