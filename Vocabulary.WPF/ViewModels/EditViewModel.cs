using System.Windows.Input;

namespace Vocabulary.WPF.ViewModels
{
    public class EditViewModel: WordMeaningViewModel
    {
        public EditViewModel(WordMeaning wordMeaning, Action<WordMeaning> onSave, Action onCancel) : base(wordMeaning)
        {
            SaveCommand = new DelegateCommand(() => onSave(WordMeaning));
            CancelCommand = new DelegateCommand(onCancel);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

    }
}
