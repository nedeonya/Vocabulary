namespace Vocabulary.Data.Entities
{
    public record Meaning(Guid Id, string Description, string? Example, Guid WordId) : IMeaning
    {
        public Meaning() : this(Guid.NewGuid(), string.Empty, string.Empty, Guid.Empty)
        {
        }
        
        public Meaning(string Description, string? Example, Guid WordId) : this(Guid.NewGuid(), Description, Example, WordId)
        {
        }
    }
}

