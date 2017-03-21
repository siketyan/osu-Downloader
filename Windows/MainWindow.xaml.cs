using Newtonsoft.Json;
using osu_Downloader.Objects;
using osu_Downloader.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
        private Config config;

        public MainWindow()
        {
            InitializeComponent();

            if (File.Exists("config.json"))
            {
                config = Config.Open();
            }
            else
            {
                config = new Config();

                var window = new LoginWindow();
                window.ShowDialog();

                var response = window.Response;
                config.SessionID = response.SessionID;
                config.Save();
            }

	        api = new API(config.SessionID);
            Result = new ObservableCollection<Beatmap>();

            DataContext = this;
        }

        private async void Search(object sender, RoutedEventArgs e)
        {
            SearchButton.IsEnabled = false;
            Loader.Visibility = Visibility.Visible;
            Result.Clear();

            var beatmaps = await api.SearchAsync(Query.Text);
            foreach (var beatmap in beatmaps)
            {
                Result.Add(beatmap);
            }

            Loader.Visibility = Visibility.Hidden;
            SearchButton.IsEnabled = true;
        }

        private void SelectBeatmap(object sender, MouseButtonEventArgs e)
        {
            var id = (long)((Border)sender).Tag;
            Selected = Result.Where(b => b.ID == id)
                             .FirstOrDefault();

            OnPropertyChanged("Selected");
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            Search(this, null);
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
