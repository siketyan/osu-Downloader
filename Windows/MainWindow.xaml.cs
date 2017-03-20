using osu_Downloader.Objects;
using osu_Downloader.Utilities;
using System.Collections.ObjectModel;
using System.Windows;

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
    }
}
