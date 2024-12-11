using Firebase.Database;
using System.Collections.ObjectModel;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
    private FirebaseService firebaseService;
    public ObservableCollection<Tarea> Tareas { get; set; }

    public MainPage()
    {
        InitializeComponent();
        firebaseService = new FirebaseService();
        Tareas = new ObservableCollection<Tarea>();
        CollectionViewTareas.ItemsSource = Tareas;

        _ = CargarTareasAsync();
    }

    private async Task CargarTareasAsync()
    {
        var tareas = await firebaseService.ObtenerTareasAsync();
        Tareas.Clear();
        foreach (var tarea in tareas)
        {
            Tareas.Add(tarea);
        }
    }

    private async void Insertar_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(EntryTarea.Text))
        {
            var nuevaTarea = new Tarea { NombreTarea = EntryTarea.Text };
            await firebaseService.InsertarTareaAsync(nuevaTarea);
            Tareas.Add(nuevaTarea);
            EntryTarea.Text = string.Empty;
        }
    }

    private async void Modificar_Clicked(object sender, EventArgs e)
    {
        if (CollectionViewTareas.SelectedItem is Tarea tareaSeleccionada && !string.IsNullOrWhiteSpace(EntryTarea.Text))
        {
            tareaSeleccionada.NombreTarea = EntryTarea.Text;
            await firebaseService.ModificarTareaAsync(tareaSeleccionada.IdTarea, tareaSeleccionada);

            Tareas.Remove(tareaSeleccionada);
            Tareas.Add(tareaSeleccionada);

            EntryTarea.Text = string.Empty;
        }
    }

    private async void Borrar_Clicked(object sender, EventArgs e)
    {
        if (CollectionViewTareas.SelectedItem is Tarea tareaSeleccionada)
        {
            await firebaseService.EliminarTareaAsync(tareaSeleccionada.IdTarea);
            Tareas.Remove(tareaSeleccionada);
        }
    }

    private void OnTareaSeleccionada(object sender, SelectionChangedEventArgs e)
    {
        if (CollectionViewTareas.SelectedItem is Tarea tareaSeleccionada)
        {
            EntryTarea.Text = tareaSeleccionada.NombreTarea;
        }
    }
}


