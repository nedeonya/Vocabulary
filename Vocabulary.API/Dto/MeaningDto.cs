using Vocabulary.Contracts;

namespace Vocabulary.API.Dto;

public record MeaningDto(Guid Id, string Description, string? Example, Guid WordId): IMeaning
{
    public MeaningDto() : this(Guid.NewGuid(), string.Empty, string.Empty, Guid.Empty)
    {
    }
    
    public MeaningDto(string Description, string? Example, Guid WordId) : this(Guid.NewGuid(), Description, Example, WordId)
    {
    }
}
