using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociada;
using GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.AvistamientoFlora
{
    public class FaunaAsociadaRepository:IFaunaAsociadaReadOnlyRepository
    {
        private readonly Context _context;

        public FaunaAsociadaRepository(Context context)
        {
            _context = context;
        }

        public Task EditarFaunaAsociada(FaunaAsociada faunaAsociada)
        {
            throw new NotImplementedException();
        }

        public Task<List<FaunaAsociada>> ListFaunaAsociada()
        {
            throw new NotImplementedException();
        }

        public Task<int> RegistrarFaunaAsociada(FaunaAsociada faunaAsociada)
        {
            throw new NotImplementedException();
        }
    }
}
