namespace Vocabulary.Data.Entities;

public interface IWord
{
    Guid Id { get; }
    string Name { get; }
    ICollection<IMeaning> Meanings { get; }
} 