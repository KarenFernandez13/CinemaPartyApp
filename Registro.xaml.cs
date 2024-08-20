using ObligatorioTaller1.Models;

namespace ObligatorioTaller1;

public partial class Registro : ContentPage
{
	public Registro()
	{
		InitializeComponent();
	}

    private async void ConfirmarButtonClicked(object sender, EventArgs e)
    {
        // Lógica para registrar un nuevo usuario
        var username = UsernameEntry.Text;
        var password = PasswordEntry.Text;
        var mail = MailEntry.Text;
        var tel = TelEntry.Text;

        int telefono;
        bool esNumero = int.TryParse(tel, out telefono);

        if (esNumero)
        {
            var nuevoCliente = new Cliente
            {
                NombreUsuario = username,
                Password = password,
                Email = mail,
                Telefono = telefono
            };

            // Guardar los datos del usuario en SQLite
            await App.Database.SaveClienteAsync(nuevoCliente);
            await DisplayAlert("", "Registrado con éxito", "Cerrar");
            await Navigation.PopAsync();
        }
        else
        {
            // Manejar el error cuando la conversión no es exitosa
            await DisplayAlert("Error", "El campo teléfono debe ser un número válido.", "OK");
        }
    }

    private async void Camara_Clicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                var fileName = Path.Combine(FileSystem.AppDataDirectory, "profile_photo.jpg");

                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(fileName))
                {
                    await stream.CopyToAsync(newStream);
                }

                await DisplayAlert("Genial!", "Foto tomada correctamente", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo tomar la foto", "Cerrar");
        }
    }
}