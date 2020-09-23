using GestionAmbiental.Application.Repositories.ConsumoAgua;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua.EditarConsumoAgua
{
    public class EditarConsumoAgua : IEditarConsumoAgua
    {
        private readonly IEditarConsumoAguaReadOnlyRepository _editarConsumoAguaReadOnlyRepository;

        public EditarConsumoAgua(IEditarConsumoAguaReadOnlyRepository editarConsumoAguaReadOnlyRepository)
        {
            _editarConsumoAguaReadOnlyRepository = editarConsumoAguaReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarConsumoAguaReadOnlyRepository));
        }

        public async Task Execute(int idConsumo, int resolucion, string fecha, double tiempoBombeo, double lecturaInicial, double lecturaFinal, double volBombeado, double caudal)
        {
            DateTime fechaDT = Convert.ToDateTime(fecha);

            var consumo = new Domain.ConsumoAgua.ConsumoAgua
            {
                IdConsumoAgua = idConsumo,
                ResolucionConsumoAgua = new Domain.ConsumoAgua.ResolucionConsumoAgua
                {
                    IdResolucionConsumoAgua = resolucion
                },
                Fecha = fechaDT,
                TiempoBombeo = tiempoBombeo,
                LecturaInicial = lecturaInicial,
                LecturaFinal = lecturaFinal,
                VolumenBombeado = volBombeado,
                CaudalPromedio = caudal,
                Estado = "Registrado"
            };

            await _editarConsumoAguaReadOnlyRepository.EditarConsumoAgua(consumo);
        }
    }
}
