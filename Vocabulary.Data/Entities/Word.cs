using System.ComponentModel.DataAnnotations.Schema;

namespace Vocabulary.Data.Entities
{
    public record Word(Guid Id, string Name, ICollection<Meaning> Meanings) : IWord
    {
        public Word(): this(Guid.NewGuid(), string.Empty, new List<Meaning>()) { }
    }
}
