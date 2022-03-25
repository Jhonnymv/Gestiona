using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.MedioVerificacionCMCB;
using GestionAmbiental.Domain.CompromisosMCB;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.CompromisosMCB
{
    public class MediioVerificacionRepository : IMedioVerificacionReadOnlyRepository
    {
        private readonly Context _contex;

        public MediioVerificacionRepository(Context context)
        {
            _contex = context;
        }

        public async Task EditarMedioVerificacion(MedioVerificacionCMCB input)
        {
            var entity = await _contex.MedioVerificacionCMCB.FirstOrDefaultAsync(x => x.Id.Equals(input.Id));
            if (entity != null)
            {
                entity.Medio = input.Medio;
            }
            _contex.MedioVerificacionCMCB.Update(entity);
            await _contex.SaveChangesAsync();
        }

        public async Task<List<MedioVerificacionCMCB>> ListarMedioVerificacion()
        {
            var datos = await _contex.MedioVerificacionCMCB.ToListAsync();
            var ouput = new List<MedioVerificacionCMCB>();
            foreach (var item in datos)
            {
                ouput.Add(new MedioVerificacionCMCB
                {
                    Id = item.Id,
                    Medio = item.Medio,
                });
            }
            return ouput;
        }

        public async Task<int> RegistrarMedioVerificacion(MedioVerificacionCMCB input)
        {
            var entity = new Entities.CompromisosMCB.MedioVerificacionCMCB
            {
                Medio =input.Medio,
            };

            await _contex.MedioVerificacionCMCB.AddAsync(entity);
            await _contex.SaveChangesAsync();
            return entity.Id;
        }
    }
}
