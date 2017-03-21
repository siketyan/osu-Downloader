using Newtonsoft.Json;
using System.IO;

namespace osu_Downloader.Utilities
{
    public class Config
    {
        [JsonProperty("version")]
        public int Version { get; set; } = 0;

        [JsonProperty("sessionID")]
        public string SessionID { get; set; }

        public static Config Open()
        {
            using (var reader = new StreamReader("config.json"))
            {
                return JsonConvert.DeserializeObject<Config>(reader.ReadToEnd());
            }
        }

        public void Save()
        {
            using (var writer = new StreamWriter("config.json"))
            {
                writer.Write(JsonConvert.SerializeObject(this));
                writer.Flush();
            }
        }
    }
}