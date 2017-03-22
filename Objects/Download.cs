using System;

namespace osu_Downloader.Objects
{
    public class Download
    {
        public double ProgressBarWidth { get; set; }
        public string ProgressStr { get; private set; }
        public double Progress
        {
            get { return progress; }
            set
            {
                progress = value;

                ProgressBarWidth = progress * 2;
                ProgressStr = Math.Round(progress, 2, MidpointRounding.AwayFromZero)
                                  .ToString() + " %";
            }
        }
        public Beatmap Beatmap { get; set; }

        private double progress;
    }
}