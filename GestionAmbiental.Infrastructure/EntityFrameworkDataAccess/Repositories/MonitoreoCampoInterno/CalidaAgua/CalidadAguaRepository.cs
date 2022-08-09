using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.CalidadAgua;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.CalidaAgua
{
    public class CalidadAguaRepository : ICalidadAguaReadOnlyRepository
    {
        private readonly Context _context;

        public CalidadAguaRepository(Context context)
        {
            _context = context;
        }
        public async Task<Domain.MonitoreoCampoInterno.CalidadAgua.CalidadAgua> BuscarPorID(int Id)
        {
            var data = await _context.CalidadAgua.FindAsync(Id);
            var dataUser = await _context.Usuarios.FindAsync(data.Id_Usuario);
            var ouput = new Domain.MonitoreoCampoInterno.CalidadAgua.CalidadAgua
            {
                Id = Id,
                Fecha = data.Fecha,
                Hora = data.Hora,
                Cod_Estacion = data.Cod_Estacion,
                Descripcion= data.Descripcion,
                Ubicacion= data.Ubicacion,
                Responsable=data.Id_Usuario!=null?new Domain.ValueObjects.Usuario
                {
                    IdUsuario=(int)data.Id_Usuario, Nombres=dataUser.Nombres,Apellidos=dataUser.Apellidos
                }:null,
                TemperaturaResult=data.TemperaturaResult,
                TemperaturaEquipo=data.TemperaturaEquipo,
                PhResult=data.PhResult,
                PhEquipo=data.PhEquipo,
                ConductividadResult=data.ConductividadResult,
                ConductividadEquipo=data.ConductividadEquipo,
                OxigenoResult=data.OxigenoResult,
                CloroEquipo=data.OxigenoEquipo,
                TurbiedadResult=data.TurbiedadResult,
                TurbiedadEquipo=data.TurbiedadEquipo,
                CloroResult=data.CloroResult,
                OxigenoEquipo=data.CloroEquipo
            };
            return ouput;
            
        }

        public Task EditarCalidadAgua(Domain.MonitoreoCampoInterno.CalidadAgua.CalidadAgua calidadAgua)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Domain.MonitoreoCampoInterno.CalidadAgua.CalidadAgua>> ListCalidadAgua()
        {
            var listRegistros = await _context.CalidadAgua.ToListAsync();

            var output = new List<Domain.MonitoreoCampoInterno.CalidadAgua.CalidadAgua>();

            foreach (var item in listRegistros)
            {
                var dataUser=await _context.Usuarios.FindAsync(item.Id_Usuario);
                output.Add(new Domain.MonitoreoCampoInterno.CalidadAgua.CalidadAgua
                {
                    Id=item.Id,
                    Fecha=item.Fecha,
                    Hora=item.Hora,
                    Cod_Estacion=item.Cod_Estacion,
                    Descripcion=item.Descripcion,
                    Ubicacion=item.Ubicacion,
                    Responsable = new Domain.ValueObjects.Usuario { IdUsuario = (int)item.Id_Usuario, Nombres = dataUser.Nombres, DNI = dataUser.DNI },
                    TemperaturaResult = item.TemperaturaResult,
                    TemperaturaEquipo = item.TemperaturaEquipo,
                    PhResult = item.PhResult,
                    PhEquipo = item.PhEquipo,
                    ConductividadResult = item.ConductividadResult,
                    ConductividadEquipo = item.ConductividadEquipo,
                    OxigenoResult = item.OxigenoResult,
                    CloroEquipo = item.OxigenoEquipo,
                    TurbiedadResult = item.TurbiedadResult,
                    TurbiedadEquipo = item.TurbiedadEquipo,
                    CloroResult = item.CloroResult,
                    OxigenoEquipo = item.CloroEquipo
                });
            }
            return output;
        }

        public async Task<int> RegistrarCalidaAgua(Domain.MonitoreoCampoInterno.CalidadAgua.CalidadAgua calidadAgua)
        {
            var Entity=new Entities.MonitoreoCampoInterno.Calidad_Agua.CalidadAgua();
            Entity.Fecha = calidadAgua.Fecha;
            Entity.Hora = calidadAgua.Hora;
            Entity.Cod_Estacion = calidadAgua.Cod_Estacion;
            Entity.Descripcion=calidadAgua.Descripcion;
            Entity.Ubicacion = calidadAgua.Ubicacion;
            Entity.Id_Usuario = calidadAgua.Responsable.IdUsuario;
            Entity.TemperaturaResult = calidadAgua.TemperaturaResult;
            Entity.TemperaturaEquipo = calidadAgua.TemperaturaEquipo;
            Entity.PhResult=calidadAgua.PhResult;
            Entity.PhEquipo = calidadAgua.PhEquipo;
            Entity.ConductividadResult = calidadAgua.ConductividadResult;
            Entity.ConductividadEquipo = calidadAgua.ConductividadEquipo;
            Entity.OxigenoResult = calidadAgua.OxigenoResult;
            Entity.OxigenoEquipo = calidadAgua.OxigenoEquipo;
            Entity.TurbiedadResult=calidadAgua.TurbiedadResult;
            Entity.TurbiedadEquipo = calidadAgua.TurbiedadEquipo;
            Entity.CloroResult = calidadAgua.CloroResult;
            Entity.CloroEquipo = calidadAgua.CloroEquipo;

            _context.CalidadAgua.Add(Entity);
            await _context.SaveChangesAsync();

            return Entity.Id;
        }
    }
}
