using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.Zona;
using GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.AvistamientoFlora
{
    public class ZonaRepository:IZonaReadOnlyRepository
    {
        private readonly Context _context;

        public ZonaRepository(Context context)
        {
            _context = context;
        }

        public async Task EditarZona(Zona input)
        {
            var entity=await _context.ZonaMCIF.FirstOrDefaultAsync(x=>x.Id.Equals(input.Id));
            if (entity != null)
            {
                entity.Nombre=input.Nombre;
                entity.Descripcion=input.Descripcion;
            }
            _context.ZonaMCIF.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Zona>> ListZonas()
        {
            var zonas = await _context.ZonaMCIF.ToListAsync();
            var ouput = new List<Zona>();
            foreach (var item in zonas)
            {
                ouput.Add(new Domain.MonitoreoCampoInterno.AvistamientoFlora.Zona
                {
                    Id = item.Id,
                    Nombre= item.Nombre,
                    Descripcion= item.Descripcion
                });
            }
            return ouput;
        }

        public async Task<int> RegistrarZona(Zona input)
        {
            var entity = new Entities.MonitoreoCampoInterno.AvistamientoFlora.Zona
            {
                Nombre = input.Nombre,
                Descripcion = input.Descripcion
            };
            await _context.ZonaMCIF.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
            
        }

        public Task<int> RegistrarZonaMCIF(Zona zona)
        {
            throw new NotImplementedException();
        }
    }
}
