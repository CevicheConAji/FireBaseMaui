namespace MauiApp1;

using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FirebaseService
{
    private readonly FirebaseClient firebase;

    public FirebaseService()
    {
        // Reemplaza con la URL correcta de tu Realtime Database
        firebase = new FirebaseClient("https://fir-prueba-f4b3f-default-rtdb.firebaseio.com/");
    }

    // Obtener todas las tareas
    public async Task<List<Tarea>> ObtenerTareasAsync()
    {
        var tareas = await firebase.Child("Tareas").OnceAsync<Tarea>();
        var listaTareas = new List<Tarea>();

        foreach (var tarea in tareas)
        {
            listaTareas.Add(new Tarea
            {
                IdTarea = tarea.Object.IdTarea,
                NombreTarea = tarea.Object.NombreTarea
            });
        }

        return listaTareas;
    }

    // Insertar una nueva tarea
    public async Task InsertarTareaAsync(Tarea tarea)
    {
        await firebase.Child("Tareas").PostAsync(tarea);
    }

    // Modificar una tarea existente
    public async Task ModificarTareaAsync(string idTarea, Tarea tarea)
    {
        var tareaFirebase = (await firebase.Child("Tareas").OnceAsync<Tarea>())
            .FirstOrDefault(t => t.Object.IdTarea == idTarea);

        if (tareaFirebase != null)
        {
            await firebase.Child("Tareas").Child(tareaFirebase.Key).PutAsync(tarea);
        }
    }

    // Eliminar una tarea
    public async Task EliminarTareaAsync(string idTarea)
    {
        var tareaFirebase = (await firebase.Child("Tareas").OnceAsync<Tarea>())
            .FirstOrDefault(t => t.Object.IdTarea == idTarea);

        if (tareaFirebase != null)
        {
            await firebase.Child("Tareas").Child(tareaFirebase.Key).DeleteAsync();
        }
    }
}
