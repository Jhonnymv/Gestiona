using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.GestorTablasMaestras
{
    public interface IGestorTablasMaestrasReadOnlyRepository
    {
        //Tb_Usuario
        Task<List<Usuario>> GetUsuarios(string filtro);
        Task<Usuario> GetUsuario(int idUsuario);
        Task UpdateUsuario(Usuario usuario);
        //Task<int> AddUsuario(Usuario usuario);
        //Tb_Empresa
        Task<List<Empresa>> GetEmpresas(string filtro);
        Task<Empresa> GetEmpresa(int idEmpresa);
        Task UpdateEmpresa(Empresa empresa);
        Task<int> AddEmpresa(Empresa empresa);
        //Tb_Gerencia
        Task<List<Gerencia>> GetGerencias(string filtro);
        Task<Gerencia> GetGerencia(int idGerencia);
        Task UpdateGerencia(Gerencia gerencia);
        Task <int>AddGerencia(Gerencia gerencia);
        //Tb_TipoCondicion
        Task<List<TipoCondicion>> GetTiposCondicion(string filtro);
        Task<TipoCondicion> GetTipoCondicion(int idTipoCondicion);
        Task UpdateTipoCondicion(TipoCondicion tipoCondicion);
        Task <int>AddTipoCondicion(TipoCondicion tipoCondicion);
        //Tb_EO_RS
        Task<List<EO_RS>> GetEO_RS(string filtro);
        Task<EO_RS> GetEO_RS(int idEO_RS);
        Task UpdateEO_RS(EO_RS eors);
        Task<int> AddEO_RS(EO_RS eors);
        //Tb_ACR_CMRS
        Task<List<ACR_CMRS>> GetACR_CMRS(string filtro);
        Task<ACR_CMRS> GetACR_CMRS(int id_acr_cmrs);
        Task UpdateACR_CMRS(ACR_CMRS acr_cmrs);
        Task<int> AddACR_CMRS(ACR_CMRS acr_cmrs);
        //Tb_Conductor_RS
        Task<List<Conductor_RS>> GetConductor_RS(string filtro);
        Task<Conductor_RS> GetConductor_RS(int id_conductor);
        Task UpdateConductor_RS(Conductor_RS conductor_RS);
        Task<int> AddConductor_RS(Conductor_RS conductor_RS);
        //Tb_Motivo_Ingreso_Salida
        Task<List<Motivo_Ingreso_Salida>> GetMotivo_Ingreso_Salida(string filtro);
        Task<Motivo_Ingreso_Salida> GetMotivo_Ingreso_Salida(int id_motivo);
        Task UpdateMotivo_Ingreso_Salida(Motivo_Ingreso_Salida motivo);
        Task<int> AddMotivo_Ingreso_Salida(Motivo_Ingreso_Salida motivo);
        //Tb_Residuo_Solido
        Task<List<Residuo_Solido>> GetResiduo_Solido(string filtro);
        Task<Residuo_Solido> GetResiduo_Solido(int id_residuo_solido);
        Task UpdateResiduo_Solido(Residuo_Solido residuo_solido);
        Task<int> AddResiduo_Solido(Residuo_Solido residuo_solido);
        //Tb_Responsable_ACR_CMRS
        Task<List<Responsable_ARC_CRMS>> GetResponsable_ARC_CRMS(string filtro);
        Task<Responsable_ARC_CRMS> GetResponsable_ARC_CRMS(int id_responsable);
        Task UpdateResponsable_ARC_CRMS(Responsable_ARC_CRMS responsable);
        Task<int> AddResponsable_ARC_CRMS(Responsable_ARC_CRMS responsable);
        //Tb_Vehiculo_RS
        Task<List<Vehiculo_RS>> GetVehiculo_RS(string filtro);
        Task<Vehiculo_RS> GetVehiculo_RS(int id_vehiculo_rs);
        Task UpdateVehiculo_RS(Vehiculo_RS vehiculo_rs);
        Task<int> AddVehiculo_RS(Vehiculo_RS vehiculo_rs);
    }
}
