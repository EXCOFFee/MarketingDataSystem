// ==================== SERVICIO DE GESTIÓN DE METADATOS DE REPORTES ====================
// Este servicio gestiona la CONFIGURACIÓN y METADATOS de reportes empresariales
// CAPA: Application - Orquesta operaciones CRUD para metadatos de reportes
// RESPONSABILIDAD: Gestionar definiciones, configuraciones y metadatos de reportes
// DIFERENCIA: ReporteService (metadatos) vs GeneradorReporteService (archivos Excel)
// SOLID: Cumple principios S (responsabilidad única), D (inversión dependencias)
// PATRÓN: Service Layer + Repository + Unit of Work para transacciones ACID

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;                                    // Mapeo automático entre DTOs y entidades
using MarketingDataSystem.Core.DTOs;
using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Application.Interfaces;

namespace MarketingDataSystem.Application.Services
{
    /// <summary>
    /// Servicio de gestión de metadatos y configuraciones de reportes empresariales
    /// RESPONSABILIDAD: Gestionar definiciones de reportes, no generar archivos físicos
    /// ARQUITECTURA: Capa Application - orquesta operaciones CRUD de metadatos de reportes
    /// DIFERENCIACIÓN CLARA:
    /// - ReporteService: Gestiona METADATOS (nombres, tipos, configuraciones, frecuencia)
    /// - GeneradorReporteService: Genera ARCHIVOS FÍSICOS (.xlsx) con datos reales
    /// CASOS DE USO EMPRESARIALES:
    /// - Configurar nuevos tipos de reportes (ventas, stock, clientes)
    /// - Definir parámetros de reportes (fechas, filtros, columnas)
    /// - Gestionar catálogo de reportes disponibles para usuarios
    /// - Configurar automatización de reportes (frecuencia, destinatarios)
    /// - Auditar historial de configuraciones de reportes
    /// SOLID:
    /// - S: Una sola responsabilidad (gestión de metadatos de reportes)
    /// - D: Depende de abstracciones (IUnitOfWork, IMapper)
    /// PATRÓN: Service Layer que encapsula lógica de negocio de reportes
    /// TRANSACCIONES: Usa Unit of Work para garantizar consistencia ACID
    /// MAPEO: AutoMapper para convertir entre DTOs (API) y entidades (dominio)
    /// ESCALABILIDAD: Base para reportes complejos con parámetros y filtros avanzados
    /// </summary>
    public class ReporteService : IReporteService
    {
        // ========== DEPENDENCIAS PARA GESTIÓN DE METADATOS ==========
        private readonly IUnitOfWork _unitOfWork;  // Acceso transaccional a repositorios
        private readonly IMapper _mapper;          // Mapeo automático DTO ↔ Entidad

        /// <summary>
        /// Constructor con inyección de dependencias para operaciones de metadatos de reportes
        /// PATRÓN: Dependency Injection - todas las dependencias son abstracciones testeable
        /// UNIT OF WORK: Garantiza transacciones ACID para operaciones relacionadas
        /// AUTOMAPPER: Simplifica conversión entre capas (API ↔ Domain)
        /// SOLID: Principio D - depende de interfaces, no implementaciones concretas
        /// </summary>
        /// <param name="unitOfWork">Unidad de trabajo para acceso transaccional a repositorios</param>
        /// <param name="mapper">Mapeador automático entre DTOs y entidades de dominio</param>
        public ReporteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork; // Principio D de SOLID - Inversión de dependencias
            _mapper = mapper;         // AutoMapper para transformaciones automáticas
        }

        /// <summary>
        /// Obtiene los metadatos de un reporte específico por ID
        /// CASO DE USO: Consultar configuración de reporte para visualización o edición
        /// FLUJO: ID → Buscar en repositorio → Mapear a DTO → Retornar metadatos
        /// INFORMACIÓN RETORNADA: Nombre, tipo, parámetros, configuración, etc.
        /// ERROR HANDLING: Retorna null si el reporte no existe (manejo graceful)
        /// EJEMPLOS DE USO:
        /// - Dashboard administrativo mostrando lista de reportes
        /// - Formulario de edición de configuración de reporte
        /// - API para obtener definición de reporte específico
        /// </summary>
        /// <param name="id">ID único del reporte a consultar</param>
        /// <returns>DTO con metadatos del reporte o null si no existe</returns>
        public async Task<ReporteDto> GetByIdAsync(int id)
        {
            // ========== BÚSQUEDA EN REPOSITORIO ==========
            var entity = await _unitOfWork.Reportes.GetByIdAsync(id);
            
            // ========== MAPEO AUTOMÁTICO A DTO ==========
            return _mapper.Map<ReporteDto>(entity); // AutoMapper convierte entidad a DTO
        }

        /// <summary>
        /// Obtiene todos los metadatos de reportes disponibles en el sistema
        /// CASO DE USO: Listar catálogo completo de reportes para selección de usuario
        /// FLUJO: Consultar repositorio → Mapear lista completa → Retornar catálogo
        /// INFORMACIÓN: Lista completa de reportes configurados en el sistema
        /// ORDENAMIENTO: Por defecto del repositorio (típicamente por fecha de creación)
        /// EJEMPLOS DE USO:
        /// - Dashboard ejecutivo con reportes disponibles
        /// - Selector de reportes en interfaz administrativa
        /// - API para mobile app con lista de reportes
        /// - Catálogo de reportes para nuevos usuarios
        /// </summary>
        /// <returns>Lista completa de DTOs con metadatos de todos los reportes</returns>
        public async Task<IEnumerable<ReporteDto>> GetAllAsync()
        {
            // ========== CONSULTA COMPLETA DEL CATÁLOGO ==========
            var entities = await _unitOfWork.Reportes.GetAllAsync();
            
            // ========== MAPEO DE LISTA COMPLETA ==========
            return _mapper.Map<IEnumerable<ReporteDto>>(entities); // Lista de DTOs
        }

        /// <summary>
        /// Crea una nueva configuración de reporte en el sistema
        /// CASO DE USO: Definir nuevo tipo de reporte con sus parámetros y configuración
        /// FLUJO: DTO → Mapear a entidad → Persistir → Retornar DTO con ID generado
        /// TRANSACCIÓN: Operación ACID garantizada por Unit of Work
        /// VALIDACIÓN: AutoMapper + validaciones de dominio en la entidad
        /// EJEMPLOS DE CONFIGURACIÓN:
        /// - Reporte de ventas mensuales con filtros por región
        /// - Reporte de stock bajo con alertas automáticas
        /// - Reporte de clientes VIP con segmentación avanzada
        /// - Reporte de performance con KPIs personalizados
        /// </summary>
        /// <param name="reporteDto">DTO con configuración del nuevo reporte</param>
        /// <returns>DTO del reporte creado con ID asignado por base de datos</returns>
        public async Task<ReporteDto> CreateAsync(ReporteDto reporteDto)
        {
            // ========== MAPEO A ENTIDAD DE DOMINIO ==========
            var entity = _mapper.Map<Reporte>(reporteDto); // DTO → Entidad
            
            // ========== PERSISTENCIA EN REPOSITORIO ==========
            await _unitOfWork.Reportes.AddAsync(entity);    // Agregar a contexto
            await _unitOfWork.SaveChangesAsync();           // Commit transaccional
            
            // ========== RETORNO CON ID GENERADO ==========
            return _mapper.Map<ReporteDto>(entity); // Entidad con ID → DTO
        }

        /// <summary>
        /// Actualiza la configuración de un reporte existente
        /// CASO DE USO: Modificar parámetros, filtros o configuración de reporte
        /// FLUJO: DTO → Mapear a entidad → Marcar modificado → Persistir → Retornar
        /// TRANSACCIÓN: Operación ACID para mantener consistencia
        /// CONCURRENCIA: Entity Framework maneja conflictos de concurrencia
        /// EJEMPLOS DE ACTUALIZACIONES:
        /// - Cambiar frecuencia de ejecución (diario → semanal)
        /// - Modificar filtros de datos (agregar nueva región)
        /// - Actualizar destinatarios de email automático
        /// - Cambiar formato de salida o columnas incluidas
        /// </summary>
        /// <param name="reporteDto">DTO con configuración actualizada del reporte</param>
        /// <returns>DTO del reporte actualizado</returns>
        public async Task<ReporteDto> UpdateAsync(ReporteDto reporteDto)
        {
            // ========== MAPEO A ENTIDAD ACTUALIZADA ==========
            var entity = _mapper.Map<Reporte>(reporteDto); // DTO → Entidad actualizada
            
            // ========== MARCADO PARA ACTUALIZACIÓN ==========
            _unitOfWork.Reportes.Update(entity);          // Marcar como modificado
            await _unitOfWork.SaveChangesAsync();         // Commit de cambios
            
            // ========== RETORNO DE ENTIDAD ACTUALIZADA ==========
            return _mapper.Map<ReporteDto>(entity); // Entidad actualizada → DTO
        }

        /// <summary>
        /// Elimina la configuración de un reporte del sistema
        /// CASO DE USO: Remover reportes obsoletos o no utilizados
        /// FLUJO: ID → Buscar entidad → Verificar existencia → Eliminar → Commit
        /// SEGURIDAD: Verificación de existencia antes de eliminar
        /// TRANSACCIÓN: Operación ACID para garantizar consistencia
        /// CONSIDERACIONES:
        /// - No eliminar reportes con historial de ejecución activo
        /// - Verificar dependencias antes de eliminar
        /// - Considerar soft delete para auditoría
        /// EJEMPLOS DE ELIMINACIÓN:
        /// - Reportes de prueba que ya no se necesitan
        /// - Reportes duplicados o mal configurados
        /// - Reportes de funcionalidades descontinuadas
        /// </summary>
        /// <param name="id">ID único del reporte a eliminar</param>
        public async Task DeleteAsync(int id)
        {
            // ========== BÚSQUEDA Y VERIFICACIÓN ==========
            var entity = await _unitOfWork.Reportes.GetByIdAsync(id);
            if (entity != null) // Verificar existencia antes de eliminar
            {
                // ========== ELIMINACIÓN SEGURA ==========
                _unitOfWork.Reportes.Delete(entity);   // Marcar para eliminación
                await _unitOfWork.SaveChangesAsync();  // Commit transaccional
            }
            // Si no existe, no hace nada (operación idempotente)
        }

        // ========== MÉTODOS FUTUROS PARA GESTIÓN AVANZADA DE REPORTES ==========
        // TODO: Implementar funcionalidades empresariales adicionales:
        
        /// <summary>
        /// Obtiene reportes filtrados por tipo (ventas, stock, clientes, etc.)
        /// </summary>
        // public async Task<IEnumerable<ReporteDto>> GetByTipoAsync(TipoReporte tipo)
        // {
        //     // Filtrar reportes por categoría para organización
        //     // Útil para dashboards categorizados por área de negocio
        // }

        /// <summary>
        /// Obtiene reportes programados para ejecución automática
        /// </summary>
        // public async Task<IEnumerable<ReporteDto>> GetReportesProgramadosAsync()
        // {
        //     // Lista de reportes con automatización configurada
        //     // Esencial para scheduler de reportes automáticos
        // }

        /// <summary>
        /// Clona configuración de reporte existente con modificaciones
        /// </summary>
        // public async Task<ReporteDto> ClonarReporteAsync(int reporteId, string nuevoNombre)
        // {
        //     // Duplicar configuración para crear variantes similares
        //     // Acelera creación de reportes con configuraciones parecidas
        // }

        /// <summary>
        /// Valida configuración de reporte antes de crear/actualizar
        /// </summary>
        // public async Task<ValidationResult> ValidarConfiguracionAsync(ReporteDto reporteDto)
        // {
        //     // Validación avanzada de parámetros y configuración
        //     // Prevenir errores en tiempo de ejecución de reportes
        // }

        /// <summary>
        /// Obtiene estadísticas de uso de reportes
        /// </summary>
        // public async Task<ReporteStatsDto> GetEstadisticasUsoAsync(int reporteId)
        // {
        //     // Métricas: ejecuciones, tiempo promedio, usuarios únicos
        //     // Esencial para optimización y decisiones de negocio
        // }

        // ========== CONSIDERACIONES PARA ESCALABILIDAD ==========
        // FUTURO: Para sistemas de reportes de nivel empresarial:
        // - Versionado de configuraciones de reportes
        // - Workflow de aprobación para cambios críticos
        // - Templates predefinidos para reportes comunes
        // - Integración con herramientas BI (Power BI, Tableau)
        // - Reportes con parámetros dinámicos avanzados
        // - Distribución automática por roles y permisos
        // - Caché inteligente para reportes frecuentes
        // - Exportación a múltiples formatos (PDF, CSV, JSON)
    }
} 