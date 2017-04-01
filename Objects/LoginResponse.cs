using Newtonsoft.Json;

namespace osu_Downloader.Objects
{
    public class LoginResponse
    {
        public string SessionID { get; set; }
        public string DownloadSessionID { get; set; }

        [JsonProperty("header")]
        public string Header { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }
}