using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora;
using GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.AvistamientoFlora
{
    public class AvistamientoFloraRepository : IAvistamientoFloraReadOnlyRepository
    {
        private readonly Context _context;

        public AvistamientoFloraRepository(Context context)
        {
            _context = context;
        }

        public async Task<Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora> BuscarPorID(int Id)
        {
            var data = await _context.AvistamientoFloraMCIF.FindAsync(Id);

            var dataZona = await _context.ZonaMCIF.FindAsync(data.Id_Zona);
            var ouput = new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora
            {
                Id = Id,
                IdFicha = data.IdFicha,
                Zona = data.Id_Zona != null ? new Domain.MonitoreoCampoInterno.AvistamientoFlora.Zona { Id = (int)data.Id_Zona, Nombre = dataZona.Nombre, Descripcion = dataZona.Descripcion } : null,
                Ubicacion_Este = data.Ubicacion_Este,
                Ubicacion_Norte = data.Ubicacion_Norte,
                Ubicacion_Altitud = data.Ubicacion_Altitud,
                Geneto = data.Geneto,
                Rameto = data.Rameto,
                Condicion_Reubicacion = data.Condicion_Reubicacion,
                Sustrato = data.Sustrato,
                Cobertura_Diametro_Mayor = data.Cobertura_Diametro_Mayor,
                Cobertura_Diametro_Menor = data.Cobertura_Diametro_Menor,
                Cobertura_Diametro_Altura = data.Cobertura_Diametro_Altura,
                Fotos = data.Fotos
            };
            return ouput;
        }

        public Task EditarAvistamientoFlora(Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora avistamientoFlora)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora>> ListAvistamientoFlora()
        {
            var avistamientos = await _context.AvistamientoFloraMCIF.ToListAsync();

            var ouput = new List<Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora>();
            foreach (var item in avistamientos)
            {
                var dataZona = await _context.ZonaMCIF.FindAsync(item.Id_Zona);

                ouput.Add(new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora
                {
                    Id = item.Id,
                    IdFicha = item.IdFicha,
                    Fechavalidacion = item.FechaValidacion,
                    Zona = item.Id_Zona != null ? new Domain.MonitoreoCampoInterno.AvistamientoFlora.Zona { Id = (int)item.Id_Zona, Nombre = dataZona.Nombre, Descripcion = dataZona.Descripcion } : null,
                    Ubicacion_Este = item.Ubicacion_Este,
                    Ubicacion_Norte = item.Ubicacion_Norte,
                    Ubicacion_Altitud = item.Ubicacion_Altitud,
                    Geneto = item.Geneto,
                    Rameto = item.Rameto,
                    Condicion_Reubicacion = item.Condicion_Reubicacion,
                    Sustrato = item.Sustrato,
                    Cobertura_Diametro_Mayor = item.Cobertura_Diametro_Mayor,
                    Cobertura_Diametro_Menor = item.Cobertura_Diametro_Menor,
                    Cobertura_Diametro_Altura = item.Cobertura_Diametro_Altura,
                    Fotos = item.Fotos
                });
            }
            return ouput;
        }

        public async Task<int> RegistrarAvistamientoFlora(Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora avistamientoFlora)
        {
            var Entity = new Entities.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora();

            Entity.IdFicha = avistamientoFlora.IdFicha;
            Entity.FechaValidacion = avistamientoFlora.Fechavalidacion;
            Entity.Id_Zona = avistamientoFlora.Zona.Id;
            Entity.Ubicacion_Este = avistamientoFlora.Ubicacion_Este;
            Entity.Ubicacion_Norte = avistamientoFlora.Ubicacion_Norte;
            Entity.Ubicacion_Altitud = avistamientoFlora.Ubicacion_Altitud;
            Entity.Geneto = avistamientoFlora.Geneto;
            Entity.Rameto = avistamientoFlora.Rameto;
            Entity.Condicion_Reubicacion = avistamientoFlora.Condicion_Reubicacion;
            Entity.Sustrato = avistamientoFlora.Sustrato;
            Entity.Cobertura_Diametro_Mayor = avistamientoFlora.Cobertura_Diametro_Mayor;
            Entity.Cobertura_Diametro_Menor = avistamientoFlora.Cobertura_Diametro_Menor;
            Entity.Cobertura_Diametro_Altura = avistamientoFlora.Cobertura_Diametro_Altura;

            _context.AvistamientoFloraMCIF.Add(Entity);
            await _context.SaveChangesAsync();

            return Entity.Id;
        }
    }
}
