using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.Vigor;
using GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.AvistamientoFlora
{
    public class VigorRepository: IVigorReadOnlyRepository
    {
        private readonly Context _context;

        public VigorRepository(Context context)
        {
            _context = context;
        }

        public Task EditarVigorMCIF(Vigor vigor)
        {
            throw new NotImplementedException();
        }

        public Task<List<Vigor>> ListVigor()
        {
            throw new NotImplementedException();
        }

        public Task<int> RegistrarVigorMCIF(Vigor vigor)
        {
            throw new NotImplementedException();
        }
    }
}
