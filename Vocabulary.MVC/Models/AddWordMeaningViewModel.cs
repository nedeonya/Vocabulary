
namespace Vocabulary.MVC.Models;

public record AddWordMeaningViewModel(string Name, string Description, string? Example)
{
    public AddWordMeaningViewModel() : this(string.Empty, string.Empty, string.Empty)
    {
    }
}