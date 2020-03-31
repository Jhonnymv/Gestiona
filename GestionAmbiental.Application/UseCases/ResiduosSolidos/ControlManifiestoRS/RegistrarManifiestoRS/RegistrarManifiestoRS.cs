using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.RegistrarManifiestoRS
{
    public class RegistrarManifiestoRS : IRegistrarManifiestoRS
    {
        private readonly IRegistrarManifiestoRSReadOnlyRepository _registrarManifiestoRSReadOnlyRepository;
        private readonly IGetManifiestosRSReadOnlyRepository _getManifiestosRSReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public RegistrarManifiestoRS(
            IRegistrarManifiestoRSReadOnlyRepository registrarManifiestoRSReadOnlyRepository, 
            IGetManifiestosRSReadOnlyRepository getManifiestosRSReadOnlyRepository, 
            IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _registrarManifiestoRSReadOnlyRepository = registrarManifiestoRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarManifiestoRSReadOnlyRepository));
            _getManifiestosRSReadOnlyRepository = getManifiestosRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(getManifiestosRSReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task<int> Execute(
            string opcion,
            int Serie_Manifiesto, 
            string Num_Manifiesto,
            string Fecha_Emision_Manifiesto,
            string Observacion_Manifiesto,
            int Id_Residuo_Solido,
            double Cantidad_RS_Evacuado,
            int Id_EO_RS_Salida, 
            string Registro_EO_RS_Salida,
            string Fecha_Vencimiento_Registro_Salida,
            string Num_Autorizacion_Municipal_Salida, 
            string Num_Autorizacion_Hoja_Ruta, 
            int Id_Conductor,
            string Num_Licencia_Condutor, 
            int Id_Vehiculo, 
            double Capacidad_Vehiculo,
            int Id_EO_RS_Llegada,
            string Registro_EO_RS_Llegada,
            string Fecha_Vencimiento_Registro_Llegada, 
            string Num_Autorizacion_Sanitaria_Llegada,
            string Num_Autorizacion_Municipal_Llegada,
            string usuario_red)
        {
            var input = new Domain.ResiduosSolidos.Manifiesto_RS();
                        
            switch (opcion)
            {
                case "Registrar":
                    input.Estado = EstadosResiduosSolidos.Registrado.ToString(); 
                    input.Item = (Convert.ToInt32(await _getManifiestosRSReadOnlyRepository.GetLastItem()) + 1).ToString();
                    break;
                case "GuardarEstado":
                    input.Estado = EstadosResiduosSolidos.Pendiente.ToString();
                    input.Item = "TBD";
                    break;
            }
            DateTime? fEmisionManifiesto, fVencRegSalida, fVencRegLlegada;

            if (Fecha_Emision_Manifiesto != null)
                fEmisionManifiesto = Convert.ToDateTime(Fecha_Emision_Manifiesto);
            else
                fEmisionManifiesto = null;

            if (Fecha_Vencimiento_Registro_Salida != null)
                fVencRegSalida = Convert.ToDateTime(Fecha_Vencimiento_Registro_Salida);
            else
                fVencRegSalida = null;

            if (Fecha_Vencimiento_Registro_Llegada != null)
                fVencRegLlegada = Convert.ToDateTime(Fecha_Vencimiento_Registro_Llegada);
            else
                fVencRegLlegada = null;

            input.Id_Registrado_Por = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuario_red);
            input.Serie_Manifiesto = Serie_Manifiesto;
            input.Num_Manifiesto = Num_Manifiesto;
            input.Fecha_Emision_Manifiesto = fEmisionManifiesto.Value;
            input.Observacion_Manifiesto = Observacion_Manifiesto;
            input.Residuo_Solido = new Domain.ValueObjects.Residuo_Solido();
            input.Residuo_Solido.Id_Residuo_Solido = Id_Residuo_Solido;
            input.Cantidad_RS_Evacuado = Cantidad_RS_Evacuado;
            input.EO_RS_Salida = new Domain.ValueObjects.EO_RS();
            input.EO_RS_Salida.Id_EO_RS = Id_EO_RS_Salida;
            input.Registro_EO_RS_Salida = Registro_EO_RS_Salida;
            input.Fecha_Vencimiento_Registro_Salida = fVencRegSalida;
            input.Num_Autorizacion_Municipal_Salida = Num_Autorizacion_Municipal_Salida;
            input.Num_Autorizacion_Hoja_Ruta = Num_Autorizacion_Hoja_Ruta;
            input.Conductor = new Domain.ValueObjects.Conductor_RS();
            input.Conductor.Id_Conductor_RS = Id_Conductor;
            input.Num_Licencia_Condutor = Num_Licencia_Condutor;
            input.Vehiculo = new Domain.ValueObjects.Vehiculo_RS();
            input.Vehiculo.Id_Vehiculo_RS = Id_Vehiculo;
            input.Capacidad_Vehiculo = Capacidad_Vehiculo;
            input.EO_RS_Llegada = new Domain.ValueObjects.EO_RS();
            input.EO_RS_Llegada.Id_EO_RS = Id_EO_RS_Llegada;
            input.Registro_EO_RS_Llegada = Registro_EO_RS_Llegada;
            input.Fecha_Vencimiento_Registro_Llegada = fVencRegLlegada;
            input.Num_Autorizacion_Sanitaria_Llegada = Num_Autorizacion_Sanitaria_Llegada;
            input.Num_Autorizacion_Municipal_Llegada = Num_Autorizacion_Municipal_Llegada;

            return await _registrarManifiestoRSReadOnlyRepository.RegistrarManifiestosRS(input);
        }
    }
}
