using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.Uso;
using GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.AvistamientoFlora
{
    public class UsoRepository: IUsoReadOnlyRepository
    {
        private readonly Context _context;

        public UsoRepository(Context context)
        {
            _context = context;
        }

        public Task EditarUsosMCIF(Uso uso)
        {
            throw new NotImplementedException();
        }

        public Task<List<Uso>> ListUsos()
        {
            throw new NotImplementedException();
        }

        public Task<int> RegistrarUsosMCIF(Uso uso)
        {
            throw new NotImplementedException();
        }
    }
}
