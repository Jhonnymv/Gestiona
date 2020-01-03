using GestionAmbiental.Application.Repositories.Info_SGA;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<List<Domain.ValueObjects.Info_SGA>> GetInfo_SGAs(string filtro)
        {
            var info = await _context.Info_SGAs.Where(x => x.Tipo.Contains(filtro == null ? x.Tipo : filtro)).OrderBy(x => x.Tipo).ToListAsync();
            var output = new List<Domain.ValueObjects.Info_SGA>();
            foreach (var item in info)
                output.Add(new Domain.ValueObjects.Info_SGA(item.Id_Info, item.Tipo, item.Info));
            return output;
        }
        public async Task UpdateInfo_SGA(Domain.ValueObjects.Info_SGA info)
        {
            var info_SGAEntity = await _context.Info_SGAs.FirstOrDefaultAsync(x => x.Id_Info.Equals(info.Id_Info));
            info_SGAEntity.Info = info.Info;
            _context.Info_SGAs.Update(info_SGAEntity);
            await _context.SaveChangesAsync();
        }   
    }
}
