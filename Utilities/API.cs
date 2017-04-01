using Newtonsoft.Json;
using osu_Downloader.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private const string baseURLOld = "https://osu.ppy.sh/";

        public string SessionID { get; private set; }
        public string DownloadSessionID { get; private set; }

        public API(string sid)
        {
            SessionID = sid;
        }

        private string Get(string url, Dictionary<string, string> parameters)
        {
            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.Cookie] = "osu_session=" + SessionID;

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

        private static Tuple<string, string> Post(string url, Dictionary<string, string> parameters)
        {
            using (var wc = new WebClient())
            {
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                
                var data = wc.UploadData(
                               url, "POST",
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

        private static string CookiePost(string url, Dictionary<string, string> parameters)
        {
            var wc = new CookieWebClient();
            var data = wc.Post(
                           new Uri(url),
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
            
            Debug.WriteLine(wc.Cookies);

            return wc.Cookies;
        }

        public static LoginResponse Login(string username, string password)
        {
            var response = Post(
                               baseURL + "users/login",
                               new Dictionary<string, string>
                               {
                                   {"username", username},
                                   {"password", password}
                               }
                           );

            var dResponse = CookiePost(
                               "https://osu.ppy.sh/forum/ucp.php?mode=login",
                               new Dictionary<string, string>
                               {
                                   {"username", username},
                                   {"password", password},
                                   {"autologin", "1"},
                                   {"login", "login"}
                               }
                            );

            var sessionID = Regex.Split(response.Item2, "(?<!expires=.{3}),")
                                 .Select(s => s.Split(';').First().Split('='))
                                 .Select(xs => new { Name = xs.First(), Value = string.Join("=", xs.Skip(1).ToArray()) })
                                 .Where(a => a.Name == "osu_session")
                                 .FirstOrDefault()
                                 .Value;

            var dSessionID = Regex.Split(dResponse, "(?<!expires=.{3}),")
                                 .Select(s => s.Split(';').First().Split('='))
                                 .Select(xs => new { Name = xs.First(), Value = string.Join("=", xs.Skip(1).ToArray()) })
                                 .Where(a => a.Name == "phpbb3_2cjk5_sid")
                                 .FirstOrDefault()
                                 .Value;

            var data = JsonConvert.DeserializeObject<LoginResponse>(response.Item1);
            data.SessionID = sessionID;
            data.DownloadSessionID = dSessionID;

            return data;
        }

        public static async Task<LoginResponse> LoginAsync(string username, string password)
        {
            return await Task.Run(() => Login(username, password));
        }

        public Beatmap[] Search(string query,
                                GameModeSearchOption mode = GameModeSearchOption.Osu,
                                RankStatusSearchOption status = RankStatusSearchOption.RankedAndApproved,
                                GenreSearchOption genre = GenreSearchOption.Any,
                                LanguageSearchOption language = LanguageSearchOption.Any,
                                ExtraSearchOption extra = ExtraSearchOption.Any)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("q", query);
            parameters.Add("m", ((int)mode).ToString());
            parameters.Add("s", ((int)status).ToString());
            if (genre != GenreSearchOption.Any) parameters.Add("g", ((int)genre).ToString());
            if (language != LanguageSearchOption.Any) parameters.Add("l", ((int)language).ToString());
            if (extra != ExtraSearchOption.Any) parameters.Add("e", ((ExtraSearchOption)extra).GetValue());

            return JsonConvert.DeserializeObject<Beatmap[]>(
                       Get("beatmapsets/search", parameters)
                   );
        }

        public async Task<Beatmap[]> SearchAsync(string query,
                                                  GameModeSearchOption mode = GameModeSearchOption.Osu,
                                                  RankStatusSearchOption status = RankStatusSearchOption.RankedAndApproved,
                                                  GenreSearchOption genre = GenreSearchOption.Any,
                                                  LanguageSearchOption language = LanguageSearchOption.Any,
                                                  ExtraSearchOption extra = ExtraSearchOption.Any)
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
        Any = 0,
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
        Any = 0,
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
        Italian = 11
    }

    public enum ExtraSearchOption
    {
        Any = 0,
        Video = 1,
        Storyboard = 2,
        Both = 3
    }

    public static class ExtraSearchOptionExt
    {
        private static readonly string[] values = new string[] { "0", "1", "0-1" };
        
        public static string GetValue(this ExtraSearchOption extra)
        {
            return values[(int)extra - 1];
        }
    }
}