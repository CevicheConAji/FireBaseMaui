namespace MauiApp1;

public class Tarea
{
    public string IdTarea { get; set; } = Guid.NewGuid().ToString();
    public string NombreTarea { get; set; }
}
