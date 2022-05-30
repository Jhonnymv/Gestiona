using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.NombreCientifico;
using GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.AvistamientoFlora
{
    public class NombreCientificoRepository:INombreCientificoReadOnlyRepository
    {
        private readonly Context _context;

        public NombreCientificoRepository(Context context)
        {
            _context = context;
        }

        public Task EditarNombreCientificoMCIF(NombreCientifico nombreCientifico)
        {
            throw new NotImplementedException();
        }

        public Task<List<NombreCientifico>> ListNombreCientifico()
        {
            throw new NotImplementedException();
        }

        public Task<int> RegistrarNombreCientificoMCIF(NombreCientifico nombreCientifico)
        {
            throw new NotImplementedException();
        }
    }
}
