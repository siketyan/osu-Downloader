using osu_Downloader.Objects;
using osu_Downloader.Utilities;
using System.Windows;

namespace osu_Downloader.Windows
{
    /// <summary>
    /// LoginWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginResponse Response { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void Login(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled = false;
            Response = await API.LoginAsync(Username.Text, Password.Password);
            Close();
        } 
    }
}