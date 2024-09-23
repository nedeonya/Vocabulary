using Vocabulary.Contracts;

namespace Vocabulary.WPF
{
    public record Word(Guid Id, string Name, IList<Meaning> Meanings, string? UserId) : IWord
    {
        public Word() : this(Guid.NewGuid(), string.Empty, new List<Meaning>(), null)
        {
        }
        ICollection<IMeaning> IWord.Meanings => Meanings.Cast<IMeaning>().ToList();
    }

}
