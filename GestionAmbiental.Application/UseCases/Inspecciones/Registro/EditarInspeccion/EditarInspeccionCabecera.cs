using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.EditarInspeccion
{
    public class EditarInspeccionCabecera : IEditarInspeccionCabecera
    {
        private readonly IEditarInspeccionReadOnlyRepository _editarInspeccionReadonlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public EditarInspeccionCabecera(
            IEditarInspeccionReadOnlyRepository editarInspeccionReadonlyRepository,
            IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _editarInspeccionReadonlyRepository = editarInspeccionReadonlyRepository ?? throw new ArgumentNullException(nameof(editarInspeccionReadonlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(
            int idCabecera,
            string objetivo,
            int gerenciaMarc,
            int empresaMarc,
            string lugar,
            string fechaInspeccion,
            string hora,
            int responsableLugar,
            string inspectores,
            string opcion,
            string usuario_red)
        {

            var cabecera = new NuevaInspeccionCabecera();
            cabecera.IdCabecera = idCabecera;
            cabecera.IdGerencia = gerenciaMarc;
            cabecera.IdEmpresa = empresaMarc;
            cabecera.FechaInspeccion = Convert.ToDateTime(fechaInspeccion + " " + hora);
            cabecera.LugarInspeccionado = lugar;
            cabecera.IdResponsableArea = responsableLugar;
            cabecera.Inspectores = inspectores;
            cabecera.Objetivo = objetivo;

            var Id_Usuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuario_red);

            switch (opcion)
            {
                case "Observacion": cabecera.Estado = EstadosInspeccion.Pendiente.ToString(); cabecera.Codigo = "TBD"; break;
                case "Guardar":
                    cabecera.Estado = EstadosInspeccion.Registrado.ToString();
                    var codigoAnterior = await _editarInspeccionReadonlyRepository.GetUltimoCodigo();
                    if (codigoAnterior != null)
                    {
                        var correlativo = (Convert.ToInt32(codigoAnterior.Remove(0, 3))+1).ToString().PadLeft(5, '0');                        
                        cabecera.Codigo = "IP-" +correlativo;
                    }
                    else
                        cabecera.Codigo = "IP-00001";
                    break;
                case "GuardarEstado": cabecera.Estado = EstadosInspeccion.Pendiente.ToString(); cabecera.Codigo = "TBD"; break;
            }

            await _editarInspeccionReadonlyRepository.EditarInspeccionCabecera(cabecera, Id_Usuario);
        }
    }
}
