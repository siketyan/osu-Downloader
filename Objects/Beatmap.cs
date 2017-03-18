using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace osu_Downloader.Objects
{
    public class Beatmap
    {
        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("play_count")]
        public long PlayCount { get; set; }

        [JsonProperty("favourite_count")]
        public long FavouriteCount { get; set; }

        [JsonProperty("submitted_date")]
        public string SubmittedDate { get; set; }

        [JsonProperty("last_updated")]
        public string LastUpdatedDate { get; set; }

        [JsonProperty("ranked_date")]
        public string RankedDate { get; set; }

        [JsonProperty("creator")]
        public string Creator { get; set; }

        [JsonProperty("user_id")]
        public long CreatorID { get; set; }

        [JsonProperty("bpm")]
        public float BPM { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("covers")]
        public Cover Covers { get; set; }

        [JsonProperty("previewUrl")]
        public string PreviewUrl { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("video")]
        public bool HasVideo { get; set; }

        [JsonProperty("has_scores")]
        public bool HasScores { get; set; }

        [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))]
        public RankStatus Ranked { get; set; }

        [JsonProperty("beatmaps")]
        public Difficulty[] Difficulties { get; set; }
    }

    public class Cover
    {
        [JsonProperty("cover")]
        public string CoverUrl { get; set; }

        [JsonProperty("cover@2x")]
        public string Cover2xUrl { get; set; }

        [JsonProperty("card")]
        public string CardUrl { get; set; }

        [JsonProperty("card@2x")]
        public string Card2xUrl { get; set; }

        [JsonProperty("list")]
        public string ListUrl { get; set; }

        [JsonProperty("list@2x")]
        public string List2xUrl { get; set; }
    }

    public class Difficulty
    {
        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("version")]
        public string Name { get; set; }

        [JsonProperty("mode")]
        public GameMode Mode { get; set; }

        [JsonProperty("total_length")]
        public int Length { get; set; }

        [JsonProperty("difficulty_rating")]
        public float Stars { get; set; }

        [JsonProperty("cs")]
        public float CircleSize { get; set; }

        [JsonProperty("drain")]
        public float HPDrain { get; set; }

        [JsonProperty("accuracy")]
        public float Accuracy { get; set; }

        [JsonProperty("ar")]
        public float ApproachRate { get; set; }

        [JsonProperty("playcount")]
        public long PlayCount { get; set; }

        [JsonProperty("passcount")]
        public long PassCount { get; set; }

        [JsonProperty("count_circles")]
        public long CircleCount { get; set; }

        [JsonProperty("count_sliders")]
        public long SliderCount { get; set; }

        [JsonProperty("last_updated")]
        public string LastUpdatedDate { get; set; }

        [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))]
        public RankStatus Ranked { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public enum RankStatus
    {
        Loved,
        Qualified,
        Approved,
        Ranked,
        Pending,
        WIP,
        Graveyard
    }

    public enum GameMode
    {
        Osu,
        Taiko,
        Fruits,
        Mania
    }
}