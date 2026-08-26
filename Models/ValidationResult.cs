namespace GeneradorPlantillas.Models;

public class ValidationResult
{
    public bool IsValid => Errores.Count == 0;
    public List<string> Errores { get; set; } = new();
}