using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Incidentes.InvestigacionFinal
{
    public class GetItemsInvestigacionFinalRepository : IGetItemsInvestigacionFinalReadOnlyRepository
    {
        private readonly Context _context;

        public GetItemsInvestigacionFinalRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Producto_Derramado_Incidente>> GetProductosDerramados(int IdInvestigacion)
        {
            var productosDerramados = await _context.Producto_Derramado_Incidente.Where(x => x.Id_Investigacion_Final.Equals(IdInvestigacion) && x.Estado.Equals(EstadosIncidentes.Registrado.ToString())).ToListAsync();
            
            var productos = await _context.Producto_Incidente.ToListAsync();
            var tiposProducto = await _context.Tipo_Producto_Incidente.ToListAsync();
            var unidadesMedida = await _context.Unidad_Medida.ToListAsync();
            var estadosEquipo = await _context.Estado_Equipo_Incidente.ToListAsync();

            var output = new List<Producto_Derramado_Incidente>();

            foreach(var item in productosDerramados)
            {
                var outputItem = new Producto_Derramado_Incidente();
                var producto = productos.FirstOrDefault(x => x.Id_Producto_Incidente.Equals(item.Id_Producto_Incidente));
                var tipoProducto = tiposProducto.FirstOrDefault(x => x.Id_Tipo_Producto_Incidente.Equals(item.Id_Tipo_Producto));
                var unidadMedida = unidadesMedida.FirstOrDefault(x => x.Id_Unidad_Medida.Equals(item.Id_Unidad));
                var estadoEquipo = estadosEquipo.FirstOrDefault(x => x.Id_Estado_Equipo_Incidente.Equals(item.Id_Estado_Equipo_Responsable));

                outputItem.Id_Producto_Derramado_Incidente = item.Id_Producto_Derramado_Incidente;
                outputItem.Id_Investigacion_Final = item.Id_Investigacion_Final;

                outputItem.Producto_Incidente = new Domain.Incidentes.Items.Producto_Incidente();
                outputItem.Producto_Incidente.Id_Producto_Incidente = producto?.Id_Producto_Incidente;
                outputItem.Producto_Incidente.Descripcion = producto?.Descripcion;
                outputItem.Producto_Incidente.Estado = producto?.Estado;

                outputItem.Tipo_Producto = new Domain.Incidentes.Items.Tipo_Producto_Incidente();
                outputItem.Tipo_Producto.Id_Tipo_Producto_Incidente = tipoProducto?.Id_Tipo_Producto_Incidente;
                outputItem.Tipo_Producto.Descripcion = tipoProducto?.Descripcion;
                outputItem.Tipo_Producto.Estado = tipoProducto?.Estado;

                outputItem.Estado_Producto = item.Estado_Producto;
                outputItem.Cantidad_Derramada = item.Cantidad_Derramada;

                outputItem.Unidad = new Domain.Incidentes.Items.Unidad_Medida();
                outputItem.Unidad.Id_Unidad_Medida = unidadMedida?.Id_Unidad_Medida;
                outputItem.Unidad.Tipo = unidadMedida?.Tipo;
                outputItem.Unidad.Descripcion = unidadMedida?.Descripcion;
                outputItem.Unidad.Estado = unidadMedida?.Estado;

                outputItem.Equipo_Responsable = item.Equipo_Responsable;

                outputItem.Estado_Equipo_Responsable = new Domain.Incidentes.Items.Estado_Equipo_Incidente();
                outputItem.Estado_Equipo_Responsable.Id_Estado_Equipo_Incidente = estadoEquipo?.Id_Estado_Equipo_Incidente;
                outputItem.Estado_Equipo_Responsable.Descripcion = estadoEquipo?.Descripcion;
                outputItem.Estado_Equipo_Responsable.Estado = estadoEquipo?.Estado;

                outputItem.Cantidad_Recuperado = item.Cantidad_Recuperado;
                outputItem.Cantidad_No_Recuperado = item.Cantidad_No_Recuperado;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }

        public async Task<Producto_Derramado_Incidente> GetProductoDerramado(int IdProducto)
        {
            var productoDerramado = await _context.Producto_Derramado_Incidente.FirstOrDefaultAsync(x => x.Id_Producto_Derramado_Incidente.Equals(IdProducto));
            
            var output = new Producto_Derramado_Incidente();

            var producto = await _context.Producto_Incidente.FirstOrDefaultAsync(x => x.Id_Producto_Incidente.Equals(productoDerramado.Id_Producto_Incidente));
            var tipoProducto = await _context.Tipo_Producto_Incidente.FirstOrDefaultAsync(x => x.Id_Tipo_Producto_Incidente.Equals(productoDerramado.Id_Tipo_Producto));
            var unidadMedida = await _context.Unidad_Medida.FirstOrDefaultAsync(x => x.Id_Unidad_Medida.Equals(productoDerramado.Id_Unidad));
            var estadoEquipo = await _context.Estado_Equipo_Incidente.FirstOrDefaultAsync(x => x.Id_Estado_Equipo_Incidente.Equals(productoDerramado.Id_Estado_Equipo_Responsable));

            output.Id_Producto_Derramado_Incidente = productoDerramado.Id_Producto_Derramado_Incidente;
            output.Id_Investigacion_Final = productoDerramado.Id_Investigacion_Final;

            output.Producto_Incidente = new Domain.Incidentes.Items.Producto_Incidente();
            output.Producto_Incidente.Id_Producto_Incidente = producto?.Id_Producto_Incidente;
            output.Producto_Incidente.Descripcion = producto?.Descripcion;
            output.Producto_Incidente.Estado = producto?.Estado;

            output.Tipo_Producto = new Domain.Incidentes.Items.Tipo_Producto_Incidente();
            output.Tipo_Producto.Id_Tipo_Producto_Incidente = tipoProducto?.Id_Tipo_Producto_Incidente;
            output.Tipo_Producto.Descripcion = tipoProducto?.Descripcion;
            output.Tipo_Producto.Estado = tipoProducto?.Estado;

            output.Estado_Producto = productoDerramado.Estado_Producto;
            output.Cantidad_Derramada = productoDerramado.Cantidad_Derramada;

            output.Unidad = new Domain.Incidentes.Items.Unidad_Medida();
            output.Unidad.Id_Unidad_Medida = unidadMedida?.Id_Unidad_Medida;
            output.Unidad.Tipo = unidadMedida?.Tipo;
            output.Unidad.Descripcion = unidadMedida?.Descripcion;
            output.Unidad.Estado = unidadMedida?.Estado;

            output.Equipo_Responsable = productoDerramado.Equipo_Responsable;

            output.Estado_Equipo_Responsable = new Domain.Incidentes.Items.Estado_Equipo_Incidente();
            output.Estado_Equipo_Responsable.Id_Estado_Equipo_Incidente = estadoEquipo?.Id_Estado_Equipo_Incidente;
            output.Estado_Equipo_Responsable.Descripcion = estadoEquipo?.Descripcion;
            output.Estado_Equipo_Responsable.Estado = estadoEquipo?.Estado;

            output.Cantidad_Recuperado = productoDerramado.Cantidad_Recuperado;
            output.Cantidad_No_Recuperado = productoDerramado.Cantidad_No_Recuperado;
            output.Estado = productoDerramado.Estado;

            return output;
        }

        public async Task<List<Involucrado_Incidente_Investigacion>> GetInvolucradosInvestigacion(int IdInvestigacion)
        {
            var involucrados = await _context.Involucrado_Incidente_Investigacion.Where(x => x.Id_Investigacion_Final.Equals(IdInvestigacion) && x.Estado.Equals(EstadosIncidentes.Registrado.ToString())).ToListAsync();
            var output = new List<Involucrado_Incidente_Investigacion>();
            foreach (var item in involucrados)
            {
                var outputItem = new Involucrado_Incidente_Investigacion();
                outputItem.Id_Involucrado_Incidente_Investigacion = item.Id_Involucrado_Incidente_Investigacion;
                outputItem.Id_Investigacion_Final = item.Id_Investigacion_Final;
                outputItem.Id_Usuario = item.Id_Usuario;
                outputItem.Nombres = item.Nombres;
                outputItem.Apellidos = item.Apellidos;
                outputItem.Email = item.Email;
                outputItem.Celular = item.Celular;
                outputItem.Doc_Identidad = item.Doc_Identidad;
                outputItem.Edad = item.Edad;
                outputItem.Puesto_Trabajo = item.Puesto_Trabajo;
                outputItem.Empresa = item.Empresa;
                outputItem.Turno = item.Turno;
                outputItem.Tipo_Involucrado = item.Tipo_Involucrado;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }

        public async Task<Involucrado_Incidente_Investigacion> GetInvolucradoInvestigacion(int IdInvolucradoIncidente)
        {
            var entity = await _context.Involucrado_Incidente_Investigacion.FirstOrDefaultAsync(x => x.Id_Involucrado_Incidente_Investigacion.Equals(IdInvolucradoIncidente));

            var output = new Involucrado_Incidente_Investigacion();
            output.Id_Involucrado_Incidente_Investigacion = entity.Id_Involucrado_Incidente_Investigacion;
            output.Id_Investigacion_Final = entity.Id_Investigacion_Final;
            output.Id_Usuario = entity.Id_Usuario;
            output.Nombres = entity.Nombres;
            output.Apellidos = entity.Apellidos;
            output.Email = entity.Email;
            output.Celular = entity.Celular;
            output.Doc_Identidad = entity.Doc_Identidad;
            output.Edad = entity.Edad;
            output.Puesto_Trabajo = entity.Puesto_Trabajo;
            output.Empresa = entity.Empresa;
            output.Turno = entity.Turno;
            output.Tipo_Involucrado = entity.Tipo_Involucrado;
            output.Estado = entity.Estado;

            return output;
        }

        public async Task<List<Accion_Incidente_Investigacion>> GetAccionesInvestigacion(int IdInvestigacion)
        {
            var involucrados = await _context.Accion_Incidente_Investigacion.Where(x => x.Id_Investigacion_Final.Equals(IdInvestigacion) && !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString())).ToListAsync();
            var output = new List<Accion_Incidente_Investigacion>();
            foreach (var item in involucrados)
            {
                var outputItem = new Accion_Incidente_Investigacion();

                outputItem.Id_Accion_Incidente_Investigacion = item.Id_Accion_Incidente_Investigacion;
                outputItem.Id_Investigacion_Final = item.Id_Investigacion_Final;
                outputItem.Tipo_Accion = item.Tipo_Accion;
                outputItem.Accion_Inmediata = item.Accion_Inmediata;
                outputItem.Id_Usuario = item.Id_Usuario;
                outputItem.Id_Usuario2 = item.Id_Usuario2;
                outputItem.Responsable = item.Responsable;
                outputItem.Fecha = item.Fecha;
                outputItem.Estatus = item.Estatus;
                outputItem.Fecha_Verificacion = item.Fecha_Verificacion;
                outputItem.Comentarios_Verificacion = item.Comentarios_Verificacion;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }
        public async Task<List<Accion_Incidente_Investigacion>> GetAccionesInvestigacion()
        {
            var involucrados = await _context.Accion_Incidente_Investigacion.Where(x => !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString())).ToListAsync();
            var output = new List<Accion_Incidente_Investigacion>();
            foreach (var item in involucrados)
            {
                var outputItem = new Accion_Incidente_Investigacion();

                outputItem.Id_Accion_Incidente_Investigacion = item.Id_Accion_Incidente_Investigacion;
                outputItem.Id_Investigacion_Final = item.Id_Investigacion_Final;
                outputItem.Tipo_Accion = item.Tipo_Accion;
                outputItem.Accion_Inmediata = item.Accion_Inmediata;
                outputItem.Id_Usuario = item.Id_Usuario;
                outputItem.Id_Usuario2 = item.Id_Usuario2;
                outputItem.Responsable = item.Responsable;
                outputItem.Fecha = item.Fecha;
                outputItem.Estatus = item.Estatus;
                outputItem.Fecha_Verificacion = item.Fecha_Verificacion;
                outputItem.Comentarios_Verificacion = item.Comentarios_Verificacion;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }

        public async Task<Accion_Incidente_Investigacion> GetAccionInvestigacion(int IdAccionCorrectiva)
        {
            var entity = await _context.Accion_Incidente_Investigacion.FirstOrDefaultAsync(x => x.Id_Accion_Incidente_Investigacion.Equals(IdAccionCorrectiva));
            var output = new Accion_Incidente_Investigacion();
            output.Id_Accion_Incidente_Investigacion = entity.Id_Accion_Incidente_Investigacion;
            output.Id_Investigacion_Final = entity.Id_Investigacion_Final;
            output.Accion_Inmediata = entity.Accion_Inmediata;
            output.Tipo_Accion = entity.Tipo_Accion;
            output.Id_Usuario = entity.Id_Usuario;
            output.Id_Usuario2 = entity.Id_Usuario2;
            output.Responsable = entity.Responsable;
            output.Fecha = entity.Fecha;
            output.Estatus = entity.Estatus;
            output.Evidencia = entity.Evidencia;
            output.Fecha_Implementacion = entity.Fecha_Implementacion;
            output.Comentarios_Implementacion = entity.Comentarios_Implementacion;
            output.Fecha_Verificacion = entity.Fecha_Verificacion;
            output.Comentarios_Verificacion = entity.Comentarios_Verificacion;
            output.Estado = entity.Estado;
            return output;
        }

        public async Task<List<Causa_Incidente_Investigacion>> GetCausasInvestigacion(int IdInvestigacion)
        {
            var causas = await _context.Causa_Incidente_Investigacion.Where(x => x.Id_Investigacion_Final.Equals(IdInvestigacion) && x.Estado.Equals(EstadosIncidentes.Registrado.ToString())).ToListAsync();
            var output = new List<Causa_Incidente_Investigacion>();
            foreach (var item in causas)
            {
                var outputItem = new Causa_Incidente_Investigacion();

                outputItem.Id_Causa_Incidente_Investigacion = item.Id_Causa_Incidente_Investigacion;
                outputItem.Id_Investigacion_Final = item.Id_Investigacion_Final;
                outputItem.Id_Acto_Condicion = item.Id_Acto_Condicion;
                outputItem.Tipo_Acto_Condicion = item.Tipo_Acto_Condicion;
                outputItem.Id_Factor_Causa_Incidente = item.Id_Factor_Causa_Incidente;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }

        public async Task<Causa_Incidente_Investigacion> GetCausaInvestigacion(int IdCausaIncidente)
        {
            var entity = await _context.Causa_Incidente_Investigacion.FirstOrDefaultAsync(x => x.Id_Causa_Incidente_Investigacion.Equals(IdCausaIncidente));
            var output = new Causa_Incidente_Investigacion();
            output.Id_Causa_Incidente_Investigacion = entity.Id_Causa_Incidente_Investigacion;
            output.Id_Investigacion_Final = entity.Id_Investigacion_Final;
            output.Id_Acto_Condicion = entity.Id_Acto_Condicion;
            output.Tipo_Acto_Condicion = entity.Tipo_Acto_Condicion;
            output.Id_Factor_Causa_Incidente = entity.Id_Factor_Causa_Incidente;
            output.Descripcion = entity.Descripcion;
            output.Estado = entity.Estado;
            return output;
        }

    }
}
