using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.Salud;
using GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.AvistamientoFlora
{
    public class SaludRepository: ISaludReadOnlyRepository
    {
        private readonly Context _context;

        public SaludRepository(Context context)
        {
            _context = context;
        }

        public Task EditarSaludMCIF(Salud salud)
        {
            throw new NotImplementedException();
        }

        public Task<List<Salud>> ListSalud()
        {
            throw new NotImplementedException();
        }

        public Task<int> RegistrarSaludMCIF(Salud salud)
        {
            throw new NotImplementedException();
        }
    }
}
