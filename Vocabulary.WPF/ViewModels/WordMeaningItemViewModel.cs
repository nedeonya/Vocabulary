using System.Windows.Input;

namespace Vocabulary.WPF.ViewModels
{
    public class WordMeaningItemViewModel : WordMeaningViewModel
    {
        public WordMeaningItemViewModel(WordMeaning wordMeaning, Action<WordMeaning> onEdit, Action<WordMeaning> onDelete): base(wordMeaning)
        {
            EditCommand = new DelegateCommand(() => onEdit(wordMeaning));
            DeleteCommand = new DelegateCommand(() => onDelete(wordMeaning));
        }

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
    }
}
