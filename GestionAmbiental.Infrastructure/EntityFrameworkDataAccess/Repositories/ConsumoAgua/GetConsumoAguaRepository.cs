using GestionAmbiental.Application.Repositories.ConsumoAgua;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.ConsumoAgua
{
    public class GetConsumoAguaRepository : IGetConsumoAguaReadOnlyRepository
    {
        private readonly Context _context;

        public GetConsumoAguaRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Domain.ConsumoAgua.ConsumoAgua> GetConsumoAgua(int Id)
        {
            var data = await _context.Consumos_Agua.FindAsync(Id);
            var output = new Domain.ConsumoAgua.ConsumoAgua();
            if (data != null)
            {
                var resolucion = await _context.Resoluciones_Consumo_Agua.FindAsync(data.Id_Resolucion_Consumo_Agua);
                var fuente = await _context.Fuentes_Agua.FindAsync(resolucion.Id_Fuente_Agua);
                var origen = await _context.Origenes_Agua.FindAsync(fuente.Id_Origen_Agua);

                output.IdConsumoAgua = data.Id_Consumo_Agua;
                output.ResolucionConsumoAgua = new Domain.ConsumoAgua.ResolucionConsumoAgua
                {
                    IdResolucionConsumoAgua = resolucion.Id_Resolucion_Consumo_Agua,
                    Anio = resolucion.Anio,
                    TipoUso = resolucion.Tipo_Uso,
                    ClaseDerecho = resolucion.Clase_Derecho,
                    Resolucion = resolucion.Resolucion,
                    FuenteAgua = new Domain.ConsumoAgua.FuenteAgua
                    {
                        IdFuenteAgua = fuente.Id_Fuente_Agua,
                        Descripcion = fuente.Descripcion,
                        Coordenadas_UTM = fuente.Coordenadas_UTM,
                        OrigenAgua = new Domain.ConsumoAgua.OrigenAgua
                        {
                            IdOrigenAgua = origen.Id_Origen_Agua,
                            Descripcion = origen.Descripcion,
                            Estado = origen.Estado
                        },
                        Estado = fuente.Estado
                    },
                    VolumenOtorgado = resolucion.Volumen_Otorgado,
                    Actividad = resolucion.Actividad,
                    Estado = resolucion.Estado
                };
                output.Fecha = data.Fecha;
                output.TiempoBombeo = data.Tiempo_Bombeo;
                output.LecturaInicial = data.Lectura_Inicial;
                output.LecturaFinal = data.Lectura_Final;
                output.VolumenBombeado = data.Volumen_Bombeado;
                output.CaudalPromedio = data.Caudal_Promedio;
                output.IdUsuario = data.Id_Usuario;
                output.FechaRegistro = data.Fecha_Registro;
                output.Estado = data.Estado;
            }
            return output;
        }

        public async Task<List<Domain.ConsumoAgua.ConsumoAgua>> GetConsumosAgua(DateTime? fecha)
        {
            var data = await _context.Consumos_Agua.ToListAsync();

            if (fecha.HasValue)
                data = data.Where(x => x.Fecha.Date.Equals(fecha.Value.Date)).ToList();

            var output = new List<Domain.ConsumoAgua.ConsumoAgua>();

            var resoluciones = await _context.Resoluciones_Consumo_Agua.ToListAsync();
            var fuentes = await _context.Fuentes_Agua.ToListAsync();
            var origenes = await _context.Origenes_Agua.ToListAsync();
            foreach (var item in data)
            {
                var resolucion = resoluciones.FirstOrDefault(x => x.Id_Resolucion_Consumo_Agua.Equals(item.Id_Resolucion_Consumo_Agua));
                var fuente = fuentes.FirstOrDefault(x => x.Id_Fuente_Agua.Equals(resolucion.Id_Fuente_Agua));
                var origen = origenes.FirstOrDefault(x => x.Id_Origen_Agua.Equals(fuente.Id_Origen_Agua));

                var outputItem = new Domain.ConsumoAgua.ConsumoAgua
                {
                    IdConsumoAgua = item.Id_Consumo_Agua,
                    ResolucionConsumoAgua = new Domain.ConsumoAgua.ResolucionConsumoAgua
                    {
                        IdResolucionConsumoAgua = resolucion.Id_Resolucion_Consumo_Agua,
                        Anio = resolucion.Anio,
                        TipoUso = resolucion.Tipo_Uso,
                        ClaseDerecho = resolucion.Clase_Derecho,
                        Resolucion = resolucion.Resolucion,
                        FuenteAgua = new Domain.ConsumoAgua.FuenteAgua
                        {
                            IdFuenteAgua = fuente.Id_Fuente_Agua,
                            Descripcion = fuente.Descripcion,
                            Coordenadas_UTM = fuente.Coordenadas_UTM,
                            OrigenAgua = new Domain.ConsumoAgua.OrigenAgua
                            {
                                IdOrigenAgua = origen.Id_Origen_Agua,
                                Descripcion = origen.Descripcion,
                                Estado = origen.Estado
                            },
                            Estado = fuente.Estado
                        },
                        VolumenOtorgado = resolucion.Volumen_Otorgado,
                        Actividad = resolucion.Actividad,
                        Estado = resolucion.Estado
                    },
                    Fecha = item.Fecha,
                    TiempoBombeo = item.Tiempo_Bombeo,
                    LecturaInicial = item.Lectura_Inicial,
                    LecturaFinal = item.Lectura_Final,
                    VolumenBombeado = item.Volumen_Bombeado,
                    CaudalPromedio = item.Caudal_Promedio,
                    IdUsuario = item.Id_Usuario,
                    FechaRegistro = item.Fecha_Registro,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<Domain.ConsumoAgua.ConsumoAgua>> GetConsumosAguaDescarga(DateTime fechaInicio, DateTime fechaFina, int idResolucion)
        {
            var data = await _context.Consumos_Agua.ToListAsync();

            data = data.Where(x =>
            x.Id_Resolucion_Consumo_Agua.Equals(idResolucion) &&
            x.Fecha.Date >= fechaInicio.Date &&
            x.Fecha.Date <= fechaFina.Date)
                .ToList();

            var output = new List<Domain.ConsumoAgua.ConsumoAgua>();

            var resolucion = await _context.Resoluciones_Consumo_Agua.FindAsync(idResolucion);
            var fuente = await _context.Fuentes_Agua.FirstOrDefaultAsync(x => x.Id_Fuente_Agua.Equals(resolucion.Id_Fuente_Agua));
            var origen = await _context.Origenes_Agua.FirstOrDefaultAsync(x => x.Id_Origen_Agua.Equals(fuente.Id_Origen_Agua));

            foreach (var item in data)
            {
                var outputItem = new Domain.ConsumoAgua.ConsumoAgua
                {
                    IdConsumoAgua = item.Id_Consumo_Agua,
                    ResolucionConsumoAgua = new Domain.ConsumoAgua.ResolucionConsumoAgua
                    {
                        IdResolucionConsumoAgua = resolucion.Id_Resolucion_Consumo_Agua,
                        Anio = resolucion.Anio,
                        TipoUso = resolucion.Tipo_Uso,
                        ClaseDerecho = resolucion.Clase_Derecho,
                        Resolucion = resolucion.Resolucion,
                        FuenteAgua = new Domain.ConsumoAgua.FuenteAgua
                        {
                            IdFuenteAgua = fuente.Id_Fuente_Agua,
                            Descripcion = fuente.Descripcion,
                            Coordenadas_UTM = fuente.Coordenadas_UTM,
                            OrigenAgua = new Domain.ConsumoAgua.OrigenAgua
                            {
                                IdOrigenAgua = origen.Id_Origen_Agua,
                                Descripcion = origen.Descripcion,
                                Estado = origen.Estado
                            },
                            Estado = fuente.Estado
                        },
                        VolumenOtorgado = resolucion.Volumen_Otorgado,
                        Actividad = resolucion.Actividad,
                        Estado = resolucion.Estado
                    },
                    Fecha = item.Fecha,
                    TiempoBombeo = item.Tiempo_Bombeo,
                    LecturaInicial = item.Lectura_Inicial,
                    LecturaFinal = item.Lectura_Final,
                    VolumenBombeado = item.Volumen_Bombeado,
                    CaudalPromedio = item.Caudal_Promedio,
                    IdUsuario = item.Id_Usuario,
                    FechaRegistro = item.Fecha_Registro,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<Domain.ConsumoAgua.ConsumoAgua>> GetConsumosAguaDescarga(int idResolucion, int anio)
        {
            var data = await _context.Consumos_Agua.ToListAsync();

            data = data.Where(x =>
            x.Id_Resolucion_Consumo_Agua.Equals(idResolucion)&&
            x.Fecha.Year.Equals(anio))
            
                .ToList();

            var output = new List<Domain.ConsumoAgua.ConsumoAgua>();

            var resolucion = await _context.Resoluciones_Consumo_Agua.FindAsync(idResolucion);
            var fuente = await _context.Fuentes_Agua.FirstOrDefaultAsync(x => x.Id_Fuente_Agua.Equals(resolucion.Id_Fuente_Agua));
            var origen = await _context.Origenes_Agua.FirstOrDefaultAsync(x => x.Id_Origen_Agua.Equals(fuente.Id_Origen_Agua));

            foreach (var item in data)
            {
                var outputItem = new Domain.ConsumoAgua.ConsumoAgua
                {
                    IdConsumoAgua = item.Id_Consumo_Agua,
                    ResolucionConsumoAgua = new Domain.ConsumoAgua.ResolucionConsumoAgua
                    {
                        IdResolucionConsumoAgua = resolucion.Id_Resolucion_Consumo_Agua,
                        Anio = resolucion.Anio,
                        TipoUso = resolucion.Tipo_Uso,
                        ClaseDerecho = resolucion.Clase_Derecho,
                        Resolucion = resolucion.Resolucion,
                        FuenteAgua = new Domain.ConsumoAgua.FuenteAgua
                        {
                            IdFuenteAgua = fuente.Id_Fuente_Agua,
                            Descripcion = fuente.Descripcion,
                            Coordenadas_UTM = fuente.Coordenadas_UTM,
                            OrigenAgua = new Domain.ConsumoAgua.OrigenAgua
                            {
                                IdOrigenAgua = origen.Id_Origen_Agua,
                                Descripcion = origen.Descripcion,
                                Estado = origen.Estado
                            },
                            Estado = fuente.Estado
                        },
                        VolumenOtorgado = resolucion.Volumen_Otorgado,
                        Actividad = resolucion.Actividad,
                        Estado = resolucion.Estado
                    },
                    Fecha = item.Fecha,
                    TiempoBombeo = item.Tiempo_Bombeo,
                    LecturaInicial = item.Lectura_Inicial,
                    LecturaFinal = item.Lectura_Final,
                    VolumenBombeado = item.Volumen_Bombeado,
                    CaudalPromedio = item.Caudal_Promedio,
                    IdUsuario = item.Id_Usuario,
                    FechaRegistro = item.Fecha_Registro,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<Domain.ConsumoAgua.ConsumoAgua> GetRecienteConsumoAgua(int idResolucion)
        {
            var data = await _context.Consumos_Agua.Where(x =>
            x.Id_Resolucion_Consumo_Agua.Equals(idResolucion))
                .OrderByDescending(x => x.Fecha_Registro)
                .ToListAsync();

            var ultimoRegistro = data
                .GroupBy(x => x.Fecha)
                .OrderByDescending(x => x.Key)
                .FirstOrDefault();

            if (ultimoRegistro != null)
            {
                var output = new Domain.ConsumoAgua.ConsumoAgua();
                var resolucion = await _context.Resoluciones_Consumo_Agua.FindAsync(idResolucion);
                var fuente = await _context.Fuentes_Agua.FindAsync(resolucion.Id_Fuente_Agua);
                var origen = await _context.Origenes_Agua.FindAsync(fuente.Id_Origen_Agua);

                output.ResolucionConsumoAgua = new Domain.ConsumoAgua.ResolucionConsumoAgua
                {
                    IdResolucionConsumoAgua = resolucion.Id_Resolucion_Consumo_Agua,
                    Anio = resolucion.Anio,
                    TipoUso = resolucion.Tipo_Uso,
                    ClaseDerecho = resolucion.Clase_Derecho,
                    Resolucion = resolucion.Resolucion,
                    FuenteAgua = new Domain.ConsumoAgua.FuenteAgua
                    {
                        IdFuenteAgua = fuente.Id_Fuente_Agua,
                        Descripcion = fuente.Descripcion,
                        Coordenadas_UTM = fuente.Coordenadas_UTM,
                        OrigenAgua = new Domain.ConsumoAgua.OrigenAgua
                        {
                            IdOrigenAgua = origen.Id_Origen_Agua,
                            Descripcion = origen.Descripcion,
                            Estado = origen.Estado
                        },
                        Estado = fuente.Estado
                    },
                    VolumenOtorgado = resolucion.Volumen_Otorgado,
                    Actividad = resolucion.Actividad,
                    Estado = resolucion.Estado
                };
                output.Fecha = ultimoRegistro.First().Fecha;
                output.TiempoBombeo = ultimoRegistro.Sum(x => x.Tiempo_Bombeo);
                output.LecturaInicial = ultimoRegistro.Last().Lectura_Inicial;
                output.LecturaFinal = ultimoRegistro.First().Lectura_Final;
                output.VolumenBombeado = ultimoRegistro.Sum(x => x.Volumen_Bombeado);
                output.CaudalPromedio = Math.Round(ultimoRegistro.Sum(x => x.Caudal_Promedio) / ultimoRegistro.Count(), 2);
                output.IdUsuario = ultimoRegistro.First().Id_Usuario;
                output.FechaRegistro = ultimoRegistro.First().Fecha_Registro;
                output.Estado = ultimoRegistro.First().Estado;
                return output;
            }
            else
                return null;
        }
    }
}
