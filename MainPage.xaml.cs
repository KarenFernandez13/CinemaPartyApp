using System.Collections.ObjectModel;
using ObligatorioTaller1.Models;
using ObligatorioTaller1.Servicios;
using ObligatorioTaller1;
using Microsoft.Maui.Controls;
using System.Xml.Linq;
using System.Linq;
using System.Text.Json;

namespace ObligatorioTaller1
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Populares> Populares { get; set; }
        public List<Populares> TopRated { get; set; }
        public List<Populares> Upcoming { get; set; }

        private readonly IPeliculasPopularesService _services;
        public Command<Populares> TapCommand { get; }

        private const string FileName = "favoritos.json";
        public ObservableCollection<Populares> PeliculasFavoritas { get; set; }
        int userId = Preferences.Get("UserId", 0);

        public MainPage()
        {
            InitializeComponent();

            _services = DependencyService.Get<IPeliculasPopularesService>();
            PeliculasFavoritas = new ObservableCollection<Populares>();            
            
            TapCommand = new Command<Populares>(OnPosterTapped);
            BindingContext = this;

            CargarFavoritos(userId);
            cargarDatos();
        }

        private string ObtenerNombreArchivoFavoritos(int userId) => $"favoritos_{userId}.json";

        private void cargarDatos()
        {
            try
            {
                // Cargar populares
                Populares = new ObservableCollection<Populares>(_services.Obtener());
                CarruselPopulares.ItemsSource = Populares;

                // Cargar top rated
                TopRated = _services.ObtenerTopRated();
                CarruselTopRated.ItemsSource = TopRated;

                //Cargar upcoming
                Upcoming = _services.ObtenerUpcoming();
                CarruselUpcoming.ItemsSource = Upcoming;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                DisplayAlert("Error", ex.Message, "OK");
            }
        }

        
        private async void OnPosterTapped(Populares pelicula)
        {
            if (pelicula != null)
            {
                Detalles detallesPage = new Detalles(pelicula, AgregarAFavoritos, PeliculasFavoritas);
                await Navigation.PushAsync(detallesPage);
            }
        }

        private async void PerfilBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PerfilUsuario());
        }

        // MANEJO DE FAVORITOS
        private async void GuardarFavoritos(int userId)
        {
            try
            {
                var fileName = ObtenerNombreArchivoFavoritos(userId);
                var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
                var json = JsonSerializer.Serialize(PeliculasFavoritas);
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "No se pudo guardar la lista de favoritos.", "Cerrar");
            }
        }

        public async Task<ObservableCollection<Populares>> CargarFavoritos(int userId)
        {
            try
            {
                var fileName = ObtenerNombreArchivoFavoritos(userId);
                var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                if (File.Exists(filePath))
                {
                    var json = await File.ReadAllTextAsync(filePath);
                    var peliculasFavoritas = JsonSerializer.Deserialize<ObservableCollection<Populares>>(json);
                    if (peliculasFavoritas != null)
                    {
                        return peliculasFavoritas; // Devuelve la lista
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "No se pudo cargar la lista de favoritos.", "Cerrar");
            }

            return new ObservableCollection<Populares>(); // Devuelve una lista vacía si ocurre un error o no hay favoritos
        }


        private void OnFavoriteButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var pelicula = button?.BindingContext as Populares;

            if (pelicula != null)
            {
                CambiarEstadoFavorito(pelicula, button);
                GuardarFavoritos(userId);
            }
        }

        private void CambiarEstadoFavorito(Populares pelicula, Button button)
        {
            if (PeliculasFavoritas.Contains(pelicula))
            {
                PeliculasFavoritas.Remove(pelicula);
                button.Text = "🤍";
            }
            else
            {
                PeliculasFavoritas.Add(pelicula);
                button.Text = "❤️";
            }
        }

        private void OnButtonBindingContextChanged(object sender, EventArgs e)
        {
            var button = sender as Button;
            var pelicula = button?.BindingContext as Populares;

            if (pelicula != null)
            {
                button.Text = PeliculasFavoritas.Contains(pelicula) ? "❤️" : "🤍";
            }
        }

        public void AgregarAFavoritos(Populares pelicula)
        {
            if (!PeliculasFavoritas.Contains(pelicula))
            {
                PeliculasFavoritas.Add(pelicula);
                GuardarFavoritos(userId);
            }
        }

        public void EliminarDeFavoritos(Populares pelicula)
        {
            if (PeliculasFavoritas.Contains(pelicula))
            {
                PeliculasFavoritas.Remove(pelicula);
                GuardarFavoritos(userId); // Actualiza la lista en el almacenamiento

            }
        }
       

        //BARRA DE BUSQUEDA
        private async void OnSearchBarButtonPressed(object sender, EventArgs e)
        {
            var searchBar = sender as SearchBar;
            string query = searchBar.Text;

            if (!string.IsNullOrEmpty(query))
            {
                var resultados = await BuscarPeliculasAsync(query);

                // Navega a SearchPage pasando los resultados
                await Navigation.PushAsync(new SearchPage(resultados, AgregarAFavoritos, PeliculasFavoritas));
            }
        }

        private async Task<List<Populares>> BuscarPeliculasAsync(string query)
        {
            string apiKey = "d116ec09b26fb3628590a8c54dc92802";
            string url = $"https://api.themoviedb.org/3/search/movie?query={query}&language=es_ES&api_key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    var searchResult = JsonSerializer.Deserialize<MovieSearchResults>(jsonResult);

                    if (searchResult?.Results != null)
                    {
                        return searchResult.Results;
                    }
                    else
                    {
                        return new List<Populares>();
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No se pudieron cargar los resultados.", "Cerrar");
                    return new List<Populares>();
                }
            }
        }

        
    }
}  
