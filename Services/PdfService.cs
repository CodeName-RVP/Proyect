using GeneradorPlantillas.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GeneradorPlantillas.Services;

public class PdfService
{
    public byte[] GenerarPdfPrueba(Invitado invitado, byte[] qr)
    {
        var documento = Document.Create(contenedor =>
        {
            contenedor.Page(pagina =>
            {
                pagina.Size(PageSizes.A4);
                pagina.Margin(40);

                pagina.Header()
                    .AlignCenter()
                    .Text("CONTROL DE ASISTENCIA")
                    .FontSize(24)
                    .Bold();

                pagina.Content()
                    .PaddingTop(30)
                    .Column(columna =>
                    {
                        columna.Spacing(20);

                        columna.Item()
                            .Text($"Líder de equipo: {invitado.LiderEquipo ?? "Sin líder"}")
                            .FontSize(16)
                            .Bold();

                        columna.Item()
                            .Row(fila =>
                            {
                                fila.RelativeItem()
                                    .AlignMiddle()
                                    .Text(invitado.Nombre)
                                    .FontSize(18);

                                fila.ConstantItem(150)
                                    .Image(qr);
                            });
                    });

                pagina.Footer()
                    .AlignCenter()
                    .Text(texto =>
                    {
                        texto.Span("ID: ");
                        texto.Span(invitado.IdInvitado);
                    });
            });
        });

        return documento.GeneratePdf();
    }

    public byte[] GenerarPdfLider(
    string nombreLider,
    List<Invitado> invitados,
    QrService qrService)
    {
        var documento = Document.Create(contenedor =>
        {
            contenedor.Page(pagina =>
            {
                pagina.Size(PageSizes.A4);
                pagina.Margin(40);

                pagina.Header()
                    .AlignCenter()
                    .Text("CONTROL DE ASISTENCIA")
                    .FontSize(24)
                    .Bold();

                pagina.Content()
                    .PaddingTop(25)
                    .Column(columna =>
                    {
                        columna.Spacing(15);

                        columna.Item()
                            .Text($"Líder de equipo: {nombreLider}")
                            .FontSize(16)
                            .Bold();

                        foreach (var invitado in invitados)
                        {
                            var qr = qrService.GenerarQr(invitado);

                            columna.Item()
                                .Border(1)
                                .Padding(10)
                                .Row(fila =>
                                {
                                    fila.RelativeItem()
                                        .AlignMiddle()
                                        .Text(invitado.Nombre)
                                        .FontSize(16);

                                    fila.ConstantItem(100)
                                        .Image(qr);
                                });
                        }
                    });

                pagina.Footer()
                    .AlignCenter()
                    .Text("Generado por el sistema de control de asistencia");
            });
        });

        return documento.GeneratePdf();
    }
}

