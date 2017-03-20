using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace osu_Downloader.Objects
{
    public class User
    {
        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("joinDate")]
        public string JoinDate { get; set; }

        [JsonProperty("country")]
        public Country Country { get; set; }

        [JsonProperty("age")]
        public int? Age { get; set; }

        [JsonProperty("avatarUrl")]
        public string AvatarURL { get; set; }

        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }
        
        [JsonProperty("isSupporter")]
        public bool IsSupporter { get; set; }

        [JsonProperty("isGMT")]
        public bool IsGMT { get; set; }

        [JsonProperty("isQAT")]
        public bool IsQAT { get; set; }

        [JsonProperty("isBNG")]
        public bool IsBNG { get; set; }

        [JsonProperty("interests")]
        public string Interests { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("lastvisit")]
        public string LastVisit { get; set; }

        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        [JsonProperty("lastfm")]
        public string LastFm { get; set; }

        [JsonProperty("skype")]
        public string Skype { get; set; }

        [JsonProperty("playstyle", ItemConverterType = typeof(StringEnumConverter))]
        public PlayStyle[] PlayStyle { get; set; }

        [JsonProperty("playmode")]
        public string PlayMode { get; set; }

        [JsonProperty("profileColour")]
        public string ProfileColour { get; set; }

        [JsonProperty("profileOrder")]
        public string[] ProfileOrder { get; set; }

        [JsonProperty("cover")]
        public ProfileCover Cover { get; set; }

        [JsonProperty("kudosu")]
        public Kudosu Kudosu { get; set; }

        [JsonProperty("userAchievements")]
        public Achievement[] Achievements { get; set; }

        [JsonProperty("defaultStatistics")]
        public Statistics DefaultStatistics { get; set; }
    }

    public class Country
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ProfileCover
    {
        [JsonProperty("customUrl")]
        public string CustomURL { get; set; }

        [JsonProperty("url")]
        public string URL { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }
    }

    public class Kudosu
    {
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("available")]
        public long Available { get; set; }
    }

    public class Achievement
    {
        [JsonProperty("achievement_id")]
        public int ID { get; set; }

        [JsonProperty("achieved_at")]
        public string AchievedDate { get; set; }
    }

    public class Statistics
    {
        [JsonProperty("rank")]
        public Rank Rank { get; set; }

        [JsonProperty("level")]
        public Level Level { get; set; }

        [JsonProperty("pp")]
        public float PP { get; set; }

        [JsonProperty("rankedScore")]
        public long RankedScore { get; set; }

        [JsonProperty("hitAccuracy")]
        public float Accuracy { get; set; }
        
        [JsonProperty("playCount")]
        public long PlayCount { get; set; }

        [JsonProperty("totalScore")]
        public long TotalScore { get; set; }

        [JsonProperty("totalHits")]
        public long TotalHits { get; set; }

        [JsonProperty("maximumCombo")]
        public int MaxCombo { get; set; }

        [JsonProperty("replaysWatchedByOthers")]
        public long ReplaysWatchedByOthers { get; set; }

        [JsonProperty("scoreRanks")]
        public ScoreRanks ScoreRanks { get; set; }
    }

    public class Rank
    {
        [JsonProperty("isRanked")]
        public bool IsRanked { get; set; }

        [JsonProperty("global")]
        public long? Global { get; set; }
        
        [JsonProperty("country")]
        public long? Country { get; set; }
    }

    public class Level
    {
        [JsonProperty("current")]
        public int Current { get; set; }

        [JsonProperty("progress")]
        public int Progress { get; set; }
    }

    public class ScoreRanks
    {
        [JsonProperty("XH")]
        public long XH { get; set; }

        [JsonProperty("SH")]
        public long SH { get; set; }

        [JsonProperty("X")]
        public long X { get; set; }

        [JsonProperty("S")]
        public long S { get; set; }

        [JsonProperty("A")]
        public long A { get; set; }
    }

    public enum PlayStyle
    {
        Mouse,
        Keyboard,
        Tablet,
        Touch
    }
}