using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.ConsultarManifiestoRSCierre;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.ConsultarManifiestosRSCierre;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.EditarManifiestoRS.CompletarManifiestoRS;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.EliminarManifiestoRS;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.GetSelectItems;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.ResiduosSolidos.ControlManifiestoRS.CompletarManifiestoRS
{
    public class CompletarManifiestoController : Controller
    {
        private readonly IGetSelectItems _getSelectItems;
        private readonly IConsultarManifiestosRSCierre _consultarManifiestosRS;
        private readonly IConsultarManifiestoRSCierre _consultarManifiestoRS;
        private readonly ICompletarManifiestoRS _completarManifiestoRS;
        private readonly IEliminarManifiestoRS _eliminarManifiestoRS;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IDataProtector _protector;

        public CompletarManifiestoController(
            IGetSelectItems getSelectItems,
            IConsultarManifiestosRSCierre consultarManifiestosRS,
            IConsultarManifiestoRSCierre consultarManifiestoRS,
            ICompletarManifiestoRS completarManifiestoRS,
            IDataProtectionProvider dataProtectionProvider,
            IEliminarManifiestoRS eliminarManifiestoRS,
            IGetInfo_SGA info_SGA,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _getSelectItems = getSelectItems;
            _consultarManifiestosRS = consultarManifiestosRS;
            _consultarManifiestoRS = consultarManifiestoRS;
            _eliminarManifiestoRS = eliminarManifiestoRS;
            _info_SGA = info_SGA;
            _completarManifiestoRS = completarManifiestoRS;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.IngresoResiduosPeligrosSIdRouteValue);
        }

        public async Task<IActionResult> Resumen()
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
                var items = await _getSelectItems.Execute();
                var residuos_solidos = items.ItemsResiduoSolido.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Residuo_Solido.ToString(), Text = g.Descripcion }).ToList();
                residuos_solidos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var eo_rs = items.ItemsEO_RS.OrderBy(x => x.Razon_Social).Select(g => new SelectListItem() { Value = g.Id_EO_RS.ToString(), Text = g.Razon_Social }).ToList();
                eo_rs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var asesoresMA = items.ItemsAsesoresMA.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                asesoresMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.ResiduosSolidos = residuos_solidos;
                ViewBag.EO_RS = eo_rs;
                ViewBag.AsesoresMA = asesoresMA;
                return View();
            }
        }
        public async Task<IActionResult> GetTablaResumenFiltrada(string nManifiesto, string fechaEmision, string observacion, int residuoSolido, int asesorMAP, int eors_salida, int eors_llegada, int pagina, int rpp)
        {
            var datosTabla = await _consultarManifiestosRS.Execute(nManifiesto, fechaEmision, observacion, residuoSolido, eors_salida, eors_llegada, asesorMAP, pagina, rpp);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.nManifiesto % rpp != 0 ? (datosTabla.nManifiesto / rpp) + 1 : datosTabla.nManifiesto / rpp;
            if (datosTabla.nManifiesto > 0)
            {
                foreach (var fila in datosTabla.manifiestosRS)
                {
                    var id_encriptado = _protector.Protect(fila.Id_Manifiesto_RS.ToString());
                    filasTabla += "<tr>";
                    if (fila.Estado.Equals(EstadosResiduosSolidos.Cerrado.ToString()))
                    {
                        filasTabla += "<td></td>";
                        filasTabla += "<td><a href=\"/CompletarManifiesto/DetalleManifiesto?IdManifiesto=" + id_encriptado + "\">" + fila.Item + "</a></td>";
                    }
                    else
                    {
                        filasTabla += "<td><img class=\"icon\" src=\"../img/icons/bell.png\"/></td>";
                        filasTabla += "<td><a href=\"/CompletarManifiesto/CompletarManifiesto?IdManifiesto=" + id_encriptado + "\">" + fila.Item + "</a></td>";
                    }

                    filasTabla += "<td>" + fila.Serie_Manifiesto + "</td>";
                    filasTabla += "<td>" + fila.Num_Manifiesto + "</td>";
                    filasTabla += "<td>" + fila.Fecha_Emision_Manifiesto.ToString("dd/MM/yyyy") + "</td>";
                    filasTabla += "<td>" + fila.Observacion_Manifiesto + "</td>";
                    filasTabla += "<td>" + fila.Residuo_Solido + "</td>";
                    filasTabla += "<td>" + fila.Cantidad_RS_Evacuado + "</td>";
                    filasTabla += "<td>" + fila.EO_RS_Salida + "</td>";
                    filasTabla += "<td>" + fila.EO_RS_Llegada + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
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
        public async Task<IActionResult> DetalleManifiesto(string IdManifiesto)
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

                var IdDecrypted = Convert.ToInt32(_protector.Unprotect(IdManifiesto));
                var data = await _consultarManifiestoRS.Execute(IdDecrypted, userInfo.NombreUsuario);

                var model = new DetalleManifiestoRSModel();
                model.Id_Encriptado = IdManifiesto;
                model.Id_Manifiesto_RS = IdDecrypted;
                model.Serie_Manifiesto = data.Serie_Manifiesto;
                model.Num_Manifiesto = data.Num_Manifiesto;
                model.Fecha_Emision_Manifiesto = data.Fecha_Emision_Manifiesto.ToString("yyyy-MM-dd");
                model.Observacion_Manifiesto = data.Observacion_Manifiesto;
                model.Id_Residuo_Solido = data.Id_Resido_Solido;
                model.Cantidad_RS_Evacuado = data.Cantidad_RS_Evacuado;
                model.Id_EO_RS_Salida = data.Id_EO_RS_Salida;
                model.Registro_EO_RS_Salida = data.Registro_EO_RS_Salida;
                model.Fecha_Vencimiento_Registro_Salida = data.Fecha_Vencimiento_Registro_Salida!=null? data.Fecha_Vencimiento_Registro_Salida.Value.ToString("yyyy-MM-dd") :"";
                model.Num_Autorizacion_Municipal_Salida = data.Num_Autorizacion_Municipal_Salida;
                model.Num_Autorizacion_Hoja_Ruta = data.Num_Autorizacion_Hoja_Ruta;
                //Conductor
                model.Id_Conductor = data.Id_Conductor;
                model.Num_Licencia_Condutor = data.Num_Licencia_Condutor;
                model.Id_Vehiculo = data.Id_Vehiculo;
                model.Capacidad_Vehiculo = data.Capacidad_Vehiculo;
                model.Id_EO_RS_Llegada = data.Id_EO_RS_Llegada;
                model.Registro_EO_RS_Llegada = data.Registro_EO_RS_Llegada;
                model.Fecha_Vencimiento_Registro_Llegada = data.Fecha_Vencimiento_Registro_Llegada!= null ? data.Fecha_Vencimiento_Registro_Llegada.Value.ToString("yyyy-MM-dd"):"";
                model.Num_Autorizacion_Sanitaria_Llegada = data.Num_Autorizacion_Sanitaria_Llegada;
                model.Num_Autorizacion_Municipal_Llegada = data.Num_Autorizacion_Municipal_Llegada;
                model.Permiso = data.Permiso;
                model.Estado = data.Estado;
                //Asesor
                model.Cantidad_RS_Dispuesto = data.Cantidad_RS_Dispuesto!=null? data.Cantidad_RS_Dispuesto.Value:0;
                if(data.Fecha_Devolucion!=null)
                    model.Fecha_Devolucion = data.Fecha_Devolucion.Value.ToString("yyyy-MM-dd");
                model.Id_Asesor = data.Id_Asesor_MAP;
                model.Certificado = data.Certificado_Trat_Disp;
                model.Informe_Transporte = data.Informe_Transporte;
                model.Manifiesto_Doc = data.Manifiesto;

                var items = await _getSelectItems.Execute();
                var residuos_solidos = items.ItemsResiduoSolido.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Residuo_Solido.ToString(), Text = g.Descripcion }).ToList();
                residuos_solidos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var eo_rs = items.ItemsEO_RS.OrderBy(x => x.Razon_Social).Select(g => new SelectListItem() { Value = g.Id_EO_RS.ToString(), Text = g.Razon_Social }).ToList();
                eo_rs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var asesores_map = items.ItemsAsesoresMA.OrderBy(x => x.Nombres).Select(x => new SelectListItem() { Value = x.IdUsuario.ToString(), Text = x.Nombres + " " + x.Apellidos }).ToList();
                asesores_map.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var conductores = items.ItemsConductor.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.Id_Conductor_RS.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                conductores.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var vehiculos = items.ItemsVehiculo.OrderBy(x => x.Placa).Select(g => new SelectListItem() { Value = g.Id_Vehiculo_RS.ToString(), Text = g.Placa }).ToList();
                vehiculos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Conductores = conductores;
                ViewBag.Vehiculos = vehiculos;
                ViewBag.ResiduosSolidos = residuos_solidos;
                ViewBag.EO_RS = eo_rs;
                ViewBag.AsesoresMAP = asesores_map;

                return View(model);
            }
        }
        public async Task<IActionResult> CompletarManifiesto(string IdManifiesto)
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
                var IdDecrypted = Convert.ToInt32(_protector.Unprotect(IdManifiesto));
                var data = await _consultarManifiestoRS.Execute(IdDecrypted, userInfo.NombreUsuario);
                var model = new CompletarManifiestoRSModel();
                model.Id_Encriptado = IdManifiesto;
                model.Id_Manifiesto_RS = IdDecrypted;
                model.Serie_Manifiesto = data.Serie_Manifiesto;
                model.Num_Manifiesto = data.Num_Manifiesto;
                model.Fecha_Emision_Manifiesto = data.Fecha_Emision_Manifiesto.ToString("yyyy-MM-dd");
                model.Observacion_Manifiesto = data.Observacion_Manifiesto;
                model.Id_Residuo_Solido = data.Id_Resido_Solido;
                model.Cantidad_RS_Evacuado = data.Cantidad_RS_Evacuado;
                model.Id_EO_RS_Salida = data.Id_EO_RS_Salida;
                model.Registro_EO_RS_Salida = data.Registro_EO_RS_Salida;
                model.Fecha_Vencimiento_Registro_Salida = data.Fecha_Vencimiento_Registro_Salida!=null? data.Fecha_Vencimiento_Registro_Salida.Value.ToString("yyyy-MM-dd") : "";
                model.Num_Autorizacion_Municipal_Salida = data.Num_Autorizacion_Municipal_Salida;
                model.Num_Autorizacion_Hoja_Ruta = data.Num_Autorizacion_Hoja_Ruta;
                //Conductor
                model.Id_Conductor = data.Id_Conductor;
                model.Num_Licencia_Condutor = data.Num_Licencia_Condutor;
                model.Id_Vehiculo = data.Id_Vehiculo;
                model.Capacidad_Vehiculo = data.Capacidad_Vehiculo;
                model.Id_EO_RS_Llegada = data.Id_EO_RS_Llegada;
                model.Registro_EO_RS_Llegada = data.Registro_EO_RS_Llegada;
                model.Fecha_Vencimiento_Registro_Llegada = data.Fecha_Vencimiento_Registro_Llegada!=null? data.Fecha_Vencimiento_Registro_Llegada.Value.ToString("yyyy-MM-dd"):"";
                model.Num_Autorizacion_Sanitaria_Llegada = data.Num_Autorizacion_Sanitaria_Llegada;
                model.Num_Autorizacion_Municipal_Llegada = data.Num_Autorizacion_Municipal_Llegada;
                model.Estado = data.Estado;
                model.Permiso = data.Permiso;
                //Asesor
                model.Cantidad_RS_Dispuesto = data.Cantidad_RS_Dispuesto!=null ? data.Cantidad_RS_Dispuesto.Value : 0;
                model.Fecha_Devolucion = data.Fecha_Devolucion != null ? data.Fecha_Devolucion.Value.ToString("yyyy-MM-dd") : null;
                model.Id_Asesor = data.Id_Asesor_MAP != null ? data.Id_Asesor_MAP : 0;

                model.Certificado_Lleno = data.Certificado_Trat_Disp != null ? JsonConvert.SerializeObject(data.Certificado_Trat_Disp) : null;
                model.Informe_Transporte_Lleno = data.Informe_Transporte != null ? JsonConvert.SerializeObject(data.Informe_Transporte) : null;
                model.Manifiesto_Doc_Lleno = data.Manifiesto != null ? JsonConvert.SerializeObject(data.Manifiesto) : null;

                var items = await _getSelectItems.Execute();
                var residuos_solidos = items.ItemsResiduoSolido.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Residuo_Solido.ToString(), Text = g.Descripcion }).ToList();
                residuos_solidos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var eo_rs = items.ItemsEO_RS.OrderBy(x => x.Razon_Social).Select(g => new SelectListItem() { Value = g.Id_EO_RS.ToString(), Text = g.Razon_Social }).ToList();
                eo_rs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var asesores_map = items.ItemsAsesoresMA.OrderBy(x => x.Nombres).Select(x => new SelectListItem() { Value = x.IdUsuario.ToString(), Text = x.Nombres + " " + x.Apellidos }).ToList();
                asesores_map.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var conductores = items.ItemsConductor.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.Id_Conductor_RS.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                conductores.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var vehiculos = items.ItemsVehiculo.OrderBy(x => x.Placa).Select(g => new SelectListItem() { Value = g.Id_Vehiculo_RS.ToString(), Text = g.Placa }).ToList();
                vehiculos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Conductores = conductores;
                ViewBag.Vehiculos = vehiculos;
                ViewBag.ResiduosSolidos = residuos_solidos;
                ViewBag.EO_RS = eo_rs;
                ViewBag.AsesoresMAP = asesores_map;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CompletarManifiesto(CompletarManifiestoRSModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var certificados = new List<string>();
                var informes = new List<string>();
                var manifiesto = new List<string>();
                var max_file_size = 8388608;

                if (model.Certificado != null)
                {
                    for (int i = 0; i < model.Certificado.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Certificado[i].FileName);

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPGE", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Certificado[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName("cert") + ".zip";
                            var filePath = await UploadZipFileAsync(model.Certificado[i], newfileName, "cert");
                            certificados.Add(filePath);
                        }
                    }
                }
                else if (model.Certificado_Lleno != null)
                {
                    certificados.AddRange(new List<string>(model.Certificado_Lleno.Split(',')));
                }
                if (model.Informe_Transporte != null)
                {
                    for (int i = 0; i < model.Informe_Transporte.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Informe_Transporte[i].FileName);

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPGE", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Informe_Transporte[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName("inf") + ".zip";
                            var filePath = await UploadZipFileAsync(model.Informe_Transporte[i], newfileName, "inf");
                            informes.Add(filePath);
                        }
                    }
                }
                else if (model.Informe_Transporte_Lleno != null)
                {
                    informes.AddRange(new List<string>(model.Informe_Transporte_Lleno.Split(',')));
                }
                if (model.Manifiesto_Doc != null)
                {
                    for (int i = 0; i < model.Manifiesto_Doc.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Manifiesto_Doc[i].FileName);

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPGE", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Manifiesto_Doc[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName("man") + ".zip";
                            var filePath = await UploadZipFileAsync(model.Manifiesto_Doc[i], newfileName, "man");
                            manifiesto.Add(filePath);
                        }
                    }
                }
                else if (model.Manifiesto_Doc_Lleno != null)
                {
                    manifiesto.AddRange(new List<string>(model.Manifiesto_Doc_Lleno.Split(',')));
                }

                await _completarManifiestoRS.Execute(
                    model.Id_Manifiesto_RS,
                    model.opcion,
                    model.Serie_Manifiesto,
                    model.Num_Manifiesto,
                    model.Fecha_Emision_Manifiesto,
                    model.Observacion_Manifiesto,
                    model.Id_Residuo_Solido,
                    model.Cantidad_RS_Evacuado,
                    model.Id_EO_RS_Salida,
                    model.Registro_EO_RS_Salida,
                    model.Fecha_Vencimiento_Registro_Salida,
                    model.Num_Autorizacion_Municipal_Salida,
                    model.Num_Autorizacion_Hoja_Ruta,
                    model.Id_Conductor,
                    model.Num_Licencia_Condutor,
                    model.Id_Vehiculo,
                    model.Capacidad_Vehiculo,
                    model.Id_EO_RS_Llegada,
                    model.Registro_EO_RS_Llegada,
                    model.Fecha_Vencimiento_Registro_Llegada,
                    model.Num_Autorizacion_Sanitaria_Llegada,
                    model.Num_Autorizacion_Municipal_Llegada,
                    certificados,
                    model.Cantidad_RS_Dispuesto,
                    model.Fecha_Devolucion,
                    informes,
                    manifiesto,
                    model.Id_Asesor,
                    userInfo.NombreUsuario);
                return RedirectToAction("Resumen");
            }
            return RedirectToAction("CompletarManifiesto",new { IdManifiesto = model.Id_Encriptado });
        }
        public async Task<IActionResult> EliminarManifiesto(string IdManifiesto)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var Id_Decrypted = Convert.ToInt32(_protector.Unprotect(IdManifiesto));
            await _eliminarManifiestoRS.Execute(Id_Decrypted, userInfo.NombreUsuario);
            return RedirectToAction("Resumen");
        }
        private string CreateUniqueFileName(string tipo)
        {
            switch (tipo)
            {
                case "cert": return "Ev_RS_Certi_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss"); break;
                case "inf": return "Ev_RS_Infor_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss"); break;
                default: return "Ev_RS_Manif_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss"); break;
            }
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".zip","application/zip" }
            };
        }
        public async Task<IActionResult> Download(string fileName, string tipo)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                try
                {
                    var encryptedFile = await DownloadFTPFileAsync(fileName, tipo);

                    /* Save encrypted file in temp path*/
                    var tempPath = Path.GetTempPath();

                    var SGATempPath = tempPath + "SGA-Marcobre\\";
                    if (!Directory.Exists(SGATempPath))
                    {
                        Directory.CreateDirectory(SGATempPath);
                    }
                    var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
                    var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";

                    /*decrypt file*/
                    var decryptedFileName = "desencriptado.zip";
                    await DecryptFileAsync(encryptedFile, archivePath + decryptedFileName);
                    /*read decrypted file*/
                    FileStream stream = new FileStream(archivePath + decryptedFileName, FileMode.Open);

                    return File(stream, GetContentType(decryptedFileName), fileName);
                }
                catch (Exception e)
                {
                    return Content("No se encontró el archivo: " + e.Message);
                }
            }
        }
        public async Task<string> UploadFTPFileAsync(string path, string newFileName, string tipo)
        {
            /*if (!(HttpContext.Session.GetString("SessionUser") != null))
                return "NoAutorizado";
            else
            {*/
            Tipo_Info_SGA directorio;
            switch (tipo)
            {
                case "cert": directorio = Tipo_Info_SGA.FTPConnectionEvidenciaRSC; break;
                case "inf": directorio = Tipo_Info_SGA.FTPConnectionEvidenciaRSI; break;
                default: directorio = Tipo_Info_SGA.FTPConnectionEvidenciaRSM; break;
            }
            var serverInfo = await _info_SGA.Execute(directorio);

            string host = "", port = "", user = "", pass = "", dir = "";
            foreach (var item in serverInfo.Info)
            {
                switch (item.Key)
                {
                    case "host": host = item.Value; break;
                    case "port": port = item.Value; break;
                    case "user": user = item.Value; break;
                    case "password": pass = item.Value; break;
                    case "directory": dir = item.Value; break;
                }
            }

            using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
            {
                sftp.Connect();
                sftp.ChangeDirectory(dir);
                using (FileStream fs = System.IO.File.OpenRead(path))
                {
                    sftp.UploadFile(fs, newFileName, true);
                }
                sftp.Dispose();
            }
            return newFileName;
            //}
        }
        public async Task<string> UploadZipFileAsync(IFormFile file, string newFileName, string tipo)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return "NoAutorizado";
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "SGA-Marcobre/";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission/" + userInfo.NombreUsuario + "/";
                var archiveFile = SGATempPath + "zip/" + userInfo.NombreUsuario + "/archive.zip";
                var archivePath = SGATempPath + "zip/" + userInfo.NombreUsuario + "/";

                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                }
                if (Directory.Exists(archivePath))
                {
                    Directory.Delete(archivePath, true);
                }

                Directory.CreateDirectory(filePath);
                Directory.CreateDirectory(archivePath);

                var fileName = filePath + file.FileName;
                if (file.Length > 0)
                {
                    var stream = new FileStream(fileName, FileMode.Create);
                    //CopyToAsync corrompe los archivos
                    file.CopyTo(stream);
                    stream.Close();
                }
                ZipFile.CreateFromDirectory(filePath, archiveFile);

                var encryptedArchiveFile = SGATempPath + "zip/" + userInfo.NombreUsuario + "/encriptado.zip";
                await EncryptFileAsync(archiveFile, archivePath + "encriptado.zip");

                return await UploadFTPFileAsync(encryptedArchiveFile, newFileName, tipo);
            }
        }
        public async Task<string> DownloadFTPFileAsync(string fileName, string tipo)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return "NoAutorizado";
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                Tipo_Info_SGA directorio;
                switch (tipo)
                {
                    case "cert": directorio = Tipo_Info_SGA.FTPConnectionEvidenciaRSC; break;
                    case "inf": directorio = Tipo_Info_SGA.FTPConnectionEvidenciaRSI; break;
                    default: directorio = Tipo_Info_SGA.FTPConnectionEvidenciaRSM; break;
                }
                var serverInfo = await _info_SGA.Execute(directorio);
                string host = "", port = "", user = "", pass = "", dir = "";
                foreach (var item in serverInfo.Info)
                {
                    switch (item.Key)
                    {
                        case "host": host = item.Value; break;
                        case "port": port = item.Value; break;
                        case "user": user = item.Value; break;
                        case "password": pass = item.Value; break;
                        case "directory": dir = item.Value; break;
                    }
                }

                /* Save file in application server to decrypt */
                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "SGA-Marcobre/";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission/" + userInfo.NombreUsuario + "/";
                var archivePath = SGATempPath + "zip/" + userInfo.NombreUsuario + "/";

                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                }
                if (Directory.Exists(archivePath))
                {
                    Directory.Delete(archivePath, true);
                }

                Directory.CreateDirectory(filePath);
                Directory.CreateDirectory(archivePath);
                /*New file downloaded name*/
                var downloadedFilePath = archivePath + fileName;
                /*Copy data to new temp file*/
                var fileStream = System.IO.File.Create(downloadedFilePath);
                using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
                {
                    sftp.Connect();
                    sftp.ChangeDirectory(dir);

                    //Descargar archivo de sftp a nuevo archivo temporal creado
                    sftp.DownloadFile(fileName, fileStream);

                    sftp.Dispose();
                }
                //ftpStream.CopyTo(fileStream);
                fileStream.Close();

                return downloadedFilePath;
            }
        }
        private async Task EncryptFileAsync(string inputFile, string outputFile)
        {
            var dataToEncrypt = await _info_SGA.Execute(Tipo_Info_SGA.FileEncryption);

            var key = new byte[32];
            var IV = new byte[16];

            foreach (var item in dataToEncrypt.Info)
            {
                switch (item.Key)
                {
                    case "key": key = Encoding.UTF8.GetBytes(item.Value); break;
                    case "iv": IV = Encoding.UTF8.GetBytes(item.Value); break;
                }
            }

            string cryptFile = outputFile;
            FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

            RijndaelManaged RMCrypto = new RijndaelManaged();

            CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateEncryptor(key, IV),
                CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            int data;
            while ((data = fsIn.ReadByte()) != -1)
                cs.WriteByte((byte)data);


            fsIn.Close();
            cs.Close();
            fsCrypt.Close();
        }
        private async Task DecryptFileAsync(string inputFile, string outputFile)
        {
            var dataToEncrypt = await _info_SGA.Execute(Tipo_Info_SGA.FileEncryption);

            var key = new byte[32];
            var IV = new byte[16];

            foreach (var item in dataToEncrypt.Info)
            {
                switch (item.Key)
                {
                    case "key": key = Encoding.UTF8.GetBytes(item.Value); break;
                    case "iv": IV = Encoding.UTF8.GetBytes(item.Value); break;
                }
            }
            RijndaelManaged RMCrypto = new RijndaelManaged();

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

            CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateDecryptor(key, IV),
                CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int data;
            while ((data = cs.ReadByte()) != -1)
                fsOut.WriteByte((byte)data);

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();

        }
    }

}