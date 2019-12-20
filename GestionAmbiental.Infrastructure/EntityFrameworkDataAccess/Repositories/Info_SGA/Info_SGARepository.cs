using GestionAmbiental.Application.Repositories.Info_SGA;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Info_SGA
{
    public class Info_SGARepository : IInfo_SGAReadOnlyRepository
    {
        private Context _context;

        public Info_SGARepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Domain.ValueObjects.Info_SGA> GetInfo_SGA(string tipo)
        {
            var info = await _context.Info_SGAs.FirstOrDefaultAsync(x => x.Tipo.Equals(tipo));
            var output = new Domain.ValueObjects.Info_SGA();
            output.Id_Info = info.Id_Info;
            output.Info = info.Info;
            output.Tipo = info.Tipo;
            return output;
        }
    }
}
