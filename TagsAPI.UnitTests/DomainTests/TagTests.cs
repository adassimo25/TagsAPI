using TagsAPI.Domain;

namespace TagsAPI.UnitTests.DomainTests
{
    public class TagTests
    {
        [Fact]
        public void Create_ForGivenValues_CreatesTag()
        {
            var id = Guid.NewGuid();
            var name = "Tag1";
            var count = 10;
            var share = 0.5;
            var creationDate = DateTime.Now;

            var tag = Tag.Create(id, name, count, share, creationDate);

            Assert.Equal(id, tag.Id);
            Assert.Equal(name, tag.Name);
            Assert.Equal(count, tag.Count);
            Assert.Equal(share, tag.Share);
            Assert.Equal(creationDate, tag.CreationDate);
            Assert.Equal(creationDate, tag.ModificationDate);
        }

        [Fact]
        public void ChangeCount_ForNewCountValue_ChangesCountValueAndModificationDate()
        {
            var tag = new Tag();
            var newCount = 10;
            var modificationDate = DateTime.Now;

            tag.ChangeCount(newCount, modificationDate);

            Assert.Equal(newCount, tag.Count);
            Assert.Equal(modificationDate, tag.ModificationDate);
        }

        [Fact]
        public void ChangeShare_ForNewShareValue_ChangesShareValueAndModificationDate()
        {
            var tag = new Tag();
            var newShare = 0.5;
            var modificationDate = DateTime.Now;

            tag.ChangeShare(newShare, modificationDate);

            Assert.Equal(newShare, tag.Share);
            Assert.Equal(modificationDate, tag.ModificationDate);
        }

        [Theory]
        [InlineData(5, 5, false)]
        [InlineData(5, 10, true)]
        [InlineData(10, 10, false)]
        [InlineData(10, 5, true)]
        public void UpdateIfNeeded_ForGivenValues_UpdatesCountIfDifferentAndReturnsTrue(int oldCount, int newCount, bool expected)
        {
            var tag = Tag.Create(Guid.NewGuid(), "Tag1", oldCount, 0.5, DateTime.Now);
            var updateTime = DateTime.Now.AddMinutes(1);

            bool updated = tag.UpdateIfNeeded(newCount, tag.Share, updateTime);

            Assert.Equal(expected, updated);
            Assert.Equal(expected ? newCount : oldCount, tag.Count);
            Assert.Equal(expected ? updateTime : tag.CreationDate, tag.ModificationDate);
        }

        [Theory]
        [InlineData(0.5, 0.5, false)]
        [InlineData(0.5, 0.25, true)]
        [InlineData(0.25, 0.25, false)]
        [InlineData(1.0000009, 1.0, false)]
        [InlineData(1.0000011, 1.0, true)]
        public void UpdateIfNeeded_ForGivenValues_UpdatesShareIfDifferentAndReturnsTrue(double oldShare, double newShare, bool expected)
        {
            var tag = Tag.Create(Guid.NewGuid(), "Tag1", 10, oldShare, DateTime.Now);
            var updateTime = DateTime.Now.AddMinutes(1);

            bool updated = tag.UpdateIfNeeded(tag.Count, newShare, updateTime);

            Assert.Equal(expected, updated);
            Assert.Equal(expected ? newShare : oldShare, tag.Share);
            Assert.Equal(expected ? updateTime : tag.CreationDate, tag.ModificationDate);
        }
    }
}
