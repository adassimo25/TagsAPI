using Microsoft.EntityFrameworkCore;

namespace TagsAPI.DataAccess.Repositories
{
    public abstract class AbstractRepository<TEntity, TIdentity>(TagsDbContext dbContext) : IRepository<TEntity, TIdentity>
        where TEntity : class
        where TIdentity : notnull
    {
        protected TagsDbContext DbContext { get; } = dbContext;
        protected DbSet<TEntity> DbSet { get; } = dbContext.Set<TEntity>();

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
            DbContext.SaveChanges();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
            DbContext.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
            DbContext.SaveChanges();
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
            DbContext.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
            DbContext.SaveChanges();
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
            DbContext.SaveChanges();
        }

        public TEntity Find(TIdentity id)
        {
            return DbSet.Find(id)!;
        }

        public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbSet.Add(entity);
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            DbSet.AddRange(entities);
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbSet.Remove(entity);
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            DbSet.RemoveRange(entities);
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbSet.Update(entity);
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            DbSet.UpdateRange(entities);
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public abstract Task<TEntity> FindAsync(TIdentity id, CancellationToken cancellationToken = default);
    }

    public interface IRepository<TEntity, TIdentity>
        where TEntity : class
    {
        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void UpdateRange(IEnumerable<TEntity> entities);

        TEntity Find(TIdentity id);

        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task RemoveRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<TEntity> FindAsync(TIdentity id, CancellationToken cancellationToken = default);
    }
}
