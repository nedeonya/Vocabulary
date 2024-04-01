namespace Vocabulary.Data.Entities;

public interface IMeaning
{
    Guid Id { get; }
    string Description { get; }
    string? Example { get; }
    Guid WordId { get; }
}