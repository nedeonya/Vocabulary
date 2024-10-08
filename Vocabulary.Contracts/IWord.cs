namespace Vocabulary.Contracts;

public interface IWord
{
    Guid Id { get; }
    string Name { get; }
    ICollection<IMeaning> Meanings { get; }
    string? UserId { get; }
} 