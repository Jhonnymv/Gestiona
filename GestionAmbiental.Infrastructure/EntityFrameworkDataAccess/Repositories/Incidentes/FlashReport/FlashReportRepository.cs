using GestionAmbiental.Application.Repositories.Incidentes.FlashReport;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.FlashReport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Incidentes.FlashReport
{
    internal class FlashReportRepository : IFlashReportReadOnlyRepository
    {
        private readonly Context _context;

        public FlashReportRepository(Context context)
        {
            _context = context;
        }

        public async Task<int> RegistrarFlashReport(Flash_Report flashReport)
        {
            var entity = new Entities.Incidentes.FlashReport.Flash_Report
            {
                Codigo = flashReport.Codigo,
                Id_Gerencia = flashReport.Id_Gerencia,
                Id_Empresa = flashReport.Id_Empresa,
                Lugar = flashReport.Lugar,
                Fecha_Hora = flashReport.Fecha_Hora,
                Nivel_Incidente = flashReport.Id_Nivel_Incidente,
                Id_Componente_Ambiental = flashReport.Id_Componente_Ambiental,
                Id_Producto_Incidente = flashReport.Id_Producto_Incidente,
                Cantidad_Derramada = flashReport.Cantidad_Derramada,
                Id_Unidad_Cantidad_Derramada = flashReport.Id_Unidad_Cantidad_Derramada,
                Descripcion = flashReport.Descripcion,
                Registro_Fotografico = flashReport.Registro_Fotografico,
				Archivos_Registro_Fotografico = flashReport.Archivos_Registro_Fotografico,
				Id_Supervisor_Marcobre = flashReport.Id_Supervisor_Marcobre,
                Supervisor_Contratista = flashReport.Supervisor_Contratista,
                Cargo_Supervisor_Marcobre = flashReport.Cargo_Supervisor_Marcobre,
                Cargo_Supervisor_Contratista = flashReport.Cargo_Supervisor_Contratista,
                Estado = flashReport.Estado,
                Id_Registrado_Por = flashReport.Id_Registrado_Por
            };

            _context.Flash_Report.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id_Flash_Report;
        }

        public async Task<Flash_Report> DetalleFlashReport(int id)
        {
            var flashReport = await _context.Flash_Report.FirstOrDefaultAsync(x => x.Id_Flash_Report == id);

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario == flashReport.Id_Supervisor_Marcobre);
            var gerencia = await _context.Gerencias.FirstOrDefaultAsync(x => x.Id_Gerencia == flashReport.Id_Gerencia);
            var empresa = await _context.Empresas.FirstOrDefaultAsync(x => x.Id_Empresa == flashReport.Id_Empresa);
            var producto = await _context.Producto_Incidente.FirstOrDefaultAsync(x => x.Id_Producto_Incidente == flashReport.Id_Producto_Incidente);
			var nivelIncidente = await _context.Nivel_Incidente.FirstOrDefaultAsync(x => x.Id_Nivel_Incidente == flashReport.Nivel_Incidente);

			var output = new Flash_Report();

            if (flashReport != null) {
                output.Id_Flash_Report = flashReport.Id_Flash_Report;
                output.Codigo = flashReport.Codigo;
                output.Id_Gerencia = flashReport.Id_Gerencia;
                output.Id_Empresa = flashReport.Id_Empresa;
                output.Lugar = flashReport.Lugar;
                output.Fecha_Hora = flashReport.Fecha_Hora;
                output.Id_Nivel_Incidente = flashReport.Nivel_Incidente;
                output.Id_Componente_Ambiental = flashReport.Id_Componente_Ambiental;
                output.Id_Producto_Incidente = flashReport.Id_Producto_Incidente;
                output.Cantidad_Derramada = flashReport.Cantidad_Derramada;
                output.Id_Unidad_Cantidad_Derramada = flashReport.Id_Unidad_Cantidad_Derramada;
                output.Descripcion = flashReport.Descripcion;
                output.Registro_Fotografico = flashReport.Registro_Fotografico;
				output.Archivos_Registro_Fotografico = flashReport.Archivos_Registro_Fotografico;
				output.Id_Supervisor_Marcobre = flashReport.Id_Supervisor_Marcobre;
                output.Supervisor_Contratista = flashReport.Supervisor_Contratista;
                output.Cargo_Supervisor_Marcobre = flashReport.Cargo_Supervisor_Marcobre;
                output.Cargo_Supervisor_Contratista = flashReport.Cargo_Supervisor_Contratista;
                output.Estado = flashReport.Estado;
                output.Id_Registrado_Por = flashReport.Id_Registrado_Por;
                output.Supervisor_Marcobre = new Domain.ValueObjects.Usuario
                {
                    IdUsuario = usuario.Id_Usuario,
                    Nombres = usuario.Nombres,
                    Apellidos = usuario.Apellidos,
                    Email = usuario.Email,
                    Estado = usuario.Estado
                };
                output.Gerencia = new Domain.ValueObjects.Gerencia
                {
                    IdGerencia = gerencia.Id_Gerencia,
                    Descripcion = gerencia.Descripcion
                };
                output.Nivel_Incidente = new Domain.Incidentes.Items.Nivel_Incidente
				{
					Id_Nivel_Incidente = nivelIncidente.Id_Nivel_Incidente,
					Descripcion = nivelIncidente.Descripcion
				};
				output.Empresa = new Domain.ValueObjects.Empresa
                {
                    IdEmpresa = empresa.Id_Empresa,
                    RazonSocial = empresa.Razon_Social
                };
                output.Producto_Incidente = new Domain.Incidentes.Items.Producto_Incidente
                {
                    Id_Producto_Incidente = producto.Id_Producto_Incidente,
                    Descripcion = producto.Descripcion
                }; 
            }
            return output;
        }

        public async Task<int> EditarFlashReport(Flash_Report flashReport)
        {
            var entity = await _context.Flash_Report.FirstOrDefaultAsync(x => x.Id_Flash_Report == flashReport.Id_Flash_Report);
			if (entity == null)
			{
				throw new InvalidOperationException($"Flash_Report with Id {flashReport.Id_Flash_Report} not found.");
			}
			// Update the properties of the existing entity
			entity.Codigo = flashReport.Codigo;
			entity.Id_Gerencia = flashReport.Id_Gerencia;
			entity.Id_Empresa = flashReport.Id_Empresa;
			entity.Lugar = flashReport.Lugar;
			entity.Fecha_Hora = flashReport.Fecha_Hora;
			entity.Nivel_Incidente = flashReport.Id_Nivel_Incidente;
			entity.Id_Componente_Ambiental = flashReport.Id_Componente_Ambiental;
			entity.Id_Producto_Incidente = flashReport.Id_Producto_Incidente;
			entity.Cantidad_Derramada = flashReport.Cantidad_Derramada;
			entity.Id_Unidad_Cantidad_Derramada = flashReport.Id_Unidad_Cantidad_Derramada;
			entity.Descripcion = flashReport.Descripcion;
			entity.Registro_Fotografico = flashReport.Registro_Fotografico;
			entity.Archivos_Registro_Fotografico = flashReport.Archivos_Registro_Fotografico;
			entity.Id_Supervisor_Marcobre = flashReport.Id_Supervisor_Marcobre;
			entity.Supervisor_Contratista = flashReport.Supervisor_Contratista;
			entity.Cargo_Supervisor_Marcobre = flashReport.Cargo_Supervisor_Marcobre;
			entity.Cargo_Supervisor_Contratista = flashReport.Cargo_Supervisor_Contratista;
			entity.Estado = flashReport.Estado;

            _context.Flash_Report.Update(entity);
            await _context.SaveChangesAsync();

            return entity.Id_Flash_Report;
        }

        public async Task<List<Flash_Report>> ListarFlashReport(DateTime? fechaInicio, DateTime? fechaFin, int[] gerencia, int[] titular, List<string> estado, int[] responsable, int[] asesor, string codigo)
        {
            var flashReports = await _context.Flash_Report.ToListAsync();
            
            if (fechaInicio.HasValue)
                flashReports = flashReports.Where(x => x.Fecha_Hora.Date >= fechaInicio.Value.Date).ToList();
            if (fechaFin.HasValue)
                flashReports = flashReports.Where(x => x.Fecha_Hora.Date <= fechaFin.Value.Date).ToList();
            if (gerencia.Length > 0)
                flashReports = flashReports.Where(x => gerencia.Contains(x.Id_Gerencia)).ToList();           
            if (asesor.Length > 0)
                flashReports = flashReports.Where(x => asesor.Contains(x.Id_Supervisor_Marcobre)).ToList();
            //if (codigo != null && codigo.Length > 0)
            //    flashReports = flashReports.Where(x => x.Codigo.Contains(codigo)).ToList();            

            var output = new List<Flash_Report>();
			var usuarios = await _context.Usuarios.ToListAsync();
			var gerencias = await _context.Gerencias.ToListAsync();
			var empresas = await _context.Empresas.ToListAsync();
			var productos = await _context.Producto_Incidente.ToListAsync();
			var nivelesIncidentes = await _context.Nivel_Incidente.ToListAsync();
            var unidades = await _context.Unidad_Medida.ToListAsync();

            foreach (var item in flashReports)
            {
				var usuario = usuarios.Find(x => x.Id_Usuario == item.Id_Supervisor_Marcobre);
				var gerenciaN = gerencias.Find(x => x.Id_Gerencia == item.Id_Gerencia);
				var empresaN = empresas.Find(x => x.Id_Empresa == item.Id_Empresa);
				var nivelIncidente = nivelesIncidentes.Find(x => x.Id_Nivel_Incidente == item.Nivel_Incidente);
                var producto = productos.Find(x => x.Id_Producto_Incidente == item.Id_Producto_Incidente);
                var unidad = unidades.Find(x => x.Id_Unidad_Medida == item.Id_Unidad_Cantidad_Derramada);

                var outputItem = new Domain.Incidentes.FlashReport.Flash_Report { 
                    Id_Flash_Report = item.Id_Flash_Report,
                    Codigo = item.Codigo,
                    Id_Gerencia = item.Id_Gerencia,
                    Cantidad_Derramada = item.Cantidad_Derramada,
                    Cargo_Supervisor_Contratista = item.Cargo_Supervisor_Contratista,
                    Cargo_Supervisor_Marcobre = item.Cargo_Supervisor_Marcobre,
                    Id_Componente_Ambiental = item.Id_Componente_Ambiental,
                    Descripcion = item.Descripcion,
                    Id_Empresa = item.Id_Empresa,
                    Estado = item.Estado,
                    Fecha_Hora = item.Fecha_Hora,
                    Id_Producto_Incidente = item.Id_Producto_Incidente,
                    Lugar = item.Lugar,
					Id_Nivel_Incidente = item.Nivel_Incidente,
                    Id_Unidad_Cantidad_Derramada = item.Id_Unidad_Cantidad_Derramada,
                    Id_Supervisor_Marcobre = item.Id_Supervisor_Marcobre,
                    Supervisor_Contratista = item.Supervisor_Contratista,
                    Registro_Fotografico = item.Registro_Fotografico,
					Archivos_Registro_Fotografico = item.Archivos_Registro_Fotografico
				};

				outputItem.Supervisor_Marcobre = new Domain.ValueObjects.Usuario
				{
					IdUsuario = usuario.Id_Usuario,
					Nombres = usuario.Nombres,
					Apellidos = usuario.Apellidos,
					Email = usuario.Email,
					Estado = usuario.Estado
				};
				outputItem.Gerencia = new Domain.ValueObjects.Gerencia
				{
					IdGerencia = gerenciaN.Id_Gerencia,
					Descripcion = gerenciaN.Descripcion
				};
                outputItem.Unidad_Cantidad_Derramada = new Domain.Incidentes.Items.Unidad_Medida
                {
                    Id_Unidad_Medida = unidad.Id_Unidad_Medida,
                    Descripcion = unidad.Descripcion,
                    Estado = unidad.Estado,
                    Tipo = unidad.Tipo
                };
                outputItem.Nivel_Incidente = new Domain.Incidentes.Items.Nivel_Incidente
				{
					Id_Nivel_Incidente = nivelIncidente.Id_Nivel_Incidente,
					Descripcion = nivelIncidente.Descripcion
				};
				outputItem.Empresa = new Domain.ValueObjects.Empresa
				{
					IdEmpresa = empresaN.Id_Empresa,
					RazonSocial = empresaN.Razon_Social
				};
				outputItem.Producto_Incidente = new Domain.Incidentes.Items.Producto_Incidente
				{
					Id_Producto_Incidente = producto.Id_Producto_Incidente,
					Descripcion = producto.Descripcion
				};
				output.Add(outputItem);
            }
            return output;
        }
		public async Task<string> GetUltimoCodigo()
		{
			var codigo = await _context.Flash_Report
				.Where(x => !x.Codigo.Equals("TBD") && (x.Estado.Equals(EstadosIncidentes.Difundido.ToString()) || x.Estado.Equals(EstadosIncidentes.PendienteInvestigacion.ToString())))
				.OrderBy(x => x.Codigo).Select(x => x.Codigo)
				.LastOrDefaultAsync();
			if (codigo != null)
				return codigo;
			else
				return "IA-00000000";
		}

		public async Task<string> GetEvidenciasReporte(int IdFlashReport)
		{
			var reporte = await _context.Flash_Report.FirstOrDefaultAsync(x => x.Id_Flash_Report.Equals(IdFlashReport));
			if (reporte != null && reporte.Archivos_Registro_Fotografico != null)
				return reporte.Archivos_Registro_Fotografico;
			else
				return "[]";
		}
	}
}
