using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MarketingDataSystem.Core.Entities;

namespace MarketingDataSystem.Core.Interfaces
{
    /// <summary>
    /// Interfaz genérica para los repositorios
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    public interface IGenericRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Obtiene una entidad por su ID
        /// </summary>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todas las entidades
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Obtiene entidades que cumplen con un predicado
        /// </summary>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Agrega una nueva entidad
        /// </summary>
        Task AddAsync(T entity);

        /// <summary>
        /// Actualiza una entidad existente
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Elimina una entidad
        /// </summary>
        void Delete(T entity);

        /// <summary>
        /// Guarda los cambios en la base de datos
        /// </summary>
        Task<int> SaveChangesAsync();
    }
} 