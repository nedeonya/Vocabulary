namespace Vocabulary.MVC.Models;

public record EditWordMeaningViewModel(Guid WordId, string Name, Guid MeaningId, string Description, string? Example)
{
    public EditWordMeaningViewModel() : this(Guid.NewGuid(), string.Empty, Guid.NewGuid(),  string.Empty, string.Empty)
    {
    }
}