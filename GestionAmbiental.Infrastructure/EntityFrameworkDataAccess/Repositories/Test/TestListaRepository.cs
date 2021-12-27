using GestionAmbiental.Application.Repositories.Test;
using GestionAmbiental.Domain.Test;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Test
{
    public class TestListaRepository : ITestListaReadOnlyRepository
    {
        private readonly Context _context;
        public TestListaRepository (Context context)
        {
            _context = context;
        }
        public async Task<List<TestLista>> ListarTestLista()
        {
            //Consultar registros
            var filtrados = await _context.Test_Listas.ToListAsync();

            //Nueva lista en blanco
            var output = new List<Domain.Test.TestLista>();

            //Mapear registros en nueva lista
            foreach (var item in filtrados)
            {
                //Agregar elementos a nueva lista en blanco
                output.Add(new Domain.Test.TestLista
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Estado = item.Estado
                });
            }
            //retornar nueva lista
            return output;
        }
        public async Task<TestLista> BuscarPorID(int Id)
        {
            var entidad = await _context.Test_Listas.FindAsync(Id);
            //Nuevo elemento en blanco
            var output = new Domain.Test.TestLista();

            if (entidad != null)
            {
                //Mapear registros en nuevo elemento
                output.Id = entidad.Id;
                output.Nombre = entidad.Nombre;
                output.Estado = entidad.Estado;
            }

            return output;
        }
        public async Task<int> RegistrarTestLista(TestLista input)
        {
            var entity = new Entities.Test.Test_Lista
            {
                Nombre = input.Nombre,
                Estado = input.Estado
            };

            await _context.Test_Listas.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }
        public async Task EditarTestLista(TestLista input)
        {
            //buscar el registro
            var entity = await _context.Test_Listas.FindAsync(input.Id);
            if (entity != null)
            {
                entity.Nombre = input.Nombre;
                entity.Estado = input.Estado;
               

                _context.Test_Listas.Update(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task EliminarTestLista(int IdEliminado)
        {
            var entity = await _context.Test_Listas.FindAsync(IdEliminado);
            if (entity != null)
            {
                entity.Estado = "INA";

                _context.Test_Listas.Update(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
