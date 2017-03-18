using Newtonsoft.Json;
using osu_Downloader.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace osu_Downloader.Utilities
{
    public class API
    {
        private const string baseURL = "https://new.ppy.sh/";
        private string sessionID;

        public API(string sid)
        {
            sessionID = sid;
        }

        private string Get(string url, Dictionary<string, string> parameters)
        {
            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.Cookie] = "osu_session=" + sessionID;

                return wc.DownloadString(
                           baseURL + url + "?"
                               + string.Join(
                                     "&", parameters.Select(p => p.Key + "=" + p.Value)
                                 )
                       );
            }
        }

        private string Post(string url, Dictionary<string, string> parameters)
        {
            using (var wc = new WebClient())
            {
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                byte[] request = Encoding.ASCII.GetBytes(
                                     string.Join(
                                         "&", parameters.Select(p => p.Key + "=" + p.Value)
                                     )
                                 );
                byte[] response = wc.UploadData(baseURL + url, "POST", request);

                return Encoding.ASCII.GetString(response);
            }
        }

        public Beatmap[] Search(string query,
                                GameModeSearchOption mode = GameModeSearchOption.Osu,
                                RankStatusSearchOption status = RankStatusSearchOption.Any,
                                GenreSearchOption? genre = null,
                                LanguageSearchOption? language = null,
                                ExtraSearchOption? extra = null)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("q", query);
            parameters.Add("m", ((int)mode).ToString());
            parameters.Add("s", ((int)status).ToString());
            if (genre != null) parameters.Add("g", ((int)genre).ToString());
            if (language != null) parameters.Add("l", ((int)language).ToString());
            if (extra != null) parameters.Add("e", ((ExtraSearchOption)extra).GetValue());

            return JsonConvert.DeserializeObject<Beatmap[]>(
                       Get("beatmapsets/search", parameters)
                   );
        }
    }

    public enum GameModeSearchOption
    {
        Osu = 0,
        Taiko = 1,
        Catch = 2,
        Mania = 3
    }

    public enum RankStatusSearchOption
    {
        RankedAndApproved = 0,
        Approved = 1,
        Favourites = 2,
        ModRequests = 3,
        Pending = 4,
        Graveyard = 5,
        MyMaps = 6,
        Any = 7,
        Loved = 8
    }

    public enum GenreSearchOption
    {
        Unspecified = 1,
        VideoGame = 2,
        Anime = 3,
        Rock = 4,
        Pop = 5,
        Other = 6,
        Novelty = 7,
        HipHop = 9,
        Electronic = 10
    }

    public enum LanguageSearchOption
    {
        Other = 1,
        English = 2,
        Japanese = 3,
        Chinese = 4,
        Instrumental = 5,
        Korean = 6,
        French = 7,
        German = 8,
        Swedish = 9,
        Spanish = 10,
        Italian = 11,
    }

    public enum ExtraSearchOption
    {
        Video,
        Storyboard,
        Both
    }

    public static class ExtraSearchOptionExt
    {
        private static readonly string[] values = new string[] { "0", "1", "0-1" };
        
        public static string GetValue(this ExtraSearchOption extra)
        {
            return values[(int)extra];
        }
    }
}