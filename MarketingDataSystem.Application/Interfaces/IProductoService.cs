// ==================== INTERFAZ CRÍTICA - SERVICIO DE CATÁLOGO DE PRODUCTOS ====================
// Esta interfaz define el contrato para operaciones críticas del catálogo empresarial
// CAPA: Application Layer - Contratos de servicios de catálogo maestro
// CRITICIDAD: ALTA - Gestiona el catálogo que alimenta ventas, pricing e inventario
// RESPONSABILIDAD: Definir operaciones de negocio para gestión de productos
// CLEAN ARCHITECTURE: Abstracción que permite inversión de dependencias
// PATRÓN: Service Layer Pattern + Repository Pattern

using System.Collections.Generic;
using System.Threading.Tasks;
using MarketingDataSystem.Core.DTOs;

namespace MarketingDataSystem.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de gestión de catálogo de productos - DATOS MAESTROS EMPRESARIALES
    /// RESPONSABILIDAD: Definir contrato para operaciones de negocio del catálogo de productos
    /// ARQUITECTURA: Application Layer Service Interface en Clean Architecture
    /// CRITICIDAD: ALTA - Maneja catálogo maestro que alimenta todo el ecosistema comercial
    /// PRINCIPIOS SOLID:
    /// - Interface Segregation: Interfaz específica para operaciones de productos
    /// - Dependency Inversion: Abstracción para inversión de dependencias
    /// - Single Responsibility: Solo operaciones relacionadas con catálogo de productos
    /// CASOS DE USO EMPRESARIALES:
    /// - E-commerce: Gestión de catálogo para tiendas online
    /// - Punto de Venta: Información de productos para sistemas POS
    /// - Gestión de Inventario: Base para control de stock y reposición
    /// - Pricing: Administración de precios y políticas comerciales
    /// - Marketing: Datos para campañas y promociones de productos
    /// - Reportes Ejecutivos: Análisis de catálogo y performance de productos
    /// - Integración ERP: Sincronización con sistemas de gestión empresarial
    /// - Compras: Información para órdenes de compra y proveedores
    /// IMPLEMENTACIÓN:
    /// - ProductoService: Implementación concreta con lógica de negocio
    /// - Mock/Stub: Para testing unitario sin dependencias externas
    /// - Proxy: Para caching, logging, o decoradores de funcionalidad
    /// INYECCIÓN DE DEPENDENCIAS:
    /// - Registrada en DI container para inversión de dependencias
    /// - Permite testabilidad y flexibilidad arquitectónica
    /// - Facilita intercambio de implementaciones
    /// GESTIÓN DE CATÁLOGO:
    /// - Categorización jerárquica de productos
    /// - Pricing dinámico y políticas comerciales
    /// - Lifecycle management (activo/inactivo/descontinuado)
    /// - Integración con sistemas de proveedores
    /// PERFORMANCE:
    /// - Caching inteligente para consultas frecuentes
    /// - Indexación optimizada para búsquedas
    /// - Lazy loading para relaciones complejas
    /// SEGURIDAD:
    /// - Validaciones de autorización para modificaciones
    /// - Audit trail de cambios en catálogo
    /// - Integridad de datos maestros
    /// </summary>
    public interface IProductoService
    {
        /// <summary>
        /// Obtiene un producto específico por su identificador único - CONSULTA DE CATÁLOGO CRÍTICA
        /// PROPÓSITO: Recuperar detalles completos de un producto del catálogo maestro
        /// CASOS DE USO EMPRESARIALES:
        /// - E-commerce: Mostrar detalles de producto en páginas de producto
        /// - Punto de Venta: Información completa para transacciones de venta
        /// - Inventario: Consulta de especificaciones para gestión de stock
        /// - Pricing: Verificación de precios actuales y políticas
        /// - Marketing: Datos para materiales promocionales y campañas
        /// - Reportes: Inclusión en reportes de análisis de productos
        /// - Integración: Sincronización con sistemas externos y ERP
        /// - Compras: Información para órdenes de compra y proveedores
        /// VALIDACIONES CRÍTICAS:
        /// - ID válido: Verificar que el ID sea un entero positivo válido
        /// - Existencia: Confirmar que el producto existe en el catálogo
        /// - Estado: Verificar el estado del producto (activo/inactivo)
        /// - Integridad: Validar que los datos del producto sean consistentes
        /// PERFORMANCE:
        /// - Consulta optimizada con índices en base de datos
        /// - Caching de productos frecuentemente consultados
        /// - Lazy loading de relaciones para eficiencia
        /// - Proyección de campos específicos según uso
        /// CASOS DE RESPUESTA:
        /// - Producto Activo: Retorna ProductoDto completo
        /// - Producto Inactivo: Puede retornar con indicador de estado
        /// - Producto No Encontrado: Manejo específico de error
        /// MANEJO DE ERRORES:
        /// - ProductoNotFoundException: Cuando el producto no existe
        /// - ValidationException: Cuando el ID no es válido
        /// - DataIntegrityException: Cuando hay problemas de integridad
        /// TRANSACCIONALIDAD: Operación de solo lectura (no requiere transacción)
        /// </summary>
        /// <param name="id">Identificador único del producto a consultar</param>
        /// <returns>ProductoDto con los detalles completos del producto</returns>
        /// <exception cref="ProductoNotFoundException">Cuando el producto con el ID especificado no existe</exception>
        /// <exception cref="ValidationException">Cuando el ID proporcionado no es válido</exception>
        Task<ProductoDto> GetByIdAsync(int id);
        
        /// <summary>
        /// Obtiene todos los productos del catálogo - CONSULTA MASIVA DE CATÁLOGO CRÍTICA
        /// PROPÓSITO: Recuperar listado completo de productos para gestión y análisis
        /// CASOS DE USO EMPRESARIALES:
        /// - E-commerce: Listados de productos en categorías y búsquedas
        /// - Dashboards Ejecutivos: Vista completa del catálogo empresarial
        /// - Gestión de Inventario: Base de datos para control de stock
        /// - Reportes: Análisis completo de catálogo y performance
        /// - Marketing: Datos para campañas masivas y segmentación
        /// - Export/Import: Migración de catálogo entre sistemas
        /// - Pricing: Análisis masivo de precios y competitividad
        /// - Compras: Revisión completa para órdenes de reposición
        /// CONSIDERACIONES DE PERFORMANCE:
        /// - Paginación: Implementar paginación para catálogos grandes
        /// - Filtros: Permitir filtrado por categoría, proveedor, precio, etc.
        /// - Ordenamiento: Support para múltiples criterios de ordenamiento
        /// - Proyección: Retornar solo campos necesarios para reducir overhead
        /// - Caching: Cache distribuido para consultas frecuentes
        /// - Índices: Asegurar índices apropiados para consultas complejas
        /// VALIDACIONES CRÍTICAS:
        /// - Autorización: Verificar permisos para consultar catálogo completo
        /// - Límites: Implementar límites de cantidad para evitar sobrecarga
        /// - Filtros: Validar parámetros de filtrado y búsqueda
        /// - Performance: Monitorear tiempo de respuesta y optimizar
        /// OPCIONES DE CONSULTA:
        /// - Productos Activos: Solo productos disponibles para venta
        /// - Catálogo Completo: Incluir productos inactivos para administración
        /// - Vista Resumida: Solo campos esenciales para listados
        /// - Vista Detallada: Información completa para administración
        /// ESCALABILIDAD:
        /// - Consultas optimizadas para grandes catálogos
        /// - Streaming de datos para datasets masivos
        /// - Compresión de respuestas para transferencia eficiente
        /// - CDN integration para caching global
        /// WARNING: Operación potencialmente costosa - implementar paginación
        /// </summary>
        /// <returns>Colección de ProductoDto con todos los productos del catálogo</returns>
        Task<IEnumerable<ProductoDto>> GetAllAsync();
        
        /// <summary>
        /// Crea un nuevo producto en el catálogo - OPERACIÓN DE EXPANSIÓN DE CATÁLOGO CRÍTICA
        /// PROPÓSITO: Agregar nuevo producto al catálogo maestro empresarial
        /// CASOS DE USO EMPRESARIALES:
        /// - Expansión de Catálogo: Agregar nuevos productos a la oferta
        /// - Lanzamiento de Productos: Introducir productos innovadores
        /// - Importación Masiva: Carga de productos desde proveedores
        /// - Integración ERP: Sincronización de productos desde sistemas externos
        /// - Seasonal Products: Productos estacionales y promocionales
        /// - Private Label: Productos de marca propia
        /// - Marketplace: Productos de terceros en plataformas
        /// - Testing: Productos de prueba para validación de mercado
        /// VALIDACIONES CRÍTICAS OBLIGATORIAS:
        /// - Nombre Único: Verificar que no exista producto con el mismo nombre
        /// - Categoría Válida: Confirmar que la categoría existe en taxonomía
        /// - Proveedor Existente: Verificar que el proveedor está registrado
        /// - Precio Válido: Confirmar que el precio es positivo y coherente
        /// - Datos Requeridos: Verificar que todos los campos obligatorios estén presentes
        /// - Business Rules: Cumplir con todas las reglas de negocio por categoría
        /// - Compliance: Verificar cumplimiento regulatorio según tipo de producto
        /// OPERACIONES ATÓMICAS CRÍTICAS:
        /// - Inserción de Producto: Crear registro en catálogo maestro
        /// - Asignación de SKU: Generar código único si no se proporciona
        /// - Configuración de Pricing: Establecer precios y políticas iniciales
        /// - Setup de Inventario: Configurar parámetros de stock inicial
        /// - Indexación de Búsqueda: Agregar a índices de búsqueda
        /// - Audit Trail: Registrar evento de creación de producto
        /// - Notificaciones: Alertar a stakeholders sobre nuevo producto
        /// INTEGRACIÓN AUTOMÁTICA:
        /// - ERP Sync: Sincronizar con sistemas de gestión empresarial
        /// - E-commerce: Publicar en plataformas de venta online
        /// - POS Systems: Actualizar sistemas de punto de venta
        /// - Pricing Engines: Registrar en sistemas de pricing dinámico
        /// - Analytics: Incluir en sistemas de análisis y reporting
        /// TRANSACCIONALIDAD:
        /// - Operación completamente atómica (todo o nada)
        /// - Rollback automático en caso de cualquier error
        /// - Consistency garantizada entre sistemas relacionados
        /// SEGURIDAD:
        /// - Autorización requerida para crear productos
        /// - Validación de permisos por categoría de producto
        /// - Audit trail completo de creación
        /// - Verificación de integridad de datos
        /// </summary>
        /// <param name="productoDto">Datos del producto a crear con toda la información necesaria</param>
        /// <returns>ProductoDto creado con ID asignado y datos confirmados</returns>
        /// <exception cref="ValidationException">Cuando los datos del producto no son válidos</exception>
        /// <exception cref="ProductoAlreadyExistsException">Cuando ya existe un producto con el mismo nombre</exception>
        /// <exception cref="CategoriaNotFoundException">Cuando la categoría especificada no existe</exception>
        /// <exception cref="ProveedorNotFoundException">Cuando el proveedor especificado no existe</exception>
        /// <exception cref="UnauthorizedException">Cuando el usuario no tiene permisos</exception>
        /// <exception cref="BusinessRuleException">Cuando se viola una regla de negocio</exception>
        Task<ProductoDto> CreateAsync(ProductoDto productoDto);
        
        /// <summary>
        /// Actualiza un producto existente en el catálogo - OPERACIÓN DE MANTENIMIENTO CRÍTICA
        /// PROPÓSITO: Modificar producto existente manteniendo integridad del catálogo
        /// CASOS DE USO EMPRESARIALES:
        /// - Actualización de Precios: Cambios de pricing según mercado
        /// - Cambio de Proveedores: Modificar sourcing de productos
        /// - Actualización de Especificaciones: Mejorar descripciones y atributos
        /// - Reclasificación: Cambiar categorías según evolución del catálogo
        /// - Compliance Updates: Ajustes requeridos por regulaciones
        /// - Quality Improvements: Mejoras basadas en feedback de clientes
        /// - Seasonal Adjustments: Ajustes estacionales de productos
        /// - Integration Sync: Sincronización con cambios de sistemas externos
        /// VALIDACIONES CRÍTICAS:
        /// - Existencia: Confirmar que el producto existe en catálogo
        /// - Modificabilidad: Verificar que el producto puede ser modificado
        /// - Integridad: Validar que los cambios no afectan integridad del sistema
        /// - Dependencies: Verificar impacto en ventas activas e inventario
        /// - Business Rules: Cumplir con reglas de negocio específicas por categoría
        /// - Pricing Impact: Analizar impacto de cambios de precio en ventas
        /// - Stock Impact: Verificar impacto en inventario y órdenes pendientes
        /// OPERACIONES ATÓMICAS CRÍTICAS:
        /// - Actualización de Producto: Modificar registro en catálogo maestro
        /// - Historización: Mantener historial de cambios para auditoría
        /// - Price Impact Analysis: Analizar impacto de cambios de precio
        /// - Stock Adjustment: Ajustar parámetros de inventario si es necesario
        /// - Search Reindexing: Actualizar índices de búsqueda
        /// - Cache Invalidation: Invalidar caches de producto modificado
        /// - System Sync: Sincronizar cambios con sistemas dependientes
        /// IMPACTO EN SISTEMAS RELACIONADOS:
        /// - Active Sales: Verificar impacto en ventas en progreso
        /// - Pending Orders: Considerar órdenes pendientes de procesamiento
        /// - Price Lists: Actualizar listas de precios en todos los canales
        /// - Inventory: Ajustar parámetros de stock si es necesario
        /// - Marketing: Notificar cambios para actualizar materiales
        /// TRANSACCIONALIDAD:
        /// - Operación completamente atómica con rollback
        /// - Consistency garantizada con sistemas relacionados
        /// - Locking optimista para evitar conflictos concurrentes
        /// SEGURIDAD:
        /// - Autenticación y autorización estricta
        /// - Logging detallado de todos los cambios
        /// - Approval workflow para cambios críticos (precios)
        /// COMPLIANCE:
        /// - Registro detallado para auditorías
        /// - Cumplimiento con regulaciones de productos
        /// - Trazabilidad completa de modificaciones
        /// </summary>
        /// <param name="productoDto">Datos actualizados del producto con cambios aplicados</param>
        /// <returns>ProductoDto actualizado con cambios confirmados</returns>
        /// <exception cref="ProductoNotFoundException">Cuando el producto a actualizar no existe</exception>
        /// <exception cref="ProductoNotModifiableException">Cuando el producto no puede ser modificado</exception>
        /// <exception cref="UnauthorizedException">Cuando el usuario no tiene permisos suficientes</exception>
        /// <exception cref="ValidationException">Cuando los datos actualizados no son válidos</exception>
        /// <exception cref="BusinessRuleException">Cuando la actualización viola reglas de negocio</exception>
        /// <exception cref="ConcurrencyException">Cuando hay conflictos de modificación concurrente</exception>
        Task<ProductoDto> UpdateAsync(ProductoDto productoDto);
        
        /// <summary>
        /// Elimina un producto del catálogo - OPERACIÓN DE DESCONTINUACIÓN CRÍTICA
        /// PROPÓSITO: Remover producto del catálogo bajo condiciones controladas
        /// CASOS DE USO EMPRESARIALES:
        /// - Descontinuación de Productos: Retirar productos obsoletos
        /// - End of Life: Productos que llegaron al final de su ciclo
        /// - Compliance: Eliminación requerida por regulaciones
        /// - Quality Issues: Retiro por problemas de calidad o seguridad
        /// - Supplier Changes: Eliminación por cambios de proveedores
        /// - Testing Cleanup: Eliminación de productos de prueba
        /// - Catalog Optimization: Limpieza de catálogo para optimización
        /// - Duplicate Removal: Eliminación de productos duplicados
        /// WARNING: OPERACIÓN DE ALTO IMPACTO - VERIFICAR DEPENDENCIAS
        /// RESTRICCIONES CRÍTICAS:
        /// - Dependencias Activas: Verificar que no hay ventas activas
        /// - Stock Existente: Considerar inventario actual del producto
        /// - Órdenes Pendientes: Verificar órdenes de compra pendientes
        /// - Referencias: Comprobar referencias en otros sistemas
        /// - Historical Data: Preservar datos históricos para reportes
        /// - Legal Retention: Cumplir con requerimientos de retención legal
        /// OPERACIONES ATÓMICAS CRÍTICAS:
        /// - Soft Delete: Marcar como inactivo en lugar de eliminación física
        /// - Stock Resolution: Resolver inventario existente del producto
        /// - Order Processing: Procesar órdenes pendientes relacionadas
        /// - Price List Update: Remover de listas de precios activas
        /// - Search Removal: Remover de índices de búsqueda públicos
        /// - Historical Preservation: Preservar datos para reportes históricos
        /// - Audit Recording: Registrar eliminación para compliance
        /// ALTERNATIVAS RECOMENDADAS:
        /// - Soft Delete: Marcar como inactivo en lugar de eliminar
        /// - Archive: Mover a archivo para preservar historial
        /// - Deprecation: Marcar como descontinuado con fecha de sunset
        /// - Status Change: Cambiar estado a "Not Available" temporalmente
        /// VERIFICACIONES PRE-ELIMINACIÓN:
        /// - Active Sales: No hay ventas activas del producto
        /// - Pending Orders: No hay órdenes pendientes
        /// - Stock Level: Inventario está en cero o resuelto
        /// - Dependencies: No hay dependencias críticas en otros sistemas
        /// TRANSACCIONALIDAD:
        /// - Operación completamente atómica con múltiples rollbacks
        /// - Consistency garantizada en todos los sistemas relacionados
        /// - Verificación de integridad post-eliminación
        /// SEGURIDAD:
        /// - Autorización de nivel alto requerida
        /// - Audit trail exhaustivo e inmutable
        /// - Approval workflow multi-nivel
        /// - Notification a stakeholders
        /// COMPLIANCE:
        /// - Registro detallado para auditorías regulatorias
        /// - Cumplimiento con leyes de retención de datos
        /// - Documentación completa de razones para eliminación
        /// </summary>
        /// <param name="id">Identificador único del producto a eliminar</param>
        /// <returns>Task que representa la operación asíncrona de eliminación</returns>
        /// <exception cref="ProductoNotFoundException">Cuando el producto a eliminar no existe</exception>
        /// <exception cref="ProductoNotDeletableException">Cuando el producto no puede ser eliminado</exception>
        /// <exception cref="ActiveDependenciesException">Cuando existen dependencias activas</exception>
        /// <exception cref="UnauthorizedException">Cuando el usuario no tiene permisos de eliminación</exception>
        /// <exception cref="BusinessRuleException">Cuando la eliminación viola reglas críticas</exception>
        /// <exception cref="SystemIntegrityException">Cuando la eliminación afectaría integridad</exception>
        Task DeleteAsync(int id);

        // ========== MÉTODOS FUTUROS PARA GESTIÓN AVANZADA DE CATÁLOGO ==========
        // TODO: Implementar métodos adicionales para funcionalidad empresarial:

        /// <summary>
        /// Obtiene productos por categoría específica para navegación
        /// FUTURO: Navegación categórica optimizada para e-commerce
        /// </summary>
        // Task<IEnumerable<ProductoDto>> GetByCategoriaAsync(string categoria);

        /// <summary>
        /// Busca productos por texto en nombre y descripción
        /// FUTURO: Búsqueda de texto completo con ranking de relevancia
        /// </summary>
        // Task<IEnumerable<ProductoDto>> SearchAsync(string searchTerm);

        /// <summary>
        /// Obtiene productos por proveedor para gestión de sourcing
        /// FUTURO: Análisis de productos por proveedor
        /// </summary>
        // Task<IEnumerable<ProductoDto>> GetByProveedorAsync(string proveedor);

        /// <summary>
        /// Obtiene productos por rango de precios para análisis de pricing
        /// FUTURO: Análisis de productos por segmento de precio
        /// </summary>
        // Task<IEnumerable<ProductoDto>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice);

        /// <summary>
        /// Obtiene productos más vendidos para análisis de demanda
        /// FUTURO: Top productos por volumen de ventas
        /// </summary>
        // Task<IEnumerable<ProductoDto>> GetTopSellingAsync(int limit = 10);

        /// <summary>
        /// Obtiene productos con bajo stock para alertas de reposición
        /// FUTURO: Alertas automáticas de reposición
        /// </summary>
        // Task<IEnumerable<ProductoDto>> GetLowStockAsync();

        /// <summary>
        /// Valida disponibilidad y datos de producto antes de usar en ventas
        /// FUTURO: Validación previa para mejor UX en ventas
        /// </summary>
        // Task<ProductoValidationDto> ValidateForSaleAsync(int productoId);

        /// <summary>
        /// Actualiza precios masivamente según políticas de pricing
        /// FUTURO: Pricing masivo y dinámico
        /// </summary>
        // Task<int> UpdatePricesAsync(PricingUpdateDto pricingUpdate);

        /// <summary>
        /// Importa productos masivamente desde archivo o sistema externo
        /// FUTURO: Importación masiva de catálogos
        /// </summary>
        // Task<ImportResultDto> ImportProductsAsync(ProductImportDto importData);

        /// <summary>
        /// Exporta catálogo para integración con sistemas externos
        /// FUTURO: Exportación de catálogo en múltiples formatos
        /// </summary>
        // Task<ExportResultDto> ExportCatalogAsync(CatalogExportDto exportOptions);

        /// <summary>
        /// Duplica un producto para crear variantes o versiones
        /// FUTURO: Gestión de variantes de productos
        /// </summary>
        // Task<ProductoDto> DuplicateAsync(int sourceProductoId, ProductoDuplicateDto duplicateOptions);

        /// <summary>
        /// Obtiene historial de cambios de un producto para auditoría
        /// FUTURO: Trazabilidad completa de cambios en catálogo
        /// </summary>
        // Task<IEnumerable<ProductoAuditDto>> GetAuditTrailAsync(int productoId);

        // ========== CONSIDERACIONES PARA ESCALABILIDAD DE CATÁLOGO ==========
        // FUTURE ENHANCEMENTS para sistemas de catálogo empresarial:
        // - Elasticsearch: Búsqueda avanzada y faceted search
        // - Image Processing: Gestión automática de imágenes de productos
        // - AI/ML: Recomendaciones automáticas y categorización inteligente
        // - Multi-tenant: Support para múltiples organizaciones
        // - Versioning: Control de versiones de productos para trazabilidad
        // - Workflow: Procesos de aprobación para cambios críticos
        // - Integration: APIs para PIM y sistemas de gestión de productos
        // - Performance: Caching distribuido y CDN para catálogos globales
        // - Analytics: Dashboards de performance de catálogo en tiempo real
        // - Compliance: Cumplimiento automático con regulaciones por región
    }
} 