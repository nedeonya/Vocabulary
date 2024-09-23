using Vocabulary.Contracts;

namespace Vocabulary.API.Dto;

public record WordDto(Guid Id, string Name, ICollection<MeaningDto> Meanings, string? UserId) : IWord
{
    ICollection<IMeaning> IWord.Meanings => Meanings.Cast<IMeaning>().ToList();
    public WordDto() : this(Guid.NewGuid(), string.Empty, new List<MeaningDto>(), null)
    {
    }
}
