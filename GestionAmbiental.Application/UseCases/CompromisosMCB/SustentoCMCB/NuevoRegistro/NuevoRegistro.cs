using GestionAmbiental.Application.Repositories.CompromisosMCB.SustentoCMCB;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.SustentoCMCB.NuevoRegistro
{
    public class NuevoRegistro : INuevoRegistro
    {
        private readonly ISustentoReadOnlyRepository _sustentoReadOnlyRepository;

        public NuevoRegistro(ISustentoReadOnlyRepository sustentoReadOnlyRepository)
        {
            _sustentoReadOnlyRepository = sustentoReadOnlyRepository ?? throw new ArgumentNullException(nameof(sustentoReadOnlyRepository));
        }

        public async Task<int> Execute(int Id_Compromiso, string Fecha_limite, int Id_Responsable1, int Id_Responsable2, string Detalle_compromiso, string Medio_verificacion)
        {
            var input = new Domain.CompromisosMCB.SustentoCMCB
            {
                CompromisoCMCB = new Domain.CompromisosMCB.CompromisoCMCB { Id = Id_Compromiso },
                Responsable1 = new Domain.ValueObjects.Usuario { IdUsuario = Id_Responsable1 },
                Responsable2 = new Domain.ValueObjects.Usuario { IdUsuario = Id_Responsable2 },
                Detalle_compromiso = Detalle_compromiso,
                Medio_verificacion = Medio_verificacion,
                Estado = EstadosSustento.Registrado.ToString()
            };
            if (Fecha_limite == null && Fecha_limite.Length > 0)
                input.Fecha_limite = Convert.ToDateTime(Fecha_limite);

            return await _sustentoReadOnlyRepository.RegistrarSustento(input);
        }
    }
}
