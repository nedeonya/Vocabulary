using System.Text.Json.Serialization;
using Vocabulary.Data.Entities;

namespace Vocabulary.MVC.Models;
public record WordViewModel(Guid Id, string Name, List<MeaningViewModel> Meanings): IWord
{
    ICollection<IMeaning> IWord.Meanings => Meanings.Cast<IMeaning>().ToList();
    public WordViewModel() : this(Guid.NewGuid(), string.Empty, new List<MeaningViewModel>())
    {
    }
}
