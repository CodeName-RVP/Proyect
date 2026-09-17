using GeneradorPlantillas.Models;

namespace GeneradorPlantillas.Services;
public class InvitadoService
{
    public Dictionary<string, List<Invitado>> AgruparPorLider(
        List<Invitado> invitados)
    {
        return invitados
            .GroupBy(invitado =>
                string.IsNullOrWhiteSpace(invitado.LiderEquipo)
                    ? "SIN_LIDER"
                    : invitado.LiderEquipo.Trim()
            )
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.ToList()
            );
    }
}