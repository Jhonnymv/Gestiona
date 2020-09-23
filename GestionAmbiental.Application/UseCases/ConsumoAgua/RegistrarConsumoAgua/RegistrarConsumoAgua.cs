using GestionAmbiental.Application.Repositories.ConsumoAgua;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua.RegistrarConsumoAgua
{
    public class RegistrarConsumoAgua : IRegistrarConsumoAgua
    {
        private readonly IRegistrarConsumoAguaReadOnlyRepository _registrarConsumoAguaReadOnlyRepository;
        private readonly IGetUsuario _getUsuario;

        public RegistrarConsumoAgua(IRegistrarConsumoAguaReadOnlyRepository registrarConsumoAguaReadOnlyRepository, IGetUsuario getUsuario)
        {
            _registrarConsumoAguaReadOnlyRepository = registrarConsumoAguaReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarConsumoAguaReadOnlyRepository));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task<int> Execute(int resolucion, string fecha, double tiempoBombeo, double lecturaInicial, double lecturaFinal, double volBombeado, double caudal, string usuarioRed)
        {
            DateTime fechaDT = Convert.ToDateTime(fecha);
            var usuario = await _getUsuario.Execute(usuarioRed);

            var consumo = new Domain.ConsumoAgua.ConsumoAgua { 
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
                IdUsuario = usuario.IdUsuario,
                Estado = "Registrado"
            };

            return await _registrarConsumoAguaReadOnlyRepository.RegistrarConsumoAgua(consumo);

        }
    }
}
