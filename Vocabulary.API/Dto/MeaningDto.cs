using Vocabulary.Data.Entities;

namespace Vocabulary.Dto;

public class MeaningDto : IMeaning
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Description { get; set; }
    public string? Example { get; set; }
    public Guid WordId { get; set; }
}