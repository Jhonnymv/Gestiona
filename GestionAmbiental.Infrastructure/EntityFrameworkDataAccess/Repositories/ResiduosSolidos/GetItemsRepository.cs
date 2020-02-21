using GestionAmbiental.Application.Repositories.ResiduosSolidos;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.ResiduosSolidos
{

    public class GetItemsRepository : IGetItemsReadOnlyRepository
    {
        private readonly Context _context;

        public GetItemsRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<ACR_CMRS>> GetACR_CMRS()
        {
            var data = await _context.ACR_CMRs.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<ACR_CMRS>();
            foreach(var item in data)
            {
                output.Add(new ACR_CMRS(item.Id_ACR_CMRS,item.Descripcion,item.Estado));
            }
            return output;
        }

        public async Task<List<EO_RS>> GetEO_RS()
        {
            var data = await _context.EO_RSs.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<EO_RS>();
            foreach (var item in data)
            {
                output.Add(new EO_RS(item.Id_EO_RS, item.Razon_Social, item.Estado));
            }
            return output;
        }

        public async Task<List<Ingreso_Salida>> GetIngreso_Salida()
        {
            var data = await _context.Ingresos_Salidas.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<Ingreso_Salida>();
            foreach (var item in data)
            {
                output.Add(new Ingreso_Salida(item.Id_Ingreso_Salida, item.Descripcion, item.Estado));
            }
            return output;
        }

        public async Task<List<Motivo_Ingreso_Salida>> GetMotivos_Ingreso_Salida()
        {
            var data = await _context.Motivos_Ingreso_Salida.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<Motivo_Ingreso_Salida>();
            foreach (var item in data)
            {
                output.Add(new Motivo_Ingreso_Salida(item.Id_Motivo, item.Descripcion, item.Estado));
            }
            return output;
        }

        public async Task<List<Residuo_Solido>> GetResiduos_Solidos()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Tipo_Residuo>> GetTipos_Residuo()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            throw new NotImplementedException();
        }
    }
}
