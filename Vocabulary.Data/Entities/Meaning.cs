namespace Vocabulary.Data.Entities
{
    public record Meaning(Guid Id, string Description, string? Example, Guid WordId, Word Word) : IMeaning
    {
        public Meaning() : this(Guid.NewGuid(), string.Empty, string.Empty, Guid.Empty, null)
        {
        }
    }
}

