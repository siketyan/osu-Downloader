using osu_Downloader.Objects;
using osu_Downloader.Utilities;
using System;
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

            try
            {
                Response = await API.LoginAsync(Username.Text, Password.Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't log into osu!.\nCheck the network connection, username and password.\n" + ex.Message);
            }

            Close();
        } 
    }
}