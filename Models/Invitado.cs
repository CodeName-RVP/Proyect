namespace GeneradorPlantillas.Models;
public class Invitado
{
    public string IdInvitado { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? LiderEquipo { get; set; }
    public string Estado { get; set; } = "PENDIENTE";
    public DateTime? FechaRegistro { get; set; }
}