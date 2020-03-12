using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.RegistrarManifiestoRS
{
    public interface IRegistrarManifiestoRS
    {
        Task<int> Execute(
            string opcion,
            int Serie_Manifiesto,
            string Num_Manifiesto,
            DateTime Fecha_Emision_Manifiesto,
            string Observacion_Manifiesto,
            int Id_Residuo_Solido,
            double Cantidad_RS_Evacuado, 
            int Id_EO_RS_Salida, 
            string Registro_EO_RS_Salida,
            DateTime Fecha_Vencimiento_Registro_Salida,
            string Num_Autorizacion_Municipal_Salida,
            string Num_Autorizacion_Hoja_Ruta,
            int Id_Conductor,
            string Num_Licencia_Condutor,
            int Id_Vehiculo, 
            double Capacidad_Vehiculo, 
            int EO_RS_Llegada,
            string Registro_EO_RS_Llegada,
            DateTime Fecha_Vencimiento_Registro_Llegada,
            string Num_Autorizacion_Sanitaria_Llegada,
            string Num_Autorizacion_Municipal_Llegada,
            string usuario_red);
    }
}
