namespace Vocabulary.Dto;

public record WordWithMeaningDto(string Name, string Description, string? Example)
{
    public WordWithMeaningDto() : this(string.Empty, string.Empty, string.Empty)
    {
    }
}