using GeneradorPlantillas.Models;
using GeneradorPlantillas.Repositories;

namespace GeneradorPlantillas.Services;

public class DatabaseService : IInvitadoRepository
{
    private readonly List<Invitado> invitados = new()
    {
        new Invitado
        {
            IdInvitado = "INV-00001",
            Nombre = "Juan Pérez",
            LiderEquipo = "María López"
        },

        new Invitado
        {
            IdInvitado = "INV-00002",
            Nombre = "Ana García",
            LiderEquipo = "María López"
        },

        new Invitado
        {
            IdInvitado = "INV-00003",
            Nombre = "Carlos Ruiz",
            LiderEquipo = "Pedro Sánchez",
            Estado = "REGISTRADO",
            FechaRegistro = new DateTime(2026, 9, 17, 9, 30, 0)
        },

        new Invitado
        {
            IdInvitado = "INV-00004",
            Nombre = "Luis Torres",
            LiderEquipo = "María López"
        }
    };

    public List<Invitado> ObtenerInvitados()
    {
        return invitados;
    }

    public Invitado? ObtenerInvitadoPorId(string idInvitado)
    {
        return invitados.FirstOrDefault(
            invitado => invitado.IdInvitado.Equals(
                idInvitado,
                StringComparison.OrdinalIgnoreCase
            )
        );
    }

    public bool RegistrarInvitado(string idInvitado)
    {
        var invitado = ObtenerInvitadoPorId(idInvitado);

        if (invitado == null)
        {
            return false; // Invitado no encontrado
        }

        if (invitado.Estado == "REGISTRADO")
        {
            return false; // Invitado ya registrado
        }

        invitado.Estado = "REGISTRADO";
        invitado.FechaRegistro = DateTime.Now;

        return true; // Registro exitoso
    }
}