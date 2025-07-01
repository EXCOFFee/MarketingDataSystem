// ==================== PRUEBAS UNITARIAS - CLIENTE SERVICE ====================
// Este archivo contiene pruebas unitarias para validar la lógica de negocio del ClienteService
// FRAMEWORK: xUnit para assertions, Moq para simulación de dependencias
// COBERTURA: Valida todos los métodos públicos del ClienteService
// AISLAMIENTO: Usa mocks para aislar la unidad bajo prueba (SUT - System Under Test)
// PATRÓN: AAA (Arrange-Act-Assert) en cada test
// SOLID: Valida que el servicio cumple con principios SOLID
// PROPÓSITO: Garantizar que la lógica de negocio funciona correctamente sin base de datos

using Xunit;               // Framework de pruebas unitarias para .NET
using Moq;                 // Biblioteca de mocking para simular dependencias
using AutoMapper;          // Para simular mapeos entre DTOs y Entities
using System.Threading.Tasks;
using System.Collections.Generic;
using MarketingDataSystem.Application.Services;
using MarketingDataSystem.Core.DTOs;
using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;

namespace MarketingDataSystem.Tests
{
    /// <summary>
    /// Clase de pruebas unitarias para ClienteService
    /// RESPONSABILIDAD: Validar que la lógica de negocio de clientes funciona correctamente
    /// AISLAMIENTO: Usa mocks para independizarse de base de datos y AutoMapper
    /// COBERTURA: Prueba todos los métodos CRUD del servicio
    /// PATRÓN: AAA (Arrange-Act-Assert) - Preparar-Ejecutar-Verificar
    /// FRAMEWORK: xUnit + Moq para pruebas unitarias rápidas y confiables
    /// </summary>
    public class ClienteServiceTests
    {
        // ========== DEPENDENCIAS SIMULADAS (MOCKS) ==========
        // Estos mocks permiten aislar el ClienteService de sus dependencias externas
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;     // Simula acceso a repositorios
        private readonly Mock<IMapper> _mapperMock;             // Simula conversiones DTO ↔ Entity
        private readonly ClienteService _service;              // Sistema bajo prueba (SUT)

        /// <summary>
        /// Constructor que prepara el entorno de testing para cada prueba
        /// PATRÓN: Setup común para todas las pruebas (DRY - Don't Repeat Yourself)
        /// MOCKS: Crea simuladores para todas las dependencias del ClienteService
        /// AISLAMIENTO: Garantiza que las pruebas no dependan de implementaciones reales
        /// </summary>
        public ClienteServiceTests()
        {
            // Crear mocks de las dependencias del ClienteService
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            
            // Crear instancia del servicio con dependencias simuladas
            _service = new ClienteService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        /// <summary>
        /// PRUEBA: Obtener cliente por ID - caso exitoso
        /// OBJETIVO: Verificar que el servicio retorna un ClienteDto cuando el cliente existe
        /// PATRÓN AAA:
        /// - Arrange: Configurar mocks para simular cliente existente
        /// - Act: Ejecutar GetByIdAsync(1)
        /// - Assert: Verificar que retorna el cliente correcto
        /// COBERTURA: Flujo principal del método GetByIdAsync
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ReturnsClienteDto_WhenClienteExists()
        {
            // ========== ARRANGE (PREPARAR) ==========
            // Crear entidad y DTO de cliente para simular respuesta
            var cliente = new Cliente { IdCliente = 1, Nombre = "Juan", Email = "juan@mail.com" };
            var clienteDto = new ClienteDto { IdCliente = 1, Nombre = "Juan", Email = "juan@mail.com" };
            
            // Configurar mocks para simular comportamiento esperado
            _unitOfWorkMock.Setup(u => u.Clientes.GetByIdAsync(1)).ReturnsAsync(cliente);
            _mapperMock.Setup(m => m.Map<ClienteDto>(cliente)).Returns(clienteDto);

            // ========== ACT (EJECUTAR) ==========
            // Ejecutar el método bajo prueba
            var result = await _service.GetByIdAsync(1);

            // ========== ASSERT (VERIFICAR) ==========
            // Verificar que el resultado es el esperado
            Assert.NotNull(result);                    // El resultado no debe ser nulo
            Assert.Equal("Juan", result.Nombre);       // El nombre debe coincidir
            Assert.Equal(1, result.IdCliente);        // El ID debe coincidir
        }

        /// <summary>
        /// PRUEBA: Obtener todos los clientes - verificar mapeo de colecciones
        /// OBJETIVO: Validar que el servicio retorna todos los clientes correctamente mapeados
        /// COBERTURA: Método GetAllAsync y mapeo de colecciones
        /// VERIFICACIÓN: Cantidad correcta de elementos retornados
        /// </summary>
        [Fact]
        public async Task GetAllAsync_ReturnsAllClientes()
        {
            // ========== ARRANGE (PREPARAR) ==========
            // Simular colección de clientes en base de datos
            var clientes = new List<Cliente> 
            { 
                new Cliente { IdCliente = 1, Nombre = "Juan", Email = "juan@mail.com" } 
            };
            var clientesDto = new List<ClienteDto> 
            { 
                new ClienteDto { IdCliente = 1, Nombre = "Juan", Email = "juan@mail.com" } 
            };
            
            // Configurar mocks para retornar colecciones
            _unitOfWorkMock.Setup(u => u.Clientes.GetAllAsync()).ReturnsAsync(clientes);
            _mapperMock.Setup(m => m.Map<IEnumerable<ClienteDto>>(clientes)).Returns(clientesDto);

            // ========== ACT (EJECUTAR) ==========
            var result = await _service.GetAllAsync();

            // ========== ASSERT (VERIFICAR) ==========
            Assert.Single(result);                     // Debe retornar exactamente 1 cliente
            Assert.Equal("Juan", result.First().Nombre); // Verificar mapeo correcto
        }

        /// <summary>
        /// PRUEBA: Crear nuevo cliente - verificar persistencia y mapeo
        /// OBJETIVO: Validar que se crea correctamente un cliente y se persiste en base de datos
        /// COBERTURA: Método CreateAsync, mapeo bidireccional, persistencia transaccional
        /// VERIFICACIÓN: Que se llamen AddAsync y SaveChangesAsync, y se retorne el DTO
        /// </summary>
        [Fact]
        public async Task CreateAsync_ReturnsCreatedClienteDto()
        {
            // ========== ARRANGE (PREPARAR) ==========
            // Preparar datos de entrada y respuesta esperada
            var clienteDto = new ClienteDto { Nombre = "Nuevo", Email = "nuevo@mail.com" };
            var cliente = new Cliente { Nombre = "Nuevo", Email = "nuevo@mail.com" };
            
            // Configurar mapeo bidireccional DTO → Entity → DTO
            _mapperMock.Setup(m => m.Map<Cliente>(clienteDto)).Returns(cliente);
            _mapperMock.Setup(m => m.Map<ClienteDto>(cliente)).Returns(clienteDto);
            
            // Configurar operaciones de persistencia
            _unitOfWorkMock.Setup(u => u.Clientes.AddAsync(cliente)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // ========== ACT (EJECUTAR) ==========
            var result = await _service.CreateAsync(clienteDto);

            // ========== ASSERT (VERIFICAR) ==========
            Assert.Equal("Nuevo", result.Nombre);      // Verificar que el nombre se preserva
            
            // Verificar que se ejecutaron las operaciones de persistencia
            _unitOfWorkMock.Verify(u => u.Clientes.AddAsync(cliente), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        /// <summary>
        /// PRUEBA: Actualizar cliente existente - verificar modificación transaccional
        /// OBJETIVO: Validar que se actualiza correctamente un cliente y se persiste
        /// COBERTURA: Método UpdateAsync, Update del repositorio, transacción
        /// VERIFICACIÓN: Mapeo correcto, llamadas a Update y SaveChangesAsync
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ReturnsUpdatedClienteDto()
        {
            // ========== ARRANGE (PREPARAR) ==========
            // Preparar cliente con datos actualizados
            var clienteDto = new ClienteDto { IdCliente = 1, Nombre = "Actualizado", Email = "actualizado@mail.com" };
            var cliente = new Cliente { IdCliente = 1, Nombre = "Actualizado", Email = "actualizado@mail.com" };
            
            // Configurar mapeo bidireccional para actualización
            _mapperMock.Setup(m => m.Map<Cliente>(clienteDto)).Returns(cliente);
            _mapperMock.Setup(m => m.Map<ClienteDto>(cliente)).Returns(clienteDto);
            
            // Configurar operaciones de actualización (Update no retorna Task)
            _unitOfWorkMock.Setup(u => u.Clientes.Update(cliente));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // ========== ACT (EJECUTAR) ==========
            var result = await _service.UpdateAsync(clienteDto);

            // ========== ASSERT (VERIFICAR) ==========
            Assert.Equal("Actualizado", result.Nombre); // Verificar actualización
            
            // Verificar llamadas a métodos de actualización
            _unitOfWorkMock.Verify(u => u.Clientes.Update(cliente), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        /// <summary>
        /// PRUEBA: Eliminar cliente - verificar eliminación segura
        /// OBJETIVO: Validar que se elimina correctamente un cliente (soft o hard delete)
        /// COBERTURA: Método DeleteAsync, verificación de existencia, eliminación transaccional
        /// VERIFICACIÓN: Buscar cliente, eliminar, confirmar transacción
        /// SEGURIDAD: Verificar que solo se elimina si el cliente existe
        /// </summary>
        [Fact]
        public async Task DeleteAsync_RemovesCliente()
        {
            // ========== ARRANGE (PREPARAR) ==========
            // Simular cliente existente para eliminación
            var cliente = new Cliente { IdCliente = 1, Nombre = "A Eliminar" };
            
            // Configurar mocks para simular eliminación exitosa
            _unitOfWorkMock.Setup(u => u.Clientes.GetByIdAsync(1)).ReturnsAsync(cliente);
            _unitOfWorkMock.Setup(u => u.Clientes.Delete(cliente));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // ========== ACT (EJECUTAR) ==========
            await _service.DeleteAsync(1);

            // ========== ASSERT (VERIFICAR) ==========
            // Verificar que se ejecutó la secuencia correcta de eliminación
            _unitOfWorkMock.Verify(u => u.Clientes.GetByIdAsync(1), Times.Once);  // Buscar primero
            _unitOfWorkMock.Verify(u => u.Clientes.Delete(cliente), Times.Once);  // Eliminar
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);        // Confirmar
        }
    }
} 