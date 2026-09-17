using GeneradorPlantillas.Models;

namespace GeneradorPlantillas.Services;

public class PlanillaService
{
    public Dictionary<string, byte[]> GenerarPlanillas(
        List<Invitado> invitados,
        InvitadoService invitadoService,
        PdfService pdfService,
        QrService qrService)
    {
        var planillas = new Dictionary<string, byte[]>();

        var grupos = invitadoService.AgruparPorLider(invitados);

        foreach (var grupo in grupos)
        {
            var nombreLider = grupo.Key;
            var invitadosDelLider = grupo.Value;

            var pdf = pdfService.GenerarPdfLider(
                nombreLider,
                invitadosDelLider,
                qrService
            );

            planillas[nombreLider] = pdf;
        }

        return planillas;
    }
}