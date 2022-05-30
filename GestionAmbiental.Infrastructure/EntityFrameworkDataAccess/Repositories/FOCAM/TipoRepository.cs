using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.FOCAM.Tipo;
using GestionAmbiental.Domain.FOCAM;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.FOCAM
{
    public class TipoRepository:ITipoReadOnlyRepository
    {
        private readonly Context _context;

        public TipoRepository(Context context)
        {
            _context = context;
        }

        public Task<Tipo> BuscarPorID(int Id)
        {
            throw new NotImplementedException();
        }

        public  Task EditarTipo(Tipo Tipo)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Tipo>> ListTipos()
        {
            var tipos = await _context.TipoFOCAM.ToListAsync();
            var ouput=new List<Tipo>();
            foreach (var tipo in tipos)
            {
                ouput.Add(new Domain.FOCAM.Tipo
                {
                    Id = tipo.Id,
                    Nombre = tipo.Nombre,
                    Descripcion = tipo.Descripcion,
                });
            }
            return ouput;
        }

        public async Task<int> RegistrarTipo(Tipo Tipo)
        {
            var Entity = new Entities.FOCAM.Tipo();
            Entity.Nombre = Tipo.Nombre;
            Entity.Descripcion = Tipo.Descripcion;

            _context.TipoFOCAM.Add(Entity);
            await _context.SaveChangesAsync();

            return Entity.Id;
        }
    }
}
