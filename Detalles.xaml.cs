using ObligatorioTaller1.Models;
using ObligatorioTaller1.Servicios;
using System.Collections.ObjectModel;
namespace ObligatorioTaller1;

public partial class Detalles : ContentPage
{
    private readonly IPeliculasPopularesService _services;

    private readonly Action<Populares> _agregarAFavoritos; // Acción para agregar a favoritos

    private ObservableCollection<Populares> _peliculasFavoritas;

    private int movieId;

    private Populares _peliculaActual;

    public Detalles(Populares pelicula, Action<Populares> agregarAFavoritos, ObservableCollection<Populares> peliculasFavoritas)
    {
        InitializeComponent();

        _services = DependencyService.Get<IPeliculasPopularesService>();

        TitleLabel.Text = pelicula.title;
        OverviewLabel.Text = pelicula.overview;
        ReleaseDateLabel.Text = pelicula.release_date;
        string imageUrl = $"https://image.tmdb.org/t/p/w500{pelicula.poster_path}";
        PosterImage.Source = imageUrl;
        _agregarAFavoritos = agregarAFavoritos;
        _peliculasFavoritas = peliculasFavoritas;
        movieId = pelicula.id;
        _peliculaActual = pelicula;

        CargarImagenesCarrusel();
        CargarElenco();
        CargarGeneros();
        CargarCompaniasProductoras();        
    }

    private async void btnVideo_Clicked(object sender, EventArgs e)
    {
        // Mostrar el WebView y el botón "Cerrar"
        videoYT.IsVisible = true;
        btnCerrarVideo.IsVisible = true;
        string trailerUrl = await Task.Run(() => _services.ObtenerTrailer(movieId));

        if (!string.IsNullOrEmpty(trailerUrl))
        {
            var embedHtml = $@"
            <!DOCTYPE html>
            <html>
                <head>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                </head>
                <body style='margin:0; padding:0;'>
                    <iframe width='100%' height='100%' src='{trailerUrl}' frameborder='0' allow='accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture' allowfullscreen></iframe>
                </body>
            </html>";


            videoYT.Source = new HtmlWebViewSource
            {
                Html = embedHtml
            };
        }
        else
        {
            await DisplayAlert("Error", "No se encontró ningún tráiler para esta película.", "OK");
        }

        ActualizarEstadoBotonFavorito();
    }

    private void ActualizarEstadoBotonFavorito()
    {
        if (_peliculasFavoritas.Contains(_peliculaActual))
        {
            AddFavoriteButton.Text = "En favoritos"; // Cambiar el texto y/o apariencia del botón
        }
        else
        {
            AddFavoriteButton.Text = "Agregar a favoritos";
        }
    }

    private void AddFavoriteButton_Clicked(object sender, EventArgs e)
    {
        // Cambiar el estado del favorito cuando el botón es presionado
        if (_peliculasFavoritas.Contains(_peliculaActual))
        {
            _peliculasFavoritas.Remove(_peliculaActual);
            AddFavoriteButton.Text = "Agregar a favoritos"; // Cambiar a no favorito
        }
        else
        {
            _agregarAFavoritos?.Invoke(_peliculaActual);
            AddFavoriteButton.Text = "En favoritos"; // Cambiar a favorito
        }

    }


    private void btnCerrarVideo_Clicked(object sender, EventArgs e)
    {
        // Ocultar el WebView y el botón "Cerrar video"
        videoYT.IsVisible = false;
        btnCerrarVideo.IsVisible = false;

        // Mostrar el botón "Ver trailer"
        btnVideo.IsVisible = true;

        // Opcional: detener el video
        videoYT.Source = null;
    }

    private async void CargarImagenesCarrusel()
    {
        var imagenes = await _services.ObtenerImagenes(movieId);
        CarruselImages.ItemsSource = imagenes;
    }

    private async void CargarElenco()
    {
        var elenco = await _services.ObtenerElenco(movieId);
        ElencoCollectionView.ItemsSource = elenco;
    }

    private async void CargarGeneros()
    {
        var generos = await _services.ObtenerGeneros(movieId);
        GenerosCollectionView.ItemsSource = generos;
    }

    private async void CargarCompaniasProductoras()
    {
        var companias = await _services.ObtenerCompaniasProductoras(movieId);
        CompaniasCollectionView.ItemsSource = companias;
    }

    private void AddToFavorites_Clicked(object sender, EventArgs e)
    {
        var pelicula = _peliculasFavoritas.FirstOrDefault(p => p.id == movieId);

        if (pelicula == null)
        {
            _agregarAFavoritos?.Invoke(new Populares
            {
                id = movieId,
                title = TitleLabel.Text,
                overview = OverviewLabel.Text,
                poster_path = PosterImage.Source.ToString(),
                release_date = ReleaseDateLabel.Text
            });
        }
        else
        {
            DisplayAlert("Información", "Esta película ya está en favoritos.", "Cerrar");
        }
    }


}