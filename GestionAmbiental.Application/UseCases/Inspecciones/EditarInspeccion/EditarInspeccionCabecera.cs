using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.EditarInspeccion
{
    public class EditarInspeccionCabecera : IEditarInspeccionCabecera
    {
        private readonly IEditarInspeccionReadOnlyRepository _editarInspeccionReadonlyRepository;

        public EditarInspeccionCabecera(IEditarInspeccionReadOnlyRepository editarInspeccionReadonlyRepository)
        {
            _editarInspeccionReadonlyRepository = editarInspeccionReadonlyRepository ?? throw new ArgumentNullException(nameof(editarInspeccionReadonlyRepository));
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
            string opcion)
        {
            
            var cabecera = new NuevaInspeccionCabecera();
            cabecera.Id_Cabecera = idCabecera;
            cabecera.Id_Gerencia_Ejecutora = gerenciaMarc;
            cabecera.Id_Empresa_Contratista = empresaMarc;
            cabecera.Fecha_Inspeccion = Convert.ToDateTime(fechaInspeccion + " " + hora);
            cabecera.Lugar_Inspeccionado = lugar;
            cabecera.Id_Responsable_Lugar = responsableLugar;
            cabecera.Inspectores = inspectores;
            cabecera.Objetivo = objetivo;

            switch (opcion)
            {
                case "Observacion": cabecera.Estado = EstadosInspeccion.Pendiente.ToString(); cabecera.Codigo = "TBD"; break;
                case "Guardar": cabecera.Estado = EstadosInspeccion.Registrado.ToString();cabecera.Codigo = "CODIGO" ; break;
                case "GuardarEstado": cabecera.Estado = EstadosInspeccion.Pendiente.ToString(); cabecera.Codigo = "TBD"; break;
            }

            await _editarInspeccionReadonlyRepository.EditarInspeccionCabecera(cabecera);
        }
    }
}
