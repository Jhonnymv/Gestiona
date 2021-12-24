using GestionAmbiental.Application.Repositories.Test;
using GestionAmbiental.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Test
{
    public class TestRepository : ITestReadOnlyRepository
    {
        private readonly Context _context;

        public TestRepository(Context context)
        {
            _context = context;
        }
        //return ID nuevo registro
        public async Task<int> RegistrarTest(Domain.Test.Test input)
        {
            var entity = new Entities.Test.Test
            {
                Nombre = input.Nombre,
                Numero = input.Numero,
                Evidencia = input.Evidencia
            };

            await _context.Tests.AddAsync(entity);
            await _context.SaveChangesAsync();

            return 0;
        }
        //Sin return
        public async Task EditarTest(Domain.Test.Test input)
        {
            //buscar el registro
            var entity = await _context.Tests.FirstOrDefaultAsync(x=>x.Id.Equals(input.Id));
            if (entity != null)
            {
                entity.Nombre = input.Nombre;
                entity.Numero = input.Numero;
                entity.Evidencia = input.Evidencia;

                _context.Tests.Update(entity);
                await _context.SaveChangesAsync();
            }            
        }
        //Return lista de registros filtrados
        public async Task<List<Domain.Test.Test>> ListarTest()
        {
            //Consultar registros
            var filtrados = await _context.Tests.ToListAsync();

            //Nueva lista en blanco
            var output = new List<Domain.Test.Test>();

            //Mapear registros en nueva lista
            foreach (var item in filtrados)
            {
                output.Add(new Domain.Test.Test
                {
                   Id = item.Id,
                   Nombre = item.Nombre,
                   Evidencia = item.Evidencia,
                   Numero = item.Numero
                });
            }
            //retornar nueva lista
            return output;
        }
        //Sin return
        public async Task EliminarTest(int IdElimnado)
        {
            //Cambio de estado a Elimando
            //buscar el registro
            var entity = await _context.Tests.FirstOrDefaultAsync(x => x.Id.Equals(IdElimnado));
            if (entity != null)
            {
                entity.Nombre = EstadosTest.Eliminado.ToString();

                _context.Tests.Update(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
