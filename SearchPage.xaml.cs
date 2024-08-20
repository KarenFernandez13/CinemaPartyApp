using ObligatorioTaller1.Servicios;
using ObligatorioTaller1.Models;
using System.Collections.ObjectModel;

namespace ObligatorioTaller1;

public partial class SearchPage : ContentPage
{
    public ObservableCollection<Populares> BusquedaResultados { get; set; }
    public Command<Populares> TapCommand { get; }

    private readonly IPeliculasPopularesService _services;

    private readonly Action<Populares> _agregarAFavoritos;

    private ObservableCollection<Populares> _peliculasFavoritas;

    public SearchPage(List<Populares> resultados, Action<Populares> agregarAFavoritos, ObservableCollection<Populares> peliculasFavoritas)
    {
        InitializeComponent();
        _services = DependencyService.Get<IPeliculasPopularesService>() ?? throw new ArgumentNullException(nameof(_services));
        
        // Inicializa la colección de resultados de búsqueda
        BusquedaResultados = new ObservableCollection<Populares>(resultados);
        if (resultados == null || resultados.Count == 0)
        {
            DisplayAlert("Error", "No se encontraron resultados.", "Cerrar");
        }

        _agregarAFavoritos = agregarAFavoritos;
        _peliculasFavoritas = peliculasFavoritas;

        // Establece el BindingContext para que la UI pueda enlazarse con los datos
        BindingContext = this;

        TapCommand = new Command<Populares>(OnImageTapped);
    }

    private async void OnImageTapped(Populares pelicula)
    {
        if (pelicula != null)
        {
            var detallesPage = new Detalles(pelicula, _agregarAFavoritos, _peliculasFavoritas);
            await Navigation.PushAsync(detallesPage);
        }
    }


}