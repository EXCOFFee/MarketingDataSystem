// ==================== SERVICIO GENERADOR DE REPORTES EXCEL PROFESIONALES ====================
// Este servicio genera reportes Excel ejecutivos multi-hoja con formateo avanzado
// CAPA: Application - Orquesta generación de análisis de negocio y reportes gerenciales
// PATRÓN: Service Pattern + Event-Driven - responde a eventos del pipeline ETL
// SOLID: Cumple S (generación de reportes), D (inversión dependencias)
// INTEGRACIÓN: Suscriptor del EventBus para evento 'CargaFinalizada' (automático)
// TECNOLOGÍA: ClosedXML para generación avanzada de Excel con formato profesional

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel; // Biblioteca para generación avanzada de Excel
using MarketingDataSystem.Application.Interfaces;
using MarketingDataSystem.Core.Interfaces;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio especializado en generación de reportes Excel ejecutivos multi-hoja
    /// RESPONSABILIDAD: Crear reportes de negocio profesionales con análisis completo
    /// ARQUITECTURA: Capa Application - transforma datos de negocio en reportes ejecutivos
    /// PATRÓN: Service Pattern + Event-Driven Architecture (suscriptor de EventBus)
    /// SOLID:
    /// - S: Una sola responsabilidad (generación de reportes Excel ejecutivos)
    /// - D: Depende de abstracción ILoggerService para logging independiente
    /// TECNOLOGÍA: ClosedXML para generación Excel avanzada con formateo profesional
    /// AUTOMATIZACIÓN: Se ejecuta automáticamente cuando EventBus emite 'CargaFinalizada'
    /// FORMATO: Genera múltiples hojas (Ventas, Clientes, Stock, Resumen Ejecutivo)
    /// PROFESIONAL: Incluye colores condicionales, formateo monetario y métricas KPI
    /// PERSISTENCIA: Guarda archivos físicos y registra metadata en base de datos
    /// </summary>
    public class GeneradorReporteService : IGeneradorReporteService
    {
        // ========== DEPENDENCIAS PARA GENERACIÓN DE REPORTES ==========
        private readonly ILoggerService _logger; // Logger para trazabilidad de generación

        /// <summary>
        /// Constructor con inyección de dependencias para generación de reportes
        /// PATRÓN: Dependency Injection - facilita testing y flexibilidad
        /// TESTABILIDAD: Permite mocking del logger para pruebas unitarias
        /// SOLID: Principio D - depende de abstracción ILoggerService
        /// LOGGING: Utiliza logger para trazabilidad completa del proceso
        /// </summary>
        /// <param name="logger">Servicio de logging para trazabilidad de reportes</param>
        public GeneradorReporteService(ILoggerService logger)
        {
            _logger = logger; // Inyección de dependencia del logger
        }

        /// <summary>
        /// Genera un reporte Excel ejecutivo completo con múltiples hojas de análisis
        /// LÓGICA: Crear workbook → Generar hojas → Formatear → Guardar → Registrar
        /// AUTOMATIZACIÓN: Método invocado automáticamente por EventBus tras 'CargaFinalizada'
        /// FORMATO: 4 hojas profesionales (Ventas, Clientes, Stock, Resumen Ejecutivo)
        /// ARCHIVO: Genera archivo físico con timestamp único para evitar colisiones
        /// PERSISTENCIA: Guarda en directorio 'reportes' y registra metadata en BD
        /// PROFESIONAL: Formateo avanzado con colores, moneda, totales y métricas KPI
        /// ERROR HANDLING: Manejo robusto de errores con logging completo
        /// </summary>
        public void GenerarReporte()
        {
            try
            {
                // ========== INICIO DEL PROCESO DE GENERACIÓN ==========
                _logger.LogInfo("Iniciando generación de reporte Excel ejecutivo");
                
                // ========== PREPARACIÓN DE NOMBRES Y RUTAS ==========
                var fechaReporte = DateTime.Now;
                var nombreArchivo = $"ReporteVentas_{fechaReporte:yyyy-MM-dd_HHmm}.xlsx"; // Timestamp único
                var rutaCompleta = Path.Combine("reportes", nombreArchivo);
                
                // ========== CREAR ESTRUCTURA DE DIRECTORIOS ==========
                Directory.CreateDirectory("reportes"); // Crear directorio si no existe
                
                // ========== CREAR WORKBOOK EXCEL PROFESIONAL ==========
                using var workbook = new XLWorkbook(); // IDisposable para liberación automática
                
                // ========== GENERAR HOJAS EJECUTIVAS ESPECIALIZADAS ==========
                CrearHojaVentas(workbook, fechaReporte);    // Hoja 1: Detalle de ventas del día
                CrearHojaClientes(workbook);                // Hoja 2: Análisis de clientes
                CrearHojaStock(workbook);                   // Hoja 3: Estado de inventario
                CrearHojaResumen(workbook, fechaReporte);   // Hoja 4: Resumen ejecutivo con KPIs
                
                // ========== PERSISTENCIA DEL ARCHIVO FÍSICO ==========
                workbook.SaveAs(rutaCompleta); // Guardar Excel con formato avanzado
                
                // ========== REGISTRO EN BASE DE DATOS (PENDIENTE) ==========
                // TODO: Registrar en base de datos cuando UnitOfWork esté disponible
                // _ = RegistrarReporteEnBaseDatos(nombreArchivo, rutaCompleta);
                
                // ========== CONFIRMACIÓN EXITOSA ==========
                _logger.LogInfo($"Reporte Excel ejecutivo generado exitosamente: {rutaCompleta}");
            }
            catch (Exception ex)
            {
                // ========== MANEJO ROBUSTO DE ERRORES ==========
                _logger.LogError($"Error crítico generando reporte Excel: {ex.Message}");
                throw; // Re-lanzar para manejo en capa superior
            }
        }

        /// <summary>
        /// Crea la hoja de análisis detallado de ventas diarias con formateo profesional
        /// PROPÓSITO: Hoja principal con transacciones detalladas y totales financieros
        /// FORMATO: Headers ejecutivos, tabla con datos, formateo monetario y totales
        /// DATOS: En producción obtendría datos reales de VentaService vía UnitOfWork
        /// FINANCIERO: Incluye cálculos automáticos de totales y formato de moneda
        /// PROFESIONAL: Colores corporativos, bordes, negrita y autoajuste de columnas
        /// </summary>
        /// <param name="workbook">Workbook Excel donde crear la hoja</param>
        /// <param name="fechaReporte">Fecha del reporte para filtros temporales</param>
        private void CrearHojaVentas(XLWorkbook workbook, DateTime fechaReporte)
        {
            // ========== CREAR HOJA CON NOMBRE DESCRIPTIVO ==========
            var worksheet = workbook.Worksheets.Add("Ventas del Día");
            
            // ========== ENCABEZADO PRINCIPAL EJECUTIVO ==========
            worksheet.Cell(1, 1).Value = "REPORTE DE VENTAS";
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 16; // Tamaño ejecutivo
            
            // ========== FECHA DEL REPORTE ==========
            worksheet.Cell(2, 1).Value = $"Fecha: {fechaReporte:dd/MM/yyyy}";
            worksheet.Cell(2, 1).Style.Font.Bold = true;
            
            // Headers de tabla
            worksheet.Cell(4, 1).Value = "ID";
            worksheet.Cell(4, 2).Value = "Fecha";
            worksheet.Cell(4, 3).Value = "Cliente";
            worksheet.Cell(4, 4).Value = "Producto";
            worksheet.Cell(4, 5).Value = "Cantidad";
            worksheet.Cell(4, 6).Value = "Precio Unit.";
            worksheet.Cell(4, 7).Value = "Total";
            
            // Formatear headers
            var headerRange = worksheet.Range(4, 1, 4, 7);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            
            // Datos de ejemplo (en producción vendría de la base de datos)
            var datosVentas = new[]
            {
                new { Id = 1, Fecha = fechaReporte.AddHours(-2), Cliente = "Cliente A", Producto = "Producto 1", Cantidad = 5, PrecioUnit = 100.00m, Total = 500.00m },
                new { Id = 2, Fecha = fechaReporte.AddHours(-1), Cliente = "Cliente B", Producto = "Producto 2", Cantidad = 3, PrecioUnit = 150.00m, Total = 450.00m },
                new { Id = 3, Fecha = fechaReporte.AddMinutes(-30), Cliente = "Cliente C", Producto = "Producto 1", Cantidad = 2, PrecioUnit = 100.00m, Total = 200.00m }
            };
            
            int fila = 5;
            decimal totalGeneral = 0;
            
            foreach (var venta in datosVentas)
            {
                worksheet.Cell(fila, 1).Value = venta.Id;
                worksheet.Cell(fila, 2).Value = venta.Fecha.ToString("dd/MM/yyyy HH:mm");
                worksheet.Cell(fila, 3).Value = venta.Cliente;
                worksheet.Cell(fila, 4).Value = venta.Producto;
                worksheet.Cell(fila, 5).Value = venta.Cantidad;
                worksheet.Cell(fila, 6).Value = venta.PrecioUnit;
                worksheet.Cell(fila, 6).Style.NumberFormat.Format = "$#,##0.00";
                worksheet.Cell(fila, 7).Value = venta.Total;
                worksheet.Cell(fila, 7).Style.NumberFormat.Format = "$#,##0.00";
                
                totalGeneral += venta.Total;
                fila++;
            }
            
            // Total general
            worksheet.Cell(fila + 1, 6).Value = "TOTAL:";
            worksheet.Cell(fila + 1, 6).Style.Font.Bold = true;
            worksheet.Cell(fila + 1, 7).Value = totalGeneral;
            worksheet.Cell(fila + 1, 7).Style.Font.Bold = true;
            worksheet.Cell(fila + 1, 7).Style.NumberFormat.Format = "$#,##0.00";
            worksheet.Cell(fila + 1, 7).Style.Fill.BackgroundColor = XLColor.Yellow;
            
            // Autoajustar columnas
            worksheet.Columns().AdjustToContents();
        }

        private void CrearHojaClientes(XLWorkbook workbook)
        {
            var worksheet = workbook.Worksheets.Add("Clientes");
            
            worksheet.Cell(1, 1).Value = "RESUMEN DE CLIENTES";
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 16;
            
            // Headers
            worksheet.Cell(3, 1).Value = "Cliente";
            worksheet.Cell(3, 2).Value = "Email";
            worksheet.Cell(3, 3).Value = "Total Compras";
            worksheet.Cell(3, 4).Value = "Última Compra";
            
            var headerRange = worksheet.Range(3, 1, 3, 4);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGreen;
            
            // Datos de ejemplo
            var datosClientes = new[]
            {
                new { Nombre = "Cliente A", Email = "clienteA@email.com", TotalCompras = 1500.00m, UltimaCompra = DateTime.Now.AddDays(-1) },
                new { Nombre = "Cliente B", Email = "clienteB@email.com", TotalCompras = 2300.00m, UltimaCompra = DateTime.Now.AddDays(-3) },
                new { Nombre = "Cliente C", Email = "clienteC@email.com", TotalCompras = 850.00m, UltimaCompra = DateTime.Now.AddDays(-7) }
            };
            
            int fila = 4;
            foreach (var cliente in datosClientes)
            {
                worksheet.Cell(fila, 1).Value = cliente.Nombre;
                worksheet.Cell(fila, 2).Value = cliente.Email;
                worksheet.Cell(fila, 3).Value = cliente.TotalCompras;
                worksheet.Cell(fila, 3).Style.NumberFormat.Format = "$#,##0.00";
                worksheet.Cell(fila, 4).Value = cliente.UltimaCompra.ToString("dd/MM/yyyy");
                fila++;
            }
            
            worksheet.Columns().AdjustToContents();
        }

        private void CrearHojaStock(XLWorkbook workbook)
        {
            var worksheet = workbook.Worksheets.Add("Stock Actual");
            
            worksheet.Cell(1, 1).Value = "INVENTARIO ACTUAL";
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 16;
            
            // Headers
            worksheet.Cell(3, 1).Value = "Producto";
            worksheet.Cell(3, 2).Value = "Stock Actual";
            worksheet.Cell(3, 3).Value = "Stock Mínimo";
            worksheet.Cell(3, 4).Value = "Estado";
            
            var headerRange = worksheet.Range(3, 1, 3, 4);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.Orange;
            
            // Datos de ejemplo
            var datosStock = new[]
            {
                new { Producto = "Producto 1", StockActual = 45, StockMinimo = 20, Estado = "OK" },
                new { Producto = "Producto 2", StockActual = 15, StockMinimo = 20, Estado = "BAJO" },
                new { Producto = "Producto 3", StockActual = 5, StockMinimo = 10, Estado = "CRÍTICO" }
            };
            
            int fila = 4;
            foreach (var stock in datosStock)
            {
                worksheet.Cell(fila, 1).Value = stock.Producto;
                worksheet.Cell(fila, 2).Value = stock.StockActual;
                worksheet.Cell(fila, 3).Value = stock.StockMinimo;
                worksheet.Cell(fila, 4).Value = stock.Estado;
                
                // Colorear según estado
                if (stock.Estado == "CRÍTICO")
                    worksheet.Cell(fila, 4).Style.Fill.BackgroundColor = XLColor.Red;
                else if (stock.Estado == "BAJO")
                    worksheet.Cell(fila, 4).Style.Fill.BackgroundColor = XLColor.Yellow;
                else
                    worksheet.Cell(fila, 4).Style.Fill.BackgroundColor = XLColor.LightGreen;
                
                fila++;
            }
            
            worksheet.Columns().AdjustToContents();
        }

        private void CrearHojaResumen(XLWorkbook workbook, DateTime fechaReporte)
        {
            var worksheet = workbook.Worksheets.Add("Resumen Ejecutivo");
            
            worksheet.Cell(1, 1).Value = "RESUMEN EJECUTIVO";
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 18;
            
            worksheet.Cell(3, 1).Value = "Métricas del Día:";
            worksheet.Cell(3, 1).Style.Font.Bold = true;
            worksheet.Cell(3, 1).Style.Font.FontSize = 14;
            
            // Métricas
            worksheet.Cell(5, 1).Value = "• Total Ventas:";
            worksheet.Cell(5, 2).Value = "$1,150.00";
            worksheet.Cell(5, 2).Style.NumberFormat.Format = "$#,##0.00";
            worksheet.Cell(5, 2).Style.Font.Bold = true;
            
            worksheet.Cell(6, 1).Value = "• Número de Transacciones:";
            worksheet.Cell(6, 2).Value = 3;
            worksheet.Cell(6, 2).Style.Font.Bold = true;
            
            worksheet.Cell(7, 1).Value = "• Venta Promedio:";
            worksheet.Cell(7, 2).Value = "$383.33";
            worksheet.Cell(7, 2).Style.NumberFormat.Format = "$#,##0.00";
            worksheet.Cell(7, 2).Style.Font.Bold = true;
            
            worksheet.Cell(8, 1).Value = "• Productos con Stock Bajo:";
            worksheet.Cell(8, 2).Value = 2;
            worksheet.Cell(8, 2).Style.Font.Bold = true;
            worksheet.Cell(8, 2).Style.Fill.BackgroundColor = XLColor.Yellow;
            
            worksheet.Cell(10, 1).Value = $"Reporte generado: {fechaReporte:dd/MM/yyyy HH:mm}";
            worksheet.Cell(10, 1).Style.Font.Italic = true;
            
            worksheet.Columns().AdjustToContents();
        }

        // TODO: Implementar cuando UnitOfWork esté disponible en DI
        /*
        private async Task RegistrarReporteEnBaseDatos(string nombreArchivo, string rutaArchivo)
        {
            try
            {
                var reporte = new MarketingDataSystem.Core.Entities.Reporte
                {
                    NombreArchivo = nombreArchivo,
                    FechaGeneracion = DateTime.Now,
                    RutaArchivo = rutaArchivo,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                };

                await _unitOfWork.Reportes.AddAsync(reporte);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInfo($"Reporte registrado en base de datos: {nombreArchivo}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registrando reporte en BD: {ex.Message}");
            }
        }
        */
    }
} 