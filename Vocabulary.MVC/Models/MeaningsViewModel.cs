using Vocabulary.Data.Entities;

namespace Vocabulary.MVC.Models;

public record MeaningViewModel(Guid Id, string Description, string? Example, Guid WordId)
{
    public MeaningViewModel() : this(Guid.NewGuid(), string.Empty, string.Empty, Guid.Empty)
    {
    }
    
}

