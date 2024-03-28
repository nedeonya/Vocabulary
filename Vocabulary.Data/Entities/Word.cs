using System.ComponentModel.DataAnnotations.Schema;

namespace Vocabulary.Data.Entities
{
    public class Word : IWord
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public ICollection<Meaning> Meanings { get; set; }

        [NotMapped]
        public ICollection<IMeaning> WordMeanings
        {
            get => Meanings?.Cast<IMeaning>().ToList();
            set => Meanings = value?.Cast<Meaning>().ToList();
        }
    }
}
