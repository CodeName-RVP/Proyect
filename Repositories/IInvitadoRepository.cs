using GeneradorPlantillas.Models;

namespace GeneradorPlantillas.Repositories;

public interface IInvitadoRepository
{
    List<Invitado> ObtenerInvitados();

    Invitado? ObtenerInvitadoPorId(string idInvitado);

    bool RegistrarInvitado(string idInvitado);
}