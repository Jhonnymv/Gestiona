using GestionAmbiental.Application.Repositories.CompromisosMCB.FactorAmbientalCMCB;
using GestionAmbiental.Domain.CompromisosMCB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.CompromisosMCB
{
    public class FactorAmbientalRepository:IFactorAmbientalReadOnlyRepository
    {
        private readonly Context _context;

        public FactorAmbientalRepository(Context context)
        {
            _context = context;
        }

        public Task EditarFactorAmbiental(FactorAmbientalCMCB factorAmbientalCMCB)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FactorAmbientalCMCB>> ListarFactoresAmbientales()
        {
            var lista = await _context.FactorAmbientalCMCB.ToListAsync();
            var output= new List<FactorAmbientalCMCB>();
            foreach (var factor in lista)
            {
                output.Add(new Domain.CompromisosMCB.FactorAmbientalCMCB
                {
                    Id= factor.Id,
                    Factor= factor.Factor,
                    Descripcion= factor.Descripcion,
                });
            }
            return output;
        }

        public Task<int> RegistrarFactorAmbiental(FactorAmbientalCMCB factorAmbientalCMCB)
        {
            throw new NotImplementedException();
        }
    }
}
