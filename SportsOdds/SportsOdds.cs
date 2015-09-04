using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;

namespace SportsOdds
{
    public class Odds
    {
        public DateTime Date { get; set; }
        public string FavoriteTeamName { get; set; }
        public string UnderdogTeamName { get; set; }
        public string HomeTeamName { get; set; }
        public double PointSpread { get; set; }
        public double TotalPoints { get; set; }
        public string SportName { get; set; }
    }

    public class League
    {
        public string Name { get; set; }
        public List<Game> Games { get; set; }
    }

    public class Game
    {
        public string Date { get; set; }
        public string Spread { get; set; }
        public string Total { get; set; }
        public List<Team> Teams { get; set; }
    }

    public class Team
    {
        public string Name { get; set; }
        public string IsFavorite { get; set; }
        public string IsHome { get; set; }
    }

    public static class SportsOdds
    {
        public static IEnumerable<Odds> GetOdds()
        {
            var today = DateTime.Today.ToString("M/d/yyyy");
            var query = "http://asiwebservices.heritagesports.eu:8080/feeds/dannylinesfeed_json.asp?usr=usatoday&pwd=13dT34ty8&startdate=" + today;
            var result = new WebClient().DownloadString(query);
            var json = JObject.Parse(result);

            var leagues = json["lines"]["leagues"].Select(l => JsonConvert.DeserializeObject<League>(l.ToString()));
            var odds = new List<Odds>();
            foreach (var league in leagues)
                foreach (var game in league.Games)
                {
                    var odd = new Odds()
                    {
                        Date = DateTime.ParseExact(game.Date.Replace("PDT ", ""), "ddd MMM d HH:mm:ss yyyy", CultureInfo.CurrentUICulture),
                        FavoriteTeamName = HttpUtility.UrlDecode(game.Teams.First(team => team.IsFavorite == "1").Name),
                        UnderdogTeamName = HttpUtility.UrlDecode(game.Teams.First(team => team.IsFavorite != "1").Name),
                        HomeTeamName = HttpUtility.UrlDecode(game.Teams.First(team => team.IsHome == "1").Name),
                        SportName = HttpUtility.UrlDecode(league.Name),
                        TotalPoints = JsonConvert.DeserializeObject<double>(game.Total == "" ? "NaN" : game.Total.Replace("%26frac12%3B", ".5")),
                        PointSpread = JsonConvert.DeserializeObject<double>(game.Total == "" ? "NaN" : game.Total.Replace("%26frac12%3B", ".5")),
                    };

                    odds.Add(odd);
                }

            return odds;
        }
    }
}