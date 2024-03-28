namespace Vocabulary.Data.Entities
{
    public class Meaning: IMeaning
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Description { get; set; }
        public string? Example { get; set; }
        public Guid WordId { get; set; }
        public Word Word { get; set; }
    }
}

