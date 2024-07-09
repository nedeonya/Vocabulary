using Vocabulary.Data.Entities;

namespace Vocabulary.MVC.Models;
public record WordViewModel(Guid Id, string Name, List<MeaningViewModel> Meanings)
{
    public WordViewModel() : this(Guid.NewGuid(), string.Empty, new List<MeaningViewModel>())
    {
    }
}
