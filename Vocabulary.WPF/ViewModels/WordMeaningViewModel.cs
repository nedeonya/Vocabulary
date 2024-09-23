namespace Vocabulary.WPF.ViewModels
{
    public class WordMeaningViewModel: BaseViewModel
    {
        private WordMeaning _wordMeaning;
        public WordMeaning WordMeaning => _wordMeaning;

        public WordMeaningViewModel(WordMeaning wordMeaning)
        {
            _wordMeaning = wordMeaning;
            Name = wordMeaning.Name;
            Description = wordMeaning.Description;
            Example = wordMeaning.Example;
        }

        public string Name
        {
            get => _wordMeaning.Name;
            set
            {
                _wordMeaning = _wordMeaning with { Name = value };
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
            get => _wordMeaning.Description;
            set
            {
                _wordMeaning = _wordMeaning with { Description = value };
                OnPropertyChanged(nameof(Description));
            }
        }

        public string? Example
        {
            get => _wordMeaning.Example;
            set
            {
                _wordMeaning = _wordMeaning with { Example = value };
                OnPropertyChanged(nameof(Example));
            }
        }
    }
}
