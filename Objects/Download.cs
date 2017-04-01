using osu_Downloader.Utilities;
using osu_Downloader.Windows;
using System.ComponentModel;

namespace osu_Downloader.Objects
{
    public class Download : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Downloader Downloader { get; set; }
        public double ProgressBarWidth { get; private set; }
        public string ProgressStr { get; private set; } = "0 %";
        public double Progress
        {
            get { return progress; }
            set
            {
                progress = value;

                ProgressBarWidth = progress * 2;
                ProgressStr = progress + " %";

                OnPropertyChanged("Progress");
                OnPropertyChanged("ProgressStr");
                OnPropertyChanged("ProgressWidth");

                MainWindow.GetInstance().RefreshDownloads();
            }
        }
        public Beatmap Beatmap { get; set; }

        private double progress;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}