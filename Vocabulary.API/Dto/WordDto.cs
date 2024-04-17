using Vocabulary.Data.Entities;

namespace Vocabulary.Dto;

public record WordDto(Guid Id, string Name, ICollection<MeaningDto> Meanings) : IWord
{
    ICollection<IMeaning> IWord.Meanings => Meanings.Cast<IMeaning>().ToList();
    public WordDto() : this(Guid.NewGuid(), string.Empty, new List<MeaningDto>())
    {
    }
}
