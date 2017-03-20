using osu_Downloader.Objects;
using osu_Downloader.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace osu_Downloader.Windows
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Beatmap> Result { get; set; }
        public Beatmap Selected { get; set; }

        private API api;

        public MainWindow()
        {
            InitializeComponent();

            var window = new LoginWindow();
            window.ShowDialog();

            var response = window.Response;
	        api = new API(response.SessionID);
            Result = new ObservableCollection<Beatmap>();

            foreach (var beatmap in api.Search("ppp"))
            {
                Result.Add(beatmap);
            }

            DataContext = this;
        }

        private async void Search(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            Loader.Visibility = Visibility.Visible;
            Result.Clear();

            var beatmaps = await api.SearchAsync(Query.Text);
            foreach (var beatmap in beatmaps)
            {
                Result.Add(beatmap);
            }

            Loader.Visibility = Visibility.Hidden;
            ((Button)sender).IsEnabled = true;
        }

        private void SelectBeatmap(object sender, MouseButtonEventArgs e)
        {
            var id = (long)((Border)sender).Tag;
            Selected = Result.Where(b => b.ID == id)
                             .FirstOrDefault();

            OnPropertyChanged("Selected");
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
