using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using ObligatorioTaller1.Models;

namespace ObligatorioTaller1.Servicios
{
    class MovieSearchResults
    {
        [JsonPropertyName("results")]
        public List<Populares> Results { get; set; }
    }
}
