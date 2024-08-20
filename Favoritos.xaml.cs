using ObligatorioTaller1.Models;
using System.Collections.ObjectModel;
using ObligatorioTaller1.Servicios;

namespace ObligatorioTaller1;

public partial class Favoritos : ContentPage
{
    private ObservableCollection<Populares> _peliculasFavoritas;
    public Command<Populares> TapCommand { get; }

    private readonly IPeliculasPopularesService _services;

    private readonly Action<Populares> _agregarAFavoritos;
    public Command<Populares> EliminarFavoritoCommand { get; }

    private readonly Action<Populares> _eliminarDeFavoritos;
  
    public Favoritos(ObservableCollection<Populares> peliculasFavoritas, Action<Populares> agregarAFavoritos, Action<Populares> eliminarDeFavoritos)
    {
        InitializeComponent();
        _services = DependencyService.Get<IPeliculasPopularesService>();
        _peliculasFavoritas = peliculasFavoritas;        
        _agregarAFavoritos = agregarAFavoritos;
        TapCommand = new Command<Populares>(OnPosterTapped);
        BindingContext = this;
        _eliminarDeFavoritos = eliminarDeFavoritos;

        EliminarFavoritoCommand = new Command<Populares>(eliminarDeFavoritos);
    }

    public ObservableCollection<Populares> PeliculasFavoritas => _peliculasFavoritas;

    private async void OnPosterTapped(Populares pelicula)
    {
        if (pelicula != null)
        {
            var detallesPage = new Detalles(pelicula, _agregarAFavoritos, _peliculasFavoritas);
            await Navigation.PushAsync(detallesPage);
        }
    }

    private void EliminarDeFavoritos(Populares pelicula)
    {
        if (pelicula != null && _peliculasFavoritas.Contains(pelicula))
        {
            _eliminarDeFavoritos?.Invoke(pelicula); 

            CollectionFavoritos.ItemsSource = null;
            CollectionFavoritos.ItemsSource = _peliculasFavoritas;
        }
    }
}