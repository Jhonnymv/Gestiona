using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using GestionAmbiental.Domain.AvistamientoFauna.Items;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Incidentes.Items;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.GestorTablasMaestras
{
    public class GestorTablasMaestrasRepository : IGestorTablasMaestrasReadOnlyRepository
    {
        private Context _context;

        public GestorTablasMaestrasRepository(Context context)
        {
            _context = context;
        }

        public async Task<int> AddACR_CMRS(ACR_CMRS acr_cmrs)
        {
            var entity = new Entities.ResiduosSolidos.ACR_CMRS
            {
                Descripcion = acr_cmrs.Descripcion,
                Estado = "ACT"
            };
            _context.ACR_CMRs.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_ACR_CMRS;
        }

        public async Task<int> AddConductor_RS(Conductor_RS conductor_RS)
        {
            var entity = new Entities.ResiduosSolidos.Conductor_RS
            {
                Nombres = conductor_RS.Nombres,
                Apellidos = conductor_RS.Apellidos,
                NLicencia = conductor_RS.NLicencia,
                Estado = "ACT"
            };
            _context.Condutores_RS.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Conductor_RS;
        }

        public async Task<int> AddEmpresa(Empresa empresa)
        {
            var empresaEntity = new Entities.Empresa
            {
                Razon_Social = empresa.RazonSocial
            };
            _context.Empresas.Add(empresaEntity);
            await _context.SaveChangesAsync();
            return empresaEntity.Id_Empresa;
        }

        public async Task<int> AddEO_RS(EO_RS eors)
        {
            var entity = new Entities.ResiduosSolidos.EO_RS
            {
                Razon_Social = eors.Razon_Social,
                Registro = eors.Registro,
                Fecha_Vencimiento_Registro = eors.Fecha_Vencimiento_Registro,
                Estado = "ACT"
            };
            _context.EO_RSs.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_EO_RS;
        }

        public async Task<int> AddGerencia(Gerencia gerencia)
        {
            var gerenciaEntity = new Entities.Gerencia
            {
                Descripcion = gerencia.Descripcion
            };
            _context.Gerencias.Add(gerenciaEntity);
            await _context.SaveChangesAsync();
            return gerenciaEntity.Id_Gerencia;
        }

        public async Task<int> AddMotivo_Ingreso_Salida(Motivo_Ingreso_Salida motivo)
        {
            var entity = new Entities.ResiduosSolidos.Motivo_Ingreso_Salida
            {
                Descripcion = motivo.Descripcion,
                Estado = "ACT"
            };
            _context.Motivos_Ingreso_Salida.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Motivo;
        }

        public async Task<int> AddResiduo_Solido(Residuo_Solido residuo_solido)
        {
            var entity = new Entities.ResiduosSolidos.Residuo_Solido
            {
                Descripcion = residuo_solido.Descripcion,
                Id_Tipo_Residuo = residuo_solido.Tipo_Residuo.Id_Tipo_Residuo,
                Estado = "ACT"
            };
            _context.Residuos_Solidos.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Residuo_Solido;
        }

        public async Task<int> AddResponsable_ARC_CRMS(Responsable_ARC_CRMS responsable)
        {
            var entity = new Entities.ResiduosSolidos.Responsable_ACR_CRMS
            {
                Nombres = responsable.Nombres,
                Apellidos = responsable.Apellidos,
                Estado = "ACT"
            };
            _context.Responsables_ACR_CRM.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Responsable_ARC_CRMS;
        }

        public async Task<int> AddTipoCondicion(TipoCondicion tipoCondicion)
        {
            var tipoCondicionEntity = new Entities.Tipo_Condicion
            {
                Descripcion = tipoCondicion.Descripcion,
                Grupo = tipoCondicion.Grupo
            };
            _context.Tipos_Condicion.Add(tipoCondicionEntity);
            await _context.SaveChangesAsync();
            return tipoCondicionEntity.Id_Tipo_Condicion;
        }

        public async Task<int> AddUsuario_Notificar_RS(int Id_Usuario_AD)
        {
            var temp_entity = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(Id_Usuario_AD));
            var entity = new Entities.ResiduosSolidos.Usuario_Notificar_RS
            {
                Nombres = temp_entity.Nombres,
                Apellidos = temp_entity.Apellidos,
                Email = temp_entity.Email,
                Estado = "ACT",
                Id_Rol = temp_entity.Id_Rol,
                usuario_red = temp_entity.usuario_red
            };
            _context.Usuarios_Notificar_RS.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Usuarios_Notificar_RS;
        }

        public async Task<int> AddVehiculo_RS(Vehiculo_RS vehiculo_rs)
        {
            var entity = new Entities.ResiduosSolidos.Vehiculo_RS
            {
                Placa = vehiculo_rs.Placa,
                Capacidad = vehiculo_rs.Capacidad,
                Estado = "ACT"
            };
            _context.Vehiculos_RS.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Vehiculo_RS;
        }
        public async Task<List<ACR_CMRS>> GetACR_CMRS(string filtro)
        {
            var entities = await _context.ACR_CMRs.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();

            var output = new List<ACR_CMRS>();
            foreach (var item in entities)
                output.Add(new ACR_CMRS(item.Id_ACR_CMRS,item.Descripcion,item.Estado));
            return output;
        }

        public async Task<ACR_CMRS> GetACR_CMRS(int id_acr_cmrs)
        {
            var entity = await _context.ACR_CMRs.FirstOrDefaultAsync(x => x.Id_ACR_CMRS.Equals(id_acr_cmrs));
            var output = new ACR_CMRS
            {
                Id_ACR_CMRS = entity.Id_ACR_CMRS,
                Descripcion = entity.Descripcion,
                Estado = entity.Estado
            };
            return output;
        }

        public async Task<List<Conductor_RS>> GetConductor_RS(string filtro)
        {
            var entities = await _context.Condutores_RS.Where(x => x.Nombres.Contains(filtro == null ? x.Nombres : filtro) || x.Apellidos.Contains(filtro == null ? x.Apellidos : filtro)).OrderBy(x => x.Nombres).ToListAsync();

            var output = new List<Conductor_RS>();
            foreach (var item in entities)
                output.Add(new Conductor_RS(item.Id_Conductor_RS,item.Nombres,item.Apellidos,item.NLicencia,item.Estado));
            return output;
        }

        public async Task<Conductor_RS> GetConductor_RS(int id_conductor)
        {
            var entity = await _context.Condutores_RS.FirstOrDefaultAsync(x => x.Id_Conductor_RS.Equals(id_conductor));
            var output = new Conductor_RS
            {
                Id_Conductor_RS = entity.Id_Conductor_RS,
                Nombres = entity.Nombres,
                Apellidos = entity.Apellidos,
                NLicencia = entity.NLicencia,
                Estado = entity.Estado
            };
            return output;
        }

        /*public async Task<int> AddUsuario(Usuario usuario)
        {
            var usuarioEntity = new Entities.Usuario
            {
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Email = usuario.Email,
                Id_Rol = usuario.IdRol,
                Id_Superior = usuario.IdSuperior.Value != 0 ? usuario.IdSuperior : null,
                usuario_red = usuario.UsuarioRed,
                Estado = "ACT"
            };
            _context.Usuarios.Add(usuarioEntity);
            await _context.SaveChangesAsync();
            return usuarioEntity.Id_Usuario;
        }*/

        public async Task<Empresa> GetEmpresa(int idEmpresa)
        {
            var empresa = await _context.Empresas.FirstOrDefaultAsync(x => x.Id_Empresa.Equals(idEmpresa));
            var output = new Empresa
            {
                IdEmpresa = empresa.Id_Empresa,
                RazonSocial = empresa.Razon_Social
            };
            return output;            
        }
               
        public async Task<List<Empresa>> GetEmpresas(string filtro)
        {
            var empresas = await _context.Empresas.Where(x=>x.Razon_Social.Contains(filtro == null ? x.Razon_Social : filtro)).OrderBy(x=>x.Razon_Social).ToListAsync();
            
            var output = new List<Empresa>();
            foreach (var item in empresas)
                output.Add(new Empresa(item.Id_Empresa, item.Razon_Social));
            return output;
        }

        public async Task<List<EO_RS>> GetEO_RS(string filtro)
        {
            var entities = await _context.EO_RSs.Where(x => x.Razon_Social.Contains(filtro == null ? x.Razon_Social : filtro)).OrderBy(x => x.Razon_Social).ToListAsync();

            var output = new List<EO_RS>();
            foreach (var item in entities)
                output.Add(new EO_RS(item.Id_EO_RS,item.Razon_Social,item.Registro,item.Fecha_Vencimiento_Registro,item.Estado));
            return output;
        }

        public async Task<EO_RS> GetEO_RS(int idEO_RS)
        {
            var entity = await _context.EO_RSs.FirstOrDefaultAsync(x => x.Id_EO_RS.Equals(idEO_RS));
            var output = new EO_RS
            {
                Id_EO_RS = entity.Id_EO_RS,
                Razon_Social = entity.Razon_Social,
                Fecha_Vencimiento_Registro = entity.Fecha_Vencimiento_Registro,
                Registro = entity.Registro,
                Estado = entity.Estado
            };
            return output;
        }

        public async Task<Gerencia> GetGerencia(int idGerencia)
        {
            var gerencia = await _context.Gerencias.FirstOrDefaultAsync(x => x.Id_Gerencia.Equals(idGerencia));
            var output = new Gerencia
            {
                IdGerencia = gerencia.Id_Gerencia,
                Descripcion = gerencia.Descripcion
            };
            return output;
        }
               
        public async Task<List<Gerencia>> GetGerencias(string filtro)
        {
            var gerencias = await _context.Gerencias.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x=>x.Descripcion).ToListAsync();
            var output = new List<Gerencia>();
            foreach (var item in gerencias)
                output.Add(new Gerencia(item.Id_Gerencia, item.Descripcion));
            return output;
        }

        public async Task<List<Motivo_Ingreso_Salida>> GetMotivo_Ingreso_Salida(string filtro)
        {
            var entities = await _context.Motivos_Ingreso_Salida.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();

            var output = new List<Motivo_Ingreso_Salida>();
            foreach (var item in entities)
                output.Add(new Motivo_Ingreso_Salida(item.Id_Motivo,item.Descripcion,item.Estado));
            return output;
        }

        public async Task<Motivo_Ingreso_Salida> GetMotivo_Ingreso_Salida(int id_motivo)
        {
            var entity = await _context.Motivos_Ingreso_Salida.FirstOrDefaultAsync(x => x.Id_Motivo.Equals(id_motivo));
            var output = new Motivo_Ingreso_Salida
            {
                Id_Motivo = entity.Id_Motivo,
                Descripcion = entity.Descripcion,
                Estado = entity.Estado
            };
            return output;
        }

        public async Task<List<Residuo_Solido>> GetResiduo_Solido(string filtro)
        {
            var entities = await _context.Residuos_Solidos.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();
            var tipos_residuo = await _context.Tipo_Residuos.ToListAsync();

            var output = new List<Residuo_Solido>();
            foreach (var item in entities)
            {
                var tipo_residuo = tipos_residuo.FirstOrDefault(x => x.Id_Tipo_Residuo.Equals(item.Id_Tipo_Residuo));
                output.Add(new Residuo_Solido(item.Id_Residuo_Solido,new Tipo_Residuo(tipo_residuo.Id_Tipo_Residuo,tipo_residuo.Descripcion,tipo_residuo.Estado),item.Descripcion,item.Estado));
            }
                
            return output;
        }

        public async Task<Residuo_Solido> GetResiduo_Solido(int id_residuo_solido)
        {
            var entity = await _context.Residuos_Solidos.FirstOrDefaultAsync(x => x.Id_Residuo_Solido.Equals(id_residuo_solido));
            var tipo = await _context.Tipo_Residuos.FirstOrDefaultAsync(x => x.Id_Tipo_Residuo.Equals(entity.Id_Tipo_Residuo));
            var output = new Residuo_Solido
            {
                Id_Residuo_Solido = entity.Id_Residuo_Solido,
                Descripcion = entity.Descripcion,
                Tipo_Residuo = new Tipo_Residuo(tipo.Id_Tipo_Residuo,tipo.Descripcion,tipo.Estado),
                Estado = entity.Estado
            };
            return output;
        }

        public async Task<List<Responsable_ARC_CRMS>> GetResponsable_ARC_CRMS(string filtro)
        {
            var entities = await _context.Responsables_ACR_CRM.Where(x => x.Nombres.Contains(filtro == null ? x.Nombres : filtro)|| x.Apellidos.Contains(filtro == null ? x.Apellidos : filtro)).OrderBy(x => x.Nombres).ToListAsync();

            var output = new List<Responsable_ARC_CRMS>();
            foreach (var item in entities)
                output.Add(new Responsable_ARC_CRMS(item.Id_Responsable_ARC_CRMS,item.Nombres,item.Apellidos,item.Estado));
            return output;
        }

        public async Task<Responsable_ARC_CRMS> GetResponsable_ARC_CRMS(int id_responsable)
        {
            var entity = await _context.Responsables_ACR_CRM.FirstOrDefaultAsync(x => x.Id_Responsable_ARC_CRMS.Equals(id_responsable));
            var output = new Responsable_ARC_CRMS
            {
                Id_Responsable_ACR_CRMS = entity.Id_Responsable_ARC_CRMS,
                Nombres = entity.Nombres,
                Apellidos = entity.Apellidos,
                Estado = entity.Estado
            };
            return output;
        }

        public async Task<TipoCondicion> GetTipoCondicion(int idTipoCondicion)
        {
            var tipoCondicion = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(idTipoCondicion));
            var output = new TipoCondicion
            {
                IdTipoCondicion = tipoCondicion.Id_Tipo_Condicion,
                Descripcion = tipoCondicion.Descripcion,
                Grupo = tipoCondicion.Grupo
            };
            return output;
        }
               
        public async Task<List<TipoCondicion>> GetTiposCondicion(string filtro)
        {
            var tiposCondicion = await _context.Tipos_Condicion.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro) || x.Grupo.Contains(filtro == null ? x.Grupo : filtro)).OrderBy(x=>x.Grupo).ToListAsync();
            var output = new List<TipoCondicion>();
            foreach (var item in tiposCondicion)
                output.Add(new TipoCondicion(item.Id_Tipo_Condicion,item.Descripcion,item.Grupo));
            return output;
        }
               
        public async Task<Usuario> GetUsuario(int idUsuario)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(idUsuario));
            var output = new Usuario
            {
               Nombres = usuario.Nombres,
               Apellidos = usuario.Apellidos,
               Email = usuario.Email,
               IdRol = usuario.Id_Rol,
               UsuarioRed = usuario.usuario_red,
               IdSuperior = usuario.Id_Superior,
               Estado = usuario.Estado
            };
            return output;
        }
               
        public async Task<List<Usuario>> GetUsuarios(string filtro)
        {
            var usuarios = await _context.Usuarios.Where(x =>
            x.Nombres.Contains(filtro == null ? x.Nombres : filtro) ||
            x.Apellidos.Contains(filtro == null ? x.Apellidos : filtro) ||
            x.usuario_red.Contains(filtro == null ? x.usuario_red : filtro)
            ).OrderBy(x=>x.Nombres)
            .ToListAsync();
            
            var output = new List<Usuario>();
            foreach (var item in usuarios)
                output.Add(new Usuario(item.Id_Usuario,item.Id_Rol,item.usuario_red,item.Nombres,item.Apellidos,item.Email,item.Id_Superior,item.Estado));
            return output;
        }

        public async Task<List<Usuario_Notificar_RS>> GetUsuario_Notificar_RS(string filtro)
        {
            var usuarios = await _context.Usuarios_Notificar_RS.Where(x =>
            x.Nombres.Contains(filtro == null ? x.Nombres : filtro) ||
            x.Apellidos.Contains(filtro == null ? x.Apellidos : filtro)
            ).OrderBy(x => x.Nombres)
            .ToListAsync();

            var output = new List<Usuario_Notificar_RS>();
            foreach (var item in usuarios)
                output.Add(new Usuario_Notificar_RS(item.Id_Usuarios_Notificar_RS, item.Id_Rol,  item.Nombres, item.Apellidos, item.usuario_red, item.Email, item.Estado));
            return output;
        }

        public async Task<Usuario_Notificar_RS> GetUsuario_Notificar_RS(int id_usuario_notificar)
        {
            var usuario = await _context.Usuarios_Notificar_RS.FirstOrDefaultAsync(x => x.Id_Usuarios_Notificar_RS.Equals(id_usuario_notificar));
            var output = new Usuario_Notificar_RS
            {
                Id_Usuario_Notificar_RS = usuario.Id_Usuarios_Notificar_RS,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Email = usuario.Email,
                Id_Rol = usuario.Id_Rol,
                usuario_red = usuario.usuario_red,
                Estado = usuario.Estado
            };
            return output;
        }

        public async Task<List<Vehiculo_RS>> GetVehiculo_RS(string filtro)
        {
            var entities = await _context.Vehiculos_RS.Where(x => x.Placa.Contains(filtro == null ? x.Placa : filtro)).OrderBy(x => x.Placa).ToListAsync();

            var output = new List<Vehiculo_RS>();
            foreach (var item in entities)
                output.Add(new Vehiculo_RS(item.Id_Vehiculo_RS,item.Placa,item.Capacidad, item.Estado));
            return output;
        }

        public async Task<Vehiculo_RS> GetVehiculo_RS(int id_vehiculo_rs)
        {
            var entity = await _context.Vehiculos_RS.FirstOrDefaultAsync(x => x.Id_Vehiculo_RS.Equals(id_vehiculo_rs));
            var output = new Vehiculo_RS
            {
                Id_Vehiculo_RS = entity.Id_Vehiculo_RS,
                Placa = entity.Placa,
                Capacidad = entity.Capacidad,
                Estado = entity.Estado
            };
            return output;
        }

        public async Task QuitarUsuario_Notificar_RS(int id_usuario_notificar)
        {
            var entity = await _context.Usuarios_Notificar_RS.FirstOrDefaultAsync(x=>x.Id_Usuarios_Notificar_RS.Equals(id_usuario_notificar));
            _context.Usuarios_Notificar_RS.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateACR_CMRS(ACR_CMRS acr_cmrs)
        {
            var entity = await _context.ACR_CMRs.FirstOrDefaultAsync(x => x.Id_ACR_CMRS.Equals(acr_cmrs.Id_ACR_CMRS));
            entity.Descripcion = acr_cmrs.Descripcion;
            entity.Estado = acr_cmrs.Estado;
            _context.ACR_CMRs.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateConductor_RS(Conductor_RS conductor_RS)
        {
            var entity = await _context.Condutores_RS.FirstOrDefaultAsync(x => x.Id_Conductor_RS.Equals(conductor_RS.Id_Conductor_RS));
            entity.Nombres = conductor_RS.Nombres;
            entity.Apellidos = conductor_RS.Apellidos;
            entity.NLicencia = conductor_RS.NLicencia;
            entity.Estado = conductor_RS.Estado;
            _context.Condutores_RS.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmpresa(Empresa empresa)
        {
            var empresaEntity = await _context.Empresas.FirstOrDefaultAsync(x => x.Id_Empresa.Equals(empresa.IdEmpresa));
            empresaEntity.Razon_Social = empresa.RazonSocial;
            _context.Empresas.Update(empresaEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEO_RS(EO_RS eors)
        {
            var entity = await _context.EO_RSs.FirstOrDefaultAsync(x => x.Id_EO_RS.Equals(eors.Id_EO_RS));
            entity.Razon_Social = eors.Razon_Social;
            entity.Registro = eors.Registro;
            entity.Fecha_Vencimiento_Registro = eors.Fecha_Vencimiento_Registro;
            entity.Estado = eors.Estado;
            _context.EO_RSs.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGerencia(Gerencia gerencia)
        {
            var gerenciaEntity = await _context.Gerencias.FirstOrDefaultAsync(x => x.Id_Gerencia.Equals(gerencia.IdGerencia));
            gerenciaEntity.Descripcion = gerencia.Descripcion;
            _context.Gerencias.Update(gerenciaEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMotivo_Ingreso_Salida(Motivo_Ingreso_Salida motivo)
        {
            var entity = await _context.Motivos_Ingreso_Salida.FirstOrDefaultAsync(x => x.Id_Motivo.Equals(motivo.Id_Motivo));
            entity.Descripcion = motivo.Descripcion;
            entity.Estado = motivo.Estado;
            _context.Motivos_Ingreso_Salida.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateResiduo_Solido(Residuo_Solido residuo_solido)
        {
            var entity = await _context.Residuos_Solidos.FirstOrDefaultAsync(x => x.Id_Residuo_Solido.Equals(residuo_solido.Id_Residuo_Solido));
            entity.Descripcion = residuo_solido.Descripcion;
            entity.Id_Tipo_Residuo = residuo_solido.Tipo_Residuo.Id_Tipo_Residuo;
            entity.Estado = residuo_solido.Estado;
            _context.Residuos_Solidos.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateResponsable_ARC_CRMS(Responsable_ARC_CRMS responsable)
        {
            var entity = await _context.Responsables_ACR_CRM.FirstOrDefaultAsync(x => x.Id_Responsable_ARC_CRMS.Equals(responsable.Id_Responsable_ACR_CRMS));
            entity.Nombres = responsable.Nombres;
            entity.Apellidos = responsable.Apellidos;
            entity.Estado = responsable.Estado;
            _context.Responsables_ACR_CRM.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTipoCondicion(TipoCondicion tipoCondicion)
        {
            var tipoCondicionEntity = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(tipoCondicion.IdTipoCondicion));
            tipoCondicionEntity.Descripcion = tipoCondicion.Descripcion;
            tipoCondicionEntity.Grupo = tipoCondicion.Grupo;
            _context.Tipos_Condicion.Update(tipoCondicionEntity);
            await _context.SaveChangesAsync();
        }
               
        public async Task UpdateUsuario(Usuario usuario)
        {
            var usuarioEntity = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(usuario.IdUsuario));
            usuarioEntity.Nombres = usuario.Nombres;
            usuarioEntity.Apellidos = usuario.Apellidos;
            usuarioEntity.Email = usuario.Email;
            usuarioEntity.Id_Rol = usuario.IdRol;
            usuarioEntity.Id_Superior = usuario.IdSuperior.Value != 0 ? usuario.IdSuperior : null;
            usuarioEntity.usuario_red = usuario.UsuarioRed;
            usuarioEntity.Estado = usuario.Estado;

            _context.Usuarios.Update(usuarioEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUsuario_Notificar_RS(Usuario_Notificar_RS usuario_notifcar)
        {
            var usuarioEntity = await _context.Usuarios_Notificar_RS.FirstOrDefaultAsync(x => x.Id_Usuarios_Notificar_RS.Equals(usuario_notifcar.Id_Usuario_Notificar_RS));
            usuarioEntity.Nombres = usuario_notifcar.Nombres;
            usuarioEntity.Apellidos = usuario_notifcar.Apellidos;
            usuarioEntity.Email = usuario_notifcar.Email;
            usuarioEntity.Id_Rol = usuario_notifcar.Id_Rol;
            usuarioEntity.usuario_red = usuario_notifcar.usuario_red;
            usuarioEntity.Estado = usuario_notifcar.Estado;

            _context.Usuarios_Notificar_RS.Update(usuarioEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVehiculo_RS(Vehiculo_RS vehiculo_rs)
        {
            var entity = await _context.Vehiculos_RS.FirstOrDefaultAsync(x => x.Id_Vehiculo_RS.Equals(vehiculo_rs.Id_Vehiculo_RS));
            entity.Placa = vehiculo_rs.Placa;
            entity.Capacidad = vehiculo_rs.Capacidad;
            entity.Estado = vehiculo_rs.Estado;
            _context.Vehiculos_RS.Update(entity);
            await _context.SaveChangesAsync();
        }

        //Tb_Usuario_Notificar_Incidente
        public async Task<List<Usuario_Notificar_Incidente>> GetUsuario_Notificar_Incidente(string filtro){
            var usuarios = await _context.Usuario_Notificar_Incidente.Where(x =>
            x.Nombres.Contains(filtro == null ? x.Nombres : filtro) ||
            x.Apellidos.Contains(filtro == null ? x.Apellidos : filtro)
            ).OrderBy(x => x.Nombres)
            .ToListAsync();

            var output = new List<Usuario_Notificar_Incidente>();
            foreach (var item in usuarios)
                output.Add(new Usuario_Notificar_Incidente(item.Id_Usuarios_Notificar_Incidente, item.Id_Rol, item.Nombres, item.Apellidos, item.usuario_red, item.Email, item.Estado));
            return output;
        }
        public async Task<Usuario_Notificar_Incidente> GetUsuario_Notificar_Incidente(int id_usuario_notificar){
            var usuario = await _context.Usuario_Notificar_Incidente.FirstOrDefaultAsync(x => x.Id_Usuarios_Notificar_Incidente.Equals(id_usuario_notificar));
            var output = new Usuario_Notificar_Incidente
            {
                Id_Usuario_Notificar_Incidente = usuario.Id_Usuarios_Notificar_Incidente,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Email = usuario.Email,
                Id_Rol = usuario.Id_Rol,
                Usuario_Red = usuario.usuario_red,
                Estado = usuario.Estado
            };
            return output;
        }
        public async Task UpdateUsuario_Notificar_Incidente(Usuario_Notificar_Incidente usuario_notifcar){
            var usuarioEntity = await _context.Usuario_Notificar_Incidente.FirstOrDefaultAsync(x => x.Id_Usuarios_Notificar_Incidente.Equals(usuario_notifcar.Id_Usuario_Notificar_Incidente));
            usuarioEntity.Nombres = usuario_notifcar.Nombres;
            usuarioEntity.Apellidos = usuario_notifcar.Apellidos;
            usuarioEntity.Email = usuario_notifcar.Email;
            usuarioEntity.Id_Rol = usuario_notifcar.Id_Rol;
            usuarioEntity.usuario_red = usuario_notifcar.Usuario_Red;
            usuarioEntity.Estado = usuario_notifcar.Estado;

            _context.Usuario_Notificar_Incidente.Update(usuarioEntity);
            await _context.SaveChangesAsync();
        }
        public async Task QuitarUsuario_Notificar_Incidente(int id_usuario_notificar){
            var entity = await _context.Usuario_Notificar_Incidente.FirstOrDefaultAsync(x => x.Id_Usuarios_Notificar_Incidente.Equals(id_usuario_notificar));
            _context.Usuario_Notificar_Incidente.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<int> AddUsuario_Notificar_Incidente(int Id_Usuario_AD){
            var temp_entity = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(Id_Usuario_AD));
            var entity = new Entities.Incidentes.Items.Usuario_Notificar_Incidente
            {
                Nombres = temp_entity.Nombres,
                Apellidos = temp_entity.Apellidos,
                Email = temp_entity.Email,
                Estado = "ACT",
                Id_Rol = temp_entity.Id_Rol,
                usuario_red = temp_entity.usuario_red
            };
            _context.Usuario_Notificar_Incidente.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Usuarios_Notificar_Incidente;
        }

        //Tb_Componente_Ambiental
        public async Task<List<Componente_Ambiental>> GetComponente_Ambientales(string filtro)
        {
            var entities = await _context.Componente_Ambiental.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();

            var output = new List<Componente_Ambiental>();
            foreach (var item in entities)
            {
                output.Add(new Componente_Ambiental(item.Id_Componente_Ambiental,item.Descripcion,item.Estado));
            }

            return output;
        }
        public async Task<Componente_Ambiental> GetComponente_Ambiental(int id_componente)
        {
            var entity = await _context.Componente_Ambiental.FirstOrDefaultAsync(x => x.Id_Componente_Ambiental.Equals(id_componente));
            var output = new Componente_Ambiental
            {
                Id_Componente_Ambiental = entity.Id_Componente_Ambiental,
                Descripcion = entity.Descripcion,
                Estado = entity.Estado
            };
            return output;
        }
        public async Task<int> AddComponente_Ambiental(Componente_Ambiental componente)
        {
            var entity = new Entities.Incidentes.Items.Componente_Ambiental
            {
                Descripcion = componente.Descripcion,
                Estado = "ACT"
            };
            _context.Componente_Ambiental.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Componente_Ambiental;
        }
        public async Task UpdateComponente_Ambiental(Componente_Ambiental componente)
        {
            var entity = await _context.Componente_Ambiental.FirstOrDefaultAsync(x => x.Id_Componente_Ambiental.Equals(componente.Id_Componente_Ambiental));
            entity.Descripcion = componente.Descripcion;
            entity.Estado = componente.Estado;
            _context.Componente_Ambiental.Update(entity);
            await _context.SaveChangesAsync();
        }

        //Tb_Tipo_Incidente
        public async Task<List<Tipo_Incidente>> GetTipos_Incidente(string filtro)
        {
            var entities = await _context.Tipo_Incidente.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();

            var output = new List<Tipo_Incidente>();
            foreach (var item in entities)
            {
                output.Add(new Tipo_Incidente(item.Id_Tipo_Incidente, item.Descripcion, item.Estado));
            }

            return output;
        }
        public async Task<Tipo_Incidente> GetTipo_Incidente(int id_tipo)
        {
            var entity = await _context.Tipo_Incidente.FirstOrDefaultAsync(x => x.Id_Tipo_Incidente.Equals(id_tipo));
            var output = new Tipo_Incidente
            {
                Id_Tipo_Incidente = entity.Id_Tipo_Incidente,
                Descripcion = entity.Descripcion,
                Estado = entity.Estado
            };
            return output;
        }
        public async Task<int> AddTipo_Incidente(Tipo_Incidente tipo)
        {
            var entity = new Entities.Incidentes.Items.Tipo_Incidente
            {
                Descripcion = tipo.Descripcion,
                Estado = "ACT"
            };
            _context.Tipo_Incidente.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Tipo_Incidente;
        }
        public async Task UpdateTipo_Incidente(Tipo_Incidente tipo)
        {
            var entity = await _context.Tipo_Incidente.FirstOrDefaultAsync(x => x.Id_Tipo_Incidente.Equals(tipo.Id_Tipo_Incidente));
            entity.Descripcion = tipo.Descripcion;
            entity.Estado = tipo.Estado;
            _context.Tipo_Incidente.Update(entity);
            await _context.SaveChangesAsync();
        }

        //Tb_Unidad_Medida
        public async Task<List<Unidad_Medida>> GetUnidad_Medida(string filtro)
        {
            var entities = await _context.Unidad_Medida.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();

            var output = new List<Unidad_Medida>();
            foreach (var item in entities)
            {
                output.Add(new Unidad_Medida(item.Id_Unidad_Medida, item.Descripcion,item.Tipo, item.Estado));
            }
            return output;
        }
        public async Task<Unidad_Medida> GetUnidad_Medida(int id_unidad)
        {
            var entity = await _context.Unidad_Medida.FirstOrDefaultAsync(x => x.Id_Unidad_Medida.Equals(id_unidad));
            var output = new Unidad_Medida
            {
                Id_Unidad_Medida = entity.Id_Unidad_Medida,
                Descripcion = entity.Descripcion,
                Tipo = entity.Tipo,
                Estado = entity.Estado
            };
            return output;
        }
        public async Task<int> AddUnidad_Medida(Unidad_Medida unidad)
        {
            var entity = new Entities.Incidentes.Items.Unidad_Medida
            {
                Descripcion = unidad.Descripcion,
                Tipo = unidad.Tipo,
                Estado = "ACT"
            };
            _context.Unidad_Medida.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Unidad_Medida;
        }
        public async Task UpdateUnidad_Medida(Unidad_Medida unidad)
        {
            var entity = await _context.Unidad_Medida.FirstOrDefaultAsync(x => x.Id_Unidad_Medida.Equals(unidad.Id_Unidad_Medida));
            entity.Descripcion = unidad.Descripcion;
            entity.Tipo = unidad.Tipo;
            entity.Estado = unidad.Estado;
            _context.Unidad_Medida.Update(entity);
            await _context.SaveChangesAsync();
        }

        //Tb_Nivel_Incidente
        public async Task<List<Nivel_Incidente>> GetNivel_Incidente(string filtro)
        {
            var entities = await _context.Nivel_Incidente.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();

            var output = new List<Nivel_Incidente>();
            foreach (var item in entities)
            {
                output.Add(new Nivel_Incidente(item.Id_Nivel_Incidente, item.Descripcion, item.Estado));
            }

            return output;
        }
        public async Task<Nivel_Incidente> GetNivel_Incidente(int id_nivel)
        {
            var entity = await _context.Nivel_Incidente.FirstOrDefaultAsync(x => x.Id_Nivel_Incidente.Equals(id_nivel));
            var output = new Nivel_Incidente
            {
                Id_Nivel_Incidente = entity.Id_Nivel_Incidente,
                Descripcion = entity.Descripcion,
                Estado = entity.Estado
            };
            return output;
        }
        public async Task<int> AddNivel_Incidente(Nivel_Incidente nivel)
        {
            var entity = new Entities.Incidentes.Items.Nivel_Incidente
            {
                Descripcion = nivel.Descripcion,
                Estado = "ACT"
            };
            _context.Nivel_Incidente.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Nivel_Incidente;
        }
        public async Task UpdateNivel_Incidente(Nivel_Incidente nivel)
        {
            var entity = await _context.Nivel_Incidente.FirstOrDefaultAsync(x => x.Id_Nivel_Incidente.Equals(nivel.Id_Nivel_Incidente));
            entity.Descripcion = nivel.Descripcion;
            entity.Estado = nivel.Estado;
            _context.Nivel_Incidente.Update(entity);
            await _context.SaveChangesAsync();
        }

        //Tb_Producto_Incidente
        public async Task<List<Producto_Incidente>> GetProducto_Incidente(string filtro)
        {
            var entities = await _context.Producto_Incidente.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();

            var output = new List<Producto_Incidente>();
            foreach (var item in entities)
            {
                output.Add(new Producto_Incidente(item.Id_Producto_Incidente, item.Descripcion, item.Estado));
            }

            return output;
        }
        public async Task<Producto_Incidente> GetProducto_Incidente(int id_producto)
        {
            var entity = await _context.Producto_Incidente.FirstOrDefaultAsync(x => x.Id_Producto_Incidente.Equals(id_producto));
            var output = new Producto_Incidente
            {
                Id_Producto_Incidente = entity.Id_Producto_Incidente,
                Descripcion = entity.Descripcion,
                Estado = entity.Estado
            };
            return output;
        }
        public async Task<int> AddProducto_Incidente(Producto_Incidente producto)
        {
            var entity = new Entities.Incidentes.Items.Producto_Incidente
            {
                Descripcion = producto.Descripcion,
                Estado = "ACT"
            };
            _context.Producto_Incidente.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Producto_Incidente;
        }
        public async Task UpdateProducto_Incidente(Producto_Incidente producto)
        {
            var entity = await _context.Producto_Incidente.FirstOrDefaultAsync(x => x.Id_Producto_Incidente.Equals(producto.Id_Producto_Incidente));
            entity.Descripcion = producto.Descripcion;
            entity.Estado = producto.Estado;
            _context.Producto_Incidente.Update(entity);
            await _context.SaveChangesAsync();
        }

        //Tb_Tipo_Producto_Incidente
        public async Task<List<Tipo_Producto_Incidente>> GetTipo_Producto_Incidente(string filtro)
        {
            var entities = await _context.Tipo_Producto_Incidente.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();

            var output = new List<Tipo_Producto_Incidente>();
            foreach (var item in entities)
            {
                output.Add(new Tipo_Producto_Incidente(item.Id_Tipo_Producto_Incidente, item.Descripcion, item.Estado));
            }

            return output;
        }
        public async Task<Tipo_Producto_Incidente> GetTipo_Producto_Incidente(int id_tipo)
        {
            var entity = await _context.Tipo_Producto_Incidente.FirstOrDefaultAsync(x => x.Id_Tipo_Producto_Incidente.Equals(id_tipo));
            var output = new Tipo_Producto_Incidente
            {
                Id_Tipo_Producto_Incidente = entity.Id_Tipo_Producto_Incidente,
                Descripcion = entity.Descripcion,
                Estado = entity.Estado
            };
            return output;
        }
        public async Task<int> AddTipo_Producto_Incidente(Tipo_Producto_Incidente tipo)
        {
            var entity = new Entities.Incidentes.Items.Tipo_Producto_Incidente
            {
                Descripcion = tipo.Descripcion,
                Estado = "ACT"
            };
            _context.Tipo_Producto_Incidente.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Tipo_Producto_Incidente;
        }
        public async Task UpdateTipo_Producto_Incidente(Tipo_Producto_Incidente tipo)
        {
            var entity = await _context.Tipo_Producto_Incidente.FirstOrDefaultAsync(x => x.Id_Tipo_Producto_Incidente.Equals(tipo.Id_Tipo_Producto_Incidente));
            entity.Descripcion = tipo.Descripcion;
            entity.Estado = tipo.Estado;
            _context.Tipo_Producto_Incidente.Update(entity);
            await _context.SaveChangesAsync();
        }

        //Tb_Estado_Equipo_Incidente
        public async Task<List<Estado_Equipo_Incidente>> GetEstado_Equipo_Incidente(string filtro)
        {
            var entities = await _context.Estado_Equipo_Incidente.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();

            var output = new List<Estado_Equipo_Incidente>();
            foreach (var item in entities)
            {
                output.Add(new Estado_Equipo_Incidente(item.Id_Estado_Equipo_Incidente, item.Descripcion, item.Estado));
            }

            return output;
        }
        public async Task<Estado_Equipo_Incidente> GetEstado_Equipo_Incidente(int id_tipo)
        {
            var entity = await _context.Estado_Equipo_Incidente.FirstOrDefaultAsync(x => x.Id_Estado_Equipo_Incidente.Equals(id_tipo));
            var output = new Estado_Equipo_Incidente
            {
                Id_Estado_Equipo_Incidente = entity.Id_Estado_Equipo_Incidente,
                Descripcion = entity.Descripcion,
                Estado = entity.Estado
            };
            return output;
        }
        public async Task<int> AddEstado_Equipo_Incidente(Estado_Equipo_Incidente estado)
        {
            var entity = new Entities.Incidentes.Items.Estado_Equipo_Incidente
            {
                Descripcion = estado.Descripcion,
                Estado = "ACT"
            };
            _context.Estado_Equipo_Incidente.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Estado_Equipo_Incidente;
        }
        public async Task UpdateEstado_Equipo_Incidente(Estado_Equipo_Incidente estado)
        {
            var entity = await _context.Estado_Equipo_Incidente.FirstOrDefaultAsync(x => x.Id_Estado_Equipo_Incidente.Equals(estado.Id_Estado_Equipo_Incidente));
            entity.Descripcion = estado.Descripcion;
            entity.Estado = estado.Estado;
            _context.Estado_Equipo_Incidente.Update(entity);
            await _context.SaveChangesAsync();
        }

        //Tb_Lugar_Disposicion
        public async Task<List<Lugar_Disposicion>> GetLugar_Disposicion(string filtro)
        {
            var entities = await _context.Lugar_Disposicion.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();

            var output = new List<Lugar_Disposicion>();
            foreach (var item in entities)
            {
                output.Add(new Lugar_Disposicion(item.Id_Lugar_Disposicion, item.Descripcion, item.Estado));
            }

            return output;
        }
        public async Task<Lugar_Disposicion> GetLugar_Disposicion(int id_lugar)
        {
            var entity = await _context.Lugar_Disposicion.FirstOrDefaultAsync(x => x.Id_Lugar_Disposicion.Equals(id_lugar));
            var output = new Lugar_Disposicion
            {
                Id_Lugar_Disposicion = entity.Id_Lugar_Disposicion,
                Descripcion = entity.Descripcion,
                Estado = entity.Estado
            };
            return output;
        }
        public async Task<int> AddLugar_Disposicion(Lugar_Disposicion estado)
        {
            var entity = new Entities.Incidentes.Items.Lugar_Disposicion
            {
                Descripcion = estado.Descripcion,
                Estado = "ACT"
            };
            _context.Lugar_Disposicion.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Lugar_Disposicion;
        }
        public async Task UpdateLugar_Disposicion(Lugar_Disposicion estado)
        {
            var entity = await _context.Lugar_Disposicion.FirstOrDefaultAsync(x => x.Id_Lugar_Disposicion.Equals(estado.Id_Lugar_Disposicion));
            entity.Descripcion = estado.Descripcion;
            entity.Estado = estado.Estado;
            _context.Lugar_Disposicion.Update(entity);
            await _context.SaveChangesAsync();
        }

        //Tb_Tipo_Agua
        public async Task<List<Tipo_Agua>> GetTipo_Agua(string filtro)
        {
            var entities = await _context.Tipo_Agua.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();

            var output = new List<Tipo_Agua>();
            foreach (var item in entities)
            {
                output.Add(new Tipo_Agua(item.Id_Tipo_Agua, item.Descripcion, item.Estado));
            }

            return output;
        }
        public async Task<Tipo_Agua> GetTipo_Agua(int id_tipo)
        {
            var entity = await _context.Tipo_Agua.FirstOrDefaultAsync(x => x.Id_Tipo_Agua.Equals(id_tipo));
            var output = new Tipo_Agua
            {
                Id_Tipo_Agua = entity.Id_Tipo_Agua,
                Descripcion = entity.Descripcion,
                Estado = entity.Estado
            };
            return output;
        }
        public async Task<int> AddTipo_Agua(Tipo_Agua tipo)
        {
            var entity = new Entities.Incidentes.Items.Tipo_Agua
            {
                Descripcion = tipo.Descripcion,
                Estado = "ACT"
            };
            _context.Tipo_Agua.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Tipo_Agua;
        }
        public async Task UpdateTipo_Agua(Tipo_Agua tipo)
        {
            var entity = await _context.Tipo_Agua.FirstOrDefaultAsync(x => x.Id_Tipo_Agua.Equals(tipo.Id_Tipo_Agua));
            entity.Descripcion = tipo.Descripcion;
            entity.Estado = tipo.Estado;
            _context.Tipo_Agua.Update(entity);
            await _context.SaveChangesAsync();
        }

        //Tb_Tipo_Suelo
        public async Task<List<Tipo_Suelo>> GetTipo_Suelo(string filtro)
        {
            var entities = await _context.Tipo_Suelo.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();

            var output = new List<Tipo_Suelo>();
            foreach (var item in entities)
            {
                output.Add(new Tipo_Suelo(item.Id_Tipo_Suelo, item.Descripcion, item.Estado));
            }

            return output;
        }
        public async Task<Tipo_Suelo> GetTipo_Suelo(int id_tipo)
        {
            var entity = await _context.Tipo_Suelo.FirstOrDefaultAsync(x => x.Id_Tipo_Suelo.Equals(id_tipo));
            var output = new Tipo_Suelo
            {
                Id_Tipo_Suelo = entity.Id_Tipo_Suelo,
                Descripcion = entity.Descripcion,
                Estado = entity.Estado
            };
            return output;
        }
        public async Task<int> AddTipo_Suelo(Tipo_Suelo tipo)
        {
            var entity = new Entities.Incidentes.Items.Tipo_Suelo
            {
                Descripcion = tipo.Descripcion,
                Estado = "ACT"
            };
            _context.Tipo_Suelo.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Tipo_Suelo;
        }
        public async Task UpdateTipo_Suelo(Tipo_Suelo tipo)
        {
            var entity = await _context.Tipo_Suelo.FirstOrDefaultAsync(x => x.Id_Tipo_Suelo.Equals(tipo.Id_Tipo_Suelo));
            entity.Descripcion = tipo.Descripcion;
            entity.Estado = tipo.Estado;
            _context.Tipo_Suelo.Update(entity);
            await _context.SaveChangesAsync();
        }

        //Tb_Especie_Flora
        public async Task<List<Especie_Flora>> GetEspecie_Flora(string filtro)
        {
            var entities = await _context.Especie_Flora.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();

            var output = new List<Especie_Flora>();
            foreach (var item in entities)
            {
                output.Add(new Especie_Flora(item.Id_Especie_Flora, item.Descripcion, item.Estado));
            }

            return output;
        }
        public async Task<Especie_Flora> GetEspecie_Flora(int id_tipo)
        {
            var entity = await _context.Especie_Flora.FirstOrDefaultAsync(x => x.Id_Especie_Flora.Equals(id_tipo));
            var output = new Especie_Flora
            {
                Id_Especie_Flora = entity.Id_Especie_Flora,
                Descripcion = entity.Descripcion,
                Estado = entity.Estado
            };
            return output;
        }
        public async Task<int> AddEspecie_Flora(Especie_Flora tipo)
        {
            var entity = new Entities.Incidentes.Items.Especie_Flora
            {
                Descripcion = tipo.Descripcion,
                Estado = "ACT"
            };
            _context.Especie_Flora.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Especie_Flora;
        }
        public async Task UpdateEspecie_Flora(Especie_Flora tipo)
        {
            var entity = await _context.Especie_Flora.FirstOrDefaultAsync(x => x.Id_Especie_Flora.Equals(tipo.Id_Especie_Flora));
            entity.Descripcion = tipo.Descripcion;
            entity.Estado = tipo.Estado;
            _context.Especie_Flora.Update(entity);
            await _context.SaveChangesAsync();
        }

        //Tb_Especie_Fauna
        public async Task<List<Especie_Fauna>> GetEspecie_Fauna(string filtro)
        {
            var entities = await _context.Especie_Fauna.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();

            var output = new List<Especie_Fauna>();
            foreach (var item in entities)
            {
                output.Add(new Especie_Fauna(item.Id_Especie_Fauna, item.Descripcion, item.Estado));
            }

            return output;
        }
        public async Task<Especie_Fauna> GetEspecie_Fauna(int id_tipo)
        {
            var entity = await _context.Especie_Fauna.FirstOrDefaultAsync(x => x.Id_Especie_Fauna.Equals(id_tipo));
            var output = new Especie_Fauna
            {
                Id_Especie_Fauna = entity.Id_Especie_Fauna,
                Descripcion = entity.Descripcion,
                Estado = entity.Estado
            };
            return output;
        }
        public async Task<int> AddEspecie_Fauna(Especie_Fauna tipo)
        {
            var entity = new Entities.Incidentes.Items.Especie_Fauna
            {
                Descripcion = tipo.Descripcion,
                Estado = "ACT"
            };
            _context.Especie_Fauna.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Especie_Fauna;
        }
        public async Task UpdateEspecie_Fauna(Especie_Fauna tipo)
        {
            var entity = await _context.Especie_Fauna.FirstOrDefaultAsync(x => x.Id_Especie_Fauna.Equals(tipo.Id_Especie_Fauna));
            entity.Descripcion = tipo.Descripcion;
            entity.Estado = tipo.Estado;
            _context.Especie_Fauna.Update(entity);
            await _context.SaveChangesAsync();
        }
        //Tb_Factor_Causa_Incidente
        public async Task<List<Factor_Causa_Incidente>> GetFactor_Causa_Incidente(string filtro)
        {
            var entities = await _context.Factor_Causa_Incidente.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();

            var output = new List<Factor_Causa_Incidente>();
            foreach (var item in entities)
            {
                output.Add(new Factor_Causa_Incidente(item.Id_Factor_Causa_Incidente, item.Descripcion,item.Tipo_Factor, item.Estado));
            }

            return output;
        }
        public async Task<Factor_Causa_Incidente> GetFactor_Causa_Incidente(int id_factor)
        {
            var entity = await _context.Factor_Causa_Incidente.FirstOrDefaultAsync(x => x.Id_Factor_Causa_Incidente.Equals(id_factor));
            var output = new Factor_Causa_Incidente
            {
                Id_Factor_Causa_Incidente = entity.Id_Factor_Causa_Incidente,
                Descripcion = entity.Descripcion,
                Tipo_Factor = entity.Tipo_Factor,
                Estado = entity.Estado
            };
            return output;
        }
        public async Task<int> AddFactor_Causa_Incidente(Factor_Causa_Incidente factor)
        {
            var entity = new Entities.Incidentes.InvestigacionFinal.Factor_Causa_Incidente
            {
                Descripcion = factor.Descripcion,
                Tipo_Factor = factor.Tipo_Factor,
                Estado = "ACT"
            };
            _context.Factor_Causa_Incidente.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Factor_Causa_Incidente;
        }
        public async Task UpdateFactor_Causa_Incidente(Factor_Causa_Incidente factor)
        {
            var entity = await _context.Factor_Causa_Incidente.FirstOrDefaultAsync(x => x.Id_Factor_Causa_Incidente.Equals(factor.Id_Factor_Causa_Incidente));
            entity.Descripcion = factor.Descripcion;
            entity.Tipo_Factor = factor.Tipo_Factor;
            entity.Estado = factor.Estado;
            _context.Factor_Causa_Incidente.Update(entity);
            await _context.SaveChangesAsync();
        }
        //Tb_Tipo_Fauna
        public async Task<List<TipoFauna>> GetTiposFauna(string filtro)
        {
            var entities = await _context.Tipos_Fauna.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();

            var output = new List<TipoFauna>();
            foreach (var item in entities)
            {
                output.Add(new TipoFauna { 
                    IdTipoFauna = item.Id_Tipo_Fauna,
                    Descripcion = item.Descripcion, 
                    Estado = item.Estado 
                });
            }

            return output;
        }
        public async Task<TipoFauna> GetTipoFauna(int id_tipo_fauna)
        {
            var entity = await _context.Tipos_Fauna.FirstOrDefaultAsync(x => x.Id_Tipo_Fauna.Equals(id_tipo_fauna));
            var output = new TipoFauna
            {
                IdTipoFauna = entity.Id_Tipo_Fauna,
                Descripcion = entity.Descripcion,
                Estado = entity.Estado
            };
            return output;
        }
        public async Task<int> AddTipoFauna(TipoFauna tipoFauna)
        {
            var entity = new Entities.AvistamientoFauna.Tipo_Fauna
            {
                Descripcion = tipoFauna.Descripcion,
                Estado = "ACT"
            };
            _context.Tipos_Fauna.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Tipo_Fauna ;
        }
        public async Task UpdateTipoFauna(TipoFauna tipoFauna)
        {
            var entity = await _context.Tipos_Fauna.FirstOrDefaultAsync(x => x.Id_Tipo_Fauna.Equals(tipoFauna.IdTipoFauna));
            entity.Descripcion = tipoFauna.Descripcion;
            entity.Estado = tipoFauna.Estado;
            _context.Tipos_Fauna.Update(entity);
            await _context.SaveChangesAsync();
        }

        //Tb_Nombre_Comun_Fauna
        public async Task<List<NombreComunFauna>> GetNombreComun(string filtro)
        {
            var entities = await _context.Nombres_Comunes_Fauna.Where(x => x.Descripcion.Contains(filtro == null ? x.Descripcion : filtro)).OrderBy(x => x.Descripcion).ToListAsync();
            var tiposFauna = await _context.Tipos_Fauna.ToListAsync();

            var output = new List<NombreComunFauna>();
            foreach (var item in entities)
            {
                var tipoFauna = tiposFauna.FirstOrDefault(x => x.Id_Tipo_Fauna.Equals(item.Id_Tipo_Fauna));
                output.Add(new NombreComunFauna
                {
                    IdNombreComunFauna = item.Id_Nombre_Comun_Fauna,
                    Descripcion = item.Descripcion,
                    Foto = item.Foto,
                    TipoFauna = new TipoFauna
                    {
                        IdTipoFauna = tipoFauna.Id_Tipo_Fauna,
                        Descripcion = tipoFauna.Descripcion,
                        Estado = tipoFauna.Estado
                    },
                    Estado = item.Estado
                });
            }

            return output;
        }
        public async Task<NombreComunFauna> GetNombreComun(int id_nombre_comun)
        {
            var entity = await _context.Nombres_Comunes_Fauna.FirstOrDefaultAsync(x => x.Id_Nombre_Comun_Fauna.Equals(id_nombre_comun));
            var tipoFauna = await _context.Tipos_Fauna.FirstOrDefaultAsync(x => x.Id_Tipo_Fauna.Equals(entity.Id_Tipo_Fauna));
            var output = new NombreComunFauna
            {
                IdNombreComunFauna = entity.Id_Nombre_Comun_Fauna,
                Descripcion = entity.Descripcion,
                Foto = entity.Foto,
                TipoFauna = new TipoFauna
                {
                    IdTipoFauna = tipoFauna.Id_Tipo_Fauna,
                    Descripcion = tipoFauna.Descripcion,
                    Estado = tipoFauna.Estado
                },
                Estado = entity.Estado
            };
            return output;
        }
        public async Task<int> AddNombreComun(NombreComunFauna nombreComun)
        {
            var entity = new Entities.AvistamientoFauna.Nombre_Comun_Fauna
            {
                Descripcion = nombreComun.Descripcion,
                Foto = nombreComun.Foto,
                Id_Tipo_Fauna = nombreComun.TipoFauna.IdTipoFauna,
                Estado = "ACT"
            };
            _context.Nombres_Comunes_Fauna.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Nombre_Comun_Fauna;
        }
        public async Task UpdateNombreComun(NombreComunFauna nombreComun)
        {
            var entity = await _context.Nombres_Comunes_Fauna.FirstOrDefaultAsync(x => x.Id_Nombre_Comun_Fauna.Equals(nombreComun.IdNombreComunFauna));
            entity.Descripcion = nombreComun.Descripcion;
            entity.Foto = nombreComun.Foto;
            entity.Id_Tipo_Fauna = nombreComun.TipoFauna.IdTipoFauna;
            entity.Estado = nombreComun.Estado;
            _context.Nombres_Comunes_Fauna.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
