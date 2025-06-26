using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MarketingDataSystem.Core.Entities;
using MarketingDataSystem.Core.Interfaces;
using MarketingDataSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MarketingDataSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación genérica del repositorio base
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly MarketingDataContext _context;
        protected readonly DbSet<T> _entities;

        /// <summary>
        /// Constructor del repositorio genérico
        /// </summary>
        /// <param name="context">Contexto de la base de datos</param>
        public GenericRepository(MarketingDataContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        /// <summary>
        /// Obtiene una entidad por su ID
        /// </summary>
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        /// <summary>
        /// Obtiene todas las entidades
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.Where(e => e.Activo).ToListAsync();
        }

        /// <summary>
        /// Obtiene entidades que cumplen con un predicado
        /// </summary>
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Agrega una nueva entidad
        /// </summary>
        public virtual async Task AddAsync(T entity)
        {
            entity.FechaCreacion = DateTime.UtcNow;
            await _entities.AddAsync(entity);
        }

        /// <summary>
        /// Actualiza una entidad existente
        /// </summary>
        public virtual void Update(T entity)
        {
            entity.FechaModificacion = DateTime.UtcNow;
            _entities.Update(entity);
        }

        /// <summary>
        /// Elimina una entidad (soft delete)
        /// </summary>
        public virtual void Delete(T entity)
        {
            entity.Activo = false;
            entity.FechaModificacion = DateTime.UtcNow;
            _entities.Update(entity);
        }

        /// <summary>
        /// Guarda los cambios en la base de datos
        /// </summary>
        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
} 