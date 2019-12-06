using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Reportes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Reportes
{
    public class GetReportesRepository : IGetReportesReadOnlyRepository
    {
        private Context _context;

        public GetReportesRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Dimension> GetObservacionesMAP()
        {
            var data = await _context.Inspecciones_Detalle.Where(x => x.Tipo_Observacion.Equals("MAP")).CountAsync();
            var output = new Dimension();
            output.Nombre = "MAP";
            output.Valor = data;
            return output;
        }
        public async Task<Dimension> GetObservacionesSSO()
        {
            var data = await _context.Inspecciones_Detalle.Where(x => x.Tipo_Observacion.Equals("SSO")).CountAsync();
            var output = new Dimension();
            output.Nombre = "SSO";
            output.Valor = data;
            return output;
        }
        public async Task<List<Dimension>> GetObservacionesXUsuario()
        {
            var cabeceras_registradas = await _context.Inspecciones_Cabecera.Where(x => x.Estado.Equals(EstadosInspeccion.Registrado.ToString())).ToListAsync();
            var observaciones = await _context.Inspecciones_Detalle.Where(x => cabeceras_registradas.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera)).ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();

            var output = new List<Dimension>();
            foreach(var item in usuarios) {
                var nObservaciones = observaciones.Where(x => x.Id_Responsable_Marcobre.Equals(item.Id_Usuario)).Count();
                if(nObservaciones >0)
                    output.Add(new Dimension(item.Nombres + " " + item.Apellidos, nObservaciones));
            }
            return output;
        }
        public async Task<List<Dimension>> GetObservacionesXEstado()
        {
            var cabeceras_registradas = await _context.Inspecciones_Cabecera.Where(x => x.Estado.Equals(EstadosInspeccion.Registrado.ToString())).ToListAsync();
            var inspecciones = await _context.Inspecciones_Detalle.Where(x => cabeceras_registradas.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera)).ToListAsync();

            var output = new List<Dimension>();

            foreach (var item in Enum.GetNames(typeof(EstadosInspeccion)))
            {
                output.Add(new Dimension(item, inspecciones.Where(x => x.Estado.Equals(item)).Count()));
            }

            return output;
        }
        public async Task<List<ObservacionesUsuario>> GetResumenObservacionesXUsuario(string filtro)
        {
            var output = new List<ObservacionesUsuario>();

            var cabeceras_registradas = await _context.Inspecciones_Cabecera.Where(x => x.Estado.Equals(EstadosInspeccion.Registrado.ToString())).ToListAsync();
            var observaciones = await _context.Inspecciones_Detalle.Where(x => cabeceras_registradas.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera)).ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();

            foreach(var usuario in usuarios)
            {
                var detalle = new ObservacionesUsuario();
                detalle.usuario = new Domain.ValueObjects.Usuario(usuario.Id_Usuario,usuario.Id_Rol,usuario.Nombres,usuario.Apellidos,usuario.Email);
                detalle.pendientes = observaciones.Where(x => x.Id_Responsable_Marcobre.Equals(usuario.Id_Usuario) && x.Estado.Equals(EstadosInspeccion.Registrado.ToString())).Count();
                detalle.implementadas = observaciones.Where(x => x.Id_Responsable_Marcobre.Equals(usuario.Id_Usuario) && x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).Count();
                detalle.aprobadas = observaciones.Where(x => x.Id_Responsable_Marcobre.Equals(usuario.Id_Usuario) && x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).Count();
                detalle.rechazadas = observaciones.Where(x => x.Id_Responsable_Marcobre.Equals(usuario.Id_Usuario) && x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).Count();

                output.Add(detalle);
            }

            return output;
        }
        public async Task<List<Dimension>> GetInspeccionesDiarias()
        {
            var output = new List<Dimension>();
            var ndias = 30;
            var inspecciones = await _context.Inspecciones_Cabecera
                .Where(x => x.Fecha_Inspeccion > DateTime.Now.AddDays(-ndias) && x.Estado.Equals(EstadosInspeccion.Registrado.ToString()))
                .OrderBy(x => x.Fecha_Inspeccion)
                .ToListAsync();

            var inspecciones_diarias = inspecciones.Select(x => x.Fecha_Inspeccion).GroupBy(y => y.Date).ToList();

            var list_dias = new List<Dimension>();
            var list_inspecciones = new List<Dimension>();
            for (int i = ndias; i >= 0; i--)
            {
                list_dias.Add(new Dimension(DateTime.Now.AddDays(-i).ToString("dd MMMM").Substring(0, 6), 0,i));
            }
            foreach(var item in inspecciones_diarias)
            {
                list_inspecciones.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0,6), item.Count(),(int)(DateTime.Now-item.Key).TotalDays));
            }
            var distinct = list_dias.Where(x => !list_inspecciones.Select(y=>y.Nombre).ToList().Contains(x.Nombre)).ToList();

            output = distinct.Union(list_inspecciones).Where(x=>x.Index>=0).OrderByDescending(x => x.Index).ToList();

            //var union = distinct.Union(output).OrderBy(x=>x.Nombre).ToList();
            return output;
        }
        public async Task<List<Dimension>> GetObservacionesNivelRiesgoMAP()
        {
            var map = await _context.Inspecciones_Detalle.Where(x => x.Tipo_Observacion.Equals("MAP")).GroupBy(x=>x.Nivel_Riesgo_Ambiental).ToListAsync();
            var output = new List<Dimension>();
            if (map.Count == 3)
                foreach (var item in map)
                {
                    switch (item.Key)
                    {
                        case "Tolerable": output.Add(new Dimension(item.Key, item.Count(), 0)); break;
                        case "Alerta": output.Add(new Dimension(item.Key, item.Count(), 1)); break;
                        case "No Aceptable": output.Add(new Dimension(item.Key, item.Count(), 2)); break;
                    }
                }
            else
            {
                output.Add(new Dimension("Tolerable", 0, 0));
                output.Add(new Dimension("Alerta", 0, 1));
                output.Add(new Dimension("No Aceptable", 0, 2));
                foreach (var item in map)
                {
                    switch (item.Key)
                    {
                        case "Tolerable": output.FirstOrDefault(x=>x.Nombre.Equals("Tolerable")).Valor=item.Count(); break;
                        case "Alerta": output.FirstOrDefault(x => x.Nombre.Equals("Alerta")).Valor = item.Count(); break;
                        case "No Aceptable": output.FirstOrDefault(x => x.Nombre.Equals("No Aceptable")).Valor = item.Count(); break;
                    }
                }
            }
            return output;
        }
        public async Task<List<Dimension>> GetObservacionesNivelRiesgoSSO()
        {
            var sso = await _context.Inspecciones_Detalle.Where(x => x.Tipo_Observacion.Equals("SSO")).GroupBy(x => x.Nivel_Riesgo_Ambiental).ToListAsync();
            var output = new List<Dimension>();
            if (sso.Count == 3)
                foreach (var item in sso)
                {
                    switch (item.Key)
                    {
                        case "Bajo": output.Add(new Dimension(item.Key, item.Count(), 3)); break;
                        case "Medio": output.Add(new Dimension(item.Key, item.Count(), 4)); break;
                        case "Alto": output.Add(new Dimension(item.Key, item.Count(), 5)); break;
                    }
                }
            else
            {
                output.Add(new Dimension("Bajo", 0, 3));
                output.Add(new Dimension("Medio", 0, 4));
                output.Add(new Dimension("Alto", 0, 5));
                foreach (var item in sso)
                {
                    switch (item.Key)
                    {
                        case "Bajo": output.FirstOrDefault(x => x.Nombre.Equals("Bajo")).Valor = item.Count(); break;
                        case "Medio": output.FirstOrDefault(x => x.Nombre.Equals("Medio")).Valor = item.Count(); break;
                        case "Alto": output.FirstOrDefault(x => x.Nombre.Equals("Alto")).Valor = item.Count(); break;
                    }
                }
            }
            return output;
        }

        public async Task<List<Dimension>> GetObservacionesTipoInspeccionMAP()
        {
            var map = await _context.Inspecciones_Detalle.Where(x => x.Tipo_Observacion.Equals("MAP")).GroupBy(x => x.Descripcion_Condicion).ToListAsync();
            var output = new List<Dimension>();
            if (map.Count == 2)
                foreach (var item in map)
                {
                    switch (item.Key)
                    {
                        case "Condición Subestándar": output.Add(new Dimension(item.Key, item.Count(), 1)); break;
                        case "Acto Subestándar": output.Add(new Dimension(item.Key, item.Count(), 2)); break;
                    }
                }
            else
            {
                output.Add(new Dimension("Condición Subestándar", 0, 1));
                output.Add(new Dimension("Acto Subestándar", 0, 2));
                foreach (var item in map)
                {
                    switch (item.Key)
                    {
                        case "Condición Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Condición Subestándar")).Valor = item.Count(); break;
                        case "Acto Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Acto Subestándar")).Valor = item.Count(); break;
                    }
                }
            }
            return output;
        }

        public async Task<List<Dimension>> GetObservacionesTipoInspeccionSSO()
        {
            var sso = await _context.Inspecciones_Detalle.Where(x => x.Tipo_Observacion.Equals("SSO")).GroupBy(x => x.Descripcion_Condicion).ToListAsync();
            var output = new List<Dimension>();
            if (sso.Count == 2)
                foreach (var item in sso)
                {
                    switch (item.Key)
                    {
                        case "Condición Subestándar": output.Add(new Dimension(item.Key, item.Count(), 1)); break;
                        case "Acto Subestándar": output.Add(new Dimension(item.Key, item.Count(), 2)); break;
                    }
                }
            else
            {
                output.Add(new Dimension("Condición Subestándar", 0, 1));
                output.Add(new Dimension("Acto Subestándar", 0, 2));
                foreach (var item in sso)
                {
                    switch (item.Key)
                    {
                        case "Condición Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Condición Subestándar")).Valor = item.Count(); break;
                        case "Acto Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Acto Subestándar")).Valor = item.Count(); break;
                    }
                }
            }
            return output;
        }
    }
}
