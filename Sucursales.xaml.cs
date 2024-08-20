using Microsoft.Maui.Maps;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using ObligatorioTaller1.Models;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Net;

namespace ObligatorioTaller1;


public partial class Sucursales : ContentPage
{
    public ObservableCollection<Pin> pins { get; set; } = new ObservableCollection<Pin>();
    public ObservableCollection<Sucursal> SucursalesDisponibles { get; set; } = new ObservableCollection<Sucursal>();

    public Sucursales()
    {
        InitializeComponent();

        // Vincular el CollectionView con las colecciones observables
        SucursalesCollectionView.ItemsSource = SucursalesDisponibles;
      
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            SucursalesDisponibles.Clear();
            pins.Clear();
            map.Pins.Clear();
            var geolocationRequest = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(30));
            var location = await Geolocation.GetLocationAsync(geolocationRequest);

            if (location != null)
            {
                if (map != null)
                {
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(1.2)));
                    var sucursales = await App.Database.GetSucursalesAsync();

                    foreach (var sucursal in sucursales)
                    {
                        SucursalesDisponibles.Add(sucursal);
                        var pin = new Pin
                        {
                            Label = sucursal.Name,
                            Address = sucursal.Address,
                            Location = new Location(sucursal.Latitude, sucursal.Longitude)
                        };
                        pins.Add(pin);
                    }

                    foreach (var pin in pins)
                    {
                        map.Pins.Add(pin);
                    }
                    map.ItemsSource = pins;

                }
                else
                {
                    await DisplayAlert("Error", "El mapa no está inicializado.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "No se pudo obtener la ubicación.", "OK");
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error al obtener la ubicación: {ex.Message}", "OK");
        }


    }

    private async void ButtonAdd_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AgregarSucursal(this.pins));
    }

    private async void ButtonRemove_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EliminarSucursal(this.SucursalesDisponibles));
    }
}