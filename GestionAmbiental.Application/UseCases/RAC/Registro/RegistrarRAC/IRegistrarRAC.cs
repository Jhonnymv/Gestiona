using System;using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Registro.RegistrarRAC
{
    public interface IRegistrarRAC
    {
        Task<int> Execute(string nombres_apellidos, int? gerencia, int? empresa, 
            string otra_emp, int empresa_obs, int gerencia_obs, string ubicacion, 
            string fecha, string hora, string sso_ma, string clasificacion,
            string nivel_riesgo, int tipo_condicion, string descripcion, string medida, 
            string pac, int responsable_cierre,string fecha_cierre, int supervisor, string evidencia, string usuario_red);
    }
}
