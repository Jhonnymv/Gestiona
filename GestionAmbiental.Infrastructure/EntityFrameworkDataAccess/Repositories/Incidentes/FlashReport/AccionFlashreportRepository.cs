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
    class AccionFlashreportRepository : IAccionFlashReportReadOnlyRepository
    {
        private readonly Context _context;
        public AccionFlashreportRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Accion_Flash_Report> DetalleAccionFlashReport(int id)
        {
            var accionFlashReport = await _context.Accion_Flash_Report.FindAsync(id);
            var output = new Accion_Flash_Report();
            
            if (accionFlashReport !=null)
            {
                var usuario = await _context.Usuarios.FindAsync(accionFlashReport.Id_Usuario);

                output.Id_Accion_Flash_Report = accionFlashReport.Id_Accion_Flash_Report;
                output.Id_Flash_Report = accionFlashReport.Id_Flash_Report;
                output.Descripcion = accionFlashReport.Descripcion;
                output.Fecha = accionFlashReport.Fecha;   
                output.Estatus = accionFlashReport.Estatus;
                output.Estado = accionFlashReport.Estado;
                output.Responsable = accionFlashReport.Responsable;
                output.Id_Usuario = accionFlashReport.Id_Usuario;
                output.Usuario = new Domain.ValueObjects.Usuario
                {
                    IdUsuario = usuario.Id_Usuario,
                    Nombres = usuario.Nombres,
                    Apellidos = usuario.Apellidos,
                    Email = usuario.Email
                };
            }
            return output;
        }
        public async Task<List<Accion_Flash_Report>> ListarAccionesFlash(int Id_Flash_Report)
        {
            var accionFlashReport = await _context.Accion_Flash_Report.Where(x=>x.Id_Flash_Report==Id_Flash_Report && !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString())).ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();
            var output = new List<Accion_Flash_Report>();

            foreach (var item in accionFlashReport)
            {
                var usuario = usuarios.Find(x => x.Id_Usuario == item.Id_Usuario);
                output.Add(new Accion_Flash_Report
                {
                    Id_Accion_Flash_Report = item.Id_Accion_Flash_Report,
                    Id_Flash_Report = item.Id_Flash_Report,
                    Descripcion = item.Descripcion,
                    Fecha = item.Fecha,
                    Estatus = item.Estatus,
                    Estado = item.Estado,
                    Responsable = item.Responsable,
                    Id_Usuario = item.Id_Usuario,
                    Usuario = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = usuario.Id_Usuario,
                        Nombres = usuario.Nombres,
                        Apellidos = usuario.Apellidos,
                        Email = usuario.Email
                    }
                });
            }
            return output;
        }
        public async Task<int> EditarAccionFlashReport(Accion_Flash_Report accionflashReport)
        {
            var accion = await _context.Accion_Flash_Report.FindAsync(accionflashReport.Id_Accion_Flash_Report);
            if(accion != null)
            {
                accion.Id_Flash_Report = accionflashReport.Id_Flash_Report;
                accion.Descripcion = accionflashReport.Descripcion!=null? accionflashReport.Descripcion: accion.Descripcion;
                accion.Fecha = accionflashReport.Fecha != null ? accionflashReport.Fecha:accion.Fecha ;
                accion.Estatus = accionflashReport.Estatus != null ? accionflashReport.Estatus : accion.Estatus;
                accion.Estado = accionflashReport.Estado != null ? accionflashReport.Estado : accion.Estado;
                accion.Responsable = accionflashReport.Responsable != null ? accionflashReport.Responsable : accion.Responsable;
                accion.Id_Usuario = accionflashReport.Id_Usuario!=0?accionflashReport.Id_Usuario:accion.Id_Usuario;
                _context.Accion_Flash_Report.Update(accion);
                return await _context.SaveChangesAsync();
            }
            return accion.Id_Flash_Report;
        }
        public async Task<int> RegistrarAccionFlashReport(Accion_Flash_Report accionflashReport)
        {
            var accion = new Entities.Incidentes.FlashReport.Accion_Flash_Report();

            accion.Id_Flash_Report = accionflashReport.Id_Flash_Report;
			accion.Descripcion = accionflashReport.Descripcion;
            accion.Fecha = accionflashReport.Fecha;
            accion.Estatus = accionflashReport.Estatus;
            accion.Estado = accionflashReport.Estado;
            accion.Responsable = accionflashReport.Responsable;
            accion.Id_Usuario = accionflashReport.Id_Usuario;

            _context.Accion_Flash_Report.Add(accion);
            await _context.SaveChangesAsync();

            return accion.Id_Accion_Flash_Report;
        }
    }
}
