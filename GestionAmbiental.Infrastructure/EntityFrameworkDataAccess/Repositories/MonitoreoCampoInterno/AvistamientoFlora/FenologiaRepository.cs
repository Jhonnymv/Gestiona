using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.Fenologia;
using GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.AvistamientoFlora
{
    public class FenologiaRepository:IFenologiaReadOnlyRepository
    {
        private readonly Context _context;

        public FenologiaRepository(Context context)
        {
            _context = context;
        }

        public Task EditarFenologiaMCIF(Fenologia fenologiaMCIF)
        {
            throw new NotImplementedException();
        }

        public Task<List<Fenologia>> ListFenologiaMCIF()
        {
            throw new NotImplementedException();
        }

        public Task<int> RegistrarFenologiaMCIF(Fenologia fenologiaMCIF)
        {
            throw new NotImplementedException();
        }
    }
}
