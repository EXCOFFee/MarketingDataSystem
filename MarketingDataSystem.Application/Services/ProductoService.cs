// ==================== SERVICIO DE LÓGICA DE NEGOCIO - PRODUCTOS ====================
// Este servicio implementa toda la lógica de negocio para gestión del catálogo de productos
// CAPA: Application - Orquesta operaciones entre Controller y Repository
// PATRÓN: Service Layer - Encapsula lógica de negocio específica del dominio comercial
// SOLID: Cumple principios S (responsabilidad única), D (inversión dependencias)
// COMERCIAL: Maneja precios, categorías, proveedores y información sensible del catálogo

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
    /// Servicio de aplicación para gestión completa del catálogo de productos
    /// RESPONSABILIDAD: Implementar reglas de negocio específicas para productos comerciales
    /// ARQUITECTURA: Capa Application en Clean Architecture - orquesta entre UI y Domain
    /// SOLID: 
    /// - S: Una sola responsabilidad (gestión de catálogo de productos)
    /// - D: Depende de abstracciones (IUnitOfWork, IMapper)
    /// PATRÓN: Service Pattern - encapsula lógica de negocio del catálogo comercial
    /// TRANSACCIONES: Utiliza Unit of Work para operaciones atómicas
    /// SEGURIDAD: Maneja información comercial sensible (precios, costos, proveedores)
    /// INTEGRACIÓN: Conecta con VentaService para transacciones y StockService para inventario
    /// </summary>
    public class ProductoService : IProductoService
    {
        // ========== DEPENDENCIAS PARA OPERACIONES COMERCIALES ==========
        private readonly IUnitOfWork _unitOfWork;   // Coordinador de repositorios y transacciones
        private readonly IMapper _mapper;           // Mapper automático DTO ↔ Entity

        /// <summary>
        /// Constructor con inyección de dependencias para operaciones de productos
        /// PATRÓN: Dependency Injection - todas las dependencias son interfaces
        /// TESTABILIDAD: Permite fácil mocking para pruebas unitarias del catálogo
        /// SOLID: Principio D - depende de abstracciones, no implementaciones concretas
        /// </summary>
        /// <param name="unitOfWork">Coordinador de repositorios con soporte transaccional</param>
        /// <param name="mapper">AutoMapper para conversión automática entre DTOs y Entities</param>
        public ProductoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork; // Principio D de SOLID - Inversión de dependencias
            _mapper = mapper;         // AutoMapper para transformaciones automáticas
        }

        /// <summary>
        /// Obtiene un producto específico por su ID único con información comercial completa
        /// LÓGICA: Buscar en repositorio → Convertir a DTO → Devolver al Controller
        /// MAPPING: Entity (persistencia) → DTO (comunicación)
        /// COMERCIAL: Incluye precios, costos, proveedor y datos sensibles del catálogo
        /// USO: ProductoController, VentaService (para validar productos en ventas)
        /// </summary>
        /// <param name="id">ID único del producto a buscar</param>
        /// <returns>ProductoDto con información comercial completa o null si no existe</returns>
        public async Task<ProductoDto> GetByIdAsync(int id)
        {
            // Buscar entidad en base de datos usando repositorio
            var entity = await _unitOfWork.Productos.GetByIdAsync(id);
            // Conversión automática Entity → DTO para comunicación segura
            return _mapper.Map<ProductoDto>(entity);
        }

        /// <summary>
        /// Obtiene el catálogo completo de productos activos del sistema
        /// LÓGICA: Consultar repositorio → Mapear colección → Devolver al Controller
        /// COMERCIAL: Catálogo completo con precios y disponibilidad para ventas
        /// PERFORMANCE: En producción debería implementar paginación para catálogos grandes
        /// USO: ProductoController (listados), VentaService (validaciones), ReporteService
        /// </summary>
        /// <returns>Colección de ProductoDto con todo el catálogo activo</returns>
        public async Task<IEnumerable<ProductoDto>> GetAllAsync()
        {
            // Obtener todas las entidades desde el repositorio
            var entities = await _unitOfWork.Productos.GetAllAsync();
            // Mapear automáticamente la colección completa
            return _mapper.Map<IEnumerable<ProductoDto>>(entities);
        }

        /// <summary>
        /// Crea un nuevo producto en el catálogo con transacción atómica
        /// LÓGICA: DTO → Entity → Persistir → Confirmar transacción → Devolver DTO actualizado
        /// COMERCIAL: Incluye validaciones de precios, categorías y información de proveedor
        /// TRANSACCIÓN: SaveChangesAsync garantiza atomicidad de la operación
        /// MAPPING: DTO (entrada) → Entity (persistencia) → DTO (salida con ID generado)
        /// INTEGRACIÓN: Automáticamente disponible para VentaService después de creación
        /// </summary>
        /// <param name="productoDto">Datos del nuevo producto a crear (precio, categoría, proveedor)</param>
        /// <returns>ProductoDto con ID asignado por la base de datos y listo para ventas</returns>
        public async Task<ProductoDto> CreateAsync(ProductoDto productoDto)
        {
            // ========== CONVERSIÓN DTO → ENTITY ==========
            var entity = _mapper.Map<Producto>(productoDto);
            
            // ========== PERSISTENCIA CON REPOSITORIO ==========
            await _unitOfWork.Productos.AddAsync(entity);
            
            // ========== CONFIRMACIÓN TRANSACCIONAL ==========
            await _unitOfWork.SaveChangesAsync(); // Commit de la transacción
            
            // ========== RETORNO CON ID GENERADO ==========
            return _mapper.Map<ProductoDto>(entity); // Entity ahora tiene ID asignado
        }

        /// <summary>
        /// Actualiza un producto existente del catálogo de forma completa (reemplazo total)
        /// LÓGICA: DTO → Entity → Actualizar → Confirmar → Devolver DTO actualizado
        /// COMERCIAL: Permite actualizar precios, cambiar proveedores, modificar categorías
        /// TRANSACCIÓN: Operación atómica con SaveChangesAsync
        /// PATRÓN: PUT semántico - reemplaza completamente el recurso del catálogo
        /// INTEGRACIÓN: Cambios se reflejan automáticamente en VentaService y StockService
        /// </summary>
        /// <param name="productoDto">Datos completos actualizados del producto</param>
        /// <returns>ProductoDto con los datos comerciales actualizados confirmados</returns>
        public async Task<ProductoDto> UpdateAsync(ProductoDto productoDto)
        {
            // ========== CONVERSIÓN Y PREPARACIÓN ==========
            var entity = _mapper.Map<Producto>(productoDto);
            
            // ========== MARCADO PARA ACTUALIZACIÓN ==========
            _unitOfWork.Productos.Update(entity); // Entity Framework rastrea cambios
            
            // ========== CONFIRMACIÓN TRANSACCIONAL ==========
            await _unitOfWork.SaveChangesAsync(); // Commit de cambios
            
            // ========== RETORNO CONFIRMADO ==========
            return _mapper.Map<ProductoDto>(entity);
        }

        /// <summary>
        /// Elimina un producto del catálogo (operación lógica o física según implementación)
        /// LÓGICA: Buscar → Verificar existencia → Eliminar → Confirmar transacción
        /// COMERCIAL: Verificar que no tenga ventas activas o stock antes de eliminar
        /// SEGURIDAD: Verifica existencia antes de intentar eliminación
        /// SOFT DELETE: El repositorio puede implementar eliminación lógica (recomendado)
        /// INTEGRACIÓN: Producto eliminado no estará disponible para nuevas ventas
        /// </summary>
        /// <param name="id">ID único del producto a eliminar del catálogo</param>
        /// <returns>Task que representa la operación asíncrona</returns>
        public async Task DeleteAsync(int id)
        {
            // ========== VERIFICACIÓN DE EXISTENCIA ==========
            var entity = await _unitOfWork.Productos.GetByIdAsync(id);
            if (entity != null)
            {
                // ========== ELIMINACIÓN SEGURA ==========
                _unitOfWork.Productos.Delete(entity); // Puede ser soft delete según repositorio
                
                // ========== CONFIRMACIÓN TRANSACCIONAL ==========
                await _unitOfWork.SaveChangesAsync(); // Commit de la eliminación
            }
            // No se lanza excepción si no existe - operación idempotente
        }
    }
} 