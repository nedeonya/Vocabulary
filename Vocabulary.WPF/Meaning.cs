using Vocabulary.Contracts;

namespace Vocabulary.WPF
{
    public record Meaning (Guid Id, string Description, string? Example, Guid WordId): IMeaning
    {
        public Meaning() : this(Guid.NewGuid(), string.Empty, string.Empty, Guid.Empty)
        {
        }
    }
}