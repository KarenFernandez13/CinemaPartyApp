using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ObligatorioTaller1.Models
{
    public class Populares
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("title")]
        public string title { get; set; }

        [JsonPropertyName("overview")]
        public string overview { get; set; }

        [JsonPropertyName("poster_path")]
        public string poster_path { get; set; }

        public string poster_url = "https://image.tmdb.org/t/p/w500/";

        [JsonPropertyName("release_date")]
        public string release_date { get; set; }

        public string PosterUrl => $"{poster_url}{poster_path}";

    }
}
