using GestionAmbiental.Application.Repositories.ResiduosSolidos;
using GestionAmbiental.Domain.Enum;
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
        public async Task<List<Empresa>> GetEmpresa()
        {
            var data = await _context.Empresas.ToListAsync();
            var output = new List<Empresa>();
            foreach (var item in data)
            {
                output.Add(new Empresa(item.Id_Empresa, item.Razon_Social,item.Estado));
            }
            return output;
        }
        public async Task<List<ACR_CMRS>> GetACR_CMRS()
        {
            var data = await _context.ACR_CMRs.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<ACR_CMRS>();
            foreach (var item in data)
            {
                output.Add(new ACR_CMRS(item.Id_ACR_CMRS, item.Descripcion, item.Estado));
            }
                return output;
        }

        public async Task<List<EO_RS>> GetEO_RS()
        {
            var data = await _context.EO_RSs.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<EO_RS>();
            foreach (var item in data)
            {
                output.Add(new EO_RS(item.Id_EO_RS, item.Razon_Social,item.Registro,item.Fecha_Vencimiento_Registro, item.Estado));
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
            var data = await _context.Residuos_Solidos.Where(x=> x.Estado.Equals("ACT")).ToListAsync();
            var tipos = await _context.Tipo_Residuos.ToListAsync();
            var output = new List<Residuo_Solido>();
            foreach (var item in data)
            {
                var tipo = tipos.FirstOrDefault(x => x.Id_Tipo_Residuo.Equals(item.Id_Tipo_Residuo));
                var tipo_model = new Tipo_Residuo(tipo.Id_Tipo_Residuo, tipo.Descripcion,tipo.Estado);
                output.Add(new Residuo_Solido(item.Id_Residuo_Solido, tipo_model, item.Descripcion, item.Estado));
            }
            return output;
        }

        public async Task<List<Tipo_Residuo>> GetTipos_Residuo()
        {
            var data = await _context.Tipo_Residuos.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<Tipo_Residuo>();
            foreach (var item in data)
            {
                output.Add(new Tipo_Residuo(item.Id_Tipo_Residuo,item.Descripcion,item.Estado));
            }
            return output;
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            var data = await _context.Usuarios.Where(x => x.Estado.Equals("ACT") && !x.Id_Rol.Equals((int)RSRoles.Admin)).ToListAsync();
            var output = new List<Usuario>();
            foreach (var item in data)
            {
                output.Add(new Usuario(item.Id_Usuario, item.Id_Rol, item.Nombres, item.Apellidos, item.Email));
            }
            return output;
        }
        public async Task<List<Responsable_ARC_CRMS>> GetResponsables_ARC_CRM()
        {
            var data = await _context.Responsables_ACR_CRM.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<Responsable_ARC_CRMS>();
            foreach (var item in data)
            {
                output.Add(new Responsable_ARC_CRMS(item.Id_Responsable_ARC_CRMS, item.Nombres,item.Apellidos, item.Estado));
            }
            return output;
        }
        public async Task<List<Gerencia>> GetGerencias()
        {
            var data = await _context.Gerencias.ToListAsync();
            var output = new List<Gerencia>();
            foreach (var item in data)
            {
                output.Add(new Gerencia(item.Id_Gerencia, item.Descripcion));
            }
            return output;
        }
        public async Task<List<Conductor_RS>> GetConductores_RS()
        {
            var data = await _context.Condutores_RS.ToListAsync();
            var output = new List<Conductor_RS>();
            foreach (var item in data)
            {
                output.Add(new Conductor_RS(item.Id_Conductor_RS,item.Nombres,item.Apellidos,item.NLicencia, item.Estado));
            }
            return output;
        }
        public async Task<List<Vehiculo_RS>> GetVehiculos_RS()
        {
            var data = await _context.Vehiculos_RS.ToListAsync();
            var output = new List<Vehiculo_RS>();
            foreach (var item in data)
            {
                output.Add(new Vehiculo_RS(item.Id_Vehiculo_RS,item.Placa,item.Capacidad,item.Estado));
            }
            return output;
        }
    }
}
