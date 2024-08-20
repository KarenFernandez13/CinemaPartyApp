using Microsoft.Maui.Controls;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using System.Text.Json;
using System;
using ObligatorioTaller1.Servicios;
using ObligatorioTaller1.Models;

namespace ObligatorioTaller1
{
    public partial class Login : ContentPage
    {
        private readonly IPeliculasPopularesService _services;
        public Login()
        {
            InitializeComponent();

            _services = DependencyService.Get<IPeliculasPopularesService>();

            var manu = DeviceInfo.Current.Manufacturer;
            var name = DeviceInfo.Current.Name;
            var type = DeviceInfo.Current.DeviceType;
            var idiom = DeviceInfo.Current.Idiom;
            var version = DeviceInfo.Current.Version;
            var model = DeviceInfo.Current.Model;
            var platform = DeviceInfo.Current.Platform;
            var version2 = DeviceInfo.Current.VersionString;
        }


        private async void Huella_Clicked(object sender, EventArgs e)
        {
            if (DeviceInfo.Current.Idiom == DeviceIdiom.Desktop)
            {
                await DisplayAlert("Autenticación", "La autenticación por huella no está disponible en computadoras.", "Cerrar");
                return; // No proceder con la autenticación
            }
            var request = new AuthenticationRequestConfiguration("Authentication", "Verify fingerprint");
            var result = await CrossFingerprint.Current.AuthenticateAsync(request);

            if (result.Authenticated)
            {
                //var mainPage = new MainPage();
                //await Navigation.PushAsync(mainPage);
                Application.Current.MainPage = new FlyoutPage();
            }
            else
            {
                await DisplayAlert("Autenticacion", "No se pudo tomar el dato", "Cerrar");
            }
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var username = UsernameEntry.Text;
            var password = PasswordEntry.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Autenticación", "Por favor, ingrese usuario y contraseña", "Cerrar");
                return;
            }

            try
            {
                // Verificar si el usuario existe en la base de datos
                var cliente = await App.Database.GetClienteByCredentialsAsync(username, password);

                if (cliente != null)
                {
                    // Si la autenticación es exitosa, navega a la página principal
                    var mainPage = new MainPage();
                    await Navigation.PushAsync(mainPage);
                    Application.Current.MainPage = new FlyoutPage();
                    Preferences.Set("UserId", cliente.Id);
                }
                else
                {
                    await DisplayAlert("Autenticación", "Usuario o contraseña incorrectos", "Cerrar");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error de autenticación: {ex.Message}");
                await DisplayAlert("Error", "Ocurrió un problema al intentar autenticar. Por favor, inténtelo de nuevo.", "Cerrar");
            }
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            // Navegar a la página de registro
            await Navigation.PushAsync(new Registro());
        }
    }
}