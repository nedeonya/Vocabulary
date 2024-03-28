namespace Vocabulary.Data.Entities;

public interface IWord
{
    Guid Id { get; set; }
    string Name { get; set; }
}