using GestionAmbiental.Application.Repositories.ConsumoAgua;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua.RegistrarConsumoAguaMasivo
{
    public class RegistrarConsumoAguaMasivo : IRegistrarConsumoAguaMasivo
    {
        private readonly IRegistrarConsumoAguaReadOnlyRepository _registrarConsumoAguaReadOnlyRepository;
        private readonly IGetUsuario _getUsuario;

        public RegistrarConsumoAguaMasivo(IRegistrarConsumoAguaReadOnlyRepository registrarConsumoAguaReadOnlyRepository, IGetUsuario getUsuario)
        {
            _registrarConsumoAguaReadOnlyRepository = registrarConsumoAguaReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarConsumoAguaReadOnlyRepository));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task<int> Execute(List<ConsumoAguaInput> datos,string usuarioRed)
        {
            var usuario = await _getUsuario.Execute(usuarioRed);
            var input = new List<Domain.ConsumoAgua.ConsumoAgua>();
            foreach(var dato in datos)
            {
                input.Add(new Domain.ConsumoAgua.ConsumoAgua
                {
                    ResolucionConsumoAgua = new Domain.ConsumoAgua.ResolucionConsumoAgua {
                        IdResolucionConsumoAgua = dato.IdResolucionConsumoAgua
                    },
                    Fecha = dato.Fecha,
                    TiempoBombeo = dato.TiempoBombeo,
                    LecturaInicial = dato.LecturaInicial,
                    LecturaFinal = dato.LecturaFinal,
                    VolumenBombeado = dato.VolumenBombeado,
                    CaudalPromedio = dato.CaudalPromedio,
                    IdUsuario = usuario.IdUsuario,
                    Estado = "Registrado"
                });
            }
            return await _registrarConsumoAguaReadOnlyRepository.RegistrarConsumoAguaMasivo(input);
        }
    }
}
