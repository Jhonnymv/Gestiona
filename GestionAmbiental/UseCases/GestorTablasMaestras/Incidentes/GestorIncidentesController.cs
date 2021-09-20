using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Componente_Ambiental.AgregarComponenteAmbiental;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Componente_Ambiental.EditarComponentesAmbientales;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Componente_Ambiental.GetComponenteAmbiental;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Componente_Ambiental.GetComponentesAmbientales;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Fauna.AgregarEspecieFauna;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Fauna.EditarEspecieFauna;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Fauna.GetEspecieFauna;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Fauna.GetEspeciesFauna;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Flora.AgregarEspecieFlora;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Flora.EditarEspecieFlora;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Flora.GetEspecieFlora;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Flora.GetEspeciesFlora;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Estado_Equipo_Incidente.AgregarEstadoEquipo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Estado_Equipo_Incidente.EditarEstadoEquipo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Estado_Equipo_Incidente.GetEstadoEquipo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Estado_Equipo_Incidente.GetEstadosEquipos;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Factor_Causa_Incidente.AgregarFactorCausa;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Factor_Causa_Incidente.EditarFactorCausa;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Factor_Causa_Incidente.GetFactorCausa;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Factor_Causa_Incidente.GetFactoresCausa;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Lugar_Disposicion.AgregarLugarDisposicion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Lugar_Disposicion.EditarLugarDisposicion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Lugar_Disposicion.GetLugarDisposicion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Lugar_Disposicion.GetLugaresDisposicion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Nivel_Incidente.AgregarNivelIncidente;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Nivel_Incidente.EditarNivelIncidente;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Nivel_Incidente.GetNivelesIncidente;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Nivel_Incidente.GetNivelIncidente;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Producto_Incidente.AgregarProductoIncidente;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Producto_Incidente.EditarProductoIncidente;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Producto_Incidente.GetProductoIncidente;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Producto_Incidente.GetProductosIncidente;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Agua.AgregarTipoAgua;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Agua.EditarTipoAgua;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Agua.GetTipoAgua;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Agua.GetTiposAgua;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Incidente.AgregarTipoIncidente;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Incidente.EditarTipoIncidente;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Incidente.GetTipoIncidente;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Incidente.GetTiposIncidente;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Producto_Incidente.AgregarTipoProducto;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Producto_Incidente.EditarTipoProducto;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Producto_Incidente.GetTipoProducto;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Producto_Incidente.GetTiposProducto;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Suelo.AgregarTipoSuelo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Suelo.EditarTipoSuelo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Suelo.GetTiposSuelo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Suelo.GetTipoSuelo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Unidad_Medida.AgregarUnidadMedida;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Unidad_Medida.EditarUnidadMedida;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Unidad_Medida.GetUnidadesMedida;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Unidad_Medida.GetUnidadMedida;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.GetUsuarios;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Incidente.AgregarUsuarioIncidente;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Incidente.DeleteUsuarioIncidente;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Incidente.EditarUsuarioIncidente;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Incidente.GetUsuarioIncidente;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Incidente.GetUsuariosIncidente;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.GestorTablasMaestras.Incidentes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Incidentes
{
    public class GestorIncidentesController : Controller
    {
        //Tipo Suelo
        private readonly IAgregarTipoSuelo _agregarTipoSuelo;
        private readonly IEditarTipoSuelo _editarTipoSuelo;
        private readonly IGetTiposSuelo _getTiposSuelo;
        private readonly IGetTipoSuelo _getTipoSuelo;
        //Especie Flora
        private readonly IAgregarEspecieFlora _agregarEspecieFlora;
        private readonly IEditarEspecieFlora _editarEspecieFlora;
        private readonly IGetEspecieFlora _getEspecieFlora;
        private readonly IGetEspeciesFlora _getEspeciesFlora;
        //Especie Fauna
        private readonly IAgregarEspecieFauna _agregarEspecieFauna;
        private readonly IEditarEspecieFauna _editarEspecieFauna;
        private readonly IGetEspecieFauna _getEspecieFauna;
        private readonly IGetEspeciesFauna _getEspeciesFauna;
        //Tipo Agua
        private readonly IAgregarTipoAgua _agregarTipoAgua;
        private readonly IEditarTipoAgua _editarTipoAgua;
        private readonly IGetTiposAgua _getTiposAgua;
        private readonly IGetTipoAgua _getTipoAgua;
        //Nivel Incidente
        private readonly IAgregarNivelIncidente _agregarNivelIncidente;
        private readonly IEditarNivelIncidente _editarNivelIncidente;
        private readonly IGetNivelesIncidente _getNivelesIncidente;
        private readonly IGetNivelIncidente _getNivelIncidente;
        //Tipo Incidente
        private readonly IAgregarTipoIncidente _agregarTipoIncidente;
        private readonly IEditarTipoIncidente _editarTipoIncidente;
        private readonly IGetTipoIncidente _getTipoIncidente;
        private readonly IGetTiposIncidente _getTiposIncidente;

        //Factor Causa Ambiental
        private readonly IAgregarFactorCausa _agregarFactorCausa;
        private readonly IEditarFactorCausa _editarFactorCausa;
        private readonly IGetFactorCausa _getFactorCausa;
        private readonly IGetFactoresCausa _getFactoresCausa;
        //Lugar Disposicion
        private readonly IAgregarLugarDisposicion _agregarLugarDisposicion;
        private readonly IEditarLugarDisposicion _editarLugarDisposicion;
        private readonly IGetLugarDisposicion _getLugarDisposicion;
        private readonly IGetLugaresDisposicion _getLugaresDisposicion;
        //Componente Ambiental
        private readonly IAgregarComponenteAmbiental _agregarComponenteAmbiental;
        private readonly IEditarComponenteAmbiental _editarComponenteAmbiental;
        private readonly IGetComponenteAmbiental _getComponenteAmbiental;
        private readonly IGetComponentesAmbientales _getComponentesAmbientales;
        //Usuarios Notificar
        private readonly IAgregarUsuarioIncidente _agregarUsuarioIncidente;
        private readonly IDeleteUsuarioIncidente _deleteUsuarioIncidente;
        private readonly IEditarUsuarioIncidente _editarUsuarioIncidente;
        private readonly IGetUsuarioIncidente _getUsuarioIncidente;
        private readonly IGetUsuariosIncidente _getUsuariosIncidente;
        private readonly IGetUsuarios _getUsuarios;
        //Estado Equipo
        private readonly IAgregarEstadoEquipo _agregarEstadoEquipo;
        private readonly IEditarEstadoEquipo _editarEstadoEquipo;
        private readonly IGetEstadoEquipo _getEstadoEquipo;
        private readonly IGetEstadosEquipos _getEstadosEquipos;
        //Tipo Producto Incidente
        private readonly IAgregarTipoProducto _agregarTipoProducto;
        private readonly IEditarTipoProducto _editarTipoProducto;
        private readonly IGetTipoProducto _getTipoProducto;
        private readonly IGetTiposProducto _getTiposProducto;
        //Producto Incidente
        private readonly IAgregarProductoIncidente _agregarProductoIncidente;
        private readonly IEditarProductoIncidente _editarProductoIncidente;
        private readonly IGetProductoIncidente _getProductoIncidente;
        private readonly IGetProductosIncidente _getProductosIncidente;
        //Unidad Medida
        private readonly IAgregarUnidadMedida _agregarUnidadMedida;
        private readonly IEditarUnidadMedida _editarUnidadMedida;
        private readonly IGetUnidadesMedida _getUnidadesMedida;
        private readonly IGetUnidadMedida _getUnidadMedida;

        public GestorIncidentesController(
            IAgregarTipoSuelo agregarTipoSuelo,
            IEditarTipoSuelo editarTipoSuelo,
            IGetTiposSuelo getTiposSuelo,
            IGetTipoSuelo getTipoSuelo,
            IAgregarEspecieFlora agregarEspecieFlora,
            IEditarEspecieFlora editarEspecieFlora,
            IGetEspecieFlora getEspecieFlora,
            IGetEspeciesFlora getEspeciesFlora,
            IAgregarEspecieFauna agregarEspecieFauna,
            IEditarEspecieFauna editarEspecieFauna,
            IGetEspecieFauna getEspecieFauna,
            IGetEspeciesFauna getEspeciesFauna,
            IAgregarTipoAgua agregarTipoAgua,
            IEditarTipoAgua editarTipoAgua,
            IGetTiposAgua getTiposAgua,
            IGetTipoAgua getTipoAgua,
            IAgregarNivelIncidente agregarNivelIncidente,
            IEditarNivelIncidente editarNivelIncidente,
            IGetNivelesIncidente getNivelesIncidente,
            IGetNivelIncidente getNivelIncidente,
            IAgregarTipoIncidente agregarTipoIncidente,
            IEditarTipoIncidente editarTipoIncidente,
            IGetTipoIncidente getTipoIncidente,
            IGetTiposIncidente getTiposIncidente,
            IAgregarFactorCausa agregarFactorCausa,
            IEditarFactorCausa editarFactorCausa,
            IGetFactorCausa getFactorCausa,
            IGetFactoresCausa getFactoresCausa,
            IAgregarLugarDisposicion agregarLugarDisposicion,
            IEditarLugarDisposicion editarLugarDisposicion,
            IGetLugarDisposicion getLugarDisposicion,
            IGetLugaresDisposicion getLugaresDisposicion,
            IAgregarComponenteAmbiental agregarComponenteAmbiental,
            IEditarComponenteAmbiental editarComponenteAmbiental,
            IGetComponenteAmbiental getComponenteAmbiental,
            IGetComponentesAmbientales getComponentesAmbientales,
            IAgregarUsuarioIncidente agregarUsuarioIncidente,
            IDeleteUsuarioIncidente deleteUsuarioIncidente,
            IEditarUsuarioIncidente editarUsuarioIncidente,
            IGetUsuarioIncidente getUsuarioIncidente,
            IGetUsuariosIncidente getUsuariosIncidente,
            IGetUsuarios getUsuarios,
            IAgregarEstadoEquipo agregarEstadoEquipo,
            IEditarEstadoEquipo editarEstadoEquipo,
            IGetEstadoEquipo getEstadoEquipo,
            IGetEstadosEquipos getEstadosEquipos,
            IAgregarTipoProducto agregarTipoProducto,
            IEditarTipoProducto editarTipoProducto,
            IGetTipoProducto getTipoProducto,
            IGetTiposProducto getTiposProducto,
            IAgregarProductoIncidente agregarProductoIncidente,
            IEditarProductoIncidente editarProductoIncidente,
            IGetProductoIncidente getProductoIncidente,
            IGetProductosIncidente getProductosIncidente,
            IAgregarUnidadMedida agregarUnidadMedida,
            IEditarUnidadMedida editarUnidadMedida,
            IGetUnidadesMedida getUnidadesMedida,
            IGetUnidadMedida getUnidadMedida
            )
        {
            _agregarTipoSuelo = agregarTipoSuelo;
            _editarTipoSuelo = editarTipoSuelo;
            _getTiposSuelo = getTiposSuelo;
            _getTipoSuelo = getTipoSuelo;
            _agregarEspecieFlora = agregarEspecieFlora;
            _editarEspecieFlora = editarEspecieFlora;
            _getEspecieFlora = getEspecieFlora;
            _getEspeciesFlora = getEspeciesFlora;
            _agregarEspecieFauna = agregarEspecieFauna;
            _editarEspecieFauna = editarEspecieFauna;
            _getEspecieFauna = getEspecieFauna;
            _getEspeciesFauna = getEspeciesFauna;
            _agregarTipoAgua = agregarTipoAgua;
            _editarTipoAgua = editarTipoAgua;
            _getTiposAgua = getTiposAgua;
            _getTipoAgua = getTipoAgua;
            _agregarNivelIncidente = agregarNivelIncidente;
            _editarNivelIncidente = editarNivelIncidente;
            _getNivelesIncidente = getNivelesIncidente;
            _getNivelIncidente = getNivelIncidente;
            _agregarTipoIncidente = agregarTipoIncidente;
            _editarTipoIncidente = editarTipoIncidente;
            _getTipoIncidente = getTipoIncidente;
            _getTiposIncidente = getTiposIncidente;
            _agregarFactorCausa = agregarFactorCausa;
            _editarFactorCausa = editarFactorCausa;
            _getFactorCausa = getFactorCausa;
            _getFactoresCausa = getFactoresCausa;
            _agregarLugarDisposicion = agregarLugarDisposicion;
            _editarLugarDisposicion = editarLugarDisposicion;
            _getLugarDisposicion = getLugarDisposicion;
            _getLugaresDisposicion = getLugaresDisposicion;
            _agregarComponenteAmbiental = agregarComponenteAmbiental;
            _editarComponenteAmbiental = editarComponenteAmbiental;
            _getComponenteAmbiental = getComponenteAmbiental;
            _getComponentesAmbientales = getComponentesAmbientales;
            _agregarUsuarioIncidente = agregarUsuarioIncidente;
            _deleteUsuarioIncidente = deleteUsuarioIncidente;
            _editarUsuarioIncidente = editarUsuarioIncidente;
            _getUsuarioIncidente = getUsuarioIncidente;
            _getUsuariosIncidente = getUsuariosIncidente;
            _getUsuarios = getUsuarios;
            _agregarEstadoEquipo = agregarEstadoEquipo;
            _editarEstadoEquipo = editarEstadoEquipo;
            _getEstadoEquipo = getEstadoEquipo;
            _getEstadosEquipos = getEstadosEquipos;
            _agregarTipoProducto = agregarTipoProducto;
            _editarTipoProducto = editarTipoProducto;
            _getTipoProducto = getTipoProducto;
            _getTiposProducto = getTiposProducto;
            _agregarProductoIncidente = agregarProductoIncidente;
            _editarProductoIncidente = editarProductoIncidente;
            _getProductoIncidente = getProductoIncidente;
            _getProductosIncidente = getProductosIncidente;
            _agregarUnidadMedida = agregarUnidadMedida;
            _editarUnidadMedida = editarUnidadMedida;
            _getUnidadesMedida = getUnidadesMedida;
            _getUnidadMedida = getUnidadMedida;
        }

        public IActionResult ResumenImpacto()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                return View();
            }
        }
        //Tipo Suelo
        public async Task<IActionResult> FiltrarTipoSuelo(string filtro, int pagina, int rpp)
        {
            var data = await _getTiposSuelo.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NTipos % rpp != 0 ? (data.NTipos / rpp) + 1 : data.NTipos / rpp;
            if (data.NTipos > 0)
            {
                foreach (var fila in data.Tipos)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIncidentes/DetalleTipoSuelo?IdTipoSuelo=" + fila.Id_Tipo_Suelo + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoSuelo(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarTipoSuelo(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoSuelo(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoSuelo(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleTipoSuelo(int IdTipoSuelo)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdTipoSuelo=" + IdTipoSuelo });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var data = await _getTipoSuelo.Execute(IdTipoSuelo);
                var model = new TipoSueloModel();
                model.Id_Tipo_Suelo = data.Id_Tipo_Suelo;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarTipoSuelo(TipoSueloModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarTipoSuelo.Execute(model.Id_Tipo_Suelo, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleTipoSuelo", new { IdTipoSuelo = model.Id_Tipo_Suelo });
            }
        }
        public IActionResult AgregarTipoSuelo()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarTipoSuelo(TipoSueloModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarTipoSuelo.Execute(model.Descripcion);
                return RedirectToAction("DetalleTipoSuelo", new { IdTipoSuelo = id });
            }
            return RedirectToAction("AgregarTipoSuelo");
        }
        //Especie Flora
        public async Task<IActionResult> FiltrarEspecieFlora(string filtro, int pagina, int rpp)
        {
            var data = await _getEspeciesFlora.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NEspecies % rpp != 0 ? (data.NEspecies / rpp) + 1 : data.NEspecies / rpp;
            if (data.NEspecies > 0)
            {
                foreach (var fila in data.Especies)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIncidentes/DetalleEspecieFlora?IdEspecieFlora=" + fila.Id_Especie_Flora + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEspecieFlora(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarEspecieFlora(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEspecieFlora(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEspecieFlora(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleEspecieFlora(int IdEspecieFlora)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdEspecieFlora=" + IdEspecieFlora });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var data = await _getEspecieFlora.Execute(IdEspecieFlora);
                var model = new EspecieFloraModel();
                model.Id_Especie_Flora = data.Id_Especie_Flora;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarEspecieFlora(EspecieFloraModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarEspecieFlora.Execute(model.Id_Especie_Flora, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleEspecieFlora", new { IdEspecieFlora = model.Id_Especie_Flora });
            }
        }
        public IActionResult AgregarEspecieFlora()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName});
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarEspecieFlora(EspecieFloraModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarEspecieFlora.Execute(model.Descripcion);
                return RedirectToAction("DetalleEspecieFlora", new { IdEspecieFlora = id });
            }
            return RedirectToAction("AgregarEspecieFlora");
        }
        //Especie Fauna
        public async Task<IActionResult> FiltrarEspecieFauna(string filtro, int pagina, int rpp)
        {
            var data = await _getEspeciesFauna.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NEspecies % rpp != 0 ? (data.NEspecies / rpp) + 1 : data.NEspecies / rpp;
            if (data.NEspecies > 0)
            {
                foreach (var fila in data.Especies)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIncidentes/DetalleEspecieFauna?IdEspecieFauna=" + fila.Id_Especie_Fauna + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEspecieFauna(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarEspecieFauna(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEspecieFauna(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEspecieFauna(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleEspecieFauna(int IdEspecieFauna)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdEspecieFauna=" + IdEspecieFauna });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var data = await _getEspecieFauna.Execute(IdEspecieFauna);
                var model = new EspecieFaunaModel();
                model.Id_Especie_Fauna = data.Id_Especie_Fauna;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarEspecieFauna(EspecieFaunaModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarEspecieFauna.Execute(model.Id_Especie_Fauna, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleEspecieFauna", new { IdEspecieFauna = model.Id_Especie_Fauna });
            }
        }
        public IActionResult AgregarEspecieFauna()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName});
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarEspecieFauna(EspecieFaunaModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarEspecieFauna.Execute(model.Descripcion);
                return RedirectToAction("DetalleEspecieFauna", new { IdEspecieFauna = id });
            }
            return RedirectToAction("AgregarEspecieFauna");
        }
        //Tipo Agua
        public async Task<IActionResult> FiltrarTipoAgua(string filtro, int pagina, int rpp)
        {
            var data = await _getTiposAgua.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NTipos % rpp != 0 ? (data.NTipos / rpp) + 1 : data.NTipos / rpp;
            if (data.NTipos > 0)
            {
                foreach (var fila in data.Tipos)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIncidentes/DetalleTipoAgua?IdTipoAgua=" + fila.Id_Tipo_Agua + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoAgua(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarTipoAgua(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoAgua(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoAgua(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleTipoAgua(int IdTipoAgua)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdTipoAgua=" + IdTipoAgua });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var data = await _getTipoAgua.Execute(IdTipoAgua);
                var model = new TipoAguaModel();
                model.Id_Tipo_Agua = data.Id_Tipo_Agua;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarTipoAgua(TipoAguaModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarTipoAgua.Execute(model.Id_Tipo_Agua, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleTipoAgua", new { IdTipoAgua = model.Id_Tipo_Agua });
            }
        }
        public IActionResult AgregarTipoAgua()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarTipoAgua(TipoAguaModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarTipoAgua.Execute(model.Descripcion);
                return RedirectToAction("DetalleTipoAgua", new { IdTipoAgua = id });
            }
            return RedirectToAction("AgregarTipoAgua");
        }
        public IActionResult Resumen()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                return View();
            }
        }
        //Usuarios a Notificar
        public async Task<IActionResult> FiltrarUsuario(string filtro, int pagina, int rpp)
        {
            var data = await _getUsuariosIncidente.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Nombres</th>";
            tabla += "<th>Apellidos</th>";
            tabla += "<th>Usuario de Red</th>";
            tabla += "<th>Email</th>";
            tabla += "<th>Rol</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.nUsuarios % rpp != 0 ? (data.nUsuarios / rpp) + 1 : data.nUsuarios / rpp;
            if (data.nUsuarios > 0)
            {
                foreach (var fila in data.Usuarios)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Nombres + "</td>";
                    filasTabla += "<td>" + fila.Apellidos + "</td>";
                    filasTabla += "<td>" + fila.UsuarioRed + "</td>";
                    filasTabla += "<td>" + fila.Email + "</td>";
                    filasTabla += "<td>" + fila.Rol + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIncidentes/DetalleUsuario?IdUsuario=" + fila.IdUsuario + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarUsuario(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarUsuario(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarUsuario(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarUsuario(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";
            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleUsuario(int IdUsuario)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdUsuario=" + IdUsuario });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var enumList = Enum.GetValues(typeof(RSRoles)).OfType<RSRoles>().ToList();

                var roles = enumList.Select(g => new SelectListItem() { Value = ((int)g).ToString(), Text = g.ToString() }).ToList();
                roles.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Roles = roles;

                var Usuario = await _getUsuarioIncidente.Execute(IdUsuario);
                var model = new UsusarioNotificarModel();
                model.Id_Usuario_Notificar = Usuario.IdUsuario;
                model.Id_Rol = Usuario.IdRol;
                model.Nombres = Usuario.Nombres;
                model.Apellidos = Usuario.Apellidos;
                model.usuario_red = Usuario.UsuarioRed;
                model.Email = Usuario.Email;
                model.Estado = Usuario.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarUsuario(UsusarioNotificarModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                if (ModelState.IsValid)
                    await _editarUsuarioIncidente.Execute(model.Id_Usuario_Notificar, model.Nombres, model.Apellidos, model.Id_Rol, model.Email, model.usuario_red, model.Estado);
                return RedirectToAction("DetalleUsuario", new { IdUsuario = model.Id_Usuario_Notificar });
            }
        }
        public async Task<IActionResult> AgregarUsuario()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var UsuariosAD = await _getUsuarios.Execute();

                var usuarios = UsuariosAD.Usuarios.Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                usuarios.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.ListUsuarios = usuarios;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarUsuario(UsusarioNotificarModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarUsuarioIncidente.Execute(model.Id_Usuario_Notificar);
                return RedirectToAction("DetalleUsuario", new { IdUsuario = id });
            }
            return RedirectToAction("AgregarUsuario");
        }
        public async Task<IActionResult> QuitarUsuario(int Id_Usuario)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?Id_Usuario=" + Id_Usuario });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                await _deleteUsuarioIncidente.Execute(Id_Usuario);
                return RedirectToAction("Resumen");
            }
        }
        //Factor Causa Incidente
        public async Task<IActionResult> FiltrarFactorCausa(string filtro, int pagina, int rpp)
        {
            var data = await _getFactoresCausa.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Tipo</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NFactores % rpp != 0 ? (data.NFactores / rpp) + 1 : data.NFactores / rpp;
            if (data.NFactores > 0)
            {
                foreach (var fila in data.Factores)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Tipo_Factor + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIncidentes/DetalleFactorCausa?IdFactorCausa=" + fila.Id_Factor_Causa_Incidente + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltarFactorCausa(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltarFactorCausa(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltarFactorCausa(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltarFactorCausa(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleFactorCausa(int IdFactorCausa)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdFactorCausa=" + IdFactorCausa });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var data = await _getFactorCausa.Execute(IdFactorCausa);
                var model = new FactorCausaModel();
                model.Id_Factor_Causa_Incidente = data.Id_Factor_Causa_Incidente;
                model.Tipo_Factor = data.Tipo_Factor;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarFactorCausa(FactorCausaModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarFactorCausa.Execute(model.Id_Factor_Causa_Incidente, model.Descripcion, model.Tipo_Factor, model.Estado);
                return RedirectToAction("DetalleFactorCausa", new { IdFactorCausa = model.Id_Factor_Causa_Incidente });
            }
        }
        public IActionResult AgregarFactorCausa()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarFactorCausa(FactorCausaModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarFactorCausa.Execute(model.Descripcion,model.Tipo_Factor);
                return RedirectToAction("DetalleFactorCausa", new { IdFactorCausa = id });
            }
            return RedirectToAction("AgregarFactorCausa");
        }
        //Lugar Disposicion
        public async Task<IActionResult> FiltrarLugarDiposicion(string filtro, int pagina, int rpp)
        {
            var data = await _getLugaresDisposicion.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NLugares % rpp != 0 ? (data.NLugares / rpp) + 1 : data.NLugares / rpp;
            if (data.NLugares > 0)
            {
                foreach (var fila in data.Lugares)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIncidentes/DetalleLugarDiposicion?IdLugarDiposicion=" + fila.Id_Lugar_Disposicion + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltarLugarDiposicion(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltarLugarDiposicion(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltarLugarDiposicion(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltarLugarDiposicion(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleLugarDiposicion(int IdLugarDiposicion)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdLugarDiposicion=" + IdLugarDiposicion });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var data = await _getLugarDisposicion.Execute(IdLugarDiposicion);
                var model = new LugarDisposicionModel();
                model.Id_Lugar_Disposicion = data.Id_Lugar_Disposicion;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarLugarDiposicion(LugarDisposicionModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarLugarDisposicion.Execute(model.Id_Lugar_Disposicion, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleLugarDiposicion", new { IdLugarDiposicion = model.Id_Lugar_Disposicion });
            }
        }
        public IActionResult AgregarLugarDiposicion()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarLugarDiposicion(LugarDisposicionModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarLugarDisposicion.Execute(model.Descripcion);
                return RedirectToAction("DetalleLugarDiposicion", new { IdLugarDiposicion = id });
            }
            return RedirectToAction("AgregarLugarDiposicion");
        }
        //Componente Ambiental
        public async Task<IActionResult> FiltrarComponenteAmbiental(string filtro, int pagina, int rpp)
        {
            var data = await _getComponentesAmbientales.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NComponentes % rpp != 0 ? (data.NComponentes / rpp) + 1 : data.NComponentes / rpp;
            if (data.NComponentes > 0)
            {
                foreach (var fila in data.Componentes)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIncidentes/DetalleComponenteAmbiental?IdComponenteAmbiental=" + fila.Id_Componente_Ambiental + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltarComponenteAmbiental(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltarComponenteAmbiental(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltarComponenteAmbiental(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltarComponenteAmbiental(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleComponenteAmbiental(int IdComponenteAmbiental)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdComponenteAmbiental=" + IdComponenteAmbiental });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var data = await _getComponenteAmbiental.Execute(IdComponenteAmbiental);
                var model = new ComponenteAmbientalModel();
                model.Id_Componente_Ambiental = data.Id_Componente_Ambiental;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarComponenteAmbiental(ComponenteAmbientalModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarComponenteAmbiental.Execute(model.Id_Componente_Ambiental, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleComponenteAmbiental", new { IdComponenteAmbiental = model.Id_Componente_Ambiental });
            }
        }
        public IActionResult AgregarComponenteAmbiental()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarComponenteAmbiental(ComponenteAmbientalModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarComponenteAmbiental.Execute(model.Descripcion);
                return RedirectToAction("DetalleComponenteAmbiental", new { IdComponenteAmbiental = id });
            }
            return RedirectToAction("AgregarComponenteAmbiental");
        }
        //Estado Equipo
        public async Task<IActionResult> FiltrarEstadoEquipo(string filtro, int pagina, int rpp)
        {
            var data = await _getEstadosEquipos.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NEstados % rpp != 0 ? (data.NEstados / rpp) + 1 : data.NEstados / rpp;
            if (data.NEstados > 0)
            {
                foreach (var fila in data.Estados)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIncidentes/DetalleEstadoEquipo?IdEstadoEquipo=" + fila.Id_Estado_Equipo_Incidente + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEstadoEquipo(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarEstadoEquipo(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEstadoEquipo(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEstadoEquipo(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleEstadoEquipo(int IdEstadoEquipo)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdEstadoEquipo=" + IdEstadoEquipo });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var data = await _getEstadoEquipo.Execute(IdEstadoEquipo);
                var model = new EstadoEquipoModel();
                model.Id_Estado_Equipo = data.Id_Estado_Equipo_Incidente;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarEstadoEquipo(EstadoEquipoModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarEstadoEquipo.Execute(model.Id_Estado_Equipo, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleEstadoEquipo", new { IdEstadoEquipo = model.Id_Estado_Equipo });
            }
        }
        public IActionResult AgregarEstadoEquipo()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarEstadoEquipo(EstadoEquipoModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarEstadoEquipo.Execute(model.Descripcion);
                return RedirectToAction("DetalleEstadoEquipo", new { IdEstadoEquipo = id });
            }
            return RedirectToAction("AgregarEstadoEquipo");
        }
        //Tipo Producto Incidente
        public async Task<IActionResult> FiltrarTipoProducto(string filtro, int pagina, int rpp)
        {
            var data = await _getTiposProducto.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NTipos % rpp != 0 ? (data.NTipos / rpp) + 1 : data.NTipos / rpp;
            if (data.NTipos > 0)
            {
                foreach (var fila in data.Tipos)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIncidentes/DetalleTipoProducto?IdTipoProducto=" + fila.Id_Tipo_Producto_Incidente + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoProducto(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarTipoProducto(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoProducto(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoProducto(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleTipoProducto(int IdTipoProducto)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdTipoProducto=" + IdTipoProducto });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var data = await _getTipoProducto.Execute(IdTipoProducto);
                var model = new TipoProductoModel();
                model.Id_Tipo_Producto_Incidente = data.Id_Tipo_Producto_Incidente;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarTipoProducto(TipoProductoModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarTipoProducto.Execute(model.Id_Tipo_Producto_Incidente, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleTipoProducto", new { IdTipoProducto = model.Id_Tipo_Producto_Incidente });
            }
        }
        public IActionResult AgregarTipoProducto()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarTipoProducto(TipoProductoModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarTipoProducto.Execute(model.Descripcion);
                return RedirectToAction("DetalleTipoProducto", new { IdTipoProducto = id });
            }
            return RedirectToAction("AgregarTipoProducto");
        }
        //Producto Incidente
        public async Task<IActionResult> FiltrarProductoIncidente(string filtro, int pagina, int rpp)
        {
            var data = await _getProductosIncidente.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NPRoductos % rpp != 0 ? (data.NPRoductos / rpp) + 1 : data.NPRoductos / rpp;
            if (data.NPRoductos > 0)
            {
                foreach (var fila in data.Productos)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIncidentes/DetalleProductoIncidente?IdProductoIncidente=" + fila.Id_Producto_Incidente + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarProductoIncidente(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarProductoIncidente(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarProductoIncidente(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarProductoIncidente(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleProductoIncidente(int IdProductoIncidente)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdProductoIncidente=" + IdProductoIncidente });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var data = await _getProductoIncidente.Execute(IdProductoIncidente);
                var model = new ProductoIncidenteModel();
                model.Id_Producto_Incidente = data.Id_Producto_Incidente;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarProductoIncidente(ProductoIncidenteModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarProductoIncidente.Execute(model.Id_Producto_Incidente, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleProductoIncidente", new { IdProductoIncidente = model.Id_Producto_Incidente });
            }
        }
        public IActionResult AgregarProductoIncidente()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarProductoIncidente(ProductoIncidenteModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarProductoIncidente.Execute(model.Descripcion);
                return RedirectToAction("DetalleProductoIncidente", new { IdProductoIncidente = id });
            }
            return RedirectToAction("AgregarProductoIncidente");
        }
        //Nivel Incidente
        public async Task<IActionResult> FiltrarNivelIncidente(string filtro, int pagina, int rpp)
        {
            var data = await _getNivelesIncidente.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NNiveles % rpp != 0 ? (data.NNiveles / rpp) + 1 : data.NNiveles / rpp;
            if (data.NNiveles > 0)
            {
                foreach (var fila in data.Niveles)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIncidentes/DetalleNivelIncidente?IdNivelIncidente=" + fila.Id_Nivel_Incidente + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarNivelIncidente(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarNivelIncidente(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarNivelIncidente(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarNivelIncidente(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleNivelIncidente(int IdNivelIncidente)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdNivelIncidente=" + IdNivelIncidente });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var data = await _getNivelIncidente.Execute(IdNivelIncidente);
                var model = new NivelIncidenteModel();
                model.Id_Nivel_Incidente = data.Id_Nivel_Incidente;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarNivelIncidente(NivelIncidenteModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarNivelIncidente.Execute(model.Id_Nivel_Incidente, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleNivelIncidente", new { IdNivelIncidente = model.Id_Nivel_Incidente });
            }
        }
        public IActionResult AgregarNivelIncidente()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarNivelIncidente(NivelIncidenteModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarNivelIncidente.Execute(model.Descripcion);
                return RedirectToAction("DetalleNivelIncidente", new { IdNivelIncidente = id });
            }
            return RedirectToAction("AgregarNivelIncidente");
        }
        //Tipo Incidente
        public async Task<IActionResult> FiltrarTipoIncidente(string filtro, int pagina, int rpp)
        {
            var data = await _getTiposIncidente.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NTipos % rpp != 0 ? (data.NTipos / rpp) + 1 : data.NTipos / rpp;
            if (data.NTipos > 0)
            {
                foreach (var fila in data.Tipos)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIncidentes/DetalleTipoIncidente?IdTipoIncidente=" + fila.Id_Tipo_Incidente + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoIncidente(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarTipoIncidente(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoIncidente(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoIncidente(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleTipoIncidente(int IdTipoIncidente)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdTipoIncidente=" + IdTipoIncidente });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var data = await _getTipoIncidente.Execute(IdTipoIncidente);
                var model = new TipoIncidenteModel();
                model.Id_Tipo_Incidente = data.Id_Tipo_Incidente;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarTipoIncidente(TipoIncidenteModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarTipoIncidente.Execute(model.Id_Tipo_Incidente, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleTipoIncidente", new { IdTipoIncidente = model.Id_Tipo_Incidente });
            }
        }
        public IActionResult AgregarTipoIncidente()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName});
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarTipoIncidente(TipoIncidenteModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarTipoIncidente.Execute(model.Descripcion);
                return RedirectToAction("DetalleTipoIncidente", new { IdTipoIncidente = id });
            }
            return RedirectToAction("AgregarTipoIncidente");
        }
        //Unidades Medida
        public async Task<IActionResult> FiltrarUnidadMedida(string filtro, int pagina, int rpp)
        {
            var data = await _getUnidadesMedida.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Tipo</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NUnidades % rpp != 0 ? (data.NUnidades / rpp) + 1 : data.NUnidades / rpp;
            if (data.NUnidades > 0)
            {
                foreach (var fila in data.Unidades)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Tipo + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIncidentes/DetalleUnidadMedida?IdUnidadMedida=" + fila.Id_Unidad_Medida + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarUnidadMedida(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarUnidadMedida(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarUnidadMedida(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarUnidadMedida(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleUnidadMedida(int IdUnidadMedida)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdUnidadMedida=" + IdUnidadMedida });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                
                var tipos = Enum.GetValues(typeof(TiposUnidadMedida)).Cast<TiposUnidadMedida>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = v.ToString()
                }).ToList();

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                ViewBag.TiposUnidad = tipos;
                var data = await _getUnidadMedida.Execute(IdUnidadMedida);
                var model = new UnidadMedidaModel();
                model.Id_Unidad_Medida = data.Id_Unidad_Medida;
                model.Tipo = data.Tipo;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarUnidadMedida(UnidadMedidaModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarUnidadMedida.Execute(model.Id_Unidad_Medida, model.Descripcion, model.Tipo, model.Estado);
                return RedirectToAction("DetalleUnidadMedida", new { IdUnidadMedida = model.Id_Unidad_Medida });
            }
        }
        public IActionResult AgregarUnidadMedida()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var tipos = Enum.GetValues(typeof(TiposUnidadMedida)).Cast<TiposUnidadMedida>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = v.ToString()
                }).ToList();

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                ViewBag.TiposUnidad = tipos;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarUnidadMedida(UnidadMedidaModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarUnidadMedida.Execute(model.Descripcion,model.Tipo);
                return RedirectToAction("DetalleUnidadMedida", new { IdUnidadMedida = id });
            }
            return RedirectToAction("AgregarUnidadMedida");
        }
    }
}