using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Producto.EliminarProducto
{
    public class EliminarProductoDerramado : IEliminarProductoDerramado
    {
        private readonly IEditarInvestigacionFinalReadOnlyRepository _editarInvestigacionFinalReadOnlyRepository;

        public EliminarProductoDerramado(IEditarInvestigacionFinalReadOnlyRepository editarInvestigacionFinalReadOnlyRepository)
        {
            _editarInvestigacionFinalReadOnlyRepository = editarInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarInvestigacionFinalReadOnlyRepository));
        }

        public async Task Execute(int IdProducto)
        {
            var input = new Producto_Derramado_Incidente();

            input.Id_Producto_Derramado_Incidente = IdProducto;
            input.Estado = EstadosIncidentes.Eliminado.ToString();

            await _editarInvestigacionFinalReadOnlyRepository.EditarProductoDerramado(input);
        }
    }
}
