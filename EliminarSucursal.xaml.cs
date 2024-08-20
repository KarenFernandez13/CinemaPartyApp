using Microsoft.Maui.Controls;
using ObligatorioTaller1.Models;
using System.Collections.ObjectModel;

namespace ObligatorioTaller1;

public partial class EliminarSucursal : ContentPage
{
    public ObservableCollection<Sucursal> SucursalesDisponibles { get; set; }

    public EliminarSucursal(ObservableCollection<Sucursal> sucursalesDisponibles)
    {
        InitializeComponent();
        SucursalesDisponibles = sucursalesDisponibles;

        // Establecer el contexto de enlace
        BindingContext = this;
    }

    private async void OnEliminarSeleccionadosClicked(object sender, EventArgs e)
    {
        var selectedItems = SucursalesCollectionView.SelectedItems.Cast<Sucursal>().ToList();

        if (selectedItems.Count == 0)
        {
            await DisplayAlert("Advertencia", "Por favor, selecciona al menos una sucursal para eliminar.", "OK");
            return;
        }

        var confirm = await DisplayAlert("Confirmar", $"¿Estás seguro de que deseas eliminar {selectedItems.Count} sucursal(es)?", "Sí", "No");

        if (!confirm)
        {
            return;
        }

        foreach (var sucursal in selectedItems)
        {
            SucursalesDisponibles.Remove(sucursal);
            await App.Database.DeleteSucursalAsync(sucursal);
        }

        await DisplayAlert("Eliminado", "Las sucursales seleccionadas han sido eliminadas.", "OK");

        await Navigation.PopAsync();
    }
}
