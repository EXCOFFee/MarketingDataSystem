// ==================== INTERFAZ CRÍTICA - SERVICIO DE TRANSACCIONES FINANCIERAS ====================
// Esta interfaz define el contrato para operaciones críticas de ventas que manejan DINERO REAL
// CAPA: Application Layer - Contratos de servicios de negocio
// CRITICIDAD: MÁXIMA - Operaciones que afectan directamente el revenue empresarial
// RESPONSABILIDAD: Definir operaciones de negocio para transacciones de venta
// CLEAN ARCHITECTURE: Abstracción que permite inversión de dependencias
// PATRÓN: Service Layer Pattern + Dependency Inversion Principle (SOLID)

using System.Collections.Generic;
using System.Threading.Tasks;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de transacciones de venta - OPERACIONES FINANCIERAS CRÍTICAS
    /// RESPONSABILIDAD: Definir contrato para operaciones de negocio de ventas
    /// ARQUITECTURA: Application Layer Service Interface en Clean Architecture
    /// CRITICIDAD: MÁXIMA - Maneja transacciones que representan dinero real
    /// PRINCIPIOS SOLID:
    /// - Interface Segregation: Interfaz específica para operaciones de venta
    /// - Dependency Inversion: Abstracción para inversión de dependencias
    /// - Single Responsibility: Solo operaciones relacionadas con ventas
    /// CASOS DE USO EMPRESARIALES:
    /// - Punto de Venta: Registro de transacciones en tiempo real
    /// - E-commerce: Procesamiento de órdenes de compra online
    /// - Reportes Financieros: Consulta de datos para dashboards ejecutivos
    /// - Auditoría: Trazabilidad completa de operaciones financieras
    /// - Integración ERP: Sincronización con sistemas contables
    /// - Analytics: Análisis de patrones de venta y revenue
    /// - Compliance: Cumplimiento de regulaciones financieras
    /// IMPLEMENTACIÓN:
    /// - VentaService: Implementación concreta con lógica de negocio
    /// - Mock/Stub: Para testing unitario sin dependencias externas
    /// - Proxy: Para caching, logging, o decoradores de funcionalidad
    /// INYECCIÓN DE DEPENDENCIAS:
    /// - Registrada en DI container para inversión de dependencias
    /// - Permite testabilidad y flexibilidad arquitectónica
    /// - Facilita intercambio de implementaciones
    /// TRANSACCIONALIDAD:
    /// - Operaciones atómicas para integridad financiera
    /// - Rollback automático en caso de errores
    /// - Consistencia de datos garantizada
    /// SEGURIDAD:
    /// - Validaciones de autorización antes de operaciones
    /// - Audit trail completo de todas las operaciones
    /// - Validaciones de integridad de datos financieros
    /// </summary>
    public interface IVentaService
    {
        /// <summary>
        /// Obtiene una venta específica por su identificador único - CONSULTA FINANCIERA CRÍTICA
        /// PROPÓSITO: Recuperar detalles completos de una transacción financiera específica
        /// CASOS DE USO EMPRESARIALES:
        /// - Consulta de Detalles: Mostrar información completa de venta en interfaces
        /// - Auditoría Financiera: Verificación de transacciones específicas
        /// - Soporte al Cliente: Consulta de detalles para resolución de problemas
        /// - Reportes Detallados: Inclusión en reportes financieros ejecutivos
        /// - Integración: Sincronización con sistemas contables y ERP
        /// - Compliance: Verificación de transacciones para regulaciones
        /// - Análisis: Estudio detallado de patrones de venta específicos
        /// VALIDACIONES CRÍTICAS:
        /// - ID válido: Verificar que el ID sea un entero positivo válido
        /// - Existencia: Confirmar que la venta existe en el sistema
        /// - Autorización: Verificar permisos del usuario para acceder a la venta
        /// - Integridad: Validar que los datos de la venta sean consistentes
        /// PERFORMANCE:
        /// - Consulta optimizada con índices en base de datos
        /// - Caching de consultas frecuentes para reducir latencia
        /// - Lazy loading de relaciones para eficiencia
        /// SEGURIDAD:
        /// - Verificación de permisos antes de retornar datos
        /// - Logging de todas las consultas para audit trail
        /// - Sanitización de datos sensibles si es necesario
        /// MANEJO DE ERRORES:
        /// - VentaNotFoundException: Cuando la venta no existe
        /// - UnauthorizedException: Cuando el usuario no tiene permisos
        /// - ValidationException: Cuando el ID no es válido
        /// TRANSACCIONALIDAD: Operación de solo lectura (no requiere transacción)
        /// </summary>
        /// <param name="id">Identificador único de la venta a consultar</param>
        /// <returns>VentaDto con los detalles completos de la transacción</returns>
        /// <exception cref="VentaNotFoundException">Cuando la venta con el ID especificado no existe</exception>
        /// <exception cref="UnauthorizedException">Cuando el usuario no tiene permisos para acceder</exception>
        /// <exception cref="ValidationException">Cuando el ID proporcionado no es válido</exception>
        Task<VentaDto> GetByIdAsync(int id);
        
        /// <summary>
        /// Obtiene todas las ventas del sistema - CONSULTA MASIVA FINANCIERA CRÍTICA
        /// PROPÓSITO: Recuperar listado completo de transacciones para análisis y reportes
        /// CASOS DE USO EMPRESARIALES:
        /// - Dashboards Ejecutivos: Mostrar todas las ventas en interfaces de gestión
        /// - Reportes Financieros: Base de datos para reportes de revenue y análisis
        /// - Análisis de Tendencias: Datos históricos para identificar patrones
        /// - Reconciliación Contable: Verificación completa de transacciones
        /// - Auditoría: Revisión exhaustiva de todas las operaciones financieras
        /// - Export/Import: Migración de datos entre sistemas
        /// - Analytics: Análisis masivo de datos para business intelligence
        /// CONSIDERACIONES DE PERFORMANCE:
        /// - Paginación: Implementar paginación para grandes volúmenes de datos
        /// - Filtros: Permitir filtrado por fecha, cliente, producto, etc.
        /// - Proyección: Retornar solo campos necesarios para reducir overhead
        /// - Caching: Cache de consultas frecuentes para mejorar rendimiento
        /// - Índices: Asegurar índices apropiados en base de datos
        /// VALIDACIONES CRÍTICAS:
        /// - Autorización: Verificar permisos para consultar todas las ventas
        /// - Límites: Implementar límites de cantidad para evitar sobrecarga
        /// - Filtros: Validar filtros de fecha y parámetros de consulta
        /// SEGURIDAD:
        /// - Verificación de permisos de administrador/manager
        /// - Logging de consultas masivas para auditoría
        /// - Rate limiting para evitar abuso de consultas
        /// ESCALABILIDAD:
        /// - Consultas optimizadas para grandes volúmenes
        /// - Posible implementación de streaming para datasets masivos
        /// - Compresión de datos para transferencia eficiente
        /// WARNING: Operación potencialmente costosa - usar con precaución
        /// </summary>
        /// <returns>Colección de VentaDto con todas las transacciones del sistema</returns>
        /// <exception cref="UnauthorizedException">Cuando el usuario no tiene permisos administrativos</exception>
        /// <exception cref="SystemOverloadException">Cuando el sistema está sobrecargado</exception>
        Task<IEnumerable<VentaDto>> GetAllAsync();
        
        /// <summary>
        /// Crea una nueva venta en el sistema - OPERACIÓN FINANCIERA CRÍTICA
        /// PROPÓSITO: Registrar nueva transacción financiera que afecta directamente el revenue
        /// CASOS DE USO EMPRESARIALES:
        /// - Punto de Venta: Registro inmediato de transacciones de venta
        /// - E-commerce: Confirmación de órdenes de compra online
        /// - Ventas Telefónicas: Registro de ventas por call center
        /// - Aplicaciones Móviles: Ventas a través de apps empresariales
        /// - Integración: Recepción de ventas desde sistemas externos
        /// - Bulk Loading: Carga masiva de ventas históricas
        /// VALIDACIONES CRÍTICAS OBLIGATORIAS:
        /// - Producto Existente: Verificar que el producto existe en catálogo
        /// - Cliente Existente: Confirmar que el cliente está registrado
        /// - Stock Disponible: Verificar disponibilidad de inventario
        /// - Precio Válido: Confirmar que el precio es positivo y coherente
        /// - Cantidad Válida: Verificar que la cantidad es positiva
        /// - Integridad Financiera: Total = Cantidad × PrecioUnitario
        /// - Sucursal Válida: Verificar que la sucursal existe y está activa
        /// OPERACIONES ATÓMICAS CRÍTICAS:
        /// - Inserción de Venta: Crear registro de transacción
        /// - Actualización de Stock: Reducir inventario automáticamente
        /// - Generación de Auditoría: Crear log de transacción
        /// - Cálculo de Comisiones: Actualizar comisiones de vendedores
        /// - Integración Contable: Sincronizar con sistemas ERP
        /// TRANSACCIONALIDAD:
        /// - Operación completamente atómica (todo o nada)
        /// - Rollback automático en caso de cualquier error
        /// - Consistency garantizada entre todas las operaciones
        /// SEGURIDAD:
        /// - Autorización requerida para crear ventas
        /// - Validación de límites de venta por usuario
        /// - Audit trail completo de creación
        /// PERFORMANCE:
        /// - Validaciones optimizadas para velocidad
        /// - Índices apropiados para inserción rápida
        /// - Minimizar locks de base de datos
        /// </summary>
        /// <param name="ventaDto">Datos de la venta a crear con toda la información necesaria</param>
        /// <returns>VentaDto creada con ID asignado y datos confirmados</returns>
        /// <exception cref="ValidationException">Cuando los datos de la venta no son válidos</exception>
        /// <exception cref="ProductoNotFoundException">Cuando el producto especificado no existe</exception>
        /// <exception cref="ClienteNotFoundException">Cuando el cliente especificado no existe</exception>
        /// <exception cref="InsufficientStockException">Cuando no hay stock suficiente</exception>
        /// <exception cref="UnauthorizedException">Cuando el usuario no tiene permisos</exception>
        /// <exception cref="BusinessRuleException">Cuando se viola una regla de negocio</exception>
        Task<VentaDto> CreateAsync(VentaDto ventaDto);
        
        /// <summary>
        /// Actualiza una venta existente - OPERACIÓN FINANCIERA CRÍTICA CON RESTRICCIONES
        /// PROPÓSITO: Modificar transacción financiera existente bajo estrictas condiciones
        /// CASOS DE USO EMPRESARIALES:
        /// - Corrección de Errores: Ajustar datos incorrectos en transacciones
        /// - Actualización de Sucursal: Cambiar sucursal de origen de venta
        /// - Modificación de Metadatos: Actualizar información no financiera
        /// - Compliance: Ajustes requeridos por auditoría o regulaciones
        /// - Integración: Sincronización de cambios desde sistemas externos
        /// WARNING: Operación de alto riesgo que requiere control estricto
        /// RESTRICCIONES CRÍTICAS:
        /// - Datos Financieros: NO permitir cambios de precio, cantidad o totales
        /// - Período de Gracia: Solo permitir modificaciones dentro de ventana temporal
        /// - Autorización Elevada: Requerir permisos de supervisor/administrador
        /// - Audit Trail: Registrar todos los cambios con detalle completo
        /// - Integridad: Mantener consistencia con stock y sistemas relacionados
        /// VALIDACIONES CRÍTICAS:
        /// - Existencia: Confirmar que la venta existe
        /// - Modificabilidad: Verificar que la venta puede ser modificada
        /// - Autorización: Permisos elevados para modificación
        /// - Integridad: Validar que los cambios no afectan integridad financiera
        /// - Business Rules: Cumplir con todas las reglas de negocio
        /// TRANSACCIONALIDAD:
        /// - Operación completamente atómica con rollback
        /// - Consistency garantizada con sistemas relacionados
        /// - Locking optimista para evitar conflictos concurrentes
        /// SEGURIDAD:
        /// - Autenticación y autorización estricta
        /// - Logging detallado de todos los cambios
        /// - Notificaciones automáticas a supervisores
        /// COMPLIANCE:
        /// - Registro detallado para auditorías
        /// - Cumplimiento con regulaciones financieras
        /// - Trazabilidad completa de modificaciones
        /// </summary>
        /// <param name="ventaDto">Datos actualizados de la venta con cambios aplicados</param>
        /// <returns>VentaDto actualizada con cambios confirmados</returns>
        /// <exception cref="VentaNotFoundException">Cuando la venta a actualizar no existe</exception>
        /// <exception cref="VentaNotModifiableException">Cuando la venta no puede ser modificada</exception>
        /// <exception cref="UnauthorizedException">Cuando el usuario no tiene permisos suficientes</exception>
        /// <exception cref="ValidationException">Cuando los datos actualizados no son válidos</exception>
        /// <exception cref="BusinessRuleException">Cuando la actualización viola reglas de negocio</exception>
        /// <exception cref="ConcurrencyException">Cuando hay conflictos de modificación concurrente</exception>
        Task<VentaDto> UpdateAsync(VentaDto ventaDto);
        
        /// <summary>
        /// Elimina una venta del sistema - OPERACIÓN FINANCIERA EXTREMADAMENTE CRÍTICA
        /// PROPÓSITO: Remover transacción financiera bajo condiciones excepcionales
        /// CASOS DE USO EMPRESARIALES:
        /// - Corrección de Errores Críticos: Eliminar transacciones completamente erróneas
        /// - Compliance: Eliminación requerida por regulaciones específicas
        /// - Fraude: Remoción de transacciones fraudulentas detectadas
        /// - Testing: Limpieza de datos de prueba en entornos de desarrollo
        /// - Migración: Eliminación durante procesos de migración de datos
        /// WARNING: OPERACIÓN DE MÁXIMO RIESGO - USAR SOLO EN CASOS EXCEPCIONALES
        /// RESTRICCIONES CRÍTICAS:
        /// - Autorización Máxima: Solo administradores con permisos especiales
        /// - Ventana Temporal: Solo permitir eliminación dentro de período muy limitado
        /// - Justificación: Requerir razón documentada para eliminación
        /// - Approval Workflow: Proceso de aprobación multi-nivel
        /// - Irreversibilidad: Advertir que la operación no puede deshacerse
        /// OPERACIONES ATÓMICAS CRÍTICAS:
        /// - Eliminación de Venta: Remover registro de transacción
        /// - Reversión de Stock: Restaurar inventario automáticamente
        /// - Cancelación de Comisiones: Revertir comisiones calculadas
        /// - Ajustes Contables: Crear asientos de reversión en ERP
        /// - Notificaciones: Alertar a stakeholders sobre eliminación
        /// TRANSACCIONALIDAD:
        /// - Operación completamente atómica con múltiples rollbacks
        /// - Consistency garantizada en todos los sistemas relacionados
        /// - Verificación de integridad post-eliminación
        /// SEGURIDAD:
        /// - Autenticación multi-factor requerida
        /// - Autorización de nivel más alto del sistema
        /// - Audit trail exhaustivo e inmutable
        /// - Alertas automáticas a supervisores
        /// COMPLIANCE:
        /// - Registro detallado para auditorías regulatorias
        /// - Cumplimiento con leyes de retención de datos
        /// - Documentación completa de razones para eliminación
        /// ALTERNATIVE: Considerar "soft delete" o marcado como inactivo
        /// </summary>
        /// <param name="id">Identificador único de la venta a eliminar</param>
        /// <returns>Task que representa la operación asíncrona de eliminación</returns>
        /// <exception cref="VentaNotFoundException">Cuando la venta a eliminar no existe</exception>
        /// <exception cref="VentaNotDeletableException">Cuando la venta no puede ser eliminada</exception>
        /// <exception cref="UnauthorizedException">Cuando el usuario no tiene permisos de eliminación</exception>
        /// <exception cref="BusinessRuleException">Cuando la eliminación viola reglas críticas</exception>
        /// <exception cref="SystemIntegrityException">Cuando la eliminación afectaría integridad</exception>
        /// <exception cref="ComplianceException">Cuando la eliminación viola regulaciones</exception>
        Task DeleteAsync(int id);

        // ========== MÉTODOS FUTUROS PARA OPERACIONES AVANZADAS ==========
        // TODO: Implementar métodos adicionales para funcionalidad empresarial:

        /// <summary>
        /// Obtiene ventas por rango de fechas para reportes financieros
        /// FUTURO: Consultas temporales optimizadas para análisis de tendencias
        /// </summary>
        // Task<IEnumerable<VentaDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Obtiene ventas por cliente específico para análisis CRM
        /// FUTURO: Historial de compras por cliente para programas de fidelización
        /// </summary>
        // Task<IEnumerable<VentaDto>> GetByClienteAsync(int clienteId);

        /// <summary>
        /// Obtiene ventas por producto para análisis de performance
        /// FUTURO: Análisis de productos más vendidos y rentabilidad
        /// </summary>
        // Task<IEnumerable<VentaDto>> GetByProductoAsync(int productoId);

        /// <summary>
        /// Obtiene ventas por sucursal para análisis geográfico
        /// FUTURO: Análisis de performance por ubicación
        /// </summary>
        // Task<IEnumerable<VentaDto>> GetBySucursalAsync(string sucursal);

        /// <summary>
        /// Calcula estadísticas de ventas para dashboards ejecutivos
        /// FUTURO: KPIs y métricas agregadas en tiempo real
        /// </summary>
        // Task<VentaStatisticsDto> GetStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Obtiene top productos vendidos para análisis de demanda
        /// FUTURO: Análisis de productos más populares
        /// </summary>
        // Task<IEnumerable<ProductoVentaDto>> GetTopProductosAsync(int limit = 10);

        /// <summary>
        /// Obtiene top clientes por volumen de compras
        /// FUTURO: Análisis de clientes más valiosos
        /// </summary>
        // Task<IEnumerable<ClienteVentaDto>> GetTopClientesAsync(int limit = 10);

        /// <summary>
        /// Valida disponibilidad de stock antes de crear venta
        /// FUTURO: Validación previa para mejor UX
        /// </summary>
        // Task<bool> ValidateStockAvailabilityAsync(int productoId, int cantidad);

        /// <summary>
        /// Calcula total de venta con impuestos y descuentos
        /// FUTURO: Cálculos complejos de pricing dinámico
        /// </summary>
        // Task<decimal> CalculateTotalAsync(VentaDto ventaDto);

        /// <summary>
        /// Procesa venta con múltiples productos (carrito de compras)
        /// FUTURO: Ventas complejas con múltiples items
        /// </summary>
        // Task<VentaDto> CreateMultiProductAsync(MultiProductVentaDto multiVentaDto);

        /// <summary>
        /// Cancela una venta y revierte todas las operaciones relacionadas
        /// FUTURO: Proceso de cancelación completo con reversión de stock
        /// </summary>
        // Task<bool> CancelAsync(int ventaId, string reason);

        /// <summary>
        /// Obtiene audit trail completo de una venta específica
        /// FUTURO: Trazabilidad completa para compliance
        /// </summary>
        // Task<IEnumerable<VentaAuditDto>> GetAuditTrailAsync(int ventaId);

        // ========== CONSIDERACIONES PARA ESCALABILIDAD FINANCIERA ==========
        // FUTURE ENHANCEMENTS para sistemas financieros empresariales:
        // - Event Sourcing: Historial inmutable de todas las transacciones
        // - CQRS: Separación de commands y queries para mejor performance
        // - Saga Pattern: Transacciones distribuidas para microservicios
        // - Circuit Breaker: Protección contra fallos de sistemas externos
        // - Caching: Redis/MemoryCache para consultas frecuentes
        // - Monitoring: Métricas en tiempo real de transacciones
        // - Alerting: Notificaciones automáticas para anomalías
        // - Backup: Respaldo automático de transacciones críticas
        // - Encryption: Cifrado de datos sensibles en tránsito y reposo
        // - Compliance: Cumplimiento automático con regulaciones financieras
    }
} 