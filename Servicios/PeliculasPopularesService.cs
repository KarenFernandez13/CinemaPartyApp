using ObligatorioTaller1.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Text.Json;
using static System.Net.WebRequestMethods;


namespace ObligatorioTaller1.Servicios
{
    public class PeliculasPopularesService : IPeliculasPopularesService
    {
        private string urlApiPopular = "https://api.themoviedb.org/3/movie/popular";
        private string urlApiTopRated = "https://api.themoviedb.org/3/movie/top_rated";
        private string urlApiUpcoming = "https://api.themoviedb.org/3/movie/upcoming";
        private string apiKey = "d116ec09b26fb3628590a8c54dc92802";
        private string baseImageUrl = "https://image.tmdb.org/t/p/w500"; // Base URL para las imágenes
        
        public List<Populares> Obtener()
        {
            return ObtenerPeliculas(urlApiPopular);
        }

        public List<Populares> ObtenerTopRated()
        {
            return ObtenerPeliculas(urlApiTopRated);
        }

        public List<Populares> ObtenerUpcoming()
        {
            return ObtenerPeliculas(urlApiUpcoming);
        }

        private List<Populares> ObtenerPeliculas(string urlApi)
        {
            var client = new RestClient();
            var request = new RestRequest(urlApi, Method.Get);
            request.AddParameter("api_key", apiKey);

            RestResponse response = client.Execute(request);

            JsonNode nodos = JsonNode.Parse(response.Content);
            JsonNode results = nodos["results"];

            var peliculasData = JsonSerializer.Deserialize<List<Populares>>(results.ToString());

            // Construir URL completa para las imágenes
            foreach (var pelicula in peliculasData)
            {
                pelicula.poster_path = $"{baseImageUrl}{pelicula.poster_path}";
            }

            return peliculasData;
        }

        public Populares ObtenerDetallesPelicula(int movieId)
        {
            string urlDetalleApi = $"https://api.themoviedb.org/3/movie/{movieId}";
            var client = new RestClient();
            var request = new RestRequest(urlDetalleApi, Method.Get);
            request.AddParameter("api_key", apiKey);

            RestResponse response = client.Execute(request);

            var peliculaData = JsonSerializer.Deserialize<Populares>(response.Content);

            peliculaData.poster_path = $"{baseImageUrl}{peliculaData.poster_path}";

            return peliculaData;
        }

        // Método para obtener el tráiler de una película
        public string ObtenerTrailer(int movieId)
        {
            string urlVideosApi = $"https://api.themoviedb.org/3/movie/{movieId}/videos";
            var client = new RestClient();
            var request = new RestRequest(urlVideosApi, Method.Get);
            request.AddParameter("api_key", apiKey);

            RestResponse response = client.Execute(request);

            JsonNode nodos = JsonNode.Parse(response.Content);
            JsonNode results = nodos["results"];

            var videos = JsonSerializer.Deserialize<List<VideoResult>>(results.ToString());

            // Buscar el tráiler
            var trailer = videos.FirstOrDefault(v => v.type == "Trailer" && v.site == "YouTube");

            if (trailer != null)
            {
                // Devolver la URL de YouTube del tráiler
                return $"https://www.youtube.com/embed/{trailer.key}";
            }

            return null; // Retornar null si no se encuentra un tráiler
        }

        public async Task<List<string>> ObtenerImagenes(int movieId)
        {
            string url = $"https://api.themoviedb.org/3/movie/{movieId}/images?api_key={apiKey}";
            var client = new RestClient();
            var request = new RestRequest(url, Method.Get);

            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                JsonNode jsonNode = JsonNode.Parse(response.Content);
                JsonArray backdrops = jsonNode["backdrops"].AsArray();

                // Extraer las URLs de las imágenes
                var imagenes = backdrops.Select(b => $"https://image.tmdb.org/t/p/w500{b["file_path"].ToString()}").ToList();

                return imagenes;
            }

            return new List<string>(); // Retornar lista vacía en caso de error
        }

        public async Task<List<string>> ObtenerElenco(int movieId)
        {
            string url = $"https://api.themoviedb.org/3/movie/{movieId}/credits?api_key={apiKey}";
            var client = new RestClient();
            var request = new RestRequest(url, Method.Get);

            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                JsonNode jsonNode = JsonNode.Parse(response.Content);
                JsonArray castArray = jsonNode["cast"].AsArray();

                // Extraer los nombres del elenco
                var elenco = castArray.Select(c => c["name"].ToString()).ToList();

                return elenco;
            }

            return new List<string>(); // Retornar lista vacía en caso de error
        }

        public async Task<List<string>> ObtenerGeneros(int movieId)
        {
            string url = $"https://api.themoviedb.org/3/movie/{movieId}?api_key={apiKey}";
            var client = new RestClient();
            var request = new RestRequest(url, Method.Get);

            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                JsonNode jsonNode = JsonNode.Parse(response.Content);
                JsonArray generosArray = jsonNode["genres"].AsArray();

                // Extraer los nombres de los géneros
                var generos = generosArray.Select(g => g["name"].ToString()).ToList();

                return generos;
            }

            return new List<string>(); // Retornar lista vacía en caso de error
        }

        public async Task<List<string>> ObtenerCompaniasProductoras(int movieId)
        {
            string url = $"https://api.themoviedb.org/3/movie/{movieId}?api_key={apiKey}";
            var client = new RestClient();
            var request = new RestRequest(url, Method.Get);

            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                JsonNode jsonNode = JsonNode.Parse(response.Content);
                JsonArray companiasArray = jsonNode["production_companies"].AsArray();

                // Extraer los nombres de las compañías productoras
                var companias = companiasArray.Select(c => c["name"].ToString()).ToList();

                return companias;
            }

            return new List<string>(); // Retornar lista vacía en caso de error
        }
    }
}
