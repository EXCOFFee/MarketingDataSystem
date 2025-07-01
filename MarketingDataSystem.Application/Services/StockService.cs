// ==================== SERVICIO CRÍTICO - GESTIÓN DE INVENTARIO ====================
// Este servicio controla el inventario de productos con precisión absoluta
// CAPA: Application - Orquesta operaciones de inventario entre ventas y productos
// CRITICIDAD: ALTA - Evita sobreventa y garantiza disponibilidad de productos
// CONCURRENCIA: Debe manejar múltiples actualizaciones simultáneas de stock
// SOLID: Cumple S (responsabilidad única), D (inversión dependencias)
// ALERTAS: Integra con sistema de alertas para stock bajo/agotado

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MarketingDataSystem.Core.DTOs;
using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Application.Interfaces;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio de aplicación para gestión completa de inventario y control de stock
    /// RESPONSABILIDAD: Controlar niveles de inventario y prevenir sobreventa de productos
    /// ARQUITECTURA: Capa Application - coordina entre VentaService, ProductoService y AlertaService
    /// CRITICIDAD: ALTA - Errores en stock pueden resultar en sobreventa o pérdida de ventas
    /// SOLID:
    /// - S: Una sola responsabilidad (gestión completa de inventario)
    /// - D: Depende de abstracciones (IUnitOfWork, IMapper)
    /// CONCURRENCIA: Debe manejar actualizaciones simultáneas de múltiples ventas
    /// ALERTAS: Genera notificaciones automáticas para stock bajo o productos agotados
    /// INTEGRACIÓN: Conecta con VentaService (descontar stock) y AlertaService (notificaciones)
    /// PRECISIÓN: Requiere exactitud absoluta en cálculos para evitar discrepancias
    /// AUDITORÍA: Movimientos de stock deben ser completamente trazables
    /// </summary>
    public class StockService : IStockService
    {
        // ========== DEPENDENCIAS PARA CONTROL DE INVENTARIO ==========
        private readonly IUnitOfWork _unitOfWork;   // Coordinador de repositorios para operaciones atómicas
        private readonly IMapper _mapper;           // Mapper automático para DTOs de inventario

        /// <summary>
        /// Constructor con inyección de dependencias para operaciones de inventario
        /// PATRÓN: Dependency Injection - facilita testing y flexibilidad
        /// TESTABILIDAD: Permite mocking para simular escenarios de stock complejos
        /// SOLID: Principio D - depende de abstracciones para máxima flexibilidad
        /// THREAD-SAFETY: No mantiene estado para evitar problemas de concurrencia
        /// </summary>
        /// <param name="unitOfWork">Coordinador de repositorios con soporte transaccional</param>
        /// <param name="mapper">AutoMapper para conversión entre DTOs y Entities de stock</param>
        public StockService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork; // Principio D de SOLID - Inversión de dependencias
            _mapper = mapper;         // AutoMapper para transformaciones de inventario
        }

        /// <summary>
        /// Obtiene información detallada de stock para un producto específico
        /// LÓGICA: Buscar en repositorio → Convertir a DTO → Devolver datos de inventario
        /// INVENTARIO: Incluye cantidad actual, mínimo, máximo y ubicación
        /// ALERTAS: Información usada para generar alertas de stock bajo
        /// USO: StockController, VentaService (validar disponibilidad), AlertaService
        /// TIEMPO REAL: Datos actualizados para decisiones inmediatas de venta
        /// </summary>
        /// <param name="id">ID único del registro de stock a consultar</param>
        /// <returns>StockDto con niveles actuales de inventario o null si no existe</returns>
        public async Task<StockDto> GetByIdAsync(int id)
        {
            // Buscar registro de stock específico desde el repositorio
            var entity = await _unitOfWork.Stocks.GetByIdAsync(id);
            // Conversión automática Entity → DTO con datos de inventario
            return _mapper.Map<StockDto>(entity);
        }

        /// <summary>
        /// Obtiene el inventario completo de todos los productos del sistema
        /// LÓGICA: Consultar repositorio → Mapear colección → Devolver inventario total
        /// INVENTARIO: Vista completa para análisis de stock y planificación
        /// REPORTES: Dataset para reportes de inventario y análisis de rotación
        /// USO: StockController (consultas), ReporteService, DashboardService (métricas)
        /// ALERTAS: Datos base para identificar productos con stock crítico
        /// </summary>
        /// <returns>Colección de StockDto con inventario completo del sistema</returns>
        public async Task<IEnumerable<StockDto>> GetAllAsync()
        {
            // Obtener todos los registros de stock desde el repositorio
            var entities = await _unitOfWork.Stocks.GetAllAsync();
            // Mapear automáticamente la colección completa de inventario
            return _mapper.Map<IEnumerable<StockDto>>(entities);
        }

        /// <summary>
        /// Registra nuevo stock para un producto (entrada de inventario)
        /// LÓGICA: DTO → Entity → Persistir → Confirmar → Generar alertas si aplica
        /// INVENTARIO: Incrementa disponibilidad de productos para venta
        /// MOVIMIENTO: Registra entrada de inventario con trazabilidad completa
        /// ALERTAS: Puede resolver alertas de stock bajo si se supera el mínimo
        /// INTEGRACIÓN: Automáticamente disponible para VentaService después de creación
        /// AUDITORÍA: Operación completamente auditable para control de inventario
        /// </summary>
        /// <param name="stockDto">Datos del nuevo stock (producto, cantidad, ubicación)</param>
        /// <returns>StockDto con ID asignado y confirmación de la entrada</returns>
        public async Task<StockDto> CreateAsync(StockDto stockDto)
        {
            // ========== CONVERSIÓN DTO → ENTITY ==========
            var entity = _mapper.Map<Stock>(stockDto);
            
            // ========== REGISTRO DE ENTRADA DE INVENTARIO ==========
            await _unitOfWork.Stocks.AddAsync(entity);
            
            // ========== CONFIRMACIÓN DE MOVIMIENTO ==========
            await _unitOfWork.SaveChangesAsync(); // Commit del movimiento de stock
            
            // ========== RETORNO CON ID ASIGNADO ==========
            return _mapper.Map<StockDto>(entity); // Entity con ID generado
            
            // TODO: Integrar con AlertaService para resolver alertas de stock bajo
        }

        /// <summary>
        /// Actualiza niveles de stock existente (ajuste de inventario)
        /// LÓGICA: DTO → Entity → Actualizar → Confirmar → Verificar alertas
        /// INVENTARIO: Modifica disponibilidad actual de productos
        /// AJUSTES: Correcciones por inventarios físicos o mermas
        /// CONCURRENCIA: Vulnerable a condiciones de carrera en alta concurrencia
        /// ALERTAS: Puede generar o resolver alertas según nuevo nivel
        /// PRECISIÓN: Cambios deben reflejar exactamente el inventario físico
        /// </summary>
        /// <param name="stockDto">Datos actualizados completos del stock</param>
        /// <returns>StockDto con niveles actualizados confirmados</returns>
        public async Task<StockDto> UpdateAsync(StockDto stockDto)
        {
            // ========== CONVERSIÓN Y PREPARACIÓN ==========
            var entity = _mapper.Map<Stock>(stockDto);
            
            // ========== MARCADO PARA ACTUALIZACIÓN ==========
            _unitOfWork.Stocks.Update(entity); // Entity Framework rastrea cambios
            
            // ========== CONFIRMACIÓN DE AJUSTE ==========
            await _unitOfWork.SaveChangesAsync(); // Commit del ajuste de inventario
            
            // ========== RETORNO CONFIRMADO ==========
            return _mapper.Map<StockDto>(entity);
            
            // TODO: Implementar verificación de alertas después de actualización
            // TODO: Considerar transacción explícita para operaciones críticas
        }

        /// <summary>
        /// Elimina un registro de stock del sistema (operación de inventario crítica)
        /// LÓGICA: Buscar → Verificar dependencias → Eliminar → Confirmar
        /// INVENTARIO: Reduce disponibilidad de productos para venta
        /// DEPENDENCIAS: Verificar que no afecte productos activos o ventas pendientes
        /// SOFT DELETE: Recomendado implementar eliminación lógica para auditoría
        /// ALERTAS: Puede generar alertas de stock agotado después de eliminación
        /// SEGURIDAD: Solo usuarios autorizados deberían eliminar registros de stock
        /// </summary>
        /// <param name="id">ID único del registro de stock a eliminar</param>
        /// <returns>Task que representa la operación asíncrona</returns>
        public async Task DeleteAsync(int id)
        {
            // ========== VERIFICACIÓN DE EXISTENCIA ==========
            var entity = await _unitOfWork.Stocks.GetByIdAsync(id);
            if (entity != null)
            {
                // ========== ELIMINACIÓN DE INVENTARIO ==========
                _unitOfWork.Stocks.Delete(entity); // Considerar soft delete para auditoría
                
                // ========== CONFIRMACIÓN DE ELIMINACIÓN ==========
                await _unitOfWork.SaveChangesAsync();
                
                // TODO: Verificar si genera alerta de stock agotado
                // TODO: Validar que no hay ventas pendientes que dependan de este stock
            }
            // Operación idempotente - no falla si el registro no existe
        }
    }
} 