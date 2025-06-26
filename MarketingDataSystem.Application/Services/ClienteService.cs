// ==================== SERVICIO DE LÓGICA DE NEGOCIO - CLIENTES ====================
// Este servicio implementa toda la lógica de negocio para gestión de clientes
// CAPA: Application - Orquesta operaciones entre Controller y Repository
// PATRÓN: Service Layer - Encapsula lógica de negocio específica del dominio
// SOLID: Cumple principios S (responsabilidad única), D (inversión dependencias)
// MAPPING: Convierte entre DTOs (comunicación) y Entities (persistencia)

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
    /// Servicio de aplicación para gestión completa de clientes
    /// RESPONSABILIDAD: Implementar reglas de negocio específicas para clientes
    /// ARQUITECTURA: Capa Application en Clean Architecture - orquesta entre UI y Domain
    /// SOLID: 
    /// - S: Una sola responsabilidad (gestión de clientes)
    /// - D: Depende de abstracciones (IUnitOfWork, IMapper)
    /// PATRÓN: Service Pattern - encapsula lógica de negocio compleja
    /// TRANSACCIONES: Utiliza Unit of Work para operaciones atómicas
    /// </summary>
    public class ClienteService : IClienteService
    {
        // ========== DEPENDENCIAS PARA OPERACIONES DE NEGOCIO ==========
        private readonly IUnitOfWork _unitOfWork;   // Coordinador de repositorios y transacciones
        private readonly IMapper _mapper;           // Mapper automático DTO ↔ Entity

        /// <summary>
        /// Constructor con inyección de dependencias para operaciones de clientes
        /// PATRÓN: Dependency Injection - todas las dependencias son interfaces
        /// TESTABILIDAD: Permite fácil mocking para pruebas unitarias
        /// SOLID: Principio D - depende de abstracciones, no implementaciones concretas
        /// </summary>
        /// <param name="unitOfWork">Coordinador de repositorios con soporte transaccional</param>
        /// <param name="mapper">AutoMapper para conversión automática entre DTOs y Entities</param>
        public ClienteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork; // Principio D de SOLID - Inversión de dependencias
            _mapper = mapper;         // AutoMapper para transformaciones automáticas
        }

        /// <summary>
        /// Obtiene un cliente específico por su ID único
        /// LÓGICA: Buscar en repositorio → Convertir a DTO → Devolver al Controller
        /// MAPPING: Entity (persistencia) → DTO (comunicación)
        /// NULL SAFETY: AutoMapper maneja automáticamente entidades nulas
        /// </summary>
        /// <param name="id">ID único del cliente a buscar</param>
        /// <returns>ClienteDto con datos del cliente o null si no existe</returns>
        public async Task<ClienteDto> GetByIdAsync(int id)
        {
            // Buscar entidad en base de datos usando repositorio
            var entity = await _unitOfWork.Clientes.GetByIdAsync(id);
            // Conversión automática Entity → DTO para comunicación segura
            return _mapper.Map<ClienteDto>(entity);
        }

        /// <summary>
        /// Obtiene todos los clientes activos del sistema
        /// LÓGICA: Consultar repositorio → Mapear colección → Devolver al Controller
        /// PERFORMANCE: En producción debería implementar paginación para datasets grandes
        /// MAPPING: IEnumerable&lt;Entity&gt; → IEnumerable&lt;DTO&gt;
        /// </summary>
        /// <returns>Colección de ClienteDto con todos los clientes activos</returns>
        public async Task<IEnumerable<ClienteDto>> GetAllAsync()
        {
            // Obtener todas las entidades desde el repositorio
            var entities = await _unitOfWork.Clientes.GetAllAsync();
            // Mapear automáticamente la colección completa
            return _mapper.Map<IEnumerable<ClienteDto>>(entities);
        }

        /// <summary>
        /// Crea un nuevo cliente en el sistema con transacción atómica
        /// LÓGICA: DTO → Entity → Persistir → Confirmar transacción → Devolver DTO actualizado
        /// TRANSACCIÓN: SaveChangesAsync garantiza atomicidad de la operación
        /// MAPPING: DTO (entrada) → Entity (persistencia) → DTO (salida con ID generado)
        /// ID GENERADO: La base de datos asigna ID automáticamente
        /// </summary>
        /// <param name="clienteDto">Datos del nuevo cliente a crear</param>
        /// <returns>ClienteDto con ID asignado por la base de datos</returns>
        public async Task<ClienteDto> CreateAsync(ClienteDto clienteDto)
        {
            // ========== CONVERSIÓN DTO → ENTITY ==========
            var entity = _mapper.Map<Cliente>(clienteDto);
            
            // ========== PERSISTENCIA CON REPOSITORIO ==========
            await _unitOfWork.Clientes.AddAsync(entity);
            
            // ========== CONFIRMACIÓN TRANSACCIONAL ==========
            await _unitOfWork.SaveChangesAsync(); // Commit de la transacción
            
            // ========== RETORNO CON ID GENERADO ==========
            return _mapper.Map<ClienteDto>(entity); // Entity ahora tiene ID asignado
        }

        /// <summary>
        /// Actualiza un cliente existente de forma completa (reemplazo total)
        /// LÓGICA: DTO → Entity → Actualizar → Confirmar → Devolver DTO actualizado
        /// TRANSACCIÓN: Operación atómica con SaveChangesAsync
        /// PATRÓN: PUT semántico - reemplaza completamente el recurso
        /// TRACKING: Entity Framework rastrea cambios automáticamente
        /// </summary>
        /// <param name="clienteDto">Datos completos actualizados del cliente</param>
        /// <returns>ClienteDto con los datos actualizados confirmados</returns>
        public async Task<ClienteDto> UpdateAsync(ClienteDto clienteDto)
        {
            // ========== CONVERSIÓN Y PREPARACIÓN ==========
            var entity = _mapper.Map<Cliente>(clienteDto);
            
            // ========== MARCADO PARA ACTUALIZACIÓN ==========
            _unitOfWork.Clientes.Update(entity); // Entity Framework rastrea cambios
            
            // ========== CONFIRMACIÓN TRANSACCIONAL ==========
            await _unitOfWork.SaveChangesAsync(); // Commit de cambios
            
            // ========== RETORNO CONFIRMADO ==========
            return _mapper.Map<ClienteDto>(entity);
        }

        /// <summary>
        /// Elimina un cliente del sistema (operación lógica o física según implementación)
        /// LÓGICA: Buscar → Verificar existencia → Eliminar → Confirmar transacción
        /// SEGURIDAD: Verifica existencia antes de intentar eliminación
        /// SOFT DELETE: El repositorio puede implementar eliminación lógica (recomendado)
        /// TRANSACCIÓN: Operación atómica para consistencia
        /// </summary>
        /// <param name="id">ID único del cliente a eliminar</param>
        /// <returns>Task que representa la operación asíncrona</returns>
        public async Task DeleteAsync(int id)
        {
            // ========== VERIFICACIÓN DE EXISTENCIA ==========
            var entity = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (entity != null)
            {
                // ========== ELIMINACIÓN SEGURA ==========
                _unitOfWork.Clientes.Delete(entity); // Puede ser soft delete según repositorio
                
                // ========== CONFIRMACIÓN TRANSACCIONAL ==========
                await _unitOfWork.SaveChangesAsync(); // Commit de la eliminación
            }
            // No se lanza excepción si no existe - operación idempotente
        }
    }
} 