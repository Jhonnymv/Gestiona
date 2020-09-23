using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.ConsumoAgua.EditarConsumoAgua;
using GestionAmbiental.Application.UseCases.ConsumoAgua.GetConsumoAgua;
using GestionAmbiental.Application.UseCases.ConsumoAgua.GetConsumosAgua;
using GestionAmbiental.Application.UseCases.ConsumoAgua.Items;
using GestionAmbiental.Application.UseCases.ConsumoAgua.RegistrarConsumoAgua;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.ConsumoAgua.AguaMar
{
    public class ConsumoAguaMarController : Controller
    {
        private readonly IGetConsumosAgua _getConsumosAgua;
        private readonly IGetConsumoAgua _getConsumoAgua;
        private readonly IRegistrarConsumoAgua _registrarConsumoAgua;
        private readonly IEditarConsumoAgua _editarConsumoAgua;
        private readonly ISelectItems _selectItems;

        public ConsumoAguaMarController(
            IGetConsumosAgua getConsumosAgua, 
            IGetConsumoAgua getConsumoAgua, 
            IRegistrarConsumoAgua registrarConsumoAgua,
            IEditarConsumoAgua editarConsumoAgua,
            ISelectItems selectItems)
        {
            _getConsumosAgua = getConsumosAgua;
            _getConsumoAgua = getConsumoAgua;
            _registrarConsumoAgua = registrarConsumoAgua;
            _editarConsumoAgua = editarConsumoAgua;
            _selectItems = selectItems;
        }

        public IActionResult Resumen()
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

                //var items = await _getItems.Execute();

                //var tiposFauna = items.TiposFauna.Select(g => new SelectListItem() { Value = g.IdTipoFauna.ToString(), Text = g.Descripcion }).ToList();
                //tiposFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                //var estadosFauna = items.EstadosFauna.Select(g => new SelectListItem() { Value = g.IdEstadoFauna.ToString(), Text = g.Descripcion }).ToList();
                //estadosFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                //var lugaresReferencia = items.LugaresReferencia.Select(g => new SelectListItem() { Value = g.IdLugarReferencia.ToString(), Text = g.Descripcion }).ToList();
                //lugaresReferencia.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                //var contactosEmergencia = JsonConvert.SerializeObject(items.ContactosEmergencia.Select(x => x.Telefono).ToArray());

                //var mensajeContactos = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.WebRootPath, "ConfigFiles", "AvistamientoFauna.txt"));
                //mensajeContactos = mensajeContactos.Substring(10);

                //ViewBag.TipoFauna = tiposFauna;
                //ViewBag.EstadoFauna = estadosFauna;
                //ViewBag.LugarReferencia = lugaresReferencia;
                //ViewBag.Mensaje = mensajeContactos;
                //ViewBag.Contactos = contactosEmergencia;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetTablaResumenFiltrada(string fecha, int rpp, int pagina)
        {
            var datosTabla = await _getConsumosAgua.Execute(fecha, pagina, rpp );

            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NConsumo % rpp != 0 ? (datosTabla.NConsumo / rpp) + 1 : datosTabla.NConsumo / rpp;
            if (datosTabla.NConsumo > 0)
            {
                foreach (var fila in datosTabla.Consumos)
                {
                    //var IdEncriptado = _protector.Protect(fila.IdAvistamientoFauna.ToString());
                    filasTabla += "<tr>";
                    //if ((fila.Estado.Equals(EstadosAvistamiento.Registrado.ToString()) || fila.Estado.Equals(EstadosAvistamiento.Completado.ToString())) && !usuario.IdRol.Equals((int)InspeccionRol.Admin))
                    //{
                    //    if (fila.FechaAvistamiento > DateTime.Now.AddDays(-1))
                    //        filasTabla += "<td><img class=\"icon\" src=\"../img/icons/bell.png\"/></td>";
                    //    else
                    //        filasTabla += "<td></td>";
                    //    filasTabla += "<td><a href=\"/AvistamientoFauna/DetalleRegistro?IdAvistamiento=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                    //}
                    //else if (fila.Estado.Equals(EstadosAvistamiento.Pendiente.ToString()) || usuario.IdRol.Equals((int)InspeccionRol.Admin))
                    //{
                    //    filasTabla += "<td></td>";
                    //    filasTabla += "<td><a href=\"/AvistamientoFauna/EditarRegistro?IdAvistamiento=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                    //}
                    filasTabla += "<td><a href=\"/ConsumoAguaMar/EditarRegistro?IdConsumo=" + fila.IdConsumoAgua + "\">" + fila.IdConsumoAgua + "</a></td>";
                    filasTabla += "<td>" + fila.FuenteAgua + "</td>";
                    filasTabla += "<td>" + fila.Fecha.ToString("dd/MM/yyyy") + "</td>";
                    filasTabla += "<td>" + fila.TiempoBombeo + "</td>";
                    filasTabla += "<td>" + String.Format("{0:0.00}", fila.LecturaInicial) + "</td>";
                    filasTabla += "<td>" + String.Format("{0:0.00}", fila.LecturaFinal) + "</td>";
                    filasTabla += "<td>" + String.Format("{0:0.00}", fila.VolumenBombeado) + "</td>";
                    filasTabla += "<td>" + String.Format("{0:0.00}", fila.CaudalPromedio) + "</td>";
                    filasTabla += "</tr>";                   
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"filtrar(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"filtrar(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
                paginador += "</ul>";
            }
            else
            {
                filasTabla = "Sin datos";
            }
            dynamic response = new
            {
                Data = filasTabla,
                Paginador = paginador
            };
            return Ok(response);
        }

        public async Task<IActionResult> NuevoRegistro()
        {
            var items = await _selectItems.GetSelectItems();

            var resoluciones = items.ItemsResoluciones.Select(g => new SelectListItem() { Value = g.IdResolucionConsumoAgua.ToString(), Text = g.Resolucion }).ToList();
            resoluciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.Resoluciones = resoluciones;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NuevoRegistro(ConsumoAguaMarModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var id = await _registrarConsumoAgua.Execute(
                    model.IdResolucion,
                    model.Fecha,
                    model.TiempoBombeo,
                    model.LecturaInicial,
                    model.LecturaFinal,
                    model.VolumenAguaBombeado,
                    model.CaudalPromedio,
                    userInfo.NombreUsuario);

                return RedirectToAction("Resumen");
            }
            return RedirectToAction("Resumen");
        }
        public async Task<IActionResult> EditarRegistro(int IdConsumo)
        {

            var items = await _selectItems.GetSelectItems();

            var resoluciones = items.ItemsResoluciones.Select(g => new SelectListItem() { Value = g.IdResolucionConsumoAgua.ToString(), Text = g.Resolucion }).ToList();
            resoluciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.Resoluciones = resoluciones;

            var consumo = await _getConsumoAgua.Execute(IdConsumo);
            var model = new ConsumoAguaMarModel
            {
                Id = consumo.IdConsumoAgua,
                IdResolucion = consumo.IdResolucionConsumoAgua,
                Resolucion = consumo.ResolucionConsumoAgua,
                FuenteAgua = consumo.FuenteAgua,
                Fecha = consumo.Fecha.ToString("yyyy-MM-dd"),
                TiempoBombeo = consumo.TiempoBombeo,
                LecturaInicial = consumo.LecturaInicial,
                LecturaFinal = consumo.LecturaFinal,
                VolumenAguaBombeado = consumo.VolumenBombeado,
                CaudalPromedio = consumo.CaudalPromedio
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditarRegistro(ConsumoAguaMarModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                await _editarConsumoAgua.Execute(
                    model.Id,
                    model.IdResolucion,
                    model.Fecha,
                    model.TiempoBombeo,
                    model.LecturaInicial,
                    model.LecturaFinal,
                    model.VolumenAguaBombeado,
                    model.CaudalPromedio);
            }
            return RedirectToAction("EditarRegistro", new { IdConsumo = model.Id });
        }
    }
}
