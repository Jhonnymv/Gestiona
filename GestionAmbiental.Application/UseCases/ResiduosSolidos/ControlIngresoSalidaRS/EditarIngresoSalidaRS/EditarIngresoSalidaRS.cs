using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlIngresoSalidaRS;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.ResiduosSolidos;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.EditarIngresoSalidaRS
{
    public class EditarIngresoSalidaRS : IEditarIngresoSalidaRS
    {
        private readonly IEditarIngresoSalidaRSReadOnlyRepository _editarIngresoSalidaRSReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public EditarIngresoSalidaRS(
            IEditarIngresoSalidaRSReadOnlyRepository editarIngresoSalidaRSReadOnlyRepository,
            IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _editarIngresoSalidaRSReadOnlyRepository = editarIngresoSalidaRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarIngresoSalidaRSReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(int id_ingreso_salida_rs, string opcion, int arc_cmrs, int ingreso_salida, int motivo, int residuo_solido, string fecha, int responsable, double cantidad, int empresa, int gerencia, string comentario, string usuario_red)
        {
            var fecha_format = DateTime.Parse(fecha);
            var input = new Ingreso_Salida_RS();
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            
            if(opcion.Equals("Eliminar"))
            {
                input.Item = "0";
                input.Estado = EstadosResiduosSolidos.Eliminado.ToString();
            }
            else
            {
                input.Estado = EstadosResiduosSolidos.Registrado.ToString();
            }
                       
            input.ACR_CMRS = new ACR_CMRS();
            input.Ingreso_Salida = new Ingreso_Salida();
            input.Motivo = new Motivo_Ingreso_Salida();
            input.Residuo_Solido = new Residuo_Solido();
            input.Empresa_RS = new Empresa();
            input.Gerencia = new Gerencia();
            input.Responsable_ACR_CMRS = new Responsable_ARC_CRMS();

            input.Id_Ingreso_Salida_RS = id_ingreso_salida_rs;
            input.ACR_CMRS.Id_ACR_CMRS = arc_cmrs;
            input.Ingreso_Salida.Id_Ingreso_Salida = ingreso_salida;
            input.Motivo.Id_Motivo = motivo;
            input.Residuo_Solido.Id_Residuo_Solido = residuo_solido;
            input.Fecha_Ingreso_Salida = fecha_format;
            input.Responsable_ACR_CMRS.Id_Responsable_ACR_CRMS = responsable;
            input.Cantidad = cantidad;
            input.Empresa_RS.IdEmpresa = empresa;
            input.Gerencia.IdGerencia = gerencia;
            input.Comentario = comentario;

            await _editarIngresoSalidaRSReadOnlyRepository.EditarIngresoSalidaRS(input, usuario.IdUsuario);
        }
    }
}
