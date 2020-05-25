using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Producto.EditarProducto
{
    public class EditarProductoDerramado : IEditarProductoDerramado
    {
        private readonly IEditarInvestigacionFinalReadOnlyRepository _editarInvestigacionFinalReadOnlyRepository;

        public EditarProductoDerramado(IEditarInvestigacionFinalReadOnlyRepository editarInvestigacionFinalReadOnlyRepository)
        {
            _editarInvestigacionFinalReadOnlyRepository = editarInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarInvestigacionFinalReadOnlyRepository));
        }

        public async Task<int> Execute(int IdProducto, int IdInvestigacion, int? Id_Producto_Incidente, int? Id_Tipo_Producto, string Estado_Producto, double? Cantidad_Derramada, int? Id_Unidad, string Equipo_Responsable, int? Id_Estado_Equipo_Responsable, double? Cantidad_Recuperado, double? Cantidad_No_Recuperado)
        {
            var input = new Producto_Derramado_Incidente();

            input.Id_Producto_Derramado_Incidente = IdProducto;
            input.Id_Investigacion_Final = IdInvestigacion;
            input.Producto_Incidente = new Domain.Incidentes.Items.Producto_Incidente();
            input.Producto_Incidente.Id_Producto_Incidente = Id_Producto_Incidente;
            input.Tipo_Producto = new Domain.Incidentes.Items.Tipo_Producto_Incidente();
            input.Tipo_Producto.Id_Tipo_Producto_Incidente = Id_Tipo_Producto;
            input.Estado_Producto = Estado_Producto;
            input.Cantidad_Derramada = Cantidad_Derramada;
            input.Unidad = new Domain.Incidentes.Items.Unidad_Medida();
            input.Unidad.Id_Unidad_Medida = Id_Unidad;
            input.Equipo_Responsable = Equipo_Responsable;
            input.Estado_Equipo_Responsable = new Domain.Incidentes.Items.Estado_Equipo_Incidente();
            input.Estado_Equipo_Responsable.Id_Estado_Equipo_Incidente = Id_Estado_Equipo_Responsable;
            input.Cantidad_Recuperado = Cantidad_Recuperado;
            input.Cantidad_No_Recuperado = Cantidad_No_Recuperado;
            input.Estado = EstadosIncidentes.Registrado.ToString();

            return await _editarInvestigacionFinalReadOnlyRepository.EditarProductoDerramado(input);
        }
    }
}
