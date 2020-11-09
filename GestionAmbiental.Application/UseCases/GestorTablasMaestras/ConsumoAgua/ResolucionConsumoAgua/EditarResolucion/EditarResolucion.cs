using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.ResolucionConsumoAgua.EditarResolucion
{
    public class EditarResolucion : IEditarResolucion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarResolucion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int id, string descripcion, string anio, string tipoUso, string claseDerecho, int idFuente, double volumenOtorgado, string actividad, double? limiteDiario, double? limiteSemanal, double? limiteMensual, string fechaVencimiento, double? caudal, string estado)
        {
            DateTime? fechaVencimientoDT = null;
            if (fechaVencimiento != null)
                fechaVencimientoDT = Convert.ToDateTime(fechaVencimiento);

            var input = new Domain.ConsumoAgua.ResolucionConsumoAgua();
            input.IdResolucionConsumoAgua = id;
            input.Resolucion = descripcion;
            input.Anio = anio;
            input.TipoUso = tipoUso;
            input.ClaseDerecho = claseDerecho;
            input.FuenteAgua = new Domain.ConsumoAgua.FuenteAgua
            {
                IdFuenteAgua = idFuente
            };
            input.VolumenOtorgado = volumenOtorgado;
            input.LimiteDiario = limiteDiario;
            input.LimiteSemanal = limiteSemanal;
            input.LimiteMensual = limiteMensual;
            input.Actividad = actividad;
            input.FechaVencimiento = fechaVencimientoDT;
            input.Caudal = caudal;
            input.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateResolucionConsumoAgua(input);
        }
    }
}
