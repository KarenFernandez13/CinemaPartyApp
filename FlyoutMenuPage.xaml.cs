using ObligatorioTaller1.Models;
using ObligatorioTaller1.Servicios;

namespace ObligatorioTaller1;
public partial class FlyoutMenuPage : ContentPage
{
    public ListView ListView { get; set; }
    private readonly IPeliculasPopularesService _services;

    public FlyoutMenuPage()
    {
        InitializeComponent();

        _services = DependencyService.Get<IPeliculasPopularesService>();

        var flyoutItems = new List<FlyoutMenuItem>
        {
            new FlyoutMenuItem { Title = "Principal", TargetType = typeof(MainPage) },
            new FlyoutMenuItem { Title = "Favoritos", TargetType = typeof(Favoritos) },
            new FlyoutMenuItem { Title = "Sucursales", TargetType = typeof(Sucursales) },
            new FlyoutMenuItem { Title = "Mi Perfil", TargetType = typeof(PerfilUsuario) }
        };


        ListView = new ListView
        {
            ItemsSource = flyoutItems,
            ItemTemplate = new DataTemplate(() =>
            {
                var icon = new Image { WidthRequest = 50, HeightRequest = 40 };
                icon.SetBinding(Image.SourceProperty, "IconSource");

                var label = new Label { FontSize = 18, VerticalOptions = LayoutOptions.Center };
                label.SetBinding(Label.TextProperty, "Title");

                var stackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Padding = new Thickness(10),
                    Children = { icon, label }
                };

                return new ViewCell { View = stackLayout };
            }),
            SeparatorVisibility = SeparatorVisibility.None,
            BackgroundColor = Color.FromHex("#34495E")
        };

        Content = new StackLayout
        {
            BackgroundColor = Color.FromHex("#2C3E50"),
            Children =
            {
                new Image { Source = "logo2.png", HeightRequest = 80, WidthRequest = 80, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 20, 0, 20) },
                ListView
            }
        };
    }
}    
	
