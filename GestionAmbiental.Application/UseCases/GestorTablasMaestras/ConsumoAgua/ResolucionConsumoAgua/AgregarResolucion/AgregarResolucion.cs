using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.ResolucionConsumoAgua.AgregarResolucion
{
    public class AgregarResolucion : IAgregarResolucion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarResolucion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string descripcion, string anio, string tipoUso, string claseDerecho, int idFuente, double volumenOtorgado, double? limiteDiario, double? limiteSemanal, double? limiteMensual,string fechaVencimiento, double? caudal,  string actividad)
        {
            DateTime? fechaVencimientoDT = null;
            if (fechaVencimiento!=null)
                fechaVencimientoDT = Convert.ToDateTime(fechaVencimiento);

            var input = new Domain.ConsumoAgua.ResolucionConsumoAgua()
            {
                Resolucion = descripcion,
                Anio = anio,
                TipoUso = tipoUso,
                ClaseDerecho = claseDerecho,
                FuenteAgua = new Domain.ConsumoAgua.FuenteAgua
                {
                    IdFuenteAgua = idFuente
                },
                VolumenOtorgado = volumenOtorgado,
                LimiteDiario = limiteDiario,
                LimiteSemanal = limiteSemanal,
                LimiteMensual = limiteMensual,
                FechaVencimiento = fechaVencimientoDT,
                Caudal = caudal,
                Actividad = actividad
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddResolucionConsumoAgua(input);
        }
    }
}
