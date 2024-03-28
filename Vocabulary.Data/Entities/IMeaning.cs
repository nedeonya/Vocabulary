namespace Vocabulary.Data.Entities;

public interface IMeaning
{
    Guid Id { get; set; }
    string Description { get; set; }
    string Example { get; set; }
    Guid WordId { get; set; }
}