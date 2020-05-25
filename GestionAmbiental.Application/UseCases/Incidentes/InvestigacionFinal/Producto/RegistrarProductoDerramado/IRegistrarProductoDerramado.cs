using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Producto.RegistrarProductoDerramado
{
    public interface IRegistrarProductoDerramado
    {
        Task<int> Execute(int IdInvestigacion, int? Id_Producto_Incidente, int? Id_Tipo_Producto, string Estado_Producto, double? Cantidad_Derramada, int? Id_Unidad, string Equipo_Responsable, int? Id_Estado_Equipo_Responsable, double? Cantidad_Recuperado, double? Cantidad_No_Recuperado);
    }
}
