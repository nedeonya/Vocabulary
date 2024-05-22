using System.ComponentModel.DataAnnotations.Schema;

namespace Vocabulary.Data.Entities
{
    public record Word(Guid Id, string Name, ICollection<Meaning> Meanings) : IWord
    {
        ICollection<IMeaning> IWord.Meanings => Meanings.Cast<IMeaning>().ToList();
        public Word(): this(Guid.NewGuid(), string.Empty, new List<Meaning>()) { }
        public Word(string Name): this(Guid.NewGuid(), Name, new List<Meaning>()) { }

        public virtual bool Equals(IWord other)
        {
            return Id == other.Id && Name == other.Name;
        }
    }
}
