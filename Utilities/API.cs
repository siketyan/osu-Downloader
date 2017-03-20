using Newtonsoft.Json;
using osu_Downloader.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

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
                                     "&", 
                                     parameters.Select(
                                         p => string.Join(
                                                  "=",
                                                  HttpUtility.UrlEncode(p.Key),
                                                  HttpUtility.UrlEncode(p.Value)
                                              )
                                     )
                                 )
                       );
            }
        }

        private static Tuple<string,string> Post(string url, Dictionary<string, string> parameters)
        {
            using (var wc = new WebClient())
            {
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                var data = wc.UploadData(
                               baseURL + url, "POST",
                               Encoding.UTF8.GetBytes(
                                   string.Join(
                                       "&",
                                       parameters.Select(
                                           p => string.Join(
                                                    "=",
                                                    HttpUtility.UrlEncode(p.Key),
                                                    HttpUtility.UrlEncode(p.Value)
                                                )
                                       )
                                   )
                               )
                           );
                var cookie = wc.ResponseHeaders[HttpResponseHeader.SetCookie];

                return new Tuple<string, string>(Encoding.UTF8.GetString(data), cookie);
            }
        }

        public static LoginResponse Login(string username, string password)
        {
            var response = Post(
                               "users/login",
                               new Dictionary<string, string>
                               {
                                   {"username", username},
                                   {"password", password}
                               }
                           );

            var sessionID = Regex.Split(response.Item2, "(?<!expires=.{3}),")
                                 .Select(s => s.Split(';').First().Split('='))
                                 .Select(xs => new { Name = xs.First(), Value = string.Join("=", xs.Skip(1).ToArray()) })
                                 .Where(a => a.Name == "osu_session")
                                 .FirstOrDefault()
                                 .Value;

            var data = JsonConvert.DeserializeObject<LoginResponse>(response.Item1);
            data.SessionID = sessionID;

            return data;
        }

        private async Task<LoginResponse> LoginAsync(string username, string password)
        {
            return await Task.Run(() => Login(username, password);
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

        private async Task<Beatmap[]> SearchAsync(string query,
                                                  GameModeSearchOption mode = GameModeSearchOption.Osu,
                                                  RankStatusSearchOption status = RankStatusSearchOption.Any,
                                                  GenreSearchOption? genre = null,
                                                  LanguageSearchOption? language = null,
                                                  ExtraSearchOption? extra = null)
        {
            return await Task.Run(() => Search(query, mode, status, genre, language, extra));
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