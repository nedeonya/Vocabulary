using Vocabulary.Data.Entities;

namespace Vocabulary.Dto;

public class WordDto: IWord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
}