using osu_Downloader.Objects;
using osu_Downloader.Utilities;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace osu_Downloader.Windows
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Beatmap> Result { get; set; }

        private API api;

        public MainWindow()
        {
            InitializeComponent();

            var response = API.Login("Siketyan", "**********");
	    api = new API(response.SessionID);
            Result = new ObservableCollection<Beatmap>();

            foreach (var beatmap in api.Search("ppp"))
            {
                Result.Add(beatmap);
            }

            DataContext = this;
        }

        public async void Search(object sender, RoutedEventArgs e)
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
    }
}
