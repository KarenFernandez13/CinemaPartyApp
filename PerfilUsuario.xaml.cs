using ObligatorioTaller1.Models;
namespace ObligatorioTaller1;


public partial class PerfilUsuario : ContentPage
{
    private Database _database;
    public PerfilUsuario()
    {
        InitializeComponent();
        CargarDatosUsuario();
    }
    private async void CargarDatosUsuario()
    {
        try
        {
            // Obtener el ID del usuario logueado desde las preferencias
            var userId = ObtenerUserId();

            if (userId == 0)
            {
                await MostrarError("No se encontró el ID del usuario.");
                return;
            }

            // Obtener los datos del usuario desde la base de datos SQLite
            var usuario = await ObtenerUsuarioAsync(userId);

            if (usuario == null)
            {
                await MostrarError("No se pudo cargar la información del usuario.");
                return;
            }

            // Asignar datos a los Labels
            AsignarDatosUsuario(usuario);

            // Cargar la imagen de perfil si existe
            CargarImagenPerfil();
        }
        catch (Exception ex)
        {
            // Manejar errores inesperados
            await MostrarError("Ocurrió un error al cargar los datos del usuario.");
        }
    }

    private int ObtenerUserId()
    {
        return Preferences.Get("UserId", 0);
    }

    private async Task<Cliente> ObtenerUsuarioAsync(int userId)
    {
        return await App.Database.GetClienteAsync(userId);
    }

    private void AsignarDatosUsuario(Cliente usuario)
    {
        NameLabel.Text = usuario.NombreUsuario;
        TelephoneLabel.Text = usuario.Telefono.ToString();
        MailLabel.Text = usuario.Email;
        PasswordLabel.Text = usuario.Password;
    }

    private void CargarImagenPerfil()
    {
        var fileName = Path.Combine(FileSystem.AppDataDirectory, "profile_photo.jpg");

        if (File.Exists(fileName))
        {
            FotoPerfil.Source = ImageSource.FromFile(fileName);
        }
    }

    private async Task MostrarError(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "Cerrar");
    }
}