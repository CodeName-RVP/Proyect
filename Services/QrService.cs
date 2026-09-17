using GeneradorPlantillas.Models;
using QRCoder;

namespace GeneradorPlantillas.Services;

public class QrService
{
    public byte[] GenerarQr(Invitado invitado)
    {
        using var qrGenerator = new QRCodeGenerator();

        using var qrData = qrGenerator.CreateQrCode(
            invitado.IdInvitado,
            QRCodeGenerator.ECCLevel.Q
        );

        var pngQrCode = new PngByteQRCode(qrData);

        return pngQrCode.GetGraphic(20);
    }

    public Dictionary<string, byte[]> GenerarQrs(List<Invitado> invitados)
    {
        var qrs = new Dictionary<string, byte[]>();

        foreach (var invitado in invitados)
        {
            qrs[invitado.IdInvitado] = GenerarQr(invitado);
        }

        return qrs;
    }
}