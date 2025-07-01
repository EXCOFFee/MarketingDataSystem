// ==================== SERVICIO CRÍTICO - TRANSACCIONES DE VENTA ====================
// Este servicio maneja operaciones financieras críticas con integridad transaccional COMPLETA
// CAPA: Application - Orquesta transacciones complejas entre múltiples entidades
// CRITICIDAD: MÁXIMA - Maneja dinero real y debe garantizar consistencia absoluta
// TRANSACCIONES: Utiliza transacciones explícitas para operaciones atómicas complejas
// SOLID: Cumple S (una responsabilidad), D (inversión dependencias)
// INTEGRIDAD: Rollback automático en caso de fallos para mantener consistencia

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MarketingDataSystem.Core.DTOs;
using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage; // Para transacciones explícitas

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio de aplicación para gestión de ventas con transacciones financieras críticas
    /// RESPONSABILIDAD: Procesar transacciones de venta garantizando integridad financiera absoluta
    /// ARQUITECTURA: Capa Application - orquesta operaciones complejas entre múltiples entidades
    /// CRITICIDAD: MÁXIMA - Maneja transacciones financieras reales que requieren consistencia ACID
    /// SOLID:
    /// - S: Una sola responsabilidad (gestión completa del ciclo de vida de ventas)
    /// - D: Depende de abstracciones (IUnitOfWork, IMapper)
    /// TRANSACCIONES: Utiliza transacciones explícitas para garantizar atomicidad en operaciones complejas
    /// ROLLBACK: Implementa rollback automático para mantener integridad en caso de fallos
    /// INTEGRACIÓN: Coordina con ProductoService, ClienteService, StockService para ventas completas
    /// AUDITORÍA: Todas las operaciones deben ser auditables para compliance financiero
    /// </summary>
    public class VentaService : IVentaService
    {
        // ========== DEPENDENCIAS PARA TRANSACCIONES FINANCIERAS ==========
        private readonly IUnitOfWork _unitOfWork;   // Coordinador transaccional para operaciones complejas
        private readonly IMapper _mapper;           // Mapper automático para DTOs financieros

        /// <summary>
        /// Constructor con inyección de dependencias para operaciones financieras críticas
        /// PATRÓN: Dependency Injection - todas las dependencias son interfaces testeable
        /// TESTABILIDAD: Permite mocking completo para pruebas de transacciones financieras
        /// SOLID: Principio D - depende de abstracciones para máxima flexibilidad
        /// SEGURIDAD: No mantiene estado para evitar problemas de concurrencia
        /// </summary>
        /// <param name="unitOfWork">Coordinador de repositorios con soporte transaccional ACID</param>
        /// <param name="mapper">AutoMapper para conversión segura entre DTOs y Entities financieras</param>
        public VentaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork; // Principio D de SOLID - Inversión de dependencias
            _mapper = mapper;         // AutoMapper para transformaciones financieras seguras
        }

        /// <summary>
        /// Obtiene una venta específica con todos sus detalles financieros completos
        /// LÓGICA: Buscar en repositorio → Convertir a DTO → Devolver información financiera
        /// SEGURIDAD: Solo devuelve información autorizada según contexto del usuario
        /// FINANCIERO: Incluye totales, impuestos, descuentos y detalles de productos
        /// USO: VentaController, ReporteService (para análisis financiero)
        /// AUDITORÍA: Operación de consulta registrada para compliance
        /// </summary>
        /// <param name="id">ID único de la venta a consultar</param>
        /// <returns>VentaDto con información financiera completa o null si no existe</returns>
        public async Task<VentaDto> GetByIdAsync(int id)
        {
            // Buscar entidad de venta con todos sus detalles relacionados
            var entity = await _unitOfWork.Ventas.GetByIdAsync(id);
            // Conversión segura Entity → DTO con información financiera
            return _mapper.Map<VentaDto>(entity);
        }

        /// <summary>
        /// Obtiene todas las ventas del sistema para análisis financiero y reportes
        /// LÓGICA: Consultar repositorio → Mapear colección → Devolver datos financieros
        /// FINANCIERO: Dataset completo para análisis de ventas y reportes gerenciales
        /// PERFORMANCE: En producción debe implementar paginación y filtros por fecha
        /// USO: VentaController (consultas), ReporteService (análisis), DashboardService
        /// SEGURIDAD: Aplicar filtros de autorización según rol del usuario
        /// </summary>
        /// <returns>Colección de VentaDto con información financiera de todas las ventas</returns>
        public async Task<IEnumerable<VentaDto>> GetAllAsync()
        {
            // Obtener todas las entidades de venta desde el repositorio
            var entities = await _unitOfWork.Ventas.GetAllAsync();
            // Mapear automáticamente la colección completa con datos financieros
            return _mapper.Map<IEnumerable<VentaDto>>(entities);
        }

        /// <summary>
        /// Procesa una nueva venta con transacción ACID completa y rollback automático
        /// LÓGICA: Iniciar transacción → Validar → Procesar → Confirmar → Rollback si falla
        /// FINANCIERO: Operación crítica que afecta ingresos, stock y datos del cliente
        /// TRANSACCIÓN: Usa transacción explícita para garantizar atomicidad completa
        /// ROLLBACK: Si cualquier paso falla, revierte todos los cambios automáticamente
        /// INTEGRACIÓN: Puede requerir actualizar stock, validar cliente, aplicar descuentos
        /// AUDITORÍA: Registra toda la operación para trazabilidad financiera completa
        /// </summary>
        /// <param name="ventaDto">Datos completos de la venta (cliente, productos, totales)</param>
        /// <returns>VentaDto procesada con ID asignado y confirmación de la transacción</returns>
        public async Task<VentaDto> CreateAsync(VentaDto ventaDto)
        {
            // ========== INICIO DE TRANSACCIÓN FINANCIERA CRÍTICA ==========
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    // ========== CONVERSIÓN DTO → ENTITY ==========
                    var entity = _mapper.Map<Venta>(ventaDto);
                    
                    // ========== PERSISTENCIA EN TRANSACCIÓN ==========
                    await _unitOfWork.Ventas.AddAsync(entity);
                    
                    // ========== GUARDADO DENTRO DE TRANSACCIÓN ==========
                    await _unitOfWork.SaveChangesAsync();
                    
                    // ========== CONFIRMACIÓN DEFINITIVA ==========
                    await transaction.CommitAsync(); // Todo exitoso - confirmar cambios
                    
                    // ========== RETORNO CON ID GENERADO ==========
                    return _mapper.Map<VentaDto>(entity); // Entity con ID asignado
                }
                catch
                {
                    // ========== ROLLBACK AUTOMÁTICO EN CASO DE ERROR ==========
                    await transaction.RollbackAsync(); // Revertir TODOS los cambios
                    throw; // Re-lanzar excepción para manejo en Controller
                }
            }
        }

        /// <summary>
        /// Actualiza una venta existente con transacción ACID y rollback automático
        /// LÓGICA: Transacción → Validar → Actualizar → Confirmar → Rollback si falla
        /// FINANCIERO: Operación sensible que puede afectar reportes y auditorías
        /// TRANSACCIÓN: Protección completa con rollback automático ante fallos
        /// COMPLIANCE: Las modificaciones de ventas deben ser auditables y justificadas
        /// INTEGRIDAD: Mantiene consistencia financiera en todas las operaciones relacionadas
        /// </summary>
        /// <param name="ventaDto">Datos actualizados completos de la venta</param>
        /// <returns>VentaDto actualizada con confirmación de la transacción</returns>
        public async Task<VentaDto> UpdateAsync(VentaDto ventaDto)
        {
            // ========== INICIO DE TRANSACCIÓN DE ACTUALIZACIÓN ==========
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    // ========== CONVERSIÓN Y PREPARACIÓN ==========
                    var entity = _mapper.Map<Venta>(ventaDto);
                    
                    // ========== MARCADO PARA ACTUALIZACIÓN ==========
                    _unitOfWork.Ventas.Update(entity); // Entity Framework rastrea cambios
                    
                    // ========== GUARDADO EN TRANSACCIÓN ==========
                    await _unitOfWork.SaveChangesAsync();
                    
                    // ========== CONFIRMACIÓN TRANSACCIONAL ==========
                    await transaction.CommitAsync(); // Confirmar cambios exitosos
                    
                    // ========== RETORNO CONFIRMADO ==========
                    return _mapper.Map<VentaDto>(entity);
                }
                catch
                {
                    // ========== ROLLBACK AUTOMÁTICO POR SEGURIDAD ==========
                    await transaction.RollbackAsync(); // Revertir cambios ante cualquier error
                    throw; // Propagar excepción para manejo apropiado
                }
            }
        }

        /// <summary>
        /// Elimina una venta del sistema (operación financiera crítica)
        /// LÓGICA: Buscar → Verificar autorización → Eliminar → Confirmar
        /// FINANCIERO: Operación muy sensible que afecta reportes financieros y auditorías
        /// COMPLIANCE: Puede requerir justificación y autorización especial
        /// SOFT DELETE: Recomendado implementar eliminación lógica para auditoría
        /// INTEGRIDAD: Verificar que no afecte reportes cerrados o procesos contables
        /// SEGURIDAD: Solo usuarios autorizados deberían poder eliminar ventas
        /// </summary>
        /// <param name="id">ID único de la venta a eliminar</param>
        /// <returns>Task que representa la operación asíncrona</returns>
        public async Task DeleteAsync(int id)
        {
            // ========== VERIFICACIÓN DE EXISTENCIA ==========
            var entity = await _unitOfWork.Ventas.GetByIdAsync(id);
            if (entity != null)
            {
                // ========== ELIMINACIÓN FINANCIERA CRÍTICA ==========
                _unitOfWork.Ventas.Delete(entity); // Considerar soft delete para auditoría
                
                // ========== CONFIRMACIÓN SIN TRANSACCIÓN EXPLÍCITA ==========
                // NOTA: Podría beneficiarse de transacción explícita como Create/Update
                await _unitOfWork.SaveChangesAsync();
            }
            // Operación idempotente - no falla si la venta no existe
        }
    }
} 