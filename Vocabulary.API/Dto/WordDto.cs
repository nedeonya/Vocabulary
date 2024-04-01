using Vocabulary.Data.Entities;

namespace Vocabulary.Dto;

public record WordDto(Guid Id, string Name) : IWord
{
    public WordDto() : this(Guid.NewGuid(), string.Empty)
    {
    }
}
