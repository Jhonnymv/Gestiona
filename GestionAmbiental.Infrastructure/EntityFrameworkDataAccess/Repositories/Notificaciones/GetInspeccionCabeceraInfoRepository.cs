using GestionAmbiental.Application.Repositories.Notificaciones;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Notificaciones
{
    public class GetInspeccionCabeceraInfoRepository : IGetInspeccionCabeceraInfoReadOnlyRepository
    {
        private Context _context;

        public GetInspeccionCabeceraInfoRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<InspeccionCabecera> GetInspeccionCabecera(int idCabecera)
        {
            var cabeccera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas_Contratistas.ToListAsync();
            var gerEjectMar = gerencias.Where(g => g.Id_Gerencia.Equals(cabeccera.Id_Gerencia_Ejecutora)).FirstOrDefault().Descripcion;
            var gerEjectEPCM = gerencias.Where(g => g.Id_Gerencia.Equals(cabeccera.Id_Gerencia_EPCM)).FirstOrDefault().Descripcion;
            var empContMar = empresas.Where(e => e.Id_Empresa_Contratista.Equals(cabeccera.Id_Empresa_Contratista)).FirstOrDefault().Razon_Social;
            var empContEPCM = empresas.Where(e => e.Id_Empresa_Contratista.Equals(cabeccera.Id_Empresa_Contratista_EPCM)).FirstOrDefault().Razon_Social;
            var usuarios = await _context.Usuarios.ToListAsync();
            var responsableLugar = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(cabeccera.Id_Responsable_Lugar));
            var responsableLugarDomain = new Usuario(responsableLugar.Id_Usuario, responsableLugar.Id_Rol, responsableLugar.Nombres, responsableLugar.Apellidos, responsableLugar.Email);

            var output = new InspeccionCabecera(
                cabeccera.Id_Inspeccion_Cabecera,
                cabeccera.Codigo,
                cabeccera.Objetivo,
                gerEjectMar,
                empContMar,
                gerEjectEPCM,
                empContEPCM,
                cabeccera.Lugar_Inspeccionado,
                responsableLugarDomain,
                cabeccera.Fecha_Inspeccion,
                cabeccera.Inspectores,
                cabeccera.Estado
               );
            return output;
        }
    }
}
