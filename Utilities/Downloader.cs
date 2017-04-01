using osu_Downloader.Objects;
using System;
using System.ComponentModel;
using System.Net;

namespace osu_Downloader.Utilities
{
    public class Downloader
    {
        public event EventHandler<Download> Progress;
        public event EventHandler<Download> Downloaded;

        private WebClient client;
        private Download download;
        
        public Download Download(Beatmap beatmap, string sessionID, bool withVideo)
        {
            var req = WebRequest.CreateHttp("https://osu.ppy.sh/d/" + beatmap.ID + (withVideo ? "" : "n"));
            req.Headers[HttpRequestHeader.Cookie] = "phpbb3_2cjk5_sid=" + sessionID;
            req.AllowAutoRedirect = false;

            var res = req.GetResponse();
            var location = res.Headers[HttpResponseHeader.Location];

            using (client = new WebClient())
            {
                client.Headers[HttpRequestHeader.Cookie] = "phpbb3_2cjk5_sid=" + sessionID;
                client.DownloadProgressChanged += OnDownloadProgress;
                client.DownloadFileCompleted += OnDownloaded;

                client.DownloadFileAsync(
                    new Uri(location),
                    "downloads/" + beatmap.ID + ".osz"
                );
            }

            download = new Download
            {
                Downloader = this,
                Beatmap = beatmap
            };

            return download;
        }

        public void Cancel()
        {
            client.CancelAsync();
            client.Dispose();
        }

        private void OnDownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            download.Progress = e.ProgressPercentage;

            Progress(this, download);
        }

        private void OnDownloaded(object sender, AsyncCompletedEventArgs e)
        {
            Downloaded(this, download);
        }
    }
}