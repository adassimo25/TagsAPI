using Microsoft.EntityFrameworkCore;
using TagsAPI.Domain;

namespace TagsAPI.DataAccess.Repositories
{
    public class TagsRepository(TagsDbContext dbContext) : AbstractRepository<Tag, Guid>(dbContext)
    {
        public override Task<Tag> FindAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return DbSet.AsTracking()
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken)!;
        }
    }
}
