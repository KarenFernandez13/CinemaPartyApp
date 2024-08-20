using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using ObligatorioTaller1.Models;
using System.Collections.ObjectModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ObligatorioTaller1;

public partial class AgregarSucursal : ContentPage
{

    public ObservableCollection<Pin> pins { get; set; }
    public AgregarSucursal(ObservableCollection<Pin> pins)
    {
        this.pins = pins;
        InitializeComponent();
    }

    private async void ButtonCreate_Clicked(object sender, EventArgs e)
    {
        var name = NameEntry.Text;
        var address = AddressEntry.Text;
        var phone = PhoneEntry.Text;
        var horaApertura = HoraAperturaEntry.Text;
        var horaCierre = HoraCierreEntry.Text;
        try
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address))
            {
                await DisplayAlert("Error", "Por favor, ingresa datos v�lidos.", "OK");
                return;
            }
            if (!int.TryParse(phone, out int telefono) ||
                !int.TryParse(horaApertura, out int apertura) ||
                !int.TryParse(horaCierre, out int cierre))
            {
                await DisplayAlert("Error", "Por favor, ingresa n�meros v�lidos para el tel�fono y las horas.", "OK");
                return;
            }

            var locations = await Geocoding.GetLocationsAsync(address);
            var location = locations?.FirstOrDefault();

            if (location != null && location.Latitude != 0 && location.Longitude != 0)
            {
                var nuevaSucursal = new Sucursal
                {
                    Name = name,
                    Address = address,
                    Phone = telefono,
                    HoraApertura = apertura,
                    HoraCierre = cierre,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                };
                try
                {
                    await App.Database.GuardarSucursalAsync(nuevaSucursal);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo guardar la sucursal: {ex.Message}", "OK");
                }

                var pin = new Pin
                {
                    Label = name,
                    Address = address,
                    Location = new Location(location.Latitude, location.Longitude),
                    Type = PinType.Place
                };
                pins.Add(pin);
                await DisplayAlert("Disponible", "Se cre� la sucursal y se a�adi� el pin al mapa", "OK");
            }
            else
            {
                await DisplayAlert("Error", "No se pudo obtener la ubicaci�n a partir de la direcci�n.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurri� un error al geocodificar la direcci�n: {ex.Message}", "OK");
        }

        await Navigation.PopAsync();
    }
}