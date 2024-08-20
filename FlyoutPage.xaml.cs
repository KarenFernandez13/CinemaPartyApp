using ObligatorioTaller1.Servicios;
using ObligatorioTaller1.Models;

namespace ObligatorioTaller1;
public partial class FlyoutPage : Microsoft.Maui.Controls.FlyoutPage
{
    private readonly IPeliculasPopularesService _services;
    

    public FlyoutPage()
    {
        InitializeComponent();
        _services = DependencyService.Get<IPeliculasPopularesService>();

        // Crear una instancia de FlyoutMenuPage
        Flyout = MenuFlyoutPage;

        // Pasar el servicio a MainPage
        Detail = new NavigationPage(new MainPage());

        // Manejar la navegación desde el menú
        ((FlyoutMenuPage)Flyout).ListView.ItemSelected += OnMenuItemSelected;

        

    }

    private async void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        int userId = Preferences.Get("UserId", 0);

        var item = e.SelectedItem as FlyoutMenuItem;
        if (item != null)
        {
            Page page = null;

            // Instanciar MainPage para acceder a los métodos auxiliares
            var mainPage = new MainPage();

            if (item.TargetType == typeof(MainPage))
            {
                page = mainPage; // Usar la instancia de MainPage
            }
            else if (item.TargetType == typeof(Favoritos))
            {
                // Acceder a los métodos auxiliares a través de la instancia de MainPage
                var peliculasFavoritas = await mainPage.CargarFavoritos(userId);
                var agregarAFavoritos = new Action<Populares>(mainPage.AgregarAFavoritos);
                var eliminarDeFavoritos = new Action<Populares>(mainPage.EliminarDeFavoritos);

                // Instanciar Favoritos con los parámetros obtenidos de MainPage                
                page = new Favoritos(peliculasFavoritas, agregarAFavoritos, eliminarDeFavoritos);
            }
            else
            {
                page = (Page)Activator.CreateInstance(item.TargetType);
            }

            // Realizar la navegación
            Detail = new NavigationPage(page);
            IsPresented = false;
            ((FlyoutMenuPage)Flyout).ListView.SelectedItem = null;
        }
    }
}