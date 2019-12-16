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
        Task<int> AddUsuario(Usuario usuario);
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
    }
}
