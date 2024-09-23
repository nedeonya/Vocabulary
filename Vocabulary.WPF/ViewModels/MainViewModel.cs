using RestSharp;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Vocabulary.WPF.ViewModels
{
    public sealed class MainViewModel : BaseViewModel, IDisposable
    {    
        private readonly RestClient _restClient;

        public MainViewModel()
        {
            _restClient = new RestClient(Constants.ServiceUrl);

            Words = new ObservableCollection<WordMeaningItemViewModel>();

            ActualizeWordsList();

            AddWordCommand = new DelegateCommand(() => OpenEditView(new WordMeaning(), Method.Post));
        }
        
        public ObservableCollection<WordMeaningItemViewModel> Words { get; }
        public ICommand AddWordCommand { get; }

        public void Dispose() => _restClient.Dispose();

        private void ActualizeWordsList()
        {

            var request = new RestRequest(Constants.ApiWordUrl, Method.Get);
            var response = _restClient.Execute<IEnumerable<Word>>(request);
            if (response.IsSuccessful)
            {
                Words.Clear();

                foreach (var word in response.Data)
                {
                    var wordMeaning = new WordMeaning()
                    {
                        Id = word.Id,
                        Name = word.Name,
                        MeaningId = word.Meanings.Select(m => m.Id).First(),
                        Description = word.Meanings.Select(m => m.Description).First(),
                        Example = word.Meanings.Select(m => m.Example).First(),
                    };
                    Words.Add(new WordMeaningItemViewModel(
                        wordMeaning,
                        wm => OpenEditView(wm, Method.Put),
                        DeleteWord));
                }
            }
            else
            {
                MessageBox.Show("Failed to get words");
            }
        }

        private void OpenEditView(WordMeaning wordMeaning, Method apimethod)
        {
            EditWindow editDialog = null!;
            editDialog = new EditWindow
            {
                DataContext = new EditViewModel(wordMeaning, SaveWord, CloseWindow)
            };
            editDialog.ShowDialog();

            void SaveWord(WordMeaning wordMeaning)
            {
                string apiUrl = wordMeaning.IsEmpty() ? Constants.ApiWordUrl : $"{Constants.ApiWordUrl}/{wordMeaning.Id}/{wordMeaning.MeaningId}";

                var request = new RestRequest(apiUrl, apimethod);
                request.AddJsonBody(new
                {
                    Id = wordMeaning.Id,
                    Name = wordMeaning.Name,
                    MeaningId = wordMeaning.MeaningId,
                    Description = wordMeaning.Description,
                    Example = wordMeaning.Example
                });

                var response = _restClient.Execute(request);
                if (!response.IsSuccessful)
                {
                    MessageBox.Show("Failed to save word");
                    return;
                }
                else
                {
                    ActualizeWordsList();
                }

                CloseWindow();
            }

            void CloseWindow()
            {
                editDialog.Close();
            }
        }

        private void DeleteWord(WordMeaning wordViewModel)
        {

            if (MessageBox.Show("Are you sure you want to delete this word?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var request = new RestRequest($"{Constants.ApiWordUrl}/{wordViewModel.Id}", Method.Delete);
                var response = _restClient.Execute(request);
                if (!response.IsSuccessful)
                {
                    MessageBox.Show("Failed to delete word");
                    return;
                }
                else
                {
                    ActualizeWordsList();
                }
            }
        }
    }
}
