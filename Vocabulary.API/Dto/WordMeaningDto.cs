namespace Vocabulary.API.Dto;

public record WordMeaningDto(Guid wordId, string Name, Guid meaningId, string Description, string? Example)
{
    public WordMeaningDto() : this(Guid.NewGuid(), string.Empty, Guid.NewGuid(),  string.Empty, string.Empty)
    {
    }
}